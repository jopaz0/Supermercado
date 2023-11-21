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
    public partial class FormSelTabla : Form {
        Usuarios usuario;
        public FormSelTabla() {
            InitializeComponent();
            this.KeyPreview = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
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

        private void FormSelTabla_Load(object sender, EventArgs e) {

        }

        private void btnVolver_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void FormSelTabla_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        private void btnArticulos_Click(object sender, EventArgs e) {
            FormCRUDArticulos form = new FormCRUDArticulos();
            form.ShowDialog();
        }
        private void btnUsuarios_Click(object sender, EventArgs e) {
            FormCRUDUsuarios form = new FormCRUDUsuarios();
            form.ShowDialog();
        }

        private void btnProveedores_Click(object sender, EventArgs e) {
            FormCRUDProveedores form = new FormCRUDProveedores();
            form.ShowDialog();
        }
    }
}
