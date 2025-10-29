namespace openCV
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnBaslat = new Button();
            pbKamera = new PictureBox();
            btnYuzuKaydet = new Button();
            btnTanimaBaslat = new Button();
            lblDurum = new Label();
            ((System.ComponentModel.ISupportInitialize)pbKamera).BeginInit();
            SuspendLayout();
            // 
            // btnBaslat
            // 
            btnBaslat.Location = new Point(562, 137);
            btnBaslat.Name = "btnBaslat";
            btnBaslat.Size = new Size(215, 29);
            btnBaslat.TabIndex = 0;
            btnBaslat.Text = "Kamerayı Başlat/Durdur";
            btnBaslat.UseVisualStyleBackColor = true;
            btnBaslat.Click += btnBaslat_Click;
            // 
            // pbKamera
            // 
            pbKamera.Location = new Point(118, 30);
            pbKamera.Name = "pbKamera";
            pbKamera.Size = new Size(348, 271);
            pbKamera.SizeMode = PictureBoxSizeMode.StretchImage;
            pbKamera.TabIndex = 1;
            pbKamera.TabStop = false;
            // 
            // btnYuzuKaydet
            // 
            btnYuzuKaydet.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnYuzuKaydet.Location = new Point(572, 272);
            btnYuzuKaydet.Name = "btnYuzuKaydet";
            btnYuzuKaydet.Size = new Size(187, 29);
            btnYuzuKaydet.TabIndex = 2;
            btnYuzuKaydet.Text = "Yüzümü Kaydet";
            btnYuzuKaydet.UseVisualStyleBackColor = true;
            btnYuzuKaydet.Click += btnYuzuKaydet_Click;
            // 
            // btnTanimaBaslat
            // 
            btnTanimaBaslat.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnTanimaBaslat.Location = new Point(572, 329);
            btnTanimaBaslat.Name = "btnTanimaBaslat";
            btnTanimaBaslat.Size = new Size(187, 29);
            btnTanimaBaslat.TabIndex = 3;
            btnTanimaBaslat.Text = "Tanımayı Başlat";
            btnTanimaBaslat.UseVisualStyleBackColor = true;
            btnTanimaBaslat.Click += btnTanimaBaslat_Click;
            // 
            // lblDurum
            // 
            lblDurum.AutoSize = true;
            lblDurum.Location = new Point(118, 410);
            lblDurum.Name = "lblDurum";
            lblDurum.Size = new Size(139, 20);
            lblDurum.TabIndex = 4;
            lblDurum.Text = "Durum:Başlatılmadı";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Bisque;
            ClientSize = new Size(800, 450);
            Controls.Add(lblDurum);
            Controls.Add(btnTanimaBaslat);
            Controls.Add(btnYuzuKaydet);
            Controls.Add(pbKamera);
            Controls.Add(btnBaslat);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pbKamera).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBaslat;
        private PictureBox pbKamera;
        private Button btnYuzuKaydet;
        private Button btnTanimaBaslat;
        private Label lblDurum;
    }
}
