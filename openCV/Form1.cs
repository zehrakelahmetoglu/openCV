using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;
using System;
using System.Windows.Forms;
namespace openCV
{
    public partial class Form1 : Form
    {

        // Global nesneler
        private VideoCapture capture;
        private CascadeClassifier faceClassifier;
        private Mat frame;
        private bool isCameraRunning = false;
        // Mevcut de�i�kenlerin yan�na ekleyin:
        private const string KayitKlasoru = "TekKullaniciYuzu";
        private FaceRecognizer recognizer;
        private bool isModelTrained = false;
        private int ornekSayaci = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBaslat_Click(object sender, EventArgs e)
        {
            if (!isCameraRunning)
            {
                // Y�z S�n�fland�r�c�s�n� (Classifier) Y�kle
                // Dosyan�n bin/Debug klas�r�nde oldu�undan emin olun!
                faceClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");

                // Kameray� ba�lat (0 genellikle varsay�lan web kameras�d�r)
                capture = new VideoCapture(0);

                if (!capture.IsOpened())
                {
                    MessageBox.Show("Kamera ba�lat�lamad�. Ba�ka bir uygulama kameray� kullan�yor olabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                frame = new Mat();
                isCameraRunning = true;
                btnBaslat.Text = "Kameray� Durdur";

                // Uygulama bo�ta oldu�unda (idle) video karelerini i�lemeye ba�la
                Application.Idle += FrameProcess;
            }
            else
            {
                // Kameray� durdur ve kaynaklar� serbest b�rak
                isCameraRunning = false;
                btnBaslat.Text = "Kameray� Ba�lat/Durdur";
                Application.Idle -= FrameProcess;
                capture.Release();
               
                pbKamera.Image = null;
            }
        }

        private void FrameProcess(object sender, EventArgs e)
        {
            if (capture.IsOpened())
            {
                // Kameradan bir kare (frame) oku
                capture.Read(frame);
                if (frame.Empty())
                    return;

                // 1. ��lem: Gri tonlamaya d�n��t�r (Y�z tespiti genellikle gri tonlamada daha h�zl� �al���r)
                Mat gray = new Mat();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                // 2. Y�zleri Tespit Et
                Rect[] faces = faceClassifier.DetectMultiScale(
                    gray,
                    scaleFactor: 1.1,
                    minNeighbors: 5,
                    flags: HaarDetectionTypes.ScaleImage,
                    minSize: new OpenCvSharp.Size(30, 30)
                );

                // 3. Tespit Edilen Y�zlerin Etraf�na Dikd�rtgen �iz
                foreach (Rect face in faces)
                {
                    // frame matrisinin �zerine 2 kal�nl���nda k�rm�z� bir dikd�rtgen �iz
                    Cv2.Rectangle(frame, face, Scalar.Red, 2);
                }

                // 4. Sonucu PictureBox'ta G�ster
                pbKamera.Image = frame.ToBitmap(); // Mat'i Bitmap'e d�n��t�r

                // Bellek y�netimi: Ge�ici Mat nesnesini serbest b�rak
                gray.Dispose();
            }
        }

        // Form kapat�ld���nda kaynaklar�n serbest b�rak�ld���ndan emin ol
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCameraRunning)
            {
                capture.Release();
                
            }
        }

        private void btnYuzuKaydet_Click(object sender, EventArgs e)
        {
            if (!isCameraRunning)
            {
                MessageBox.Show("L�tfen �nce Kameray� Ba�lat/Durdur butonuna basarak kameray� a��n.", "Uyar�");
                return;
            }

            // Klas�r� olu�tur (varsa siler ve yeniden olu�turur)
            if (System.IO.Directory.Exists(KayitKlasoru))
                System.IO.Directory.Delete(KayitKlasoru, true);

            System.IO.Directory.CreateDirectory(KayitKlasoru);

            // Ana d�ng�den ge�ici olarak veri toplama moduna ge�i�
            Application.Idle -= FrameProcess;
            Application.Idle += OrnekToplaVeKaydet;

            // Butonlar� devre d��� b�rak
            btnYuzuKaydet.Enabled = false;
            btnTanimaBaslat.Enabled = false;

            // Saya�lar� s�f�rla
            ornekSayaci = 0;
        }

