using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Reg;
using Emgu.CV.Structure;


namespace Supermercado {
    public partial class FormCamaras : Form {
        private Mat Frame;
        private VideoCapture Camara;

        public FormCamaras() {
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
        

        private void FormCamaras_Load(object sender, EventArgs e) {
            Frame = new Mat();
            Camara = new VideoCapture();
            timer1.Interval = 40;
            pboxCamara1.SizeMode = PictureBoxSizeMode.StretchImage;

            Camara.Start();
            if (!timer1.Enabled) timer1.Enabled = true;
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e) {
            
                Camara.Read(Frame);
                pboxCamara1.Image = Frame.ToBitmap();
            
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}