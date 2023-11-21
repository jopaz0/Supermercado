using Emgu.CV.Ocl;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Drawing.BarCodes;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Supermercado {
    public partial class FormVentas : Form {
        SuperchinoDBModel context;
        Sesiones sesion;
        Clientes cliente;
        List<Lineas> lineas, lineasComprimidas;
        string ultimoBarcode = "0";
        DataTable tablaVentas;
        double montoTotal;
        public FormVentas(Sesiones _sesion) {
            InitializeComponent();
            this.KeyPreview = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);


            context = new SuperchinoDBModel();
            lineas = new List<Lineas>();
            lineasComprimidas = new List<Lineas>();
            montoTotal = 0;
            this.sesion = _sesion;

            tablaVentas = new DataTable();
            tablaVentas.Columns.Add("ID", typeof(int));
            tablaVentas.Columns.Add("PRODUCTO", typeof(string));
            tablaVentas.Columns.Add("DESCRIPCION", typeof(string));
            tablaVentas.Columns.Add("PRECIO U.", typeof(float));
            tablaVentas.Columns.Add("CANTIDAD", typeof(int));
            tablaVentas.Columns.Add("SUBTOTAL", typeof(float));

            RefrescarTabla();
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
        //cerrar form al presionar escape
        private void FormVentas_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
            }
        }

        //CLICKS
        private void button1_Click(object sender, EventArgs e) {
            LimpiarVenta();
        }
        private void btnBuscarCliente_Click(object sender, EventArgs e) {
            BuscarYMostrarCliente();
        }
        private void btnEliminar_Click(object sender, EventArgs e) {
            if (lineas.Count() > 0) {
                lineas.RemoveAt(lineas.Count - 1);
                ComprimirLineas();
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e) {
            CerrarVenta();
            LimpiarVenta();
        }

        //FOCUS
        private void txboxCliente_Enter(object sender, EventArgs e) {
            txboxCliente.ForeColor = Color.Black;
            txboxCliente.Text = string.Empty;
        }

        //KEYPRESS
        private void txboxCliente_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == ((char)Keys.Enter)) {
                BuscarYMostrarCliente();
            }
        }
        private void txboxBarcode_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == ((char)Keys.Enter)) {
                AgregarProducto();
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == ((char)Keys.Enter)) {
                AgregarProducto();
            }
        }

        //FUNCIONES MIAS
        private void LimpiarVenta() {
            lineas.Clear();
            lineasComprimidas.Clear();
            ComprimirLineas();
            lblCliente.Text = "Anonimo";
            cliente = context.Clientes.Find(1);
        }
        private void CerrarVenta() {
            Ventas venta = null;
            if (cliente is null) cliente = context.Clientes.Find(1);
            try {
                venta = new Ventas {
                    idSesion = sesion.idSesion,
                    idCliente = cliente.idCliente,
                    monto = montoTotal,
                    fecha = DateTime.Now,
                };
                context.Ventas.Add(venta);
                context.SaveChanges();
            }
            catch (Exception ex) {
                MessageBox.Show("Error al registrar la venta: " + ex.Message, "ERROR", MessageBoxButtons.YesNo);
            }
            try {
                foreach (Lineas linea in lineasComprimidas) {
                    linea.idVenta = venta.idVenta;
                    context.Lineas.Add(linea);
                    context.Articulos.Find(linea.idArticulo).stock -= linea.cantidad;
                }
                cliente.gasto += montoTotal;
                sesion.monto += montoTotal;
                context.SaveChanges();
                venta.GenerarTicketPDF();
            } catch (Exception ex) {
                MessageBox.Show("Error al actualizar la base de datos: " + ex.Message,"ERROR",MessageBoxButtons.YesNo);
            }
        }
        private void RefrescarTabla() {
            lblTotal.Text = "Monto Total: " + montoTotal.ToString();
            if (lineasComprimidas.Count > 0) btnGuardar.Enabled = true;
            else btnGuardar.Enabled = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = tablaVentas;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Update();
        }
        private void ComprimirLineas() {
            lineasComprimidas = new List<Lineas>();
            bool flag;
            montoTotal = 0;
            if (tablaVentas.Rows.Count > 0) tablaVentas.Clear();
            foreach (Lineas linea in lineas) {
                flag = false;
                montoTotal += (linea.monto ?? 0);
                Lineas existente = lineasComprimidas.AsEnumerable().FirstOrDefault(i => i.idArticulo == linea.idArticulo);
                if (existente != null && existente.idArticulo != 1) {
                    existente.cantidad = existente.cantidad + linea.cantidad;
                    existente.monto = existente.cantidad * existente.precioUn;
                } else {
                    lineasComprimidas.Add(new Lineas {
                        idVenta = linea.idVenta,
                        idArticulo = linea.idArticulo,
                        cantidad = linea.cantidad,
                        precioUn = linea.precioUn,
                        monto = linea.monto
                    });
                }
            }
            foreach (Lineas linea in lineasComprimidas) {
                Articulos articulo = BuscarArticuloID(linea.idArticulo);
                tablaVentas.Rows.Add(
                    articulo.idArticulo,
                    articulo.detalle,
                    articulo.presentacion,
                    linea.precioUn,
                    linea.cantidad,
                    linea.monto
                );
            }
            RefrescarTabla();
        }
        private void BuscarYMostrarCliente() {
            //validar input txboxCliente.Text
            IQueryable<Clientes> seleccion = context.Clientes
                .Where(a => a.dni.ToString() == txboxCliente.Text)
                .AsQueryable();
            if (seleccion.Count() > 1) {
                lblAyuda.Text = "Se encontraron varios clientes con el mismo dni. Contacte con un administrador del sistema para solucionar el problema.";
                return;
            }
            if (seleccion.Count() == 0) {
                DialogResult result = MessageBox.Show("No se encontro el cliente. Puede registrarlo o continuar con una venta anonima. Desea registrarlo?", "Cliente no encontrado", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) {
                    FormAgregarCliente form = new FormAgregarCliente(txboxCliente.Text);
                    form.ShowDialog();
                    form.Focus();
                }
                else {
                    txboxCliente.Text = "0";
                }
                BuscarYMostrarCliente();
            }
            Clientes cliente = seleccion.First();
            lblCliente.Text = cliente.nombre;
        }
        private void AgregarProducto() {
            double? precioVARIOS = null;
            bool validado = false;
            int cantidadTotal;
            int cantidad = (int)numCantidad.Value;
            string barcode = txboxBarcode.Text;
            Articulos articulo;
            IQueryable<Articulos> seleccion = context.Articulos
                .Where(a => a.barcode.ToString() == txboxBarcode.Text)
                .AsQueryable();
            
            if (seleccion.Count() == 0) {
                MessageBox.Show("No se encontro el articulo. Para venderlo como VARIOS, use el codigo de barras 0");
                txboxBarcode.Text = "0000000000000";
                return;
            }
            articulo = seleccion.First();
            if (articulo.idArticulo == 1) {
                while (!validado) {
                    precioVARIOS = InputBoxForm.Show("Ingrese el precio unitario del producto");
                    if (precioVARIOS != null && precioVARIOS > 0)
                        validado = true;
                }
            } else {

                cantidadTotal = lineas
                    .Where(linea => linea.idArticulo == articulo.idArticulo)
                    .Sum(linea => linea.cantidad) ?? 0;

                cantidadTotal += cantidad ;
                if (cantidadTotal > articulo.stock) {
                    MessageBox.Show($"La cantidad ingresada supera el stock existente ({articulo.stock})!");
                    txboxBarcode.Text = "0000000000000";
                    return;
                }
            }

            Lineas lineaNueva = new Lineas {
                idVenta = 0,
                idArticulo = articulo.idArticulo,
                cantidad = cantidad,
                precioUn = precioVARIOS ?? articulo.precioVenta,
                monto = (precioVARIOS ?? articulo.precioVenta) * cantidad,
            };
            lineas.Add(lineaNueva);

            ComprimirLineas();

            numCantidad.Value = 1;
            txboxBarcode.Text = "0000000000000";
            txboxBarcode.Focus();
        }
        private Articulos BuscarArticulo(string barcode) {
            IQueryable<Articulos> query;
            txboxBarcode.Text = "";
            if(string.IsNullOrEmpty(barcode)) barcode = "" ;

            while (barcode.Length < 13) barcode = "0" + barcode;
            
            query = from a in context.Articulos
                    where a.barcode == barcode
                    select a;
            if (query.Count() == 0) return BuscarArticulo("1");
            return query.First();
        }

        private void txboxCliente_TextChanged(object sender, EventArgs e) {
            TextBox textBox = (TextBox)sender;
            string texto = textBox.Text;
            string nuevoTexto = new string(texto.Where(char.IsDigit).ToArray());
            textBox.Text = nuevoTexto;
            textBox.SelectionStart = nuevoTexto.Length;
        }

        private void txboxBarcode_TextChanged(object sender, EventArgs e) {
            TextBox textBox = (TextBox)sender;
            string texto = textBox.Text;
            string nuevoTexto = new string(texto.Where(char.IsDigit).ToArray());
            while (nuevoTexto.Length > 13) {
                nuevoTexto = nuevoTexto.Substring(1);
            }
            nuevoTexto = nuevoTexto.PadLeft(13, '0');
            textBox.Text = nuevoTexto;
            textBox.SelectionStart = nuevoTexto.Length;
        }

        private Articulos BuscarArticuloID(int id) {
            return context.Articulos.Find(id);
        }


    }
}