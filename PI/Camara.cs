using AForge.Video.DirectShow;
using AForge.Video;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PI
{
    public partial class Camara : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private CascadeClassifier faceCascade; // Clasificador para detección de rostros
        private bool detectFaces = false; // Estado de detección de rostros

        public Camara()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Camara_FormClosing);

            // Cargar el clasificador de rostros (asegúrate de que el archivo xml esté en tu directorio de proyecto)
            faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("No se encontraron cámaras.");
            }
        }

        public class DetectedFace
        {
            public Rectangle FaceRegion { get; set; }
            public int FaceID { get; set; }
            public Point Center { get; set; }  // Centro del rostro
            public int FrameCount { get; set; }  // Número de fotogramas en los que la cara ha sido detectada

            public DetectedFace(Rectangle faceRegion, int faceID)
            {
                this.FaceRegion = faceRegion;
                this.FaceID = faceID;
                this.Center = new Point(faceRegion.X + faceRegion.Width / 2, faceRegion.Y + faceRegion.Height / 2);
                this.FrameCount = 0;
            }
        }

        private List<DetectedFace> detectedFaces = new List<DetectedFace>();
        private int faceCount = 1;

        // Función para comparar dos rostros detectados y verificar si son similares
        private bool AreFacesSimilar(Point center1, Point center2, int positionTolerance = 40)
        {
            // Calcular la distancia entre los centros de los dos rostros
            double distance = Math.Sqrt(Math.Pow(center1.X - center2.X, 2) + Math.Pow(center1.Y - center2.Y, 2));

            // Si la distancia entre los centros es menor que la tolerancia, consideramos que son el mismo rostro
            return distance <= positionTolerance;
        }

        // Función que procesa cada fotograma de video
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            if (detectFaces)
            {
                // Convertir el frame de Bitmap a Image<Bgr, Byte> para usar con EmguCV
                Image<Bgr, Byte> imageFrame = frame.ToImage<Bgr, Byte>();

                // Detectar los rostros en el frame
                var faces = faceCascade.DetectMultiScale(imageFrame, 1.1, 10, new Size(20, 20));

                foreach (var face in faces)
                {
                    // Buscar si el rostro ya está registrado en la lista de caras detectadas
                    var existingFace = detectedFaces.FirstOrDefault(f => AreFacesSimilar(f.Center, new Point(face.X + face.Width / 2, face.Y + face.Height / 2)));

                    int faceID;
                    if (existingFace != null)
                    {
                        // Si ya existe, reutilizamos el ID existente
                        faceID = existingFace.FaceID;

                        // Actualizamos el centro de la cara en la lista para el seguimiento
                        existingFace.Center = new Point(face.X + face.Width / 2, face.Y + face.Height / 2);
                        existingFace.FrameCount = 0;
                    }
                    else
                    {
                        // Si no existe, asignamos un nuevo ID
                        faceID = faceCount++;

                        // Crear una nueva cara detectada con el constructor
                        DetectedFace newFace = new DetectedFace(face, faceID);

                        // Guardar la nueva cara detectada
                        detectedFaces.Add(newFace);
                    }

                    // Dibujar el rectángulo alrededor del rostro
                    CvInvoke.Rectangle(imageFrame, face, new Bgr(Color.Red).MCvScalar, 2);

                    // Dibujar el texto con el número de la cara
                    string faceText = "Persona " + faceID.ToString();
                    Point textPosition = new Point(face.X, face.Y - 20);

                    // Dibujar el texto en la imagen
                    CvInvoke.PutText(imageFrame, faceText, textPosition, FontFace.HersheySimplex, 1.0, new Bgr(Color.White).MCvScalar, 2);
                }

                // Limpiar rostros no detectados durante varios fotogramas
                detectedFaces.RemoveAll(f => f.FrameCount > 50);

                // Actualizar el contador de fotogramas para las caras detectadas
                foreach (var detectedFace in detectedFaces)
                {
                    detectedFace.FrameCount++;
                }

                frame = imageFrame.ToBitmap();
            }

            pictureBox1.Image = frame;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            detectFaces = !detectFaces;
            button2.Text = detectFaces ? "Desactivar detección de rostros" : "Activar detección de rostros";
        }

        private void button4_Click(object sender, EventArgs e) // Volver
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop(); // Detener la cámara
                videoSource = null;
            }

            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void Camara_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Cierra toda la aplicación
        }
    }
}
