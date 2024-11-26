namespace PI
{
    partial class filtrosImagen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            button1 = new Button();
            label1 = new Label();
            button2 = new Button();
            comboBox1 = new ComboBox();
            button3 = new Button();
            button4 = new Button();
            pictureBox2 = new PictureBox();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 47);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(774, 499);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(95, 23);
            button1.TabIndex = 1;
            button1.Text = "Cargar imagen";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(792, 76);
            label1.Name = "label1";
            label1.Size = new Size(69, 15);
            label1.TabIndex = 2;
            label1.Text = "Histograma";
            // 
            // button2
            // 
            button2.Location = new Point(308, 12);
            button2.Name = "button2";
            button2.Size = new Size(87, 23);
            button2.TabIndex = 6;
            button2.Text = "Aplicar filtro";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(113, 12);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(189, 23);
            comboBox1.TabIndex = 7;
            // 
            // button3
            // 
            button3.Location = new Point(401, 12);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 9;
            button3.Text = "Guardar imagen";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(482, 12);
            button4.Name = "button4";
            button4.Size = new Size(63, 23);
            button4.TabIndex = 10;
            button4.Text = "Volver";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(796, 94);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(150, 212);
            pictureBox2.TabIndex = 11;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // button5
            // 
            button5.Location = new Point(796, 312);
            button5.Name = "button5";
            button5.Size = new Size(150, 42);
            button5.TabIndex = 12;
            button5.Text = "Mostrar histograma";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // filtrosImagen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(958, 558);
            Controls.Add(button5);
            Controls.Add(pictureBox2);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(comboBox1);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "filtrosImagen";
            Text = "filtrosImagen";
            Load += filtrosImagen_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private Label label1;
        private Button button2;
        private ComboBox comboBox1;
        private Button button3;
        private Button button4;
        private PictureBox pictureBox2;
        private Button button5;
    }
}