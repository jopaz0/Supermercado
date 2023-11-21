using System;
using System.Windows.Forms;

public class InputBoxForm : Form {
    private Label label;
    private TextBox textBox;
    private Button okButton;
    private Label label1;
    private Button cancelButton;

    public InputBoxForm(string prompt) {
        label = new Label {
            Text = prompt,
            Location = new System.Drawing.Point(12, 9),
            AutoSize = true
        };

        textBox = new TextBox {
            Location = new System.Drawing.Point(12, 29),
            Size = new System.Drawing.Size(260, 20)
        };

        okButton = new Button {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new System.Drawing.Point(116, 58)
        };

        cancelButton = new Button {
            Text = "Cancelar",
            DialogResult = DialogResult.Cancel,
            Location = new System.Drawing.Point(197, 58)
        };

        Controls.Add(label);
        Controls.Add(textBox);
        Controls.Add(okButton);
        Controls.Add(cancelButton);

        FormBorderStyle = FormBorderStyle.FixedDialog;
        AcceptButton = okButton;
        CancelButton = cancelButton;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Ingrese un valor numérico";
    }

    public static int Show(string prompt) {
        using (var form = new InputBoxForm(prompt)) {
            if (form.ShowDialog() == DialogResult.OK) {
                int result;
                if (int.TryParse(form.textBox.Text, out result)) {
                    return result;
                }
                else {
                    MessageBox.Show("El valor ingresado no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return 0; // Valor predeterminado si el usuario cancela o ingresa un valor no válido.
        }
    }

    private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(353, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "label1";
            // 
            // InputBoxForm
            // 
            this.ClientSize = new System.Drawing.Size(514, 318);
            this.Controls.Add(this.label1);
            this.Name = "InputBoxForm";
            this.Load += new System.EventHandler(this.InputBoxForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void InputBoxForm_Load(object sender, EventArgs e) {

    }
}
