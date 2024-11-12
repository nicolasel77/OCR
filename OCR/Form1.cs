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
using System.Windows.Forms;
using Tesseract;

namespace OCR
{
    public partial class frmInicio : Form
    {
        private List<string> rechazados;
        public frmInicio()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void cmdBuscar_Click(object sender, EventArgs e)
        {
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmdComparar_Click(object sender, EventArgs e)
        {
            //string texto_actual = leer_imagen(PixConverter.ToPix(new Bitmap(picEntrada.Image)));
            //int posicion_correcta = 0;
            //for (int i = 1; i < 4; i++)
            //{
            //    string nvo_texto = leer_imagen(PixConverter.ToPix(new Bitmap(rotar_imagen(picEntrada.Image, i))));

            //    if (nvo_texto.Length >  texto_actual.Length)
            //    { 
            //        texto_actual = nvo_texto; 
            //        posicion_correcta = i;
            //    }
            //}

            //picEntrada.Image = rotar_imagen(picEntrada.Image, posicion_correcta);


            string filePath = @"D:\demo\Ejemplos\Examples\Photo" + @"\";

            picEntrada.Image = obtener_imagen(filePath + "prueba.jpeg");




            //Bitmap imagenOriginal = new Bitmap(picEntrada.Image);

            //Bitmap imagenGris = ConvertirAEscalaDeGrises(imagenOriginal);
            //Bitmap imagenContraste = AumentarContraste(imagenGris, 2.0f); // Ajusta el contraste
            //Bitmap imagenBinaria = AplicarFiltroUmbral(imagenContraste, 128); // Umbral de 128 para binarización
            //picEntrada.Image = imagenContraste;


            // Crear una copia de la imagen original para trabajar sobre ella
            Bitmap editedImage = new Bitmap(picEntrada.Image);

            // Aumentar la exposición (brillo)
            float exposureFactor = 1.4f;  // 1.0 es sin cambio, >1.0 aumenta el brillo
            editedImage = Ajustar_Exposicion(editedImage, exposureFactor);


            picEntrada.Image = editedImage;

            //txtRespuestas.Text = "Imagen Gris: \n" + leer_imagen(PixConverter.ToPix(imagenGris));
            //txtRespuestas.Text += "\n \n \n \n";
            //txtRespuestas.Text += "Imagen Contraste: \n" + leer_imagen(PixConverter.ToPix(imagenContraste));
            //txtRespuestas.Text += "\n \n \n \n";
            //txtRespuestas.Text += "Imagen Binaria: \n" + leer_imagen(PixConverter.ToPix(imagenBinaria));

            txtSalida.Text = leer_imagen(PixConverter.ToPix(editedImage));
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {

        }


        private void cmdPrueba_Click(object sender, EventArgs e)
        {
            string filePath = @"D:\demo\Ejemplos\Examples\Birth\Approved\blanco\";

            //ExtractImagesFromPdf(filePath + "prueba.pdf", filePath);

            //lee el txt de los resultados
            string fileContent = File.ReadAllText(filePath + "values.txt");

            txtSalida.Text = leer_input(fileContent, obtener_imagen(filePath + "image.jpg"));

        }

        private string leer_input(string input, object archivo)
        {
            if (archivo is Image)
            {
                Image image = (Image)archivo;
                return leer_input_imagen(input, image);
            }
            else //Acá debería leer el pdf
            { return leer_input_pdf(input, archivo); }
        }

        private string leer_input_imagen(string input, Image foto_doc)
        {
            //Leer el archivo
            string texto = leer_imagen(PixConverter.ToPix(new Bitmap(foto_doc)));

            // Dividir el input en cada entrada usando los saltos de línea
            string[] inputLines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            rechazados = new List<string> { };

            input = "";

            input += resultados(inputLines, texto, false);

            int i;
            Bitmap foto_edit;
            for (i = 0; i < 4; i++)
            {
                if (rechazados.Count > 0)
                {
                    switch (i)
                    {
                        case 0:
                            foto_edit = Ajustar_Exposicion(new Bitmap(foto_doc), 1.4f);
                            texto = leer_imagen(PixConverter.ToPix(foto_edit));
                            input += resultados(rechazados.ToArray(), texto, false);
                            break;
                        default:
                            foto_edit = new Bitmap(rotar_imagen(foto_doc, i));
                            texto = leer_imagen(PixConverter.ToPix(foto_edit));
                            if (i == 3) { input += resultados(rechazados.ToArray(), texto, true); } else { input += resultados(rechazados.ToArray(), texto, false);  }
                            break;
                    }

                }
                else { i = 10; }
            }

            return input;
        }

        private string resultados(string[] inputLines, string texto, bool ultima)
        {
            rechazados = new List<string>();
            string input = "";
            int i;

            foreach (string line in inputLines)
            {
                // Asignar los valores capturados a las variables correspondientes
                string fieldName = line.Substring(0, line.IndexOf(":"));
                i = line.IndexOf(":") + 2;

                string operatorSign = line.Substring(i, line.IndexOf(" ", i) - i);
                i = line.IndexOf(" ", i + 1) + 1;

                string resultado = line.Substring(i, line.IndexOf(",", i) - i);
                i = line.IndexOf(",", i) + 2;

                string percentageValue = line.Substring(i, line.Length - i - 1);

                // Imprimir el resultado
                double match_percent = Matcheo(operatorSign, resultado, texto);
                if (operatorSign == "!=")
                {
                    if (match_percent >= Convert.ToDouble(percentageValue.Substring(0, percentageValue.Length - 1)))
                    {
                        if (ultima == false)
                        { rechazados.Add(line); }
                        else { input += $"{fieldName}: Rechazado {match_percent:0.00}%\n"; }
                    }
                    else { input += $"{fieldName}: Aprobado {match_percent:0.00}%\n"; }
                }
                else
                {
                    if (match_percent >= Convert.ToDouble(percentageValue.Substring(0, percentageValue.Length - 1)))
                    { input += $"{fieldName}: Aprobado {match_percent:0.00}%\n"; }
                    else
                    {
                        if (ultima == false)
                        { rechazados.Add(line); }
                        else { input += $"{fieldName}: Rechazado {match_percent:0.00}%\n"; }
                    }
                }
            }

            return input;
        }

        private string leer_input_pdf(string input, object archivo)
        {
            ////Leer el archivo
            //string texto;

            //if (archivo is Image)
            //{
            //    Image image = (Image)archivo;
            //    texto = leer_imagen(PixConverter.ToPix(new Bitmap(image)));
            //}
            //else //Acá debería leer el pdf
            //{ return ""; }

            //// Dividir el string en líneas usando saltos de línea
            //string[] inputLines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            //input = "";

            //int i = 0;

            //foreach (string line in inputLines)
            //{
            //    // Asignar los valores capturados a las variables correspondientes
            //    string fieldName = line.Substring(0, line.IndexOf(":"));
            //    i = line.IndexOf(":") + 2;

            //    string operatorSign = line.Substring(i, line.IndexOf(" ", i) - i);
            //    i = line.IndexOf(" ", i + 1);

            //    string resultado = line.Substring(i, line.IndexOf(",", i) - i);
            //    i = line.IndexOf(",", i) + 2;

            //    string percentageValue = line.Substring(i, line.Length - i - 1);

            //    // Imprimir el resultado
            //    Matcheo(operatorSign, resultado, percentageValue, texto);
            //}
            return input;
        }

        private double Matcheo(string operador, string resultado, string texto)
        {

            if (DateTime.TryParseExact(resultado, "MM/dd/yyyy", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
            {
                switch (operador)
                {
                    case "=":
                    case "!=":
                        return Buscar_matcheo_fechas(texto, resultado);
                    case ">=":
                    case "<=":
                    case "<":
                    case ">":
                        //Seccion en desarrollo
                        Console.WriteLine("Operador menor o igual");
                        break;
                }
                return 0;
            }
            else
            {
                if (resultado.IndexOf(" ") > -1 && resultado.Length > 1)
                { return Buscar_matcheo_varias_p(texto, resultado); }
                else
                { return Buscar_resultado(texto, resultado); }
            }
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
        private Image rotar_imagen(Image image, int rotaciones)
        {
            for (int i = 0; i < rotaciones; i++)
            { image.RotateFlip(RotateFlipType.Rotate90FlipNone); }

            return image;
        }

        private static Bitmap Ajustar_Exposicion(Bitmap originalImage, float exposureFactor)
        {
            // Crear una copia de la imagen original para modificarla
            Bitmap tempImage = new Bitmap(originalImage);

            for (int y = 0; y < tempImage.Height; y++)
            {
                for (int x = 0; x < tempImage.Width; x++)
                {
                    // Obtener el color del píxel
                    Color pixelColor = tempImage.GetPixel(x, y);

                    // Aumentar los valores de los canales RGB (Red, Green, Blue)
                    int r = (int)(pixelColor.R * exposureFactor);
                    int g = (int)(pixelColor.G * exposureFactor);
                    int b = (int)(pixelColor.B * exposureFactor);

                    // Asegurarse de que los valores no superen 255 (máximo valor para RGB)
                    r = Math.Min(255, Math.Max(0, r));
                    g = Math.Min(255, Math.Max(0, g));
                    b = Math.Min(255, Math.Max(0, b));

                    // Establecer el nuevo color del píxel
                    tempImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return tempImage;
        }
        private static Bitmap Ajustar_Brillo(Bitmap image, float factor)
        {
            Bitmap temp = (Bitmap)image.Clone();
            float brightness = factor - 1.0f;
            float[][] colorMatrixElements = {
            new float[] {1, 0, 0, 0, 0},
            new float[] {0, 1, 0, 0, 0},
            new float[] {0, 0, 1, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {brightness, brightness, brightness, 0, 1}
        };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            using (Graphics g = Graphics.FromImage(temp))
            {
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return temp;
        }

        private static Bitmap Ajustar_contraste(Bitmap image, float factor)
        {
            Bitmap temp = (Bitmap)image.Clone();
            float contrast = factor;
            float t = (1.0f - contrast) / 2.0f;

            float[][] colorMatrixElements = {
            new float[] {contrast, 0, 0, 0, t},
            new float[] {0, contrast, 0, 0, t},
            new float[] {0, 0, contrast, 0, t},
            new float[] {0, 0, 0, 1.0f, 0},
            new float[] {0, 0, 0, 0, 1.0f}
        };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            using (Graphics g = Graphics.FromImage(temp))
            {
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return temp;
        }

        private static Bitmap Ajustar_Sombras(Bitmap image, float shadowFactor)
        {
            Bitmap temp = (Bitmap)image.Clone();
            float shadowAdjustment = shadowFactor;

            using (Graphics g = Graphics.FromImage(temp))
            {
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Color pixelColor = image.GetPixel(x, y);
                        int r = (int)(pixelColor.R * shadowAdjustment);
                        int gColor = (int)(pixelColor.G * shadowAdjustment);
                        int b = (int)(pixelColor.B * shadowAdjustment);

                        // Limitar los valores a un rango válido
                        r = Math.Min(255, Math.Max(0, r));
                        gColor = Math.Min(255, Math.Max(0, gColor));
                        b = Math.Min(255, Math.Max(0, b));

                        temp.SetPixel(x, y, Color.FromArgb(r, gColor, b));
                    }
                }
            }
            return temp;
        }

        #endregion

        #region Binarizacion de imagen
        private Bitmap ConvertirAEscalaDeGrises(Bitmap imagenOriginal)
        {
            Bitmap imagenGris = new Bitmap(imagenOriginal.Width, imagenOriginal.Height);
            using (Graphics g = Graphics.FromImage(imagenGris))
            {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
            new float[] { 0.3f, 0.3f, 0.3f, 0, 0 },
            new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
            new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
            new float[] { 0, 0, 0, 1, 0 },
            new float[] { 0, 0, 0, 0, 1 }
                });
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(imagenOriginal, new Rectangle(0, 0, imagenOriginal.Width, imagenOriginal.Height), 0, 0, imagenOriginal.Width, imagenOriginal.Height, GraphicsUnit.Pixel, attributes);
            }
            return imagenGris;
        }

        private Bitmap AumentarContraste(Bitmap imagenGris, float factorContraste)
        {
            Bitmap imagenContraste = new Bitmap(imagenGris.Width, imagenGris.Height);
            using (Graphics g = Graphics.FromImage(imagenContraste))
            {
                float ajuste = 0.5f * (1.0f - factorContraste);
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
            new float[] { factorContraste, 0, 0, 0, 0 },
            new float[] { 0, factorContraste, 0, 0, 0 },
            new float[] { 0, 0, factorContraste, 0, 0 },
            new float[] { 0, 0, 0, 1, 0 },
            new float[] { ajuste, ajuste, ajuste, 0, 1 }
                });
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(imagenGris, new Rectangle(0, 0, imagenGris.Width, imagenGris.Height), 0, 0, imagenGris.Width, imagenGris.Height, GraphicsUnit.Pixel, attributes);
            }
            return imagenContraste;
        }

        private Bitmap AplicarFiltroUmbral(Bitmap imagenContraste, int umbral)
        {
            Bitmap imagenBinaria = new Bitmap(imagenContraste.Width, imagenContraste.Height);
            for (int x = 0; x < imagenContraste.Width; x++)
            {
                for (int y = 0; y < imagenContraste.Height; y++)
                {
                    Color color = imagenContraste.GetPixel(x, y);
                    int intensidad = (color.R + color.G + color.B) / 3;
                    if (intensidad > umbral)
                        imagenBinaria.SetPixel(x, y, Color.White);
                    else
                        imagenBinaria.SetPixel(x, y, Color.Black);
                }
            }
            return imagenBinaria;
        }

        #endregion

        #region Comparaciones

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

        private double Buscar_matcheo_varias_p(string texto, string valor)
        {
            //Hago un listado de palabras a buscar y otro de palabras en el texto

            List<string> list1 = new List<string>(valor.Split(' '));
            List<string> list2 = new List<string>(texto.Split(' '));

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
                mejor_match += list2[id_mas_prox] + " ";
                list2.Remove(list2[id_mas_prox]);

                mas_proxima = 0;
                id_mas_prox = 0;
            }

            mejor_match = mejor_match.Substring(0, mejor_match.Length - 1);

            distancia = CalcularDistanciaLevenshtein(valor, mejor_match);

            return CalcularSimilitud(valor, mejor_match, distancia) * 100;
        }

        private double Buscar_matcheo_fechas(string texto, string valor)
        {
            //Busco la fecha con el formato común y su variación al español
            if (Buscar_resultado(texto, valor) >= 80)
            { return Buscar_resultado(texto, valor); }
            else if (Buscar_resultado(texto, DateTime.ParseExact(valor, "MM/dd/yyyy", CultureInfo.GetCultureInfo("en-US")).ToString("dd/MM/yyyy")) >= 80)
            { return Buscar_resultado(texto, DateTime.ParseExact(valor, "MM/dd/yyyy", CultureInfo.GetCultureInfo("en-US")).ToString("dd/MM/yyyy")); }
            else
            {
                //Formateo la fecha a distintos formatos posibles
                valor = fecha_formateada(valor).ToLower();

                int formato_encontrado = 0;
                double valor_de_formato = 0;
                string candidato_encontrado = "";

                //Hago un listado de resultados a buscar y otro de posibles candidatos en el texto, luego me quedo con el más óptimo y devuelvo su distancia de levenshtein
                List<string> list1 = new List<string>(valor.Split(';'));

                for (int i = 0; i < list1.Count; i++)
                {
                    List<string> candidatos = new List<string>(BuscarPalabrasConContexto(texto, list1[i], 2));
                    foreach (string candidato in candidatos)
                    {
                        if (Buscar_matcheo_varias_p(candidato, list1[i]) > valor_de_formato)
                        {
                            valor_de_formato = Buscar_matcheo_varias_p(candidato, list1[i]);
                            formato_encontrado = i;
                            candidato_encontrado = candidato;
                        }
                    }
                }

                return Buscar_matcheo_varias_p(candidato_encontrado, list1[formato_encontrado]);
            }
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
        private double Buscar_resultado(string texto, string resultado)
        {
            // Leer todo el contenido del archivo y asignarlo a una variable string

            string palabra2 = texto.ToLower();
            string mejor_match = "";

            if (resultado.Length > 0 && palabra2.Length > 0)
            {
                //Separo las palabras encontradas por el OCR
                //Más Adelante la idea es que la devuelva la IA directo

                List<string> list2 = new List<string>(palabra2.Split(' '));

                double mas_proxima = 0;
                double nva_comparacion;
                int distancia;

                for (int j = 0; j < list2.Count; j++)
                {
                    distancia = CalcularDistanciaLevenshtein(resultado, list2[j]);
                    nva_comparacion = CalcularSimilitud(resultado, list2[j], distancia);
                    if (nva_comparacion > mas_proxima) { mas_proxima = nva_comparacion; mejor_match = list2[j]; }
                }

                distancia = CalcularDistanciaLevenshtein(resultado, mejor_match);

                return CalcularSimilitud(resultado, mejor_match, distancia) * 100;
            }
            else { return 0; }
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
        private string fecha_formateada(string entrada)
        {

            //mm = Mes, dd = día, aaaa = año, dm = nombre mes, snm = nombre mes corto, thh = sufijo ordinal(th)
            int dia;
            int mes;
            int año;
            string fechas = "mm/dd/aaaa;dd/mm/aaaa;dd mm aaaa;mm dd aaaa;dd de dm de aaaa;dd de dm de aaaa;dm dd aaaa;dd dm aaaa;thh day of dm aaaa;" +
                "dd snm aaaa;snm dd aaaa;dd snm / snm aaaa;";


            mes = Convert.ToInt32(entrada.Substring(0, entrada.IndexOf("/")));
            dia = Convert.ToInt32(entrada.Substring(entrada.IndexOf("/") + 1, entrada.LastIndexOf("/") - entrada.IndexOf("/") - 1));
            año = Convert.ToInt32(entrada.Substring(entrada.LastIndexOf("/") + 1));

            fechas = fechas.Replace("mm", mes.ToString());
            fechas = fechas.Replace("dd", dia.ToString());
            fechas = fechas.Replace("aaaa", año.ToString());
            fechas = fechas.Replace("dm", CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(mes));
            fechas = fechas.Replace("snm", CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(mes).Substring(0, 3));
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
        private Image obtener_imagen(string path)
        {
            return Image.FromFile(path);
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
