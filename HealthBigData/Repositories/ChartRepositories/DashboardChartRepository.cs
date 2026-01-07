using Dapper;
using HealthBigData.Context;
using HealthBigData.Dtos.DashboardDtos;
using System.Data;

namespace HealthBigData.Repositories.ChartRepositories
{
    public class DashboardChartRepository(AppDbContext _context) : IDashboardChartRepository
    {
        private readonly IDbConnection connection = _context.CreateConnection();

        public async Task<DashboardFullDto> GetDashboardChartAsync()
        {
            var sql = @"
        -- 1. Cinsiyet Dağılımı
        SELECT Cinsiyet as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY Cinsiyet;

        -- 2. Kan Grubu Dağılımı
        SELECT KanGrubu as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY KanGrubu;

        -- 3. Sigara Erkek (Sayısı)
        SELECT COUNT(*) FROM HastaKayitlari WHERE Cinsiyet='Erkek' AND SigaraKullanimi IN ('Evet', 'Var');

        -- 4. Sigara Kadın (Sayısı)
        SELECT COUNT(*) FROM HastaKayitlari WHERE Cinsiyet='Kadın' AND SigaraKullanimi IN ('Evet', 'Var');

        -- 5. Gelir Düzeyi Dağılımı
        SELECT GelirDuzeyi as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY GelirDuzeyi;

        -- 6. Sosyal Güvence Dağılımı
        SELECT Guvence as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY Guvence;

        -- 7. Bölüm Yoğunluğu (Top 10)
        SELECT TOP 10 Bolum as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY Bolum ORDER BY [Count] DESC;

        -- 8. En Çok Şikayetler (Top 5)
        SELECT TOP 5 Sikayet as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY Sikayet ORDER BY [Count] DESC;

        -- 9. En Çok İlaçlar (Top 5)
        SELECT TOP 5 IlacAdi as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY IlacAdi ORDER BY [Count] DESC;

        -- 10. Günlük Hasta Sayıları (Son 30 Gün - Formatlanmış Tarih)
        SELECT FORMAT(MuayeneTarihi, 'yyyy-MM-dd') as [Key], COUNT(*) as [Count] 
        FROM HastaKayitlari 
        WHERE MuayeneTarihi >= DATEADD(day, -30, GETDATE())
        GROUP BY FORMAT(MuayeneTarihi, 'yyyy-MM-dd') 
        ORDER BY [Key];

        -- 11. Sonuç Durumu Dağılımı
        SELECT SonucDurumu as [Key], COUNT(*) as [Count] FROM HastaKayitlari GROUP BY SonucDurumu;

        -- 12. DETAYLI LABORATUVAR ANALİZLERİ (Hepsi Tek Listede - UNION ALL)
        
        -- A) Meslek Analizi
        SELECT 
            'Meslek' as AnalizTuru,
            H.Meslek as GrupAdi,
            L.TestAdi,
            L.ReferansDurumu,
            COUNT(*) as VakaSayisi,
            AVG(L.SonucDegeri) as OrtalamaDeger
        FROM HastaKayitlari H JOIN LaboratuvarSonuclari L ON H.ProtokolNo = L.ProtokolNo
        WHERE L.TestAdi IN ('CRP', 'WBC (Lökosit)', 'Açlık Kan Şekeri', 'Hemoglobin', 'B12 Vitamini')
        GROUP BY H.Meslek, L.TestAdi, L.ReferansDurumu

        UNION ALL

        -- B) Cinsiyet Analizi
        SELECT 
            'Cinsiyet' as AnalizTuru,
            H.Cinsiyet as GrupAdi,
            L.TestAdi,
            L.ReferansDurumu,
            COUNT(*) as VakaSayisi,
            AVG(L.SonucDegeri) as OrtalamaDeger
        FROM HastaKayitlari H JOIN LaboratuvarSonuclari L ON H.ProtokolNo = L.ProtokolNo
        WHERE L.TestAdi IN ('Hemoglobin', 'Kreatinin', 'TSH', 'Kortizol')
        GROUP BY H.Cinsiyet, L.TestAdi, L.ReferansDurumu

        UNION ALL

