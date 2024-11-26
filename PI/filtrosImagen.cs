using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PI
{
    public partial class filtrosImagen : Form
    {
        private Image<Bgr, Byte> currentFrame;

        public filtrosImagen()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(filtrosImagen_FormClosing);

            // Agregar filtros al ComboBox

            comboBox1.Items.Add("Mapa de calor");
            comboBox1.Items.Add("Inversión de colores");
            comboBox1.Items.Add("Filtro Gaussiano");
            comboBox1.Items.Add("Filtro viñeta");
            comboBox1.Items.Add("Ajuste de brillo");
            comboBox1.Items.Add("Ajuste de contraste");
            comboBox1.Items.Add("Efecto de ruido");
            comboBox1.Items.Add("Filtro de tono");
            comboBox1.Items.Add("Filtro de detección de esquinas");
            comboBox1.Items.Add("Filtro de realce de detalles");
            comboBox1.SelectedIndex = 0;  // Establece un filtro por defecto
        }

        private void filtrosImagen_Load(object sender, EventArgs e)
        {
        }

        private void filtrosImagen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Cierra toda la aplicación
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e) // Al hacer clic en el botón
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedFilter = comboBox1.SelectedItem.ToString();
                ApplyFilter(selectedFilter);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un filtro.");
            }
        }

        private void ApplyFilter(string filter)
        {
            if (currentFrame == null)
            {
                MessageBox.Show("No se ha cargado ninguna imagen.");
                return;
            }

            // Aplicar el filtro seleccionado
            switch (filter)
            {
                case "Inversión de colores":
                    InvertColors(currentFrame);
                    break;

                case "Filtro Gaussiano":
                    GaussianBlur(currentFrame);
                    break;

                case "Filtro viñeta":
                    ApplyVignetteEffect(currentFrame);
                    break;

                case "Ajuste de brillo":
                    AdjustBrightness(currentFrame);
                    break;

                case "Ajuste de contraste":
                    AdjustContrast(currentFrame);
                    break;

                case "Efecto de ruido":
                    AddNoiseEffect(currentFrame);
                    break;

                case "Filtro de tono":
                    ApplyTint(currentFrame);
                    break;

                case "Filtro de detección de esquinas":
                    CornerDetection(currentFrame);
                    break;

                case "Filtro de realce de detalles":
                    SharpeningFilter(currentFrame);
                    break;

                case "Mapa de calor":
                    ApplyHeatMapEffect(currentFrame);
                    break;

                default:
                    MessageBox.Show("Filtro desconocido.");
                    return;
            }

            // Actualizar la imagen del PictureBox después de aplicar el filtro
            pictureBox1.Image = currentFrame.ToBitmap();
        }

        #region Métodos de filtros

        private void ApplyHeatMapEffect(Image<Bgr, byte> image)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Bgr pixel = image[y, x];
                    byte intensity = (byte)((pixel.Blue + pixel.Green + pixel.Red) / 3);

                    // Mapear el valor de intensidad a un color en un mapa de calor
                    byte r = (byte)Math.Min(255, (int)intensity * 2); // Rojo
                    byte g = (byte)Math.Min(255, (int)intensity); // Verde
                    byte b = (byte)Math.Min(255, 255 - (int)intensity); // Azul

                    image[y, x] = new Bgr(b, g, r); // Invertir la asignación para mapear los colores.
                }
            }
            pictureBox1.Image = image.ToBitmap();
        }

        // Inversión de colores
        private void InvertColors(Image<Bgr, Byte> image)
        {
            image._Not(); // Inversión de la imagen
            pictureBox1.Image = image.ToBitmap();
        }

        // Filtro Gaussiano
        private void GaussianBlur(Image<Bgr, byte> image)
        {
            CvInvoke.GaussianBlur(image, image, new Size(15, 15), 0);
            pictureBox1.Image = image.ToBitmap();
        }

        //Filtro viñeteo
        private void ApplyVignetteEffect(Image<Bgr, byte> image)
        {
            int width = image.Width;
            int height = image.Height;
            double cx = width / 2.0;
            double cy = height / 2.0;
            double maxDist = Math.Sqrt(cx * cx + cy * cy);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double dist = Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                    double factor = 1 - (dist / maxDist);
                    Bgr pixel = image[y, x];
                    pixel = new Bgr(pixel.Blue * factor, pixel.Green * factor, pixel.Red * factor);
                    image[y, x] = pixel;
                }
            }

            pictureBox1.Image = image.ToBitmap();
        }

        private void AdjustBrightness(Image<Bgr, Byte> image)
        {
            image._GammaCorrect(1.5); // Aumentar brillo
            pictureBox1.Image = image.ToBitmap();
        }

        // Ajuste de contraste
        private void AdjustContrast(Image<Bgr, Byte> image)
        {
            image._GammaCorrect(0.8); // Ajustar contraste
            pictureBox1.Image = image.ToBitmap();
        }

        private void AddNoiseEffect(Image<Bgr, byte> image)
        {
            Random rand = new Random();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Bgr pixel = image[y, x];
                    byte noise = (byte)rand.Next(0, 50); // Ajustar el rango de ruido
                    pixel.Blue = (byte)Math.Min(255, pixel.Blue + noise);
                    pixel.Green = (byte)Math.Min(255, pixel.Green + noise);
                    pixel.Red = (byte)Math.Min(255, pixel.Red + noise);
                    image[y, x] = pixel;
                }
            }

            pictureBox1.Image = image.ToBitmap();
        }
        // Filtro de tono (Tint)
        private void ApplyTint(Image<Bgr, byte> image)
        {
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Cols; j++)
                {
                    Bgr color = image[i, j];
                    color.Blue = (byte)Math.Min(color.Blue + 50, 255); // Ajusta estos valores para tono deseado
                    color.Red = (byte)Math.Max(color.Red - 20, 0);
                    image[i, j] = color;
                }
            }
            pictureBox1.Image = image.ToBitmap();
        }

        // Filtro de detección de esquinas
        private void CornerDetection(Image<Bgr, byte> image)
        {
            // Convertir a escala de grises
            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();

            // Aplicar la detección de esquinas de Harris
            Mat corners = new Mat();
            CvInvoke.CornerHarris(grayImage, corners, 2, 3, 0.04);

            // Normalizar el resultado para hacerlo visible
            CvInvoke.Normalize(corners, corners, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);
            Image<Gray, byte> cornersByte = corners.ToImage<Gray, byte>();

            // Dibujar círculos en las esquinas detectadas
            for (int i = 0; i < cornersByte.Width; i++)
            {
                for (int j = 0; j < cornersByte.Height; j++)
                {
                    if (cornersByte[j, i].Intensity > 150) // Umbral de intensidad
                    {
                        CvInvoke.Circle(image, new Point(i, j), 3, new MCvScalar(0, 0, 255), 1); // Marca las esquinas en rojo
                    }
                }
            }

            // Mostrar el resultado en PictureBox
            pictureBox1.Image = image.ToBitmap();
        }

        // Filtro de realce de detalles (Sharpening)
        private void SharpeningFilter(Image<Bgr, byte> image)
        {
            float[,] kernel = {
                { 0, -1, 0 },
                { -1, 5, -1 },
                { 0, -1, 0 }
            };

            ConvolutionKernelF convolutionKernel = new ConvolutionKernelF(kernel);
            CvInvoke.Filter2D(image, image, convolutionKernel, new Point(-1, -1));

            pictureBox1.Image = image.ToBitmap();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            // Cargar la imagen desde un archivo
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFrame = new Image<Bgr, byte>(openFileDialog.FileName); // Cargar la imagen
                pictureBox1.Image = currentFrame.ToBitmap(); // Convertirla a Bitmap y mostrarla en el PictureBox

                // Ajustar el tamaño de la imagen en el PictureBox
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // O cualquier otra opción que prefieras
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentFrame == null)
            {
                MessageBox.Show("No se ha cargado ninguna imagen.");
                return;
            }

            // Crear un cuadro de diálogo para que el usuario seleccione la ubicación y nombre del archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos de imagen (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";
            saveFileDialog.Title = "Guardar Imagen";

            // Si el usuario elige una ubicación y nombre de archivo
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Guardar la imagen con el filtro aplicado en la ubicación seleccionada
                    currentFrame.ToBitmap().Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png); // Puedes cambiar el formato a JPG, BMP, etc.
                    MessageBox.Show("La imagen se ha guardado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar la imagen: {ex.Message}");
                }
            }
        }

        private void histogramBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void CalcularHistograma()
        {
            if (currentFrame == null)
            {
                MessageBox.Show("No se ha cargado ninguna imagen.");
                return;
            }

            // Crear histogramas para los tres canales
            DenseHistogram histogramaRojo = new DenseHistogram(256, new RangeF(0, 256));
            DenseHistogram histogramaVerde = new DenseHistogram(256, new RangeF(0, 256));
            DenseHistogram histogramaAzul = new DenseHistogram(256, new RangeF(0, 256));

            // Separar canales
            Image<Gray, byte>[] canales = currentFrame.Split();
            histogramaRojo.Calculate(new Image<Gray, byte>[] { canales[2] }, false, null); // Canal rojo
            histogramaVerde.Calculate(new Image<Gray, byte>[] { canales[1] }, false, null); // Canal verde
            histogramaAzul.Calculate(new Image<Gray, byte>[] { canales[0] }, false, null); // Canal azul

            // Crear una imagen para dibujar el histograma
            Bitmap histImage = new Bitmap(256, 400);
            using (Graphics g = Graphics.FromImage(histImage))
            {
                g.Clear(Color.White);

                // Escalar los valores del histograma para ajustarse al tamaño de la imagen
                float maxValor = Math.Max(Math.Max(histogramaRojo.GetBinValues().Max(), histogramaVerde.GetBinValues().Max()), histogramaAzul.GetBinValues().Max());
                float escala = 400 / maxValor;

                // Dibujar cada canal
                DibujarHistograma(g, histogramaRojo, escala, Pens.Red);
                DibujarHistograma(g, histogramaVerde, escala, Pens.Green);
                DibujarHistograma(g, histogramaAzul, escala, Pens.Blue);
            }

            // Mostrar el histograma en el PictureBox
            pictureBox2.Image = histImage;
        }

        private void DibujarHistograma(Graphics g, DenseHistogram histograma, float escala, Pen color)
        {
            float[] valores = histograma.GetBinValues();

            for (int i = 0; i < valores.Length; i++)
            {
                float valor = valores[i] * escala; // Escalar el valor del histograma
                g.DrawLine(color, i, 400, i, 400 - valor);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CalcularHistograma();

            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
