using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Supermercado {
    public partial class FormCRUDUsuarios : Form {
        SuperchinoDBModel context;
        IQueryable<Usuarios> query;
        List<Usuarios> nuevosElementos;
        public FormCRUDUsuarios() {
            InitializeComponent();
            this.KeyPreview = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            context = new SuperchinoDBModel();
            nuevosElementos = new List<Usuarios>();
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
        private void PoblarComboBoxCampo() {
            var propiedadesUsuarios = typeof(Usuarios).GetProperties();
            cboxCampo.Items.AddRange(propiedadesUsuarios.Select(prop => prop.Name).ToArray());
            cboxCampo.SelectedIndex = 0;
        }
        private void btnConsultar_Click(object sender, EventArgs e) {
            Consultar();
        }
        private void Consultar() {
            List<String> camposTexto = new List<String> { "username", "password", "nombre", "direccion", "telefono" };
            float filtroParseado;
            string campoSeleccionado = cboxCampo.SelectedItem.ToString();
            string operacionSeleccionada = cboxOperacion.SelectedItem.ToString();
            string filtro = txboxFiltro.Text;

            if (string.IsNullOrEmpty(filtro) || filtro == "*") {
                query = from a in context.Usuarios
                        select a;
            }
            else {
                switch (operacionSeleccionada) {
                    case "=":
                        query = from a in context.Usuarios
                                where campoSeleccionado == "idUsuario" && a.idUsuario.ToString() == filtro
                                   || campoSeleccionado == "username" && a.username.ToString() == filtro
                                   || campoSeleccionado == "password" && a.password == filtro
                                   || campoSeleccionado == "nombre" && a.nombre == filtro
                                   || campoSeleccionado == "direccion" && a.direccion == filtro
                                   || campoSeleccionado == "telefono" && a.telefono == filtro
                                   || campoSeleccionado == "permisos" && a.permisos.ToString() == filtro
                                select a;
                        break;
                    case "!=":
                        query = from a in context.Usuarios
                                where campoSeleccionado == "idUsuario" && a.idUsuario.ToString() != filtro
                                   || campoSeleccionado == "username" && a.username.ToString() != filtro
                                   || campoSeleccionado == "password" && a.password != filtro
                                   || campoSeleccionado == "nombre" && a.nombre != filtro
                                   || campoSeleccionado == "direccion" && a.direccion != filtro
                                   || campoSeleccionado == "telefono" && a.telefono != filtro
                                   || campoSeleccionado == "permisos" && a.permisos.ToString() != filtro
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
                        query = from a in context.Usuarios
                                where campoSeleccionado == "idUsuario" && a.idUsuario < filtroParseado
                                   || campoSeleccionado == "permisos" && a.permisos < filtroParseado
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
                        query = from a in context.Usuarios
                                where campoSeleccionado == "idUsuario" && a.idUsuario > filtroParseado
                                   || campoSeleccionado == "permisos" && a.permisos > filtroParseado
                                select a;
                        break;
                    default:
                        break;
                }
            }

            dataGridView1.DataSource = query.ToList();
            dataGridView1.Update();
            OcultarYBloquear(dataGridView1);
        }
        private void OcultarYBloquear(DataGridView dgv) {
            List<int> ocultar = new List<int> { 7 };
            List<int> bloquear = new List<int> { 0 };
            foreach (int i in ocultar) dgv.Columns[i].Visible = false;
            foreach (int i in bloquear) {
                dgv.Columns[i].ReadOnly = true;
                dgv.Columns[i].DefaultCellStyle.BackColor = Color.Silver;
            }
        }
        private void btnInsertar_Click(object sender, EventArgs e) {
            CrearNuevoElemento();
        }
        private void txboxBarcode_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                CrearNuevoElemento();
            }
        }
        private void CrearNuevoElemento() {
            Usuarios nuevoElemento = new Usuarios();
            nuevoElemento.username = "invitado";
            nuevoElemento.password = "invitado";
            nuevoElemento.permisos = 0;
            nuevosElementos.Add(nuevoElemento);

            dataGridView1.DataSource = null;
            dataGridView1.Update();
            dataGridView1.DataSource = nuevosElementos;
            dataGridView1.Update();
            OcultarYBloquear(dataGridView1);

            btnGuardar.Enabled = true;
        }

        //esta wea no anda
        private void FormEditArticulos_KeyDown(object sender, KeyEventArgs e) {
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
            try {
                foreach (Usuarios elemento in nuevosElementos) {
                    if (elemento.username == "COMPLETAR!" || elemento.password == "COMPLETAR!") {
                        lblAyuda.Text = "Complete los datos de todos los nuevos usuarios para guardar!";
                        lblAyuda.ForeColor = Color.OrangeRed;
                        return;
                    }
                    context.Usuarios.Add(elemento);
                }
                nuevosElementos.Clear();
                context.SaveChanges();
                List<Usuarios> elementosModificados = ((List<Usuarios>)dataGridView1.DataSource).Where(a => context.Entry(a).State != EntityState.Unchanged).ToList();
                foreach (var elemento in elementosModificados) {
                    context.Entry(elemento).State = EntityState.Modified;
                }
                context.SaveChanges();
                lblAyuda.Text = "Cambios guardados exitosamente.";
                lblAyuda.ForeColor = Color.ForestGreen;
                btnGuardar.Enabled = false;
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
            try {
                foreach (DataGridViewRow row in selectedRows) {
                    Usuarios elemento = (Usuarios)row.DataBoundItem;
                    context.Usuarios.Remove(elemento);
                }
                context.SaveChanges();
                lblAyuda.Text = "Filas eliminadas exitosamente.";
                lblAyuda.ForeColor = Color.ForestGreen;
            }
            catch (DbUpdateException ex) {
                MessageBox.Show("Error al eliminar filas: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Consultar();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            btnGuardar.Enabled = true;
        }

        private void FormEditArticulos_FormClosed(object sender, FormClosedEventArgs e) {
        }

        private void FormCRUDUsuarios_Load(object sender, EventArgs e) {

        }
    }
}