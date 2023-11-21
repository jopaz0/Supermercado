using System;
using System.IO;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Drawing;

namespace Supermercado {

    public partial class Ventas {
        public void GenerarTicketPDF() {
            try {
                SuperchinoDBModel context = new SuperchinoDBModel();
                Sesiones sesion = context.Sesiones.Find(idSesion);
                Usuarios usuario = context.Usuarios.Find(sesion.idUsuario);
                Supermercados sucursal = context.Supermercados.Find(sesion.idSupermercado);
                Clientes cliente = context.Clientes.Find(idCliente);
                string nombreArchivoPDF = $"Ticket-Venta-{idVenta}.pdf";
                string rutaArchivoPDF = Path.Combine(Path.GetTempPath(), nombreArchivoPDF);
                string aux;

                XFont fuenteGrande = new XFont("Arial", 8);
                XFont fuente = new XFont("Arial", 4);
                double 
                    margenesX = 5, 
                    xPapel = 100, 
                    xx = xPapel - 10,
                    margenesY = 5, 
                    interlineado = 1,
                    saltoLinea = interlineado + fuente.GetHeight(),
                    yPapel = 2 * fuenteGrande.GetHeight() + 2 * interlineado + (7 + (Lineas.Count*2)) * saltoLinea,
                    textWidth;

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                //aca definir el tamaño de papel
                page.Width = xPapel;
                page.Height = yPapel;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                

                XTextFormatter tf = new XTextFormatter(gfx);
                tf.DrawString(sucursal.nombre, fuenteGrande, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += (interlineado * 2) + fuenteGrande.GetHeight();
                
                tf.DrawString($"Empleado: {usuario.nombre}", fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += saltoLinea;
                
                tf.DrawString($"Cliente: {cliente.nombre}", fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += saltoLinea;
                
                tf.DrawString($"Fecha: {fecha}", fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += saltoLinea;

                string ticketn = idVenta.ToString();
                while (ticketn.Length < 8) ticketn = "0" + ticketn;
                ticketn = ticketn.Substring(0, 3) + "-" + ticketn.Substring(3);
                tf.DrawString($"Numero de ticket: {ticketn}", fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += saltoLinea;
                
                //aca hay 50 caracteres
                tf.DrawString($"--------------------------------------------------", fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += saltoLinea;
                margenesX += 1;
                foreach (Lineas linea in Lineas) {
                    Articulos articulo = context.Articulos.Find(linea.idArticulo);
                    aux = articulo.detalle + " - " + articulo.presentacion;
                    //aux = aux.Substring(0, 50);
                    tf.DrawString(aux, fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                    margenesY += saltoLinea;

                    aux = $"$ {linea.precioUn:0.00} x {linea.cantidad} u.";
                    tf.DrawString(aux, fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                    aux = $"$ {(linea.cantidad*linea.precioUn):0.00}";
                    textWidth = gfx.MeasureString(aux, fuente).Width;
                    tf.DrawString(aux, fuente, XBrushes.Black, new XRect(xx - textWidth, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                    margenesY += saltoLinea;
                }
                margenesX -= 1;
                tf.DrawString($"--------------------------------------------------", fuente, XBrushes.Black, new XRect(margenesX, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += saltoLinea;
                aux = $"Monto de la operacion: {monto:0.00}";
                textWidth = gfx.MeasureString(aux, fuente).Width;
                tf.DrawString(aux, fuente, XBrushes.Black, new XRect(xx - textWidth, margenesY, page.Width, page.Height), XStringFormats.TopLeft);
                margenesY += saltoLinea;
                document.Save(rutaArchivoPDF);
                System.Diagnostics.Process.Start(rutaArchivoPDF);
            }
            catch (Exception ex) {
                MessageBox.Show("Error al exportar ticket PDF: " + ex.Message, "ERROR", MessageBoxButtons.YesNo);
            }

        }
    }
}