        private void OrnekToplaVeKaydet(object sender, EventArgs e)
        {
            capture.Read(frame);
            if (frame.Empty()) return;

            // Hata d�zeltme: gray nesnesini burada tan�mlay�n!
            Mat gray = new Mat();

            // G�r�nt�y� gri tonlamaya �evir
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            // Y�z tespiti yap�l�yor...
            Rect[] faces = faceClassifier.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));

            // TEST SATIRI: Y�z bulunup bulunmad���n� kontrol edelim
            if (faces.Length == 0)
            {
                // Y�z bulunamazsa buraya girer
                lblDurum.Text = $"Y�Z TESP�T ED�LEMED�! Say�: {ornekSayaci} / 20";
            }

            // Y�z tespit edildiyse
            if (faces.Length > 0)
            {
                // ... [�rnek toplama kodlar� buraya girer ve saya� ilerler] ...
            }

            // G�r�nt�y� PictureBox'ta g�ster
            pbKamera.Image = frame.ToBitmap();

            // gray nesnesini metodun sonunda serbest b�rak�n
            gray.Dispose();
        }


        private void btnTanimaBaslat_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(KayitKlasoru) || System.IO.Directory.GetFiles(KayitKlasoru, "*.png").Length < 5)
            {
                MessageBox.Show("�nce yeterli say�da (min. 5) y�z �rne�i kaydetmelisiniz.", "Uyar�");
                return;
            }

            // Y�z tan�ma nesnesini olu�tur ve e�it
            recognizer = LBPHFaceRecognizer.Create();
            var goruntuler = new List<Mat>();
            var etiketler = new List<int>();

            // Kay�tl� t�m foto�raflar� y�kle ve etiketi (tek kullan�c� oldu�u i�in 1) ata
            foreach (string dosyaYolu in System.IO.Directory.GetFiles(KayitKlasoru, "*.png"))
            {
                Mat img = Cv2.ImRead(dosyaYolu, ImreadModes.Grayscale);
                if (!img.Empty())
                {
                    goruntuler.Add(img);
                    etiketler.Add(1); // Tek kullan�c� oldu�u i�in etiketi '1' yapt�k.
                }
            }

            // Modeli e�it
            recognizer.Train(goruntuler, etiketler.ToArray());
            isModelTrained = true;
            lblDurum.Text = "Model E�itildi! Canl� Tan�ma Ba�lad�.";

            // G�r�nt�leme modundan Tan�ma moduna ge�i�
            Application.Idle -= FrameProcess;
            Application.Idle += CanliTanimaProcess;
        }

        private void CanliTanimaProcess(object sender, EventArgs e)
        {
            capture.Read(frame);
            if (frame.Empty()) return;

            Mat gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
            Rect[] faces = faceClassifier.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));

            string sonucYazi = "Tan�nmad�";
            Scalar kutuRengi = Scalar.Red;

            // Hata veren de�i�kenleri burada, metodun ba��nda tan�ml�yoruz.
            double guvenirlik = 0.0; // Ba�lang�� de�eri 0.0
            int tahminEtiket = 0;   // Ba�lang�� de�eri 0

            if (faces.Length > 0 && isModelTrained)
            {
                Rect face = faces[0];
                Mat yuzTanima = new Mat(gray, face);
                Cv2.Resize(yuzTanima, yuzTanima, new OpenCvSharp.Size(100, 100));

                // Bu sat�rlar art�k hata vermeyecek ��nk� de�i�kenler tan�ml�:
                recognizer.Predict(yuzTanima, out tahminEtiket, out guvenirlik);

                // ... (Kalan Tan�ma Mant���)
                if (guvenirlik < 70.0)
                {
                    sonucYazi = "Tan�nd�: S�Z";
                    kutuRengi = Scalar.Blue;
                }

                // ... (PutText kodlar�)

                yuzTanima.Dispose();
            }

            pbKamera.Image = frame.ToBitmap();
            // �imdi bu sat�r �al��acak ��nk� guvenirlik tan�ml�:
            lblDurum.Text = $"Durum: {sonucYazi} (G: {guvenirlik:F2})";
            gray.Dispose();
        }

    }
}


