namespace PI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //filtrosfoto
        {
            filtrosImagen filtro = new filtrosImagen();
            this.Hide();
            filtro.Show();
        }

        private void button3_Click(object sender, EventArgs e) //camara
        {
            Camara camaraForm = new Camara();
            this.Hide();
            camaraForm.Show();
        }

        private void button4_Click(object sender, EventArgs e) //manuel
        {
            Manual_usuario manual = new Manual_usuario();
            this.Hide();
            manual.Show();
        }

        private void button2_Click(object sender, EventArgs e) //nomeacuerdo
        {
            fltrosVideo fltrosVideo = new fltrosVideo();
            this.Hide();
            fltrosVideo.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}