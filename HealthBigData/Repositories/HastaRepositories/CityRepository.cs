using Dapper;
using HealthBigData.Context;
using HealthBigData.Dtos.DefaultDtos;
using System.Data;

namespace HealthBigData.Repositories.HastaRepositories
{
    public class CityRepository(AppDbContext _context) : ICityRepository
    {
        private readonly IDbConnection connection = _context.CreateConnection();
        public async Task<CityDescriptionDto> GetCityAsync(string cityName)
        {
            var sql = @"
        -- 1. Toplam Hasta
        SELECT Count(*) FROM HastaKayitlari WHERE Sehir=@Sehir;
        
        -- 2. Toplam Başvuru
        SELECT sum(BasvuruSayisi) FROM HastaKayitlari WHERE Sehir=@Sehir;
        
        -- 3. Ortalama Yaş
        SELECT AVG(Yas) FROM HastaKayitlari WHERE Sehir=@Sehir;
        
        -- 4. Meslek Listesi
        SELECT DISTINCT Meslek FROM HastaKayitlari WHERE Sehir=@Sehir;
        
        -- 5. Gelir Düzeyi Dağılımı (Düzeltildi: İsim + Sayı)
        SELECT GelirDuzeyi as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY GelirDuzeyi;
        
        -- 6. Güvence Dağılımı
        SELECT Guvence as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Guvence;
        
        -- 7. Hane Halkı Ortalaması
        SELECT AVG(HaneHalkiSayisi) FROM HastaKayitlari WHERE Sehir=@Sehir;
        
        -- 8. Cinsiyet Dağılımı
        SELECT Cinsiyet as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Cinsiyet;
        
        -- 9. Kan Grubu Dağılımı
        SELECT KanGrubu as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY KanGrubu;
        
        -- 10. BMI Kadın
        SELECT AVG(BMI) FROM HastaKayitlari WHERE Sehir=@Sehir AND Cinsiyet='Kadın';
        
        -- 11. BMI Erkek
        SELECT AVG(BMI) FROM HastaKayitlari WHERE Sehir=@Sehir AND Cinsiyet='Erkek';
        
        -- 12. Sigara Erkek
        SELECT Count(*) FROM HastaKayitlari WHERE Sehir=@Sehir AND Cinsiyet='Erkek' AND SigaraKullanimi='Evet';
        
        -- 13. Sigara Kadın
        SELECT Count(*) FROM HastaKayitlari WHERE Sehir=@Sehir AND Cinsiyet='Kadın' AND SigaraKullanimi='Evet';
        
        -- 14. Bölüm Dağılımı
        SELECT Bolum as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Bolum;
        
        -- 15. En Çok 3 Şikayet
        SELECT TOP 3 Sikayet as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Sikayet ORDER BY [Count] DESC;
        
        -- 16. Ortalama Maliyet
        SELECT AVG(Maliyet) FROM HastaKayitlari WHERE Sehir=@Sehir;
        
        -- 17. Toplam Maliyet
        SELECT SUM(Maliyet) FROM HastaKayitlari WHERE Sehir=@Sehir;
        
        -- 18. Sonuç Durumu
        SELECT SonucDurumu as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY SonucDurumu;
        
        -- 19. En Çok 3 İlaç (Sıralama mantığı düzeltildi: Sayıya göre)
        SELECT TOP 3 IlacAdi as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY IlacAdi ORDER BY [Count] DESC;
        
        -- 20. Muayene Yoğunluğu (Tarihleri string olarak alıyoruz)
        SELECT CAST(MuayeneTarihi as NVARCHAR) as [Key], count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY MuayeneTarihi ORDER BY [Count] DESC;
    ";
            using (var multi = await connection.QueryMultipleAsync(sql, new { Sehir = cityName }))
            {
                var dto = new CityDescriptionDto();

                dto.ToplamHastaSayisi = await multi.ReadFirstAsync<int>();
                dto.ToplamBasvuru = await multi.ReadFirstOrDefaultAsync<int>();
                dto.OrtalamaYas = await multi.ReadFirstOrDefaultAsync<double>();

                dto.Meslekler = (await multi.ReadAsync<string>()).ToList();

                dto.GelirDuzeyiDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.GuvenceDagilimi = (await multi.ReadAsync<StatDto>()).ToList();

                dto.OrtalamaHaneHalki = await multi.ReadFirstOrDefaultAsync<double>();

                dto.CinsiyetDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.KanGrubuDagilimi = (await multi.ReadAsync<StatDto>()).ToList();

                dto.OrtalamaBmiKadin = await multi.ReadFirstOrDefaultAsync<double>();
                dto.OrtalamaBmiErkek = await multi.ReadFirstOrDefaultAsync<double>();

                dto.SigaraKullanimiErkek = await multi.ReadFirstAsync<int>();
                dto.SigaraKullanimiKadin = await multi.ReadFirstAsync<int>();

                dto.BolumDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.EnCokSikayetler = (await multi.ReadAsync<StatDto>()).ToList();

                dto.OrtalamaMaliyet = await multi.ReadFirstOrDefaultAsync<decimal>();
                dto.ToplamMaliyet = await multi.ReadFirstOrDefaultAsync<decimal>();

                dto.SonucDurumuDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.EnCokIlaclar = (await multi.ReadAsync<StatDto>()).ToList();
                dto.EnYogunGunler = (await multi.ReadAsync<StatDto>()).ToList();

                return dto;
            }
        }
    }
}
