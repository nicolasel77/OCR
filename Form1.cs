using System;
using System.Windows.Forms;
using System.IO;
using Tesseract;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

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
                //Get the path of specified file
                lblImagen.Text = opnArchivo.FileName;
                picEntrada.ImageLocation = lblImagen.Text;

                var engine = new TesseractEngine(@"D:\tessdata", "eng");
                var image = Pix.LoadFromFile(lblImagen.Text);
                var page = engine.Process(image);

                var text = page.GetText();

                txtSalida.Text = text;
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
                    string b = txtBuscar.Text;

                    int t = txtSalida.Text.IndexOf(b);
                    if (t > -1)
                    {
                        Regex regExp = new Regex($"({b})");

                        foreach (Match match in regExp.Matches(txtSalida.Text))
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
    }
}
