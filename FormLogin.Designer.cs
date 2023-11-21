namespace Supermercado {
    partial class FormLogin {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.btnLogin = new System.Windows.Forms.Button();
            this.txboxUsuario = new System.Windows.Forms.TextBox();
            this.txboxPassword = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.Info;
            this.btnLogin.Font = new System.Drawing.Font("Yu Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLogin.Location = new System.Drawing.Point(175, 210);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(250, 36);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txboxUsuario
            // 
            this.txboxUsuario.Font = new System.Drawing.Font("Yu Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txboxUsuario.ForeColor = System.Drawing.Color.Silver;
            this.txboxUsuario.Location = new System.Drawing.Point(175, 60);
            this.txboxUsuario.MaxLength = 50;
            this.txboxUsuario.Name = "txboxUsuario";
            this.txboxUsuario.Size = new System.Drawing.Size(250, 36);
            this.txboxUsuario.TabIndex = 1;
            this.txboxUsuario.Text = "Usuario";
            this.txboxUsuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txboxUsuario.Enter += new System.EventHandler(this.txboxUsuario_Enter);
            this.txboxUsuario.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txboxUsuario_KeyPress);
            // 
            // txboxPassword
            // 
            this.txboxPassword.Font = new System.Drawing.Font("Yu Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txboxPassword.ForeColor = System.Drawing.Color.Silver;
            this.txboxPassword.Location = new System.Drawing.Point(175, 135);
            this.txboxPassword.MaxLength = 50;
            this.txboxPassword.Name = "txboxPassword";
            this.txboxPassword.Size = new System.Drawing.Size(250, 36);
            this.txboxPassword.TabIndex = 2;
            this.txboxPassword.Text = "Contraseña";
            this.txboxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txboxPassword.Enter += new System.EventHandler(this.txboxPassword_Enter);
            this.txboxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txboxPassword_KeyPress);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Yu Gothic UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(555, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "x";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txboxPassword);
            this.Controls.Add(this.txboxUsuario);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(600, 300);
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txboxUsuario;
        private System.Windows.Forms.TextBox txboxPassword;
        private System.Windows.Forms.Button button1;
    }
}

