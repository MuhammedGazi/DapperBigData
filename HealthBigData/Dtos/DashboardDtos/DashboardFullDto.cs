namespace HealthBigData.Dtos.DashboardDtos
{

    public class DashboardFullDto
    {
        public DashboardFullDto()
        {
            CinsiyetDagilimi = new List<ChartItemDto>();
            KanGrubuDagilimi = new List<ChartItemDto>();
            GelirDagilimi = new List<ChartItemDto>();
            GuvenceDagilimi = new List<ChartItemDto>();
            BolumYogunlugu = new List<ChartItemDto>();
            EnCokSikayetler = new List<ChartItemDto>();
            EnCokIlaclar = new List<ChartItemDto>();
            GunlukHastaSayilari = new List<ChartItemDto>();
            SonucDurumlari = new List<ChartItemDto>();

            LabAnalizVerileri = new List<LabAnalysisDto>();
        }

        public List<ChartItemDto> CinsiyetDagilimi { get; set; }
        public List<ChartItemDto> KanGrubuDagilimi { get; set; }

        public int SigaraKullanimiErkek { get; set; }
        public int SigaraKullanimiKadin { get; set; }

        public List<ChartItemDto> GelirDagilimi { get; set; }
        public List<ChartItemDto> GuvenceDagilimi { get; set; }
        public List<ChartItemDto> BolumYogunlugu { get; set; }
        public List<ChartItemDto> EnCokSikayetler { get; set; }
        public List<ChartItemDto> EnCokIlaclar { get; set; }
        public List<ChartItemDto> GunlukHastaSayilari { get; set; }
        public List<ChartItemDto> SonucDurumlari { get; set; }
        public List<LabAnalysisDto> LabAnalizVerileri { get; set; }
    }

    public class ChartItemDto
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }

    public class LabAnalysisDto
    {
        public string AnalizTuru { get; set; }
        public string GrupAdi { get; set; }
        public string TestAdi { get; set; }
        public string ReferansDurumu { get; set; }
        public int VakaSayisi { get; set; }
        public decimal OrtalamaDeger { get; set; }
    }
}