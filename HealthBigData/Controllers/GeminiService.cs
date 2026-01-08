using HealthBigData.Dtos.GeminiDtos;
using Newtonsoft.Json;
using System.Text;

namespace HealthBigData.Controllers
{
    public class GeminiService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private const string Model = "gemini-2.5-flash";
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/";

        public GeminiService(HttpClient client)
        {
            _client = client;
            _apiKey = "";
        }

        public async Task<string> GetGeminiDataAsync(string prompt)
        {
            var request = new GeminiRequestDto
            {
                contents = new List<Content>
                {
                    new Content
                    {
                        role="user",
                        parts=new List<Part>
                        {
                            new Part
                            {
                                text=prompt
                            }
                        }
                    }
                },
                generationConfig = new GenerationConfig
                {
                    temperature = 0.7f,
                    maxOutputTokens = 10000
                }
            };
            var jsonContent = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var url = $"{BaseUrl}{Model}:generateContent?key={_apiKey}";

            var response = await _client.PostAsync(url, httpContent);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                return message;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiResponseDto>(responseString);
            var resultText = geminiResponse.candidates.FirstOrDefault().content.parts.FirstOrDefault().text;

            return resultText ?? "Yanıt Alınamadı";
        }
    }
}
