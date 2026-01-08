<div align="center">

  <img src="https://cdn-icons-png.flaticon.com/512/3063/3063176.png" alt="logo" width="100" height="100" />
  
  <h1>🏥 HealthBigData AI Analytics</h1>
  
  <p>
    <b>.NET 9</b> ve <b>Google Gemini AI</b> destekli, 2 Milyon satırlık sağlık verisi üzerinde çalışan yeni nesil Karar Destek Sistemi.
  </p>

  <p>
    <a href="#">
      <img src="https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet" alt=".NET 9">
    </a>
    <a href="#">
      <img src="https://img.shields.io/badge/Language-C%23-blue?style=for-the-badge&logo=csharp" alt="C#">
    </a>
    <a href="#">
      <img src="https://img.shields.io/badge/Data-2%20Million%20Records-red?style=for-the-badge&logo=microsoftsqlserver" alt="Big Data">
    </a>
    <a href="#">
      <img src="https://img.shields.io/badge/AI-Google%20Gemini-orange?style=for-the-badge&logo=google" alt="Gemini AI">
    </a>
    <a href="#">
      <img src="https://img.shields.io/badge/ORM-Dapper-green?style=for-the-badge&logo=nuget" alt="Dapper">
    </a>
  </p>

  <p>
    <a href="#-özellikler">🚀 Özellikler</a> •
    <a href="#-teknoloji-yığını">🛠️ Teknolojiler</a> •
    <a href="#-kurulum">⚙️ Kurulum</a> •
    <a href="#-ekran-görüntüleri">📸 Görseller</a>
  </p>
</div>

<br />

> **💡 Önemli:** Bu proje, **gerçekçi senaryolarla üretilmiş 2.000.000 adet sentetik sağlık verisi** üzerinde çalışır. Performans optimizasyonu için EF Core yerine **Dapper** kullanılmıştır.

---

## 🚀 Özellikler

Bu dashboard sadece veri göstermez, veriyi **yorumlar**.

<table>
  <tr>
    <td width="50%">
      <h3>🌍 İnteraktif Türkiye Haritası</h3>
      <ul>
        <li>SVG tabanlı dinamik harita entegrasyonu.</li>
        <li>Şehir bazlı anlık veri filtreleme.</li>
        <li>Seçilen şehre özel demografik ve klinik analizler.</li>
      </ul>
    </td>
    <td width="50%">
      <h3>🧠 Yapay Zeka (Gemini) Analizi</h3>
      <ul>
        <li>Dashboard'daki grafik verilerini metne dönüştürür.</li>
        <li><b>Google Gemini API</b>'ye gönderir.</li>
        <li>Şehre özel sağlık stratejileri ve önlem raporları sunar.</li>
      </ul>
    </td>
  </tr>
  <tr>
    <td width="50%">
      <h3>⚡ Yüksek Performanslı Veri</h3>
      <ul>
        <li><b>.NET 9</b> altyapısı.</li>
        <li><b>Dapper ORM</b> ile milisaniyelik sorgu yanıtları.</li>
        <li>2 Milyon satır içinde anlık filtreleme.</li>
      </ul>
    </td>
    <td width="50%">
      <h3>📊 Kapsamlı Grafikler</h3>
      <ul>
        <li>Cinsiyet, Kan Grubu, Sigara Kullanımı.</li>
        <li>En sık görülen şikayetler ve yazılan ilaçlar.</li>
        <li>Chart.js ile dinamik görselleştirme.</li>
      </ul>
    </td>
  </tr>
</table>

---

## 🛠 Teknoloji Yığını

Proje, modern ve performans odaklı teknolojilerle geliştirilmiştir.

| Alan | Teknoloji | Açıklama |
| :--- | :--- | :--- |
| **Backend** | ![.NET](https://img.shields.io/badge/.NET%209-512BD4?style=flat-square&logo=dotnet&logoColor=white) | En güncel .NET sürümü |
| **Veri Tabanı** | ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white) | 2 Milyon kayıt, indeksli yapı |
| **ORM** | ![Dapper](https://img.shields.io/badge/Dapper-Micro%20ORM-green?style=flat-square) | Maksimum performans için |
| **Frontend** | ![Bootstrap](https://img.shields.io/badge/Bootstrap%205-7952B3?style=flat-square&logo=bootstrap&logoColor=white) | Admin paneli şablonu |
| **Yapay Zeka** | ![Gemini](https://img.shields.io/badge/Google%20Gemini-8E75B2?style=flat-square&logo=google&logoColor=white) | Generative AI entegrasyonu |
| **Grafikler** | ![Chart.js](https://img.shields.io/badge/Chart.js-FF6384?style=flat-square&logo=chartdotjs&logoColor=white) | Görselleştirme kütüphanesi |

---

## 📸 Ekran Görüntüleri

<details open>
<summary><b>1. Genel Dashboard Görünümü</b> (Tıklayıp Kapatabilirsiniz)</summary>
<br>
<img src="https://via.placeholder.com/1200x600?text=Dashboard+Ekran+Görüntüsü+(Buraya+Kendi+Resmini+Koy)" alt="Dashboard" width="100%">
</details>

<details>
<summary><b>2. Yapay Zeka Analiz Modalı</b></summary>
<br>
<img src="https://via.placeholder.com/800x500?text=Gemini+AI+Raporu+(Buraya+Kendi+Resmini+Koy)" alt="AI Modal" width="100%">
</details>

---

## 🧠 Nasıl Çalışır? (AI Akışı)

Projenin en can alıcı noktası olan Yapay Zeka entegrasyonunun işleyişi:

```mermaid
graph LR
A[Kullanıcı Şehir Seçer] --> B(Backend Veriyi Filtreler);
B --> C{Dashboard Güncellenir};
C -->|'Yapay Zeka Analiz Et' Tıklanır| D[Veriler JSON Formatına Çevrilir];
D --> E[Google Gemini API'ye Gönderilir];
E --> F[AI: Stratejik Sağlık Raporu Oluşturur];
F --> G[Ekranda Modal Olarak Gösterilir];
