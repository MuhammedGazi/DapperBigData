namespace HealthBigData.Dtos.DefaultDtos
{
    public class StatDto
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }

    public class CityDescriptionDto
    {
        public int ToplamHastaSayisi { get; set; }
        public int ToplamBasvuru { get; set; }
        public double OrtalamaYas { get; set; }
        public double OrtalamaHaneHalki { get; set; }
        public double OrtalamaBmiKadin { get; set; }
        public double OrtalamaBmiErkek { get; set; }
        public int SigaraKullanimiErkek { get; set; }
        public int SigaraKullanimiKadin { get; set; }
        public decimal OrtalamaMaliyet { get; set; }
        public decimal ToplamMaliyet { get; set; }

        public List<string> Meslekler { get; set; }
        public List<StatDto> GelirDuzeyiDagilimi { get; set; }
        public List<StatDto> GuvenceDagilimi { get; set; }
        public List<StatDto> CinsiyetDagilimi { get; set; }
        public List<StatDto> KanGrubuDagilimi { get; set; }
        public List<StatDto> BolumDagilimi { get; set; }
        public List<StatDto> EnCokSikayetler { get; set; } 
        public List<StatDto> SonucDurumuDagilimi { get; set; }
        public List<StatDto> EnCokIlaclar { get; set; } 
        public List<StatDto> EnYogunGunler { get; set; }
    }
}
