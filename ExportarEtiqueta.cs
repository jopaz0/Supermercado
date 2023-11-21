using System;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using ZXing;
using System.Windows;

namespace Supermercado {
    public partial class Articulos {
        public static string VerificarBarcode(string barcode) {
            if (barcode.Length == 13) return barcode;
            if (barcode.Length > 13) {
                return barcode = barcode.Substring(0, 13);
            }
            while (barcode.Length < 13) {
                barcode = "0" + barcode;
            }
            return barcode;
        }
        public void GenerarEtiqueta() {
            string nombreArchivoPDF = $"Ticket-Venta-{idArticulo}-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.pdf";
            string rutaArchivoPDF = Path.Combine(Path.GetTempPath(), nombreArchivoPDF);
            XFont fuenteMuyGrande = new XFont("Calibri", 18);
            XFont fuenteGrande = new XFont("Calibri", 12);
            XFont fuente = new XFont("Calibri", 6);
            double xPapel = 200,
                    yPapel = 150;

            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Etiqueta {detalle}";
            PdfPage page = document.AddPage();
            page.Width = xPapel;
            page.Height = yPapel;
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //rectangulito pal codigo de barras
            XRect barcodeArea = new XRect(XUnit.FromMillimeter(0), XUnit.FromMillimeter(5), XUnit.FromMillimeter(70), XUnit.FromMillimeter(25));
            XBrush barcodeAreaBrush = XBrushes.LightGray;
            gfx.DrawRectangle(barcodeAreaBrush, barcodeArea);

            //hago el codigo de barras
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.CODE_128; // Puedes cambiar el formato según tus necesidades
            barcodeWriter.Options = new ZXing.Common.EncodingOptions {
                Width = 300, // Ancho del código de barras
                Height = 100 // Altura del código de barras
            };
            barcode = VerificarBarcode(this.barcode);
            if (VerificarBarcode(this.barcode) == null) {
                MessageBox.Show("No se pudo generar la etiqueta porque no se pudo validar el codigo de barras.", "Error", MessageBoxButton.OK);
                return;
            }

            string barcodeData = barcode.ToString(); // Cambia esto por tus datos
            Bitmap barcodeBitmap = barcodeWriter.Write(barcodeData);

            // Inserto el código de barras en el rectangulito
            string tempImagePath = $"barcode{barcodeData}-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png";
            try {
                if (File.Exists(tempImagePath)) File.Delete(tempImagePath);
                barcodeBitmap.Save(tempImagePath);
            } catch (Exception ex){
                MessageBox.Show($"No se pudo guardar la imagen. Error: {ex.Message}","Error",MessageBoxButton.OK);
                return;
            }
            
            XImage barcodeImage = XImage.FromFile(tempImagePath);
            gfx.DrawImage(barcodeImage, barcodeArea);

            // Agregar texto debajo del código de barras
            //gfx.DrawString(detalle, new XFont("Arial", 10), XBrushes.Black, new XRect(XUnit.FromMillimeter(15), XUnit.FromMillimeter(35), XUnit.FromMillimeter(70), XUnit.FromMillimeter(5)), XStringFormats.TopLeft);
            //gfx.DrawString(presentacion, new XFont("Arial", 5), XBrushes.Black, new XRect(XUnit.FromMillimeter(15), XUnit.FromMillimeter(40), XUnit.FromMillimeter(70), XUnit.FromMillimeter(5)), XStringFormats.TopLeft);
            //gfx.DrawString(precioVenta.ToString(), new XFont("Arial", 10), XBrushes.Black, new XRect(XUnit.FromMillimeter(15), XUnit.FromMillimeter(45), XUnit.FromMillimeter(70), XUnit.FromMillimeter(5)), XStringFormats.TopLeft);

            // Agregar texto debajo del código de barras
            XRect detalleArea = new XRect(XUnit.FromMillimeter(5), XUnit.FromMillimeter(35), XUnit.FromMillimeter(70), XUnit.FromMillimeter(15));
            gfx.DrawString(detalle ?? "detalle?", fuenteGrande, XBrushes.Black, detalleArea, XStringFormats.TopLeft);
            XRect presentacionArea = new XRect(XUnit.FromMillimeter(5), XUnit.FromMillimeter(40), XUnit.FromMillimeter(70), XUnit.FromMillimeter(15));
            gfx.DrawString(presentacion ?? "Presentacion?", fuente, XBrushes.Black, presentacionArea, XStringFormats.TopLeft);


            // Centrar el texto del precio con símbolo de pesos y dos dígitos decimales

            if (precioVenta == null) precioVenta = 0;
            decimal precioDecimal = (decimal)precioVenta; // Convertir a decimal
            string precioFormateado = $"$ {precioDecimal.ToString()}";
            double textWidth = gfx.MeasureString(precioFormateado, fuenteMuyGrande).Width;

            XRect priceArea = new XRect((xPapel - textWidth - 10), XUnit.FromMillimeter(42), XUnit.FromMillimeter(70), XUnit.FromMillimeter(5));
            gfx.DrawString(precioFormateado, fuenteMuyGrande, XBrushes.Black, priceArea, XStringFormats.TopLeft);


            try { 
                if (File.Exists(rutaArchivoPDF)) File.Delete(rutaArchivoPDF);
                document.Save(rutaArchivoPDF);
                System.Diagnostics.Process.Start(rutaArchivoPDF);
            } catch (Exception ex){
                MessageBox.Show($"No se pudo guardar el pdf. Error: {ex.Message}","Error",MessageBoxButton.OK);
                return;
            }
        }
    }
}
