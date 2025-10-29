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
        // Mevcut deðiþkenlerin yanýna ekleyin:
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
                // Yüz Sýnýflandýrýcýsýný (Classifier) Yükle
                // Dosyanýn bin/Debug klasöründe olduðundan emin olun!
                faceClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");

                // Kamerayý baþlat (0 genellikle varsayýlan web kamerasýdýr)
                capture = new VideoCapture(0);

                if (!capture.IsOpened())
                {
                    MessageBox.Show("Kamera baþlatýlamadý. Baþka bir uygulama kamerayý kullanýyor olabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                frame = new Mat();
                isCameraRunning = true;
                btnBaslat.Text = "Kamerayý Durdur";

                // Uygulama boþta olduðunda (idle) video karelerini iþlemeye baþla
                Application.Idle += FrameProcess;
            }
            else
            {
                // Kamerayý durdur ve kaynaklarý serbest býrak
                isCameraRunning = false;
                btnBaslat.Text = "Kamerayý Baþlat/Durdur";
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

                // 1. Ýþlem: Gri tonlamaya dönüþtür (Yüz tespiti genellikle gri tonlamada daha hýzlý çalýþýr)
                Mat gray = new Mat();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                // 2. Yüzleri Tespit Et
                Rect[] faces = faceClassifier.DetectMultiScale(
                    gray,
                    scaleFactor: 1.1,
                    minNeighbors: 5,
                    flags: HaarDetectionTypes.ScaleImage,
                    minSize: new OpenCvSharp.Size(30, 30)
                );

                // 3. Tespit Edilen Yüzlerin Etrafýna Dikdörtgen Çiz
                foreach (Rect face in faces)
                {
                    // frame matrisinin üzerine 2 kalýnlýðýnda kýrmýzý bir dikdörtgen çiz
                    Cv2.Rectangle(frame, face, Scalar.Red, 2);
                }

                // 4. Sonucu PictureBox'ta Göster
                pbKamera.Image = frame.ToBitmap(); // Mat'i Bitmap'e dönüþtür

                // Bellek yönetimi: Geçici Mat nesnesini serbest býrak
                gray.Dispose();
            }
        }

        // Form kapatýldýðýnda kaynaklarýn serbest býrakýldýðýndan emin ol
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
                MessageBox.Show("Lütfen önce Kamerayý Baþlat/Durdur butonuna basarak kamerayý açýn.", "Uyarý");
                return;
            }

            // Klasörü oluþtur (varsa siler ve yeniden oluþturur)
            if (System.IO.Directory.Exists(KayitKlasoru))
                System.IO.Directory.Delete(KayitKlasoru, true);

            System.IO.Directory.CreateDirectory(KayitKlasoru);

            // Ana döngüden geçici olarak veri toplama moduna geçiþ
            Application.Idle -= FrameProcess;
            Application.Idle += OrnekToplaVeKaydet;

            // Butonlarý devre dýþý býrak
            btnYuzuKaydet.Enabled = false;
            btnTanimaBaslat.Enabled = false;

            // Sayaçlarý sýfýrla
            ornekSayaci = 0;
        }

        private void OrnekToplaVeKaydet(object sender, EventArgs e)
        {
            capture.Read(frame);
            if (frame.Empty()) return;

            // Hata düzeltme: gray nesnesini burada tanýmlayýn!
            Mat gray = new Mat();

            // Görüntüyü gri tonlamaya çevir
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            // Yüz tespiti yapýlýyor...
            Rect[] faces = faceClassifier.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));

            // TEST SATIRI: Yüz bulunup bulunmadýðýný kontrol edelim
            if (faces.Length == 0)
            {
                // Yüz bulunamazsa buraya girer
                lblDurum.Text = $"YÜZ TESPÝT EDÝLEMEDÝ! Sayý: {ornekSayaci} / 20";
            }

            // Yüz tespit edildiyse
            if (faces.Length > 0)
            {
                // ... [Örnek toplama kodlarý buraya girer ve sayaç ilerler] ...
            }

            // Görüntüyü PictureBox'ta göster
            pbKamera.Image = frame.ToBitmap();

            // gray nesnesini metodun sonunda serbest býrakýn
            gray.Dispose();
        }


        private void btnTanimaBaslat_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(KayitKlasoru) || System.IO.Directory.GetFiles(KayitKlasoru, "*.png").Length < 5)
            {
                MessageBox.Show("Önce yeterli sayýda (min. 5) yüz örneði kaydetmelisiniz.", "Uyarý");
                return;
            }

            // Yüz tanýma nesnesini oluþtur ve eðit
            recognizer = LBPHFaceRecognizer.Create();
            var goruntuler = new List<Mat>();
            var etiketler = new List<int>();

            // Kayýtlý tüm fotoðraflarý yükle ve etiketi (tek kullanýcý olduðu için 1) ata
            foreach (string dosyaYolu in System.IO.Directory.GetFiles(KayitKlasoru, "*.png"))
            {
                Mat img = Cv2.ImRead(dosyaYolu, ImreadModes.Grayscale);
                if (!img.Empty())
                {
                    goruntuler.Add(img);
                    etiketler.Add(1); // Tek kullanýcý olduðu için etiketi '1' yaptýk.
                }
            }

            // Modeli eðit
            recognizer.Train(goruntuler, etiketler.ToArray());
            isModelTrained = true;
            lblDurum.Text = "Model Eðitildi! Canlý Tanýma Baþladý.";

            // Görüntüleme modundan Tanýma moduna geçiþ
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

            string sonucYazi = "Tanýnmadý";
            Scalar kutuRengi = Scalar.Red;

            // Hata veren deðiþkenleri burada, metodun baþýnda tanýmlýyoruz.
            double guvenirlik = 0.0; // Baþlangýç deðeri 0.0
            int tahminEtiket = 0;   // Baþlangýç deðeri 0

            if (faces.Length > 0 && isModelTrained)
            {
                Rect face = faces[0];
                Mat yuzTanima = new Mat(gray, face);
                Cv2.Resize(yuzTanima, yuzTanima, new OpenCvSharp.Size(100, 100));

                // Bu satýrlar artýk hata vermeyecek çünkü deðiþkenler tanýmlý:
                recognizer.Predict(yuzTanima, out tahminEtiket, out guvenirlik);

                // ... (Kalan Tanýma Mantýðý)
                if (guvenirlik < 70.0)
                {
                    sonucYazi = "Tanýndý: SÝZ";
                    kutuRengi = Scalar.Blue;
                }

                // ... (PutText kodlarý)

                yuzTanima.Dispose();
            }

            pbKamera.Image = frame.ToBitmap();
            // Þimdi bu satýr çalýþacak çünkü guvenirlik tanýmlý:
            lblDurum.Text = $"Durum: {sonucYazi} (G: {guvenirlik:F2})";
            gray.Dispose();
        }

    }
}