        -- C) Bölge Analizi
        SELECT 
            'Bolge' as AnalizTuru,
            H.Bolge as GrupAdi,
            L.TestAdi,
            L.ReferansDurumu,
            COUNT(*) as VakaSayisi,
            AVG(L.SonucDegeri) as OrtalamaDeger
        FROM HastaKayitlari H JOIN LaboratuvarSonuclari L ON H.ProtokolNo = L.ProtokolNo
        GROUP BY H.Bolge, L.TestAdi, L.ReferansDurumu

        UNION ALL
        
        -- D) BMI Analizi
        SELECT 
            'BMI' as AnalizTuru,
            CASE 
                WHEN H.BMI < 18.5 THEN 'Zayıf'
                WHEN H.BMI BETWEEN 18.5 AND 24.9 THEN 'Normal'
                WHEN H.BMI >= 30 THEN 'Obez'
                ELSE 'Fazla Kilolu' 
            END as GrupAdi,
            L.TestAdi,
            L.ReferansDurumu,
            COUNT(*) as VakaSayisi,
            AVG(L.SonucDegeri) as OrtalamaDeger
        FROM HastaKayitlari H JOIN LaboratuvarSonuclari L ON H.ProtokolNo = L.ProtokolNo
        WHERE L.TestAdi IN ('Açlık Kan Şekeri', 'Troponin', 'Kreatinin')
        GROUP BY CASE WHEN H.BMI < 18.5 THEN 'Zayıf' WHEN H.BMI BETWEEN 18.5 AND 24.9 THEN 'Normal' WHEN H.BMI >= 30 THEN 'Obez' ELSE 'Fazla Kilolu' END, L.TestAdi, L.ReferansDurumu

        UNION ALL

        -- E) Yaş Grubu Analizi (YENİ EKLENDİ)
        SELECT 
            'YasGrubu' as AnalizTuru,
            CASE 
                WHEN H.Yas BETWEEN 0 AND 18 THEN '0-18 (Çocuk)'
                WHEN H.Yas BETWEEN 19 AND 40 THEN '19-40 (Genç)'
                WHEN H.Yas BETWEEN 41 AND 65 THEN '41-65 (Orta Yaş)'
                ELSE '65+ (Yaşlı)' 
            END as GrupAdi,
            L.TestAdi,
            L.ReferansDurumu,
            COUNT(*) as VakaSayisi,
            AVG(L.SonucDegeri) as OrtalamaDeger
        FROM HastaKayitlari H JOIN LaboratuvarSonuclari L ON H.ProtokolNo = L.ProtokolNo
        WHERE L.TestAdi IN ('Açlık Kan Şekeri', 'Kreatinin', 'Tansiyon') 
        GROUP BY CASE 
                WHEN H.Yas BETWEEN 0 AND 18 THEN '0-18 (Çocuk)'
                WHEN H.Yas BETWEEN 19 AND 40 THEN '19-40 (Genç)'
                WHEN H.Yas BETWEEN 41 AND 65 THEN '41-65 (Orta Yaş)'
                ELSE '65+ (Yaşlı)' 
            END, L.TestAdi, L.ReferansDurumu

        UNION ALL

        -- F) Hane Halkı Analizi (YENİ EKLENDİ)
        SELECT 
            'HaneHalki' as AnalizTuru,
            CAST(H.HaneHalkiSayisi as NVARCHAR) + ' Kişilik' as GrupAdi, 
            L.TestAdi,
            L.ReferansDurumu,
            COUNT(*) as VakaSayisi,
            AVG(L.SonucDegeri) as OrtalamaDeger
        FROM HastaKayitlari H JOIN LaboratuvarSonuclari L ON H.ProtokolNo = L.ProtokolNo
        WHERE L.TestAdi IN ('CRP', 'WBC (Lökosit)', 'Sedimantasyon') 
        GROUP BY H.HaneHalkiSayisi, L.TestAdi, L.ReferansDurumu;
            ";

            using (var multi = await connection.QueryMultipleAsync(sql))
            {
                var dto = new DashboardFullDto();

                dto.CinsiyetDagilimi = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.KanGrubuDagilimi = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.SigaraKullanimiErkek = await multi.ReadFirstAsync<int>();
                dto.SigaraKullanimiKadin = await multi.ReadFirstAsync<int>();

                dto.GelirDagilimi = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.GuvenceDagilimi = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.BolumYogunlugu = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.EnCokSikayetler = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.EnCokIlaclar = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.GunlukHastaSayilari = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.SonucDurumlari = (await multi.ReadAsync<ChartItemDto>()).ToList();

                dto.LabAnalizVerileri = (await multi.ReadAsync<LabAnalysisDto>()).ToList();

                return dto;
            }
        }
    }
}