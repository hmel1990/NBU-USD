using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NBU_USD
{
    public class ExchangeRate
    {
        public string Exchangedate { get; set; }
        public int R030 { get; set; }
        public string Cc { get; set; }
        public string Txt { get; set; }
        public string Enname { get; set; }
        public double Rate { get; set; }
        public int Units { get; set; }
        public decimal RatePerUnit { get; set; }
        public string Group { get; set; }
        public string Calcdate { get; set; }

        public override string ToString()
        {
            return $"{Exchangedate} - {Rate}";
        }


        public static List<ExchangeRate> FetchExchangeRatesAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Выполнение HTTP-запроса синхронно
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();

                    // Чтение содержимого ответа синхронно
                    string jsonContent = response.Content.ReadAsStringAsync().Result;

                    // Десериализация JSON в массив объектов JSON
                    List<ExchangeRate> exchangeRates = JsonSerializer.Deserialize<List<ExchangeRate>>(jsonContent);

                    return exchangeRates;
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
                return new List<ExchangeRate>();
            }
        }
       
    }
}
