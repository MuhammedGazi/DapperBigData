using HealthBigData.Repositories.HastaRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HealthBigData.Controllers
{
    public class DefaultController(GeminiService _service, ICityRepository _repository) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAiRecommendation([FromBody] CityRequest request)
        {
            if (string.IsNullOrEmpty(request.CityName))
                return Ok("Hata: Şehir ismi alınamadı.");

            var dto = await _repository.GetCityAsync(request.CityName);

            string onCikanSikayetler = dto.EnCokSikayetler != null && dto.EnCokSikayetler.Any()
                ? string.Join(", ", dto.EnCokSikayetler.Take(5).Select(x => x.Key))
                : "Veri yok";

            string yogunBolumler = dto.BolumDagilimi != null && dto.BolumDagilimi.Any()
                ? string.Join(", ", dto.BolumDagilimi.Take(3).Select(x => x.Key))
                : "Veri yok";


            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonVerisi = JsonSerializer.Serialize(dto, jsonOptions);
            string prompt =
                "Sen Sağlık Bakanlığı adına çalışan kıdemli bir veri bilimcisi ve strateji uzmanısın. " +
                "Aşağıda bir şehre ait sağlık veri tablosu ve detaylı JSON dökümü bulunmaktadır.\n\n" +

                $"--- ÖZET GÖSTERGELER ---\n" +
                $"- Nüfus ve Yük: {dto.ToplamHastaSayisi} hasta, {dto.ToplamBasvuru} başvuru.\n" +
                $"- Demografi: Ort. Yaş: {dto.OrtalamaYas:N1}, Ort. Hane Halkı: {dto.OrtalamaHaneHalki:N1}.\n" +
                $"- Risk Faktörleri: Sigara (E: {dto.SigaraKullanimiErkek}, K: {dto.SigaraKullanimiKadin}). " +
                $"BMI Ortalamaları (Kadın: {dto.OrtalamaBmiKadin:N1}, Erkek: {dto.OrtalamaBmiErkek:N1}).\n" +
                $"- Klinik Durum: En sık şikayetler [{onCikanSikayetler}]. En yoğun bölümler [{yogunBolumler}].\n" +
                $"- Finansal: Toplam harcama {dto.ToplamMaliyet:C2}, Ortalama hasta maliyeti {dto.OrtalamaMaliyet:C2}.\n\n" +

                $"--- DETAYLI VERİ SETİ (JSON) ---\n" +
                $"Aşağıdaki JSON verisi gelir dağılımı, güvence türleri ve detaylı analizleri içerir:\n" +
                $"{jsonVerisi}\n\n" +

                $"--- GÖREVİN (ANALİZ RAPORU OLUŞTUR) ---\n" +
                $"Bu verileri analiz ederek aşağıdaki 4 başlık altında profesyonel bir rapor oluştur:\n" +
                $"1. **Genel Sağlık Durumu:** Şehrin hastalık yükü ve demografik riskleri nelerdir?\n" +
                $"2. **Korelasyon Analizi:** (Örn: Gelir düzeyi, hane halkı sayısı veya meslek grupları ile hastalıklar arasında JSON verisine dayanarak bir ilişki kur.)\n" +
                $"3. **Finansal Değerlendirme:** Maliyetleri düşürmek veya verimliliği artırmak için hangi alanlara odaklanılmalı?\n" +
                $"4. **Eylem Planı:** İl Sağlık Müdürlüğü'ne önereceğin 3 somut stratejik madde nedir?\n\n" +

                $"**Format Notu:** Raporu HTML formatına uygun başlıklarla (<h4>, <strong>, <ul>, <li>) süsle. 'html' veya 'body' taglerini KULLANMA. Sadece içeriği ver.";

            var response = await _service.GetGeminiDataAsync(prompt);

            return Json(response);
        }

        public IActionResult GetCityComponent(string cityName)
        {
            ViewBag.SehirAdi = cityName;
            return ViewComponent("_DefaultCityChartJobComponent", new { cityName = cityName });
        }
    }
    public class CityRequest
    {
        public string CityName { get; set; }
    }
}
