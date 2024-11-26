using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PI
{
    public partial class Manual_usuario : Form
    {
        public Manual_usuario()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Manual_usuario_FormClosing);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form1 form1 = new Form1();
            this.Close();
            form1.Show();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            string manual = @"Esta aplicación permite aplicar filtros a imágenes y videos, activar la cámara para la detección de rostros, y guardar capturas de la cámara. Además, se pueden generar histogramas de colores para las imágenes procesadas.

                    Menú Principal
                    Desde el menú principal puedes acceder a las siguientes funcionalidades:

                    Filtros de Imagen
                    Filtros de Video
                    Cámara y Detección de Rostros
                    Manual de Usuario
                    1. Menú de Filtros de Imagen
                    Descripción General
                    Este menú te permite cargar una imagen, aplicar distintos filtros y generar un histograma RGB de la imagen con el filtro aplicado.

                    Botones y Funciones
                    Cargar Imagen: Abre el explorador de archivos para seleccionar una imagen desde tu disco.
                    Lista de Filtros: Desplegable que muestra la lista de filtros disponibles. Selecciona un filtro antes de aplicar.
                    Aplicar Filtro: Aplica el filtro seleccionado a la imagen cargada.
                    Guardar: Guarda la imagen con el filtro aplicado.
                    Volver: Regresa al menú principal.
                    Histograma: A la derecha, se muestran los niveles RGB de la imagen modificada.
                    Procedimiento para usar filtros de imagen
                    Haz clic en Cargar Imagen para seleccionar la imagen desde tu PC.
                    Elige un filtro desde el menú desplegable Lista de Filtros.
                    Presiona Aplicar Filtro para ver los resultados en la imagen cargada.
                    Si deseas guardar la imagen con el filtro aplicado, selecciona Guardar.
                    El histograma en la parte derecha te mostrará los cambios en los niveles de color.
                    2. Menú de Filtros de Video
                    Descripción General
                    Este menú permite cargar un video, aplicar filtros en tiempo real mientras el video se reproduce, y pausar/reanudar la reproducción del video.

                    Botones y Funciones
                    Cargar Video: Abre el explorador de archivos para seleccionar un video desde tu disco.
                    Lista de Filtros: Desplegable con la lista de filtros disponibles.
                    Aplicar Filtro: Aplica el filtro seleccionado al video.
                    Volver: Regresa al menú principal.
                    Play/Pause: Botón en la parte inferior que permite pausar o reanudar la reproducción del video.
                    Procedimiento para usar filtros en video
                    Haz clic en Cargar Video para seleccionar el video desde tu PC.
                    Elige un filtro desde el menú desplegable Lista de Filtros.
                    Presiona Aplicar Filtro para ver el filtro aplicado en el video.
                    Usa el botón Play/Pause para controlar la reproducción.
                    3. Menú de Cámara y Detección de Rostros
                    Descripción General
                    Este menú te permite activar la cámara de tu dispositivo, detectar rostros en tiempo real, y guardar capturas de la cámara.

                    Botones y Funciones
                    Activar Cámara: Activa la cámara de tu dispositivo y muestra el feed en tiempo real.
                    Detección de Rostro: Activa la funcionalidad de detección de rostros en el feed de la cámara.
                    Guardar Captura: Guarda una imagen del feed de la cámara con los rostros detectados.
                    Volver: Regresa al menú principal.
                    Procedimiento para usar la cámara y detección de rostros
                    Haz clic en Activar Cámara para mostrar el feed en tiempo real.
                    Presiona Detección de Rostro para enmarcar automáticamente los rostros detectados.
                    Si deseas guardar una captura con los rostros detectados, haz clic en Guardar Captura.
                    Usa el botón Volver para regresar al menú principal.
                    4. Manual de Usuario
                    Puedes acceder a este manual desde el menú principal. Aquí se explican todas las funcionalidades de la aplicación de manera detallada.

                    Notas Finales
                    Asegúrate de tener una cámara correctamente conectada y configurada en tu sistema antes de usar la opción de Cámara y Detección de Rostros.
                    Los filtros disponibles en las opciones de imagen y video permiten modificar los colores y características visuales, pero algunos filtros como blanco y negro, sepia y binarios están deshabilitados por las restricciones del proyecto.";

            labelManual.Text = manual;

        }
        private void Manual_usuario_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Cierra toda la aplicación
        }
    }
}
