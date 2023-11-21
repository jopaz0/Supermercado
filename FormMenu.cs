using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supermercado {
    public partial class FormMenu : Form {
        Sesiones sesion;
        public FormMenu(Sesiones _sesion) {
            InitializeComponent();
            this.KeyPreview = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this.sesion = _sesion;
        }

        //para agrandar achicar la ventana
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
        //fin caja negra

        private void FormMenu_Load(object sender, EventArgs e) {
            //if (sesion.Usuarios.permisos == 0) btnBBDD.Enabled = false;
            //else 
                btnBBDD.Enabled = true;
        }

        private void btnBBDD_Click(object sender, EventArgs e) {
            FormSelTabla formSeleccion = new FormSelTabla();
            formSeleccion.ShowDialog();
        }

        private void btnCerrar_Click(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        private void FormMenu_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                Environment.Exit(0);
            }
        }

        private void btnVentas_Click(object sender, EventArgs e) {
            FormVentas formVentas = new FormVentas(sesion);
            formVentas.ShowDialog();
        }

        private void btnCamaras_Click(object sender, EventArgs e) {
            FormCamaras formCamaras = new FormCamaras();
            formCamaras.Show();
        }
    }
}
