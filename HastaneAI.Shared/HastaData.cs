using Microsoft.ML.Data;

namespace HastaneAI.Shared
{
    public class HastaRiskData
    {
        // --- 1. Temel Veriler ---
        public float Yas { get; set; }
        public float BMI { get; set; }
        public float Tansiyon { get; set; }
        public string SigaraKullanimi { get; set; }

        // --- 2. Laboratuvar Testleri (float? DEĞİL, float olmalı) ---
        // ML.NET, veritabanından NULL gelirse buraya otomatik olarak "NaN" basar.

        public float Lab_CRP { get; set; }
        public float Lab_WBC { get; set; }
        public float Lab_Sedimantasyon { get; set; }

        public float Lab_Glukoz { get; set; }
        public float Lab_Kreatinin { get; set; }
        public float Lab_Potasyum { get; set; }

        public float Lab_Hemoglobin { get; set; }
        public float Lab_B12 { get; set; }

        public float Lab_TSH { get; set; }
        public float Lab_Kortizol { get; set; }

        public float Lab_Troponin { get; set; }
        public float Lab_KanIlacDuzeyi { get; set; }

        // --- 3. Hedef ---
        [ColumnName("Label")]
        public bool Label { get; set; }
    }

    // RiskTahmini sınıfında değişiklik yok, aynen kalabilir.
    public class RiskTahmini
    {
        [ColumnName("PredictedLabel")]
        public bool RiskDurumu { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}