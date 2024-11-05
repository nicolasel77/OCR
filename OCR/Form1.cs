using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tesseract;

namespace OCR
{
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void cmdBuscar_Click(object sender, EventArgs e)
        {
            pintar();
        }
       
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmdComparar_Click(object sender, EventArgs e)
        {
            Buscar_resultado();
        }

        //private void cmdPrueba_firma_Click(object sender, EventArgs e)
        //{
        //    string imagePath = lblImagen.Text;

        //    // Cargar la imagen
        //    var image = Image.FromFile(lblImagen.Text);
        //    Mat img = CvInvoke.Imread(image, ImreadModes.Color);

        //    if (img.IsEmpty)
        //    {
        //        lblRespuesta.Text = "Error: No se pudo cargar la imagen.";
        //        return;
        //    }

        //    // Convertir a escala de grises
        //    Mat gray = new Mat();
        //    CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

        //    // Aplicar umbralización
        //    Mat thresh = new Mat();
        //    CvInvoke.Threshold(gray, thresh, 127, 255, ThresholdType.BinaryInv);

        //    // Encontrar contornos
        //    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
        //    {
        //        Mat hierarchy = new Mat();
        //        CvInvoke.FindContours(thresh, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

        //        // Dibujar contornos y verificar si hay firmas
        //        bool hasSignature = false;
        //        for (int i = 0; i < contours.Size; i++)
        //        {
        //            double contourArea = CvInvoke.ContourArea(contours[i]);
        //            if (contourArea > 500) // Este valor debe ajustarse según tus necesidades
        //            {
        //                hasSignature = true;
        //                CvInvoke.DrawContours(img, contours, i, new MCvScalar(0, 0, 255), 2);
        //            }
        //        }

        //        // Guardar la imagen con los contornos dibujados
        //        string outputPath = "path_to_output_image.jpg";
        //        img.Save(outputPath);

        //        if (hasSignature)
        //        {
        //            lblRespuesta.Text = "La imagen contiene una firma.";
        //        }
        //        else
        //        {
        //            lblRespuesta.Text = "No se detectaron firmas en la imagen.";
        //        }
        //    }
        //}



        private void frmInicio_Load(object sender, EventArgs e)
        {

        }



        private void cmdPrueba_Click(object sender, EventArgs e)
        {
            //                                                              txtBuscar.Text
            string filePath = @"D:\demo\Ejemplos\Examples\Birth\Approved\" + "blanco" + @"\";

            string fileContent = File.ReadAllText(filePath + "values.txt");

            txtSalida.Text = leer_input(fileContent);

        }

        private string leer_input(string input)
        {
            // Dividir el string en líneas usando saltos de línea
            string[] inputLines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            input = "";

            int i = 0;

            foreach (string line in inputLines)
            {
                // Asignar los valores capturados a las variables correspondientes
                string fieldName = line.Substring(0, line.IndexOf(":"));
                i = line.IndexOf(":") + 2;

                string operatorSign = line.Substring(i, line.IndexOf(" ", i) - i);
                i = line.IndexOf(" ", i + 1);

                string textValue = line.Substring(i, line.IndexOf(",", i) - i);
                i = line.IndexOf(",", i + 1);

                string percentageValue = line.Substring(i, line.Length - i - 1);

                // Imprimir el resultado
                input += $"{fieldName}: ";
            }
            return input;
        }

