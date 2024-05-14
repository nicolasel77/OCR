﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tesseract;
using System.Drawing.Imaging;

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
            opnArchivo.InitialDirectory = "c:\\";
            opnArchivo.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            opnArchivo.FilterIndex = 2;
            opnArchivo.RestoreDirectory = true;

            if (opnArchivo.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                //Get the path of specified file
                lblImagen.Text = opnArchivo.FileName;
                picEntrada.ImageLocation = lblImagen.Text;

                var engine = new TesseractEngine(@"D:\tessdata", "eng");
                var image = Pix.LoadFromFile(lblImagen.Text);
                var page = engine.Process(image);

                var text = page.GetText();

                txtSalida.Text = text;
                Cursor.Current = Cursors.Default;
            }

        }

        private void cmdBuscar_Click(object sender, EventArgs e)
        {
            pintar();
        }

        private void pintar()
        {
            txtSalida.SelectAll();
            txtSalida.SelectionColor = Color.Black;
            txtSalida.DeselectAll();

            if (txtSalida.Text.Length > 0)
            {
                if (txtBuscar.Text.Length > 0)
                {
                    string b = txtBuscar.Text.ToLower();
                    string b2 = txtSalida.Text.ToLower();
                    int t = b2.IndexOf(b);
                    if (t > -1)
                    {
                        Regex regExp = new Regex($"({b})");

                        foreach (Match match in regExp.Matches(b2))
                        {
                            txtSalida.Select(match.Index, match.Length);
                            txtSalida.SelectionColor = Color.Blue;
                        }
                    }
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            pintar();
        }

        private void cmdComparar_Click(object sender, EventArgs e)
        {

            string palabra1 = txtBuscar.Text.ToLower();
            string palabra2 = txtSalida.Text.ToLower();

            palabra2 = palabra2.Replace("\n", " ");

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

                distancia = CalcularDistanciaLevenshtein(txtBuscar.Text.ToLower(), mejor_match);
                similitud = CalcularSimilitud(txtBuscar.Text, mejor_match, distancia);
                label3.Text = mejor_match;

                lblSimilitud.Text = $"La similitud es del: {similitud * 100:0.00}%";
            } else { lblSimilitud.Text = "No hay texto para comparar"; }
        }

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
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
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

        private void cmdDesaturar_Click(object sender, EventArgs e)
        {
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
            if (newValue > 255) { newValue = 255;}

            return (byte)newValue;
        }
    }
}
