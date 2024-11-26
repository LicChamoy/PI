using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Threading;
using AForge.Imaging;

namespace PI
{
    public partial class fltrosVideo : Form
    {
        private VideoCapture capture;
        private bool isPlaying = false;
        private bool isPaused = false;
        private Mat frame;
        private Image<Bgr, byte> currentFrame;
        private Thread videoThread;

        public fltrosVideo()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(fltrosVideo_FormClosing);

            // Agregar filtros al ComboBox
            comboBox1.Items.Add("Lista de filtros");
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
        private void fltrosVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            isPlaying = false; // Detener reproducción
            if (capture != null)
            {
                capture.Dispose();
            }
            if (videoThread != null && videoThread.IsAlive)
            {
                videoThread.Join(); // Esperar a que termine el hilo
            }
        }
        // Botón para cargar el video

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentFrame != null)
            {
                string selectedFilter = comboBox1.SelectedItem?.ToString();
                ApplyFilter(selectedFilter);

                // Actualizar pictureBox para mostrar el fotograma filtrado
                pictureBox1.Image = currentFrame.ToBitmap();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de video (*.mp4;*.avi;*.mkv)|*.mp4;*.avi;*.mkv";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Cerrar y liberar el recurso de captura si ya estaba abierto
                if (capture != null)
                {
                    capture.Dispose();
                }

                // Inicializar captura con el archivo seleccionado
                capture = new VideoCapture(openFileDialog.FileName);

                // Verificar si el video se cargó correctamente
                if (capture.IsOpened)
                {
                    isPlaying = true;
                    videoThread = new Thread(new ThreadStart(PlayVideo)); // Crear hilo para reproducción
                    videoThread.Start();
                }
                else
                {
                    MessageBox.Show("No se pudo abrir el archivo de video.");
                }
            }
        }
        private void PlayVideo()
        {
            while (isPlaying && capture != null && capture.IsOpened)
            {
                if (!isPaused)
                {
                    frame = capture.QueryFrame(); // Capturar el siguiente fotograma

                    if (frame == null)
                    {
                        // Si llegamos al final, reiniciamos al primer fotograma
                        capture.Set(CapProp.PosFrames, 0);
                        frame = capture.QueryFrame();
                        if (frame == null) break; // Si no se puede obtener el fotograma, salimos
                    }

                    currentFrame = frame.ToImage<Bgr, byte>(); // Convertir a Image<Bgr, byte>

                    // Usar Invoke para acceder al comboBox de forma segura desde otro hilo
                    string selectedFilter = string.Empty;
                    comboBox1.Invoke((MethodInvoker)delegate
                    {
                        selectedFilter = comboBox1.SelectedItem?.ToString();
                    });

                    // Aplicar el filtro al fotograma actual
                    ApplyFilter(selectedFilter);

                    // Actualizar pictureBox en el hilo de la UI
                    pictureBox1.Invoke((MethodInvoker)delegate
                    {
                        pictureBox1.Image = currentFrame.ToBitmap();

                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    });

                    CalcularHistograma();

                    // Esperar 30 ms para controlar la velocidad del video (aproximadamente 30 FPS)
                    Thread.Sleep(30);
                }
            }
        }
        // Método para aplicar el filtro al fotograma actual
        private void ApplyFilter(string filter)
        {
            if (currentFrame == null)
            {
                return;
            }

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
                    ApplyHeatmapEffect(currentFrame);
                    break;

                default:
                    break;
            }
        }

        // Pausar o reanudar el video
        private void button3_Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                isPaused = !isPaused;
                button3.Text = isPaused ? "Reanudar" : "Pausar";
            }
        }

        // Filtro de mapa de calor
        private void ApplyHeatmapEffect(Image<Bgr, byte> image)
        {
            // Convertimos la imagen original a escala de grises para mapear la intensidad
            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();

            // Normalizar la imagen para que los valores estén en el rango [0, 255]
            Image<Gray, byte> normalizedImage = new Image<Gray, byte>(image.Width, image.Height);
            CvInvoke.Normalize(grayImage, normalizedImage, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);

            // Aplicar la paleta de colores para generar el mapa de calor
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    // Convertir explicitamente a byte el valor de intensidad
                    byte intensity = (byte)normalizedImage[y, x].Intensity;

                    // Mapear la intensidad de píxel a un color en el mapa de calor
                    Bgr color = GetHeatmapColor(intensity);
                    image[y, x] = color;
                }
            }
        }

        // Función para obtener el color basado en la intensidad (esto simula un mapa de calor)
        private Bgr GetHeatmapColor(byte intensity)
        {
            // Escala de colores: Azul (bajo), Verde, Amarillo, Rojo (alto)
            if (intensity < 64)
            {
                // Asegúrate de hacer un cast a byte
                return new Bgr(0, (byte)(intensity * 4), (byte)255); // Azul -> Verde
            }
            else if (intensity < 128)
            {
                // Asegúrate de hacer un cast a byte
                return new Bgr(0, (byte)255, (byte)(255 - intensity * 2)); // Verde -> Amarillo
            }
            else if (intensity < 192)
            {
                // Asegúrate de hacer un cast a byte
                return new Bgr((byte)255, (byte)(255 - (intensity - 128) * 4), (byte)0); // Amarillo -> Rojo
            }
            else
            {
                // Asegúrate de hacer un cast a byte
                return new Bgr((byte)255, (byte)0, (byte)0); // Rojo
            }
        }

        // Aplicar la inversión de colores al fotograma
        private void InvertColors(Image<Bgr, byte> image)
        {
            image._Not(); // Inversión de la imagen
        }

        // Filtro Gaussiano
        private void GaussianBlur(Image<Bgr, byte> image)
        {
            CvInvoke.GaussianBlur(image, image, new Size(15, 15), 0);
        }

        // Filtro viñeta
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
        }

        // Ajuste de brillo
        private void AdjustBrightness(Image<Bgr, byte> image)
        {
            image._GammaCorrect(1.5); // Aumentar brillo
        }

        // Ajuste de contraste
        private void AdjustContrast(Image<Bgr, byte> image)
        {
            image._GammaCorrect(0.8); // Ajustar contraste
        }

        // Efecto de ruido
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
        }

        // Filtro de tono
        private void ApplyTint(Image<Bgr, byte> image)
        {
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Cols; j++)
                {
                    Bgr color = image[i, j];
                    color.Blue = (byte)Math.Min(color.Blue + 50, 255);
                    color.Red = (byte)Math.Max(color.Red - 20, 0);
                    image[i, j] = color;
                }
            }
        }

        // Detección de esquinas
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
                    if (cornersByte[j, i].Intensity > 150)
                    {
                        CvInvoke.Circle(image, new Point(i, j), 3, new MCvScalar(0, 0, 255), 1);
                    }
                }
            }
        }

        // Filtro de realce de detalles
        private void SharpeningFilter(Image<Bgr, byte> image)
        {
            float[,] kernel = {
                { 0, -1, 0 },
                { -1, 5, -1 },
                { 0, -1, 0 }
            };

            ConvolutionKernelF convolutionKernel = new ConvolutionKernelF(kernel);
            CvInvoke.Filter2D(image, image, convolutionKernel, new Point(-1, -1));
        }

        // Aplicar filtro cuando se selecciona una opción en el ComboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (capture != null)
            {
                ApplyFilter(comboBox1.SelectedItem?.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
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
            //CalcularHistograma();

            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
