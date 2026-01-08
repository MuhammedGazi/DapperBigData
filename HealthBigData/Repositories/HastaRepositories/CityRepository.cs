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
    SELECT 
        COUNT(*) as ToplamHasta,
        SUM(BasvuruSayisi) as ToplamBasvuru,
        AVG(Yas) as OrtalamaYas,
        AVG(HaneHalkiSayisi) as OrtalamaHaneHalki,
        AVG(Maliyet) as OrtalamaMaliyet,
        SUM(Maliyet) as ToplamMaliyet,
        -- Cinsiyete Özel Hesaplamalar (CASE WHEN ile)
        AVG(CASE WHEN Cinsiyet='Kadın' THEN BMI END) as OrtalamaBmiKadin,
        AVG(CASE WHEN Cinsiyet='Erkek' THEN BMI END) as OrtalamaBmiErkek,
        COUNT(CASE WHEN Cinsiyet='Erkek' AND SigaraKullanimi IN ('Evet', 'Var') THEN 1 END) as SigaraKullanimiErkek,
        COUNT(CASE WHEN Cinsiyet='Kadın' AND SigaraKullanimi IN ('Evet', 'Var') THEN 1 END) as SigaraKullanimiKadin
    FROM HastaKayitlari 
    WHERE Sehir=@Sehir;

    -- 2. Meslek Listesi
    SELECT DISTINCT Meslek FROM HastaKayitlari WHERE Sehir=@Sehir;
    
    -- 3. Gelir Dağılımı
    SELECT GelirDuzeyi as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY GelirDuzeyi;
    
    -- 4. Güvence Dağılımı
    SELECT Guvence as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Guvence;
    
    -- 5. Cinsiyet Dağılımı
    SELECT Cinsiyet as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Cinsiyet;
    
    -- 6. Kan Grubu Dağılımı
    SELECT KanGrubu as [Key], Count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY KanGrubu;
    
    -- 7. Bölüm Dağılımı
    SELECT Bolum as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Bolum;
    
    -- 8. En Çok Şikayet (Top 3)
    SELECT TOP 3 Sikayet as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY Sikayet ORDER BY [Count] DESC;
    
    -- 9. Sonuç Durumu
    SELECT SonucDurumu as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY SonucDurumu;
    
    -- 10. En Çok İlaç (Top 3)
    SELECT TOP 3 IlacAdi as [Key], COUNT(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY IlacAdi ORDER BY [Count] DESC;
    
    -- 11. Muayene Yoğunluğu
    SELECT CAST(MuayeneTarihi as NVARCHAR(20)) as [Key], count(*) as [Count] FROM HastaKayitlari WHERE Sehir=@Sehir GROUP BY CAST(MuayeneTarihi as NVARCHAR(20)) ORDER BY [Count] DESC;
";
            using (var multi = await connection.QueryMultipleAsync(sql, new { Sehir = cityName }))
            {
                var dto = new CityDescriptionDto();

                var anaIstatistikler = await multi.ReadFirstAsync<dynamic>();

                dto.ToplamHastaSayisi = (int)anaIstatistikler.ToplamHasta;
                dto.ToplamBasvuru = (int)(anaIstatistikler.ToplamBasvuru ?? 0);
                dto.OrtalamaYas = (double)(anaIstatistikler.OrtalamaYas ?? 0);
                dto.OrtalamaHaneHalki = (double)(anaIstatistikler.OrtalamaHaneHalki ?? 0);
                dto.OrtalamaMaliyet = (decimal)(anaIstatistikler.OrtalamaMaliyet ?? 0);
                dto.ToplamMaliyet = (decimal)(anaIstatistikler.ToplamMaliyet ?? 0);
                dto.OrtalamaBmiKadin = (double)(anaIstatistikler.OrtalamaBmiKadin ?? 0);
                dto.OrtalamaBmiErkek = (double)(anaIstatistikler.OrtalamaBmiErkek ?? 0);
                dto.SigaraKullanimiErkek = (int)anaIstatistikler.SigaraKullanimiErkek;
                dto.SigaraKullanimiKadin = (int)anaIstatistikler.SigaraKullanimiKadin;

                dto.Meslekler = (await multi.ReadAsync<string>()).ToList();
                dto.GelirDuzeyiDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.GuvenceDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.CinsiyetDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.KanGrubuDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.BolumDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.EnCokSikayetler = (await multi.ReadAsync<StatDto>()).ToList();
                dto.SonucDurumuDagilimi = (await multi.ReadAsync<StatDto>()).ToList();
                dto.EnCokIlaclar = (await multi.ReadAsync<StatDto>()).ToList();
                dto.EnYogunGunler = (await multi.ReadAsync<StatDto>()).ToList();

                return dto;
            }
        }
    }
}