        private bool Matcheo(string operador, string resultado, string porcentaje)
        {

            if (DateTime.TryParseExact(resultado, "MM/dd/aaaa", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
            {
                switch (operador)
                {
                    case "=":
                        Console.WriteLine("Operador de igualdad");
                        break;
                    case ">=":
                        Console.WriteLine("Operador mayor o igual");
                        break;
                    case "<=":
                        Console.WriteLine("Operador menor o igual");
                        break;
                    case "<":
                        Console.WriteLine("Operador menor o igual");
                        break;
                    case ">":
                        Console.WriteLine("Operador menor o igual");
                        break;
                }
            }
            else
            {
                switch (operador)
                {
                    case "=":
                        if (resultado.IndexOf(" ") > -1)
                        {
                            //Buscar_matcheos(BuscarPalabrasConContexto());
                        }
                        else
                        {

                        }
                        Console.WriteLine("Operador de igualdad");
                        break;
                    case "!=":
                        Console.WriteLine("Operador de desigualdad");
                        break;
                    default:
                        Console.WriteLine("Operador no reconocido");
                        break;
                }
            }

            return true;
        }


        #region Pdf
        static void ExtractImagesFromPdf(string pdfPath, string outputPath)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfPath));

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                PdfPage page = pdfDoc.GetPage(i);
                var imageListener = new ImageRenderListener(outputPath);
                var processor = new PdfCanvasProcessor(imageListener);
                processor.ProcessPageContent(page);
            }

            Console.WriteLine("Extracción completada.");
        }

        public class ImageRenderListener : IEventListener
        {
            private string outputPath;
            private int imageCounter = 1;

            public ImageRenderListener(string outputPath)
            {
                this.outputPath = outputPath;
            }

            public void EventOccurred(IEventData data, EventType type)
            {
                if (type == EventType.RENDER_IMAGE)
                {
                    var renderInfo = (ImageRenderInfo)data;
                    var imageObject = renderInfo.GetImage();

                    if (imageObject != null)
                    {
                        var imageBytes = imageObject.GetImageBytes();
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            Image image = Image.FromStream(ms);
                            string imagePath = GetUniqueFilePath(outputPath, $"imagen_{imageCounter++}.jpg");
                            image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }
            }

            public ICollection<EventType> GetSupportedEvents()
            {
                return new HashSet<EventType> { EventType.RENDER_IMAGE };
            }

            private string GetUniqueFilePath(string directory, string fileName)
            {
                string filePath = Path.Combine(directory, fileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);
                int count = 1;

                // Si el archivo ya existe, agregar un sufijo numérico al nombre del archivo
                while (File.Exists(filePath))
                {
                    string tempFileName = $"{fileNameWithoutExtension}({count++}){extension}";
                    filePath = Path.Combine(directory, tempFileName);
                }

                return filePath;
            }

        }
        #endregion

        #region Editados
        private void rotar_imagen()
        {
            // Directorio donde están las imágenes
            string directorioImagen = @"D:\demo\" + txtBuscar.Text + @"\" + cbTipo.Text + ".jpg";

            // Cargar la imagen en un objeto Bitmap
            Bitmap imagenOriginal = new Bitmap(picEntrada.Image);

            // Rotar la imagen 90 grados hacia la derecha
            imagenOriginal.RotateFlip(RotateFlipType.Rotate90FlipNone);

            // Construir el nombre de la nueva imagen rotada
            string nombreImagenRotada = directorioImagen.Replace(".jpg", "editada.jpg");

            // Guardar la copia con la imagen rotada
            imagenOriginal.Save(nombreImagenRotada);

            picEntrada.ImageLocation = nombreImagenRotada;
        }

        //Desaturar
        private void cmdDesaturar_Click(object sender, EventArgs e)
        {
            //txtRespuestas.Text = txtSalida.Text;

            // Guardar la imagen resultante
            string s = lblImagen.Text.Substring(0, lblImagen.Text.LastIndexOf("\\") + 1);

            string d = s;
            string i = lblImagen.Text.Substring(lblImagen.Text.LastIndexOf("."));
            string c = lblImagen.Text.Substring(s.Length, lblImagen.Text.Length - s.Length - i.Length);
            string n = $"{d}Copia {c}{i}";

            string outputImagePath = n;
            float exposure = 1.2f; // Adjust this value for more or less exposure

            using (Bitmap bitmap = new Bitmap(lblImagen.Text))
            {
                AdjustExposure(bitmap, exposure);
                bitmap.Save(outputImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            lblImagen.Text = n;

            picEntrada.ImageLocation = n;

        }

        static void AdjustExposure(Bitmap bitmap, float exposure)
        {
            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Adjust the exposure.
            for (int i = 0; i < rgbValues.Length; i += 4)
            {
                rgbValues[i] = AdjustPixel(rgbValues[i], exposure);     // Blue
                rgbValues[i + 1] = AdjustPixel(rgbValues[i + 1], exposure); // Green
                rgbValues[i + 2] = AdjustPixel(rgbValues[i + 2], exposure); // Red
                                                                            // Alpha channel (rgbValues[i + 3]) stays the same
            }

            // Copy the RGB values back to the bitmap.
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bitmap.UnlockBits(bmpData);
        }

        static byte AdjustPixel(byte colorValue, float exposure)
        {
            float newValue = colorValue * exposure;
            if (newValue < 0) { newValue = 0; }
            if (newValue > 255) { newValue = 255; }

            return (byte)newValue;
        }

        #endregion

        #region Comparaciones
        //Busca un listado de posibles resultados y compara cual es el más apto
        private void Busqueda_de_Resultados(string texto, string busqueda)
        {
            string[] resultados = BuscarPalabrasConContexto(texto, busqueda, 2);

            Buscar_matcheos(resultados);
        }

        static string[] BuscarPalabrasConContexto(string texto, string palabrasClave, int contexto)
        {
            // Separar el texto y las palabras clave
            string[] palabrasTexto = texto.Split(new char[] { ' ', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] palabrasBuscar = palabrasClave.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultados = new List<string>();

            // Buscar cada palabra clave en el texto
            for (int n_palabra = 0; n_palabra < palabrasBuscar.Length; n_palabra++)
            {

                for (int i = 0; i < palabrasTexto.Length; i++)
                {
                    if (palabrasTexto[i].Equals(palabrasBuscar[n_palabra], StringComparison.OrdinalIgnoreCase))
                    {
                        // Obtener las palabras en el contexto de la clave encontrada
                        int inicio = Math.Max(0, i - contexto - palabrasBuscar.Length + (palabrasBuscar.Length - n_palabra)); // Asegura que no vaya antes del inicio del array
                        int fin = Math.Min(palabrasTexto.Length - 1, i + contexto + palabrasBuscar.Length - 1 - n_palabra); // Asegura que no vaya después del final

                        // Construir el fragmento de texto con las palabras alrededor
                        List<string> fragmento = new List<string>();
                        string valor = "";
                        for (int j = inicio; j <= fin; j++)
                        {
                            valor += " " + palabrasTexto[j];
                        }
                        fragmento.Add(valor);
                        fragmento.Add("\n\n\n");

                        // Unir las palabras en un solo string y agregarlo a la lista de resultados
                        resultados.Add(string.Join(" ", fragmento));
                    }
                }
            }

            return resultados.ToArray();
        }

        private void Buscar_matcheos(string[] candidatos, string valor)
        {

            if (valor.Length > 0 && candidatos.Length > 0)
            {
                List<string> list1 = new List<string>(valor.Split(' '));

                //Separo las palabras encontradas por el OCR
                //Más Adelante la idea es que la devuelva la IA directo

                List<string> list2;

                double mas_proxima = 0;
                double nva_comparacion;
                int id_mas_prox = 0;
                int distancia;
                double similitud = 0;
                string mejor_match = "";

                for (int candidato = 0; candidato < candidatos.Length; candidato++)
                {
                    list2 = new List<string>(candidatos[candidato].Split(' '));

                    for (int i = 0; i < list1.Count; i++)
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {
                            distancia = CalcularDistanciaLevenshtein(list1[i], list2[j]);
                            nva_comparacion = CalcularSimilitud(list1[i], list2[j], distancia);
                            if (nva_comparacion > mas_proxima) { mas_proxima = nva_comparacion; id_mas_prox = j; }
                        }
                        distancia = CalcularDistanciaLevenshtein(list1[i], list2[id_mas_prox]);
                        similitud += CalcularSimilitud(list1[i], list2[id_mas_prox], distancia);
                        mejor_match = mejor_match + " " + list2[id_mas_prox];
                        list2.Remove(list2[id_mas_prox]);

                        mas_proxima = 0;
                        id_mas_prox = 0;
                    }

                    mejor_match = mejor_match.Substring(1);

                    distancia = CalcularDistanciaLevenshtein(valor, mejor_match);
                    similitud = CalcularSimilitud(valor, mejor_match, distancia);
                    //label3.Text = mejor_match;

                    lblSimilitud.Text = $"La similitud es del: {similitud * 100:0.00}%";
                    txtRespuestas.Text += candidatos[candidato] + mejor_match + $" {similitud * 100:0.00}% \n";

                    mejor_match = "";
                    similitud = 0;
                }
            }
            else { lblSimilitud.Text = "No hay texto para comparar"; }
        }

        //Comparacion individual
        private string Buscar_resultado(string texto, string busqueda)
        {           
            // Leer todo el contenido del archivo y asignarlo a una variable string
            string fileContent = File.ReadAllText(filePath + ".txt");
            string d = "";
            for (int cont = 0; cont < fileContent.Count(); cont++)
            {
                d = d + fileContent.Substring(cont, fileContent.IndexOf(":", cont) + 2 - cont);
                cont = fileContent.IndexOf(":", cont) + 2;
                //Mostrar el contenido en la consola
                //txtRespuestas.Text = fileContent;
                string palabra1 = fileContent.Substring(cont, fileContent.IndexOf(";", cont) - cont);
                string palabra2 = txtSalida.Text.ToLower();

                if (palabra1.Length > 0 && palabra2.Length > 0)
                {
                    List<string> list1 = new List<string>(palabra1.Split(' '));

                    //Separo las palabras encontradas por el OCR
                    //Más Adelante la idea es que la devuelva la IA directo

                    List<string> list2 = new List<string>(palabra2.Split(' '));

                    double mas_proxima = 0;
                    double nva_comparacion;
                    int id_mas_prox = 0;
                    int distancia;
                    double similitud = 0;
                    string mejor_match = "";

                    for (int i = 0; i < list1.Count; i++)
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {
                            distancia = CalcularDistanciaLevenshtein(list1[i], list2[j]);
                            nva_comparacion = CalcularSimilitud(list1[i], list2[j], distancia);
                            if (nva_comparacion > mas_proxima) { mas_proxima = nva_comparacion; id_mas_prox = j; }
                        }
                        distancia = CalcularDistanciaLevenshtein(list1[i], list2[id_mas_prox]);
                        similitud += CalcularSimilitud(list1[i], list2[id_mas_prox], distancia);
                        mejor_match = mejor_match + " " + list2[id_mas_prox];
                        list2.Remove(list2[id_mas_prox]);

                        mas_proxima = 0;
                        id_mas_prox = 0;
                    }

                    mejor_match = mejor_match.Substring(1);

                    distancia = CalcularDistanciaLevenshtein(palabra1, mejor_match);
                    similitud = CalcularSimilitud(palabra1, mejor_match, distancia);
                    //label3.Text = mejor_match;

                    lblSimilitud.Text = $"La similitud es del: {similitud * 100:0.00}%";
                    d = d + mejor_match + $" {similitud * 100:0.00}%";
                }
                else { lblSimilitud.Text = "No hay texto para comparar"; }
                d = d + "\n";
                cont = fileContent.IndexOf(";", cont) + 1;
            }
            return d;
        }

        #endregion

        #region Calculos
        static int CalcularDistanciaLevenshtein(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
                return m;
            if (m == 0)
                return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, //eliminación
                        d[i, j - 1] + 1),         //Inserción
                        d[i - 1, j - 1] + cost);  //Sustitución
                }
            }
            return d[n, m];
        }

        static double CalcularSimilitud(string s, string t, int distancia)
        {
            int maxLen = Math.Max(s.Length, t.Length);
            if (maxLen == 0)
                return 1.0;

            return (1.0 - distancia / (double)maxLen);
        }
        #endregion

        #region Formateos
        //Formatear fechas
        private string fecha_formateada(string f)
        {
            //mm = Mes, dd = día, aaaa = año, dm = nombre mes, snm = nombre mes corto, thh = sufijo ordinal(th)
            int dia;
            int mes;
            int año;
            string fechas = "mm/dd/aaaa;dd/mm/aaaa;dd-mm-aaaa;mm-dd-aaaa;dd mm aaaa;mm dd aaaa;dd.mm.aaaa;mm.dd.aaaa;dd de dm de aaaa;dd de dm de aaaa;dm dd aaaa;dd dm aaaa;thh day of dm aaaa;" +
                "dd snm aaaa;snm dd aaaa;dd snm / snm aaaa;";
            string entrada = "11/13/2027";

            mes = Convert.ToInt32(entrada.Substring(0, entrada.IndexOf("/")));
            dia = Convert.ToInt32(entrada.Substring(entrada.IndexOf("/") + 1, entrada.LastIndexOf("/") - entrada.IndexOf("/") - 1));
            año = Convert.ToInt32(entrada.Substring(entrada.LastIndexOf("/") + 1));

            fechas = fechas.Replace("mm", mes.ToString());
            fechas = fechas.Replace("dd", dia.ToString());
            fechas = fechas.Replace("aaaa", año.ToString());
            fechas = fechas.Replace("dm", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes));
            fechas = fechas.Replace("snm", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes).Substring(0, 3));
            fechas = fechas.Replace("thh", ordinal(dia));

            return fechas;
        }

        static string ordinal(int number)
        {
            if (number % 100 >= 11 && number % 100 <= 13)
                return number + "th";

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        #endregion

        #region Varias
        private Pix obtener_imagen(string path)
        {
            return Pix.LoadFromFile(path);
        }

        private string leer_imagen(Pix img)
        {
            var engine = new TesseractEngine(@"D:\tessdata", "eng");
            var page = engine.Process(img);

            var text = page.GetText();

            text = text.Replace("\n", " ");
            text = text.Replace(",", " ");
            text = text.Replace(".", " ");
            text = text.Replace(":", " ");
            text = text.Replace("-", " ");
            text = text.ToLower();

            return text;
        }

        #endregion
    }
}
