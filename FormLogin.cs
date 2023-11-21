using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supermercado {
    public partial class FormLogin : Form {
        public FormLogin() {
            InitializeComponent();
            this.KeyPreview = true;
        }
        private void Login() {
            using (var context = new SuperchinoDBModel()) {
                try {
                    string username = txboxUsuario.Text;
                    string password = txboxPassword.Text;
                    var query = from aux in context.Usuarios
                                where aux.username == username
                                select aux;
                    if (query.Count() < 1) {
                        MessageBox.Show("No se encontro el usuario", "Mensajito", MessageBoxButtons.OK);
                        return;
                    }
                    Usuarios usuario = query.First();
                    if (usuario.password != password) {
                        MessageBox.Show("La contraseña es incorrecta", "Mensajito", MessageBoxButtons.OK);
                        return;
                    }
                    Sesiones sesion = new Sesiones {
                        idSupermercado = 1,
                        idUsuario = usuario.idUsuario,
                        fecha = DateTime.Now.Date,
                        horaInicio = DateTime.Now,
                        horafinal = null,
                        monto = 0
                    };
                    context.Sesiones.Add(sesion);
                    context.SaveChanges();
                    FormMenu menu = new FormMenu(sesion);
                    menu.Show();
                    this.Hide();
                } catch (Exception ex) {
                    MessageBox.Show($"Error: {ex.ToString()}", "Error", MessageBoxButtons.OK);
                }
                
            }
        }
        private void btnLogin_Click(object sender, EventArgs e) {
                Login();
        }

        private void txboxUsuario_Enter(object sender, EventArgs e) {
            if (txboxUsuario.ForeColor == Color.Silver) txboxUsuario.Text = "";
            txboxUsuario.ForeColor = Color.Black;
        }


        private void txboxUsuario_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                txboxPassword.Focus();
            }
        }

        private void txboxPassword_Enter(object sender, EventArgs e) {
            if (txboxPassword.ForeColor == Color.Silver) {
                txboxPassword.Text = "";
                txboxPassword.UseSystemPasswordChar = true;
            }
            txboxUsuario.ForeColor = Color.Black;
        }

        private void txboxPassword_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                Login();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                Environment.Exit(0);
            }
        }
    }
}


/*Usuario usuario = new Usuario();
usuario.permisos = 1;
usuario.username = "admin";
usuario.password = "admin";
usuario.nombre = "administrador de mierda";
context.Usuarios.Add(usuario);
context.SaveChanges();*/