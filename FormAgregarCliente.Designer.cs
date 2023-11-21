namespace Supermercado {
    partial class FormAgregarCliente {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAgregarCliente));
            this.txboxDNI = new System.Windows.Forms.TextBox();
            this.txboxNombre = new System.Windows.Forms.TextBox();
            this.txboxMail = new System.Windows.Forms.TextBox();
            this.txboxTelefono = new System.Windows.Forms.TextBox();
            this.txboxDireccion = new System.Windows.Forms.TextBox();
            this.btnCargar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txboxDNI
            // 
            this.txboxDNI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txboxDNI.Enabled = false;
            this.txboxDNI.Font = new System.Drawing.Font("Yu Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txboxDNI.ForeColor = System.Drawing.Color.Black;
            this.txboxDNI.Location = new System.Drawing.Point(12, 12);
            this.txboxDNI.Name = "txboxDNI";
            this.txboxDNI.Size = new System.Drawing.Size(376, 35);
            this.txboxDNI.TabIndex = 0;
            this.txboxDNI.Text = "DNI*";
            this.txboxDNI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txboxDNI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txboxDNI_KeyPress);
            // 
            // txboxNombre
            // 
            this.txboxNombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txboxNombre.Font = new System.Drawing.Font("Yu Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txboxNombre.ForeColor = System.Drawing.Color.Silver;
            this.txboxNombre.Location = new System.Drawing.Point(12, 53);
            this.txboxNombre.Name = "txboxNombre";
            this.txboxNombre.Size = new System.Drawing.Size(376, 35);
            this.txboxNombre.TabIndex = 1;
            this.txboxNombre.Text = "Nombre y apellido*";
            this.txboxNombre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txboxNombre.Enter += new System.EventHandler(this.txboxNombre_Enter);
            this.txboxNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txboxNombre_KeyPress);
            // 
            // txboxMail
            // 
            this.txboxMail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txboxMail.Font = new System.Drawing.Font("Yu Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txboxMail.ForeColor = System.Drawing.Color.Silver;
            this.txboxMail.Location = new System.Drawing.Point(12, 94);
            this.txboxMail.Name = "txboxMail";
            this.txboxMail.Size = new System.Drawing.Size(376, 35);
            this.txboxMail.TabIndex = 2;
            this.txboxMail.Text = "Mail";
            this.txboxMail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txboxMail.Enter += new System.EventHandler(this.txboxMail_Enter);
            this.txboxMail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txboxMail_KeyPress);
            // 
            // txboxTelefono
            // 
            this.txboxTelefono.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txboxTelefono.Font = new System.Drawing.Font("Yu Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txboxTelefono.ForeColor = System.Drawing.Color.Silver;
            this.txboxTelefono.Location = new System.Drawing.Point(12, 135);
            this.txboxTelefono.Name = "txboxTelefono";
            this.txboxTelefono.Size = new System.Drawing.Size(376, 35);
            this.txboxTelefono.TabIndex = 3;
            this.txboxTelefono.Text = "Telefono";
            this.txboxTelefono.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txboxTelefono.Enter += new System.EventHandler(this.txboxTelefono_Enter);
            this.txboxTelefono.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txboxTelefono_KeyPress);
            // 
            // txboxDireccion
            // 
            this.txboxDireccion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txboxDireccion.Font = new System.Drawing.Font("Yu Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txboxDireccion.ForeColor = System.Drawing.Color.Silver;
            this.txboxDireccion.Location = new System.Drawing.Point(12, 176);
            this.txboxDireccion.Name = "txboxDireccion";
            this.txboxDireccion.Size = new System.Drawing.Size(376, 35);
            this.txboxDireccion.TabIndex = 4;
            this.txboxDireccion.Text = "Direccion";
            this.txboxDireccion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txboxDireccion.Enter += new System.EventHandler(this.txboxDireccion_Enter);
            this.txboxDireccion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txboxDireccion_KeyPress);
            // 
            // btnCargar
            // 
            this.btnCargar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCargar.Font = new System.Drawing.Font("Yu Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargar.Location = new System.Drawing.Point(12, 217);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(376, 56);
            this.btnCargar.TabIndex = 5;
            this.btnCargar.Text = "Cargar";
            this.btnCargar.UseVisualStyleBackColor = true;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // FormAgregarCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(400, 285);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.txboxDireccion);
            this.Controls.Add(this.txboxTelefono);
            this.Controls.Add(this.txboxMail);
            this.Controls.Add(this.txboxNombre);
            this.Controls.Add(this.txboxDNI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(4000, 285);
            this.MinimumSize = new System.Drawing.Size(400, 285);
            this.Name = "FormAgregarCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormAgregarCliente_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txboxDNI;
        private System.Windows.Forms.TextBox txboxNombre;
        private System.Windows.Forms.TextBox txboxMail;
        private System.Windows.Forms.TextBox txboxTelefono;
        private System.Windows.Forms.TextBox txboxDireccion;
        private System.Windows.Forms.Button btnCargar;
    }
}