# 👁️ Canlı Yüz Tanıma & Güvenlik Sistemi (C# & OpenCvSharp)

![Görüntü İşleme Projesi Görseli: Ekranda yüzün etrafında tanıma kutusu gösteren bir uygulama arayüzü temsili görseli]

Hızlı ve güvenilir **Bilgisayarlı Görme (Computer Vision)** uygulamaları geliştirmek için tasarlanmış bu proje, C# ve güçlü **OpenCvSharp** kütüphanesini bir araya getirerek gerçek zamanlı, tek kullanıcılı bir yüz tanıma çözümü sunar. Erişim kontrolü, kişisel güvenlik sistemleri ve temel biyometrik doğrulama için ideal bir başlangıç noktasıdır.

**ÖNE ÇIKAN ÖZELLİKLER


| ⚡ **Gerçek Zamanlı İşleme** | Kameradan alınan video akışını milisaniyeler içinde işler. |
| 🎯 **Hassas Tespit** | Yüzün pozisyonunu ve sınırlarını doğru bir şekilde belirler (Haar Cascade). |
| 🧠 **Öğrenme Yeteneği** | LBPH algoritması ile kullanıcının yüz örneklerini öğrenir ve kaydeder. |
| 👤 **Tek Kullanıcı Doğrulama** | Eğitilmiş model üzerinden kullanıcının kimliğini doğrulayarak sonuç verir. |
| 🖥️ **Kullanıcı Dostu Arayüz** | Windows Forms (WinForms) tabanlı temiz ve basit bir arayüze sahiptir. |

**PROJEYİ BAŞLATMA REHBERİ

Bu projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları takip edin.

 1. Ortam ve Gereksinimler

* **IDE:** Visual Studio 2019 veya üzeri
* **Platform:** C# (.NET Framework veya .NET Core)
* **Web Kamerası:** Çalışır durumda bir kamera cihazı.

2. Bağımlılıkları Yükleme

Proje, tüm gereksinimlerini **NuGet Paket Yöneticisi** üzerinden otomatik olarak çeker:

* `OpenCvSharp4`
* `OpenCvSharp4.runtime.win`
* `OpenCvSharp4.Extensions`

 3. Kritik Model Dosyası (XML)

Yüz tespiti modelini programa tanıtmanız gerekir:

1.  OpenCV'nin reposundan **`haarcascade_frontalface_default.xml`** dosyasını indirin.
2.  İndirdiğiniz bu dosyayı, projenin çalıştırılabilir dosyasının yanına (**`bin/Debug/`** klasörü) **kopyalayın**.

 4. Kullanım Adımları

Uygulamayı çalıştırdıktan sonra (F5):

1.  **Kamerayı Başlatın:** "Kamerayı Başlat/Durdur" butonuna tıklayın.
2.  **Yüzünüzü Kaydedin:** Yüzünüz kameradayken **"Yüzümü Kaydet"** butonuna basın. Uygulama, sayaç 20'ye ulaşana kadar yüzünüzün farklı açılarını kaydedecektir.
3.  **Modeli Eğitin:** Kayıt tamamlandığında, **"Tanımayı Başlat"** butonuna tıklayın. Model eğitilecek ve anında tanıma moduna geçecektir.

**PROJE YAPISI

| `Form1.cs` | Ana C# Form kodu. Tüm kamera, işleme ve UI mantığını içerir. |
| `haarcascade_frontalface_default.xml` | Yüz tespiti için kullanılan önceden eğitilmiş model. |
| `TekKullaniciYuzu/` | **(Çalışma Zamanı Oluşturulur)** Toplanan 20 adet yüz fotoğrafının saklandığı klasör. |
| `face_model.yaml` | **(Eğitim Sonrası Oluşur)** Eğitilmiş yüz tanıma modelinin kaydedildiği dosya. |
