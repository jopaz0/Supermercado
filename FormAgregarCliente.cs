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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Supermercado {
    public partial class FormAgregarCliente : Form {
        SuperchinoDBModel context;
        public FormAgregarCliente(string dni) {
            InitializeComponent();
            this.KeyPreview = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            context = new SuperchinoDBModel();
            txboxDNI.Text = dni;
        }

        //para poder agrandar la ventana sin bordes
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

        //cerrar form al precionar escape. va junto con {this.KeyPreview = true;} luego de inicializar
        private void FormAgregarCliente_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == ((char)Keys.Escape)) {
                this.Close();
            }
        }

        //clicks
        private void btnCargar_Click(object sender, EventArgs e) {
            AgregarCliente();
        }
        

        //formatear al ganar foco
        private void txboxNombre_Enter(object sender, EventArgs e) {
            FormatearTxboxNormal(sender);
        }
        private void txboxMail_Enter(object sender, EventArgs e) {
            FormatearTxboxNormal(sender);
        }
        private void txboxTelefono_Enter(object sender, EventArgs e) {
            FormatearTxboxNormal(sender);
        }
        private void txboxDireccion_Enter(object sender, EventArgs e) {
            FormatearTxboxNormal(sender);
        }


        //acciones al presionar enter
        private void txboxDNI_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                txboxNombre.Focus();
            }
        }
        private void txboxNombre_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                if (txboxNombre.Text.Length < 5) {
                    MessageBox.Show("El nombre es muy corto.", "Error", MessageBoxButtons.OK);
                    return;
                }
                if (!txboxNombre.Text.Contains(" ")) {
                    MessageBox.Show("Se detecto una sola palabra, debe ingresar nombre y apellido.", "Error", MessageBoxButtons.OK);
                    return;
                }
                txboxMail.Focus();
            }
        }
        private void txboxMail_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                if (EsDireccionDeCorreoValida(txboxMail.Text)) {
                    txboxTelefono.Focus();
                }
                else {
                    MessageBox.Show("La dirección de correo electrónico no es válida.");
                }
            }
        }
        private void txboxTelefono_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                txboxDireccion.Focus();
            }
        }
        private void txboxDireccion_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                AgregarCliente();
            }
        }

        //funciones
        private void AgregarCliente() {
            //validar dni
            string dniCrudo = txboxDNI.Text;
            int dni;
            if (!Int32.TryParse(dniCrudo, out dni)) {
                MessageBox.Show("No se pudo convertir el campo DNI a un numero.", "Error", MessageBoxButtons.OK);
                return;
            }
            if (txboxNombre.Text.Length < 5) {
                MessageBox.Show("El nombre es muy corto.", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!txboxNombre.Text.Contains(" ")) {
                MessageBox.Show("Se detecto una sola palabra, debe ingresar nombre y apellido.", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!EsDireccionDeCorreoValida(txboxMail.Text)) {
                MessageBox.Show("La dirección de correo electrónico no es válida.", "Error", MessageBoxButtons.OK);
                return;
            }

            //agregar a la bbdd
            Clientes cliente = new Clientes {
                dni = dni,
                nombre = txboxNombre.Text,
                mail = txboxMail.Text,
                direccion = txboxDireccion.Text,
                telefono = txboxTelefono.Text
            };
            context.Clientes.Add(cliente);
            context.SaveChanges();
            MessageBox.Show("Cliente agregado con exito!", "Exito", MessageBoxButtons.OK);
            this.Close();
        }
        private void FormatearTxboxNormal(object sender) {
            TextBox t = sender as TextBox;
            t.Text = "";
            t.ForeColor = Color.Black;
        }

        private bool EsDireccionDeCorreoValida(string email) {
            string patronCorreo = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex regex = new Regex(patronCorreo);
            return regex.IsMatch(email);
        }

    }
}