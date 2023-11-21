using PdfSharp.Drawing.BarCodes;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Supermercado {
    public partial class FormCRUDArticulos : Form {
        SuperchinoDBModel context;
        IQueryable<Articulos> query;
        Usuarios usuario;
        public FormCRUDArticulos() {
            InitializeComponent();
            this.KeyPreview = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            context = new SuperchinoDBModel();
            PoblarComboBoxCampo();
            cboxCampo.SelectedIndex = 0;
            cboxOperacion.SelectedIndex = 0;
            Consultar();
        }
        private const int cGrip = 16;
        private const int cCaption = 200;

        protected override void OnPaint(PaintEventArgs e) {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.Transparent, rc);
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x84) {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption) {
                    m.Result = (IntPtr)2;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip) {
                    m.Result = (IntPtr)17;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void DispararMensajeAlerta(string mensaje, bool box, string enc = "Error") {
            lblAyuda.Text = mensaje;
            lblAyuda.ForeColor = Color.OrangeRed;
            if (box) MessageBox.Show(mensaje, enc, MessageBoxButtons.OK);

        }

        private void PoblarComboBoxCampo() {
            var propiedades = typeof(Articulos).GetProperties();
            cboxCampo.Items.AddRange(propiedades.Select(prop => prop.Name).ToArray());
            cboxCampo.SelectedIndex = 0;
        }
        private void btnConsultar_Click(object sender, EventArgs e) {
            btnGuardar_Click(sender, e);
            Consultar();
        }
        private void Consultar() { 
            List<String> camposTexto = new List<String> { "detalle","presentacion","barcode" };
            IQueryable<Articulos> elementosParaMostrar;
            float filtroParseado;
            string campoSeleccionado = cboxCampo.SelectedItem.ToString();
            string operacionSeleccionada = cboxOperacion.SelectedItem.ToString();
            string filtro = txboxFiltro.Text;

            if (string.IsNullOrEmpty(filtro) || filtro == "*") {
                query = from a in context.Articulos
                        where a.eliminado != true
                        select a;
            }
            else {
                switch (operacionSeleccionada) {
                    case "=":
                        query = from a in context.Articulos
                                where campoSeleccionado == "idArticulo" && a.idArticulo.ToString() == filtro
                                    || campoSeleccionado == "idProveedor" && a.idProveedor.ToString() == filtro
                                    || campoSeleccionado == "detalle" && a.detalle == filtro
                                    || campoSeleccionado == "presentacion" && a.presentacion == filtro
                                    || campoSeleccionado == "precioCompra" && a.precioCompra.ToString() == filtro
                                    || campoSeleccionado == "precioVenta" && a.precioVenta.ToString() == filtro
                                    || campoSeleccionado == "stock" && a.stock.ToString() == filtro
                                    || campoSeleccionado == "barcode" && a.barcode == filtro
                                select a;
                        break;
                    case "!=":
                        query = from a in context.Articulos
                                where campoSeleccionado == "idArticulo" && a.idArticulo.ToString() != filtro
                                    || campoSeleccionado == "idProveedor" && a.idProveedor.ToString() != filtro
                                    || campoSeleccionado == "detalle" && a.detalle != filtro
                                    || campoSeleccionado == "presentacion" && a.presentacion != filtro
                                    || campoSeleccionado == "precioCompra" && a.precioCompra.ToString() != filtro
                                    || campoSeleccionado == "precioVenta" && a.precioVenta.ToString() != filtro
                                    || campoSeleccionado == "stock" && a.stock.ToString() != filtro
                                    || campoSeleccionado == "barcode" && a.barcode != filtro
                                select a;
                        break;
                    case "<":
                        if (camposTexto.Contains(campoSeleccionado)) {
                            lblAyuda.Text = "Para operaciones de < y >, el campo debe ser numérico.";
                            lblAyuda.ForeColor = Color.OrangeRed;
                            break;
                        }
                        if (!float.TryParse(filtro, out filtroParseado)) {
                            lblAyuda.Text = "No se pudo convertir el filtro a un valor numerico.";
                            lblAyuda.ForeColor = Color.OrangeRed;
                        }
                        query = from a in context.Articulos
                                where campoSeleccionado == "idArticulo" && a.idArticulo < filtroParseado
                                    || campoSeleccionado == "idProveedor" && a.idProveedor < filtroParseado
                                    || campoSeleccionado == "precioCompra" && a.precioCompra < filtroParseado
                                    || campoSeleccionado == "precioVenta" && a.precioVenta < filtroParseado
                                    || campoSeleccionado == "stock" && a.stock < filtroParseado
                                select a;
                        break;
                    case ">":
                        if (camposTexto.Contains(campoSeleccionado)) {
                            lblAyuda.Text = "Para operaciones de < y >, el campo debe ser numérico.";
                            lblAyuda.ForeColor = Color.OrangeRed;
                            break;
                        }
                        if (!float.TryParse(filtro, out filtroParseado)) {
                            lblAyuda.Text = "No se pudo convertir el filtro a un valor numerico.";
                            lblAyuda.ForeColor = Color.OrangeRed;
                        }
                        query = from a in context.Articulos
                                where campoSeleccionado == "idArticulo" && a.idArticulo > filtroParseado
                                    || campoSeleccionado == "idProveedor" && a.idProveedor > filtroParseado
                                    || campoSeleccionado == "precioCompra" && a.precioCompra > filtroParseado
                                    || campoSeleccionado == "precioVenta" && a.precioVenta > filtroParseado
                                    || campoSeleccionado == "stock" && a.stock > filtroParseado
                                select a;
                        break;
                    default:
                        break;
                }
            }


            elementosParaMostrar = (cboxCampo.SelectedIndex == 9) ? query : query.Where(a => a.eliminado != true);

            RefrescarTabla(dataGridView1, elementosParaMostrar);
        }
        private void RefrescarTabla(DataGridView dgv, IQueryable<Articulos> seleccion) {
            /*var seleccionFormateada = seleccion.Select(a => new {
                IdArticulo = a.idArticulo,
                NombreProveedor = context.Proveedores
                    .Where(p => p.idProveedor == a.idProveedor)
                    .Select(p => p.nombre)
                    .FirstOrDefault() ?? "Contrabando",
                Detalle = a.detalle,
                Marca = a.marca,
                Presentacion = a.presentacion,
                PrecioCompra = a.precioCompra,
                PrecioVenta = a.precioVenta,
                Stock = a.stock,
                Codigo = a.barcode,
            });

            dataGridView1.DataSource = null;
            dataGridView1.Update();
            dataGridView1.DataSource = seleccionFormateada.ToList();*/

            dataGridView1.DataSource = null;
            dataGridView1.Update();
            dataGridView1.DataSource = seleccion.ToList();
            List<int> ocultar = new List<int> { 9, 10, 11 };
            foreach (int i in ocultar) dgv.Columns[i].Visible = false;

            List<int> bloquear = new List<int> { 0, 1 };
            foreach (int i in bloquear) {
                dgv.Columns[i].ReadOnly = true;
                dgv.Columns[i].DefaultCellStyle.BackColor = Color.Silver;
            }

            dataGridView1.Update();
        }
        private void btnInsertar_Click(object sender, EventArgs e) {
            CrearNuevoElemento();
            txboxBarcode.Text = "";
        }
        private void txboxBarcode_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                CrearNuevoElemento();
            }
        }
        private void CrearNuevoElemento() {
            string barcode = txboxBarcode.Text.Trim();
            if (barcode == "Codigo de Barras" || barcode == "") barcode = "0000000000000";
            else {
                Int64 barcodeNum;
                if (barcode.Length > 13) {
                    DispararMensajeAlerta("La longitud maxima de un codigo de barras es de 13 caracteres.", true);
                    return;
                }
                if (!Int64.TryParse(barcode, out barcodeNum)) {
                    DispararMensajeAlerta("El codigo de barras debe estar compuesto por numeros", true);
                    return;
                }
                if (barcodeNum < 0) {
                    DispararMensajeAlerta("Detectado numero negativo, fue pasado a positivo", false);
                    barcodeNum = Math.Abs(barcodeNum);
                }
                barcode = barcodeNum.ToString();
                while (barcode.Length < 13)
                    barcode = "0" + barcode;
                if (context.Articulos.Any(a => a.barcode == barcode)) {
                    DispararMensajeAlerta("Ya existe un articulo con ese codigo!", true);
                    return;
                }
            }
            Articulos nuevoElemento = new Articulos();
            nuevoElemento.idProveedor = 1;
            nuevoElemento.barcode = barcode;
            nuevoElemento.detalle = "COMPLETAR";
            context.Articulos.Add(nuevoElemento);
            context.SaveChanges();

            cboxCampo.SelectedIndex = 2;
            cboxOperacion.SelectedIndex = 1;
            txboxFiltro.Text = "COMPLETAR";
            Consultar();

            txboxBarcode.Text = "";
            txboxBarcode.Focus();
        }

        //esta wea no anda
        private void FormCRUD_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                DialogResult dialogRes = MessageBox.Show(
                    "Esta seguro de que desea cerrar la ventana? Perdera los datos de la venta actual.",
                    "Cerrar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button2
                );
                if (dialogRes == DialogResult.Yes) {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e) {
            bool cambios = false;
            try {
                List<Articulos> elementosModificados = ((List<Articulos>)dataGridView1.DataSource).Where(a => context.Entry(a).State != EntityState.Unchanged).ToList();
                foreach (var elemento in elementosModificados) {
                    //lista de comprobaciones para guardar o no el articulo
                    Int64 barcodeNum;
                    elemento.eliminado = false;
                    if (!Int64.TryParse(elemento.barcode, out barcodeNum)) {
                        DispararMensajeAlerta($"El codigo de barras debe estar compuesto por numeros, se descarto la edicion del articulo {elemento.detalle}.", true);
                        continue;
                    }
                    if (barcodeNum < 0) {
                        DispararMensajeAlerta($"Detectado numero negativo en el codigo de barras de {elemento.detalle}, fue pasado a positivo", false);
                        barcodeNum = Math.Abs(barcodeNum);
                        elemento.barcode = barcodeNum.ToString();
                    }
                    if (elemento.barcode.Length > 13) {
                        DispararMensajeAlerta($"La longitud maxima de un codigo de barras es de 13 caracteres, se descarto la edicion del articulo {elemento.detalle}.", true);
                        continue;
                    }
                    if (elemento.precioVenta < elemento.precioCompra) {
                        DialogResult dialogRes = MessageBox.Show(
                            "El precio de venta es menor al precio de compra, es correcto?",
                            "Advertencia",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button2
                        );
                        if (dialogRes == DialogResult.No) {
                            DispararMensajeAlerta($"Se descarto la edicion del articulo {elemento.detalle}.", true);
                            continue;
                        }
                    }
                    if(elemento.stock < 0) {
                        DispararMensajeAlerta($"El stock no puede ser negativo, se descarto la edicion del articulo {elemento.detalle}.", true);
                        continue;
                    }
                    cambios = true;
                    context.Entry(elemento).State = EntityState.Modified;
                }
                if (!cambios) return;
                try {
                    context.SaveChanges();
                } catch (Exception ex) {
                    DispararMensajeAlerta($"No se pudo guardar los cambios. Error: {ex.ToString()}", true);
                    return;
                }
                
                lblAyuda.Text = "Cambios guardados exitosamente.";
                lblAyuda.ForeColor = Color.ForestGreen;
                btnGuardar.Enabled = false;
                btnEliminar.Enabled = true;
                
                Consultar();
            }
            catch (DbUpdateException ex) {
                MessageBox.Show("Error al guardar los cambios: " + ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e) {
            DataGridViewSelectedRowCollection selectedRows = dataGridView1.SelectedRows;
            if (selectedRows.Count == 0) {
                lblAyuda.Text = "No se seleccionaron filas para eliminar.";
                lblAyuda.ForeColor = Color.OrangeRed;
                return;
            }
            DialogResult result = MessageBox.Show("¿Esta seguro que desea eliminar las filas seleccionadas?", "Confirmación", MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel) return;
            
            try {
                foreach (DataGridViewRow row in selectedRows) {
                    Articulos articulo = (Articulos)row.DataBoundItem;
                    articulo.eliminado = true;
                }
                context.SaveChanges();
                Consultar();
                lblAyuda.Text = "Filas eliminadas exitosamente.";
                lblAyuda.ForeColor = Color.ForestGreen;
            }
            catch (DbUpdateException ex) {
                MessageBox.Show("Error al eliminar filas: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            btnGuardar.Enabled = true;
        }

        private void FormEditArticulos_FormClosed(object sender, FormClosedEventArgs e) {
        }

        private void btnEtiqueta_Click(object sender, EventArgs e) {
            DataGridViewSelectedRowCollection selectedRows = dataGridView1.SelectedRows;
            if (selectedRows.Count == 0) {
                lblAyuda.Text = "No se seleccionaron articulos.";
                lblAyuda.ForeColor = Color.OrangeRed;
                return;
            }
            try {
                foreach (DataGridViewRow row in selectedRows) {
                    Articulos articulo = (Articulos)row.DataBoundItem;
                    articulo.GenerarEtiqueta();
                }
                lblAyuda.Text = "Etiquetas generadas exitosamente en carpeta temporal.";
                lblAyuda.ForeColor = Color.ForestGreen;
            }
            catch (DbUpdateException ex) {
                MessageBox.Show("Error al Generar etiquetas: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void btnBorrarFiltros_Click(object sender, EventArgs e) {
            cboxCampo.SelectedIndex = 0;
            cboxOperacion.SelectedIndex = 0;
            txboxFiltro.Text = "";
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {

            //esto me borra la celda si salta un error de conversion (si meto texto a un precio o stock)
            if (e.Exception is FormatException) {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DBNull.Value;
                e.ThrowException = false;
            }
        }
    }
}