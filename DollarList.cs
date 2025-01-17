using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data;

namespace NBU_USD
{
    internal class DollarList
    {

        public List<Dollar> ListOfDollars = new List<Dollar>();

        public List<string> StringListOfDollars = new List<string>();

        public string CurrentDate { get; set; }
        public string StartDate { get; set; }
        public double CurrentSum { get; set; }
        public double RequiredSum { get; set; }
        public string FilePath { get; set; }
        //https://bank.gov.ua/NBU_Exchange/exchange_site?start=20250101&end=20250115&valcode=usd&sort=exchangedate&order=desc&json
        public List<Dollar> GetListOfDollars()
        {
            return ListOfDollars;
        }

        public DollarList()
        {
            SetFilePath();
            Run();
            SetListOfDollars();
        }

        public void SetStartDate()
        {
            Console.WriteLine("Введите дату для диапазона курсов");
            string temp = Convert.ToString(Console.ReadLine());
            StartDate = string.Concat(temp.Split(',', '.', '/').Reverse());
        }

        public void SetCurrentDate()
        {
            CurrentDate = DateTime.Now.ToString("yyyyMMdd");

        }

        public void SetFilePath()
        {
            SetStartDate();
            SetCurrentDate();
            FilePath = "https://bank.gov.ua/NBU_Exchange/exchange_site?start=" + StartDate + "&end=" + CurrentDate + "&valcode=usd&sort=exchangedate&order=desc&json";
        }

        public List<string> ReadJsonFileSync() // читает url json
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Выполнение HTTP-запроса синхронно
                    HttpResponseMessage response = client.GetAsync(FilePath).Result;
                    response.EnsureSuccessStatusCode();

                    // Чтение содержимого ответа синхронно
                    string jsonContent = response.Content.ReadAsStringAsync().Result;

                    // Десериализация JSON в массив объектов JSON
                    using (var jsonDocument = JsonDocument.Parse(jsonContent))
                    {
                        var jsonArray = jsonDocument.RootElement.EnumerateArray();

                        // Извлечение каждого элемента JSON в виде строки
                        List<string> jsonStrings = new List<string>();
                        foreach (var element in jsonArray)
                        {
                            jsonStrings.Add(element.GetRawText());
                        }

                        return jsonStrings;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении данных: {ex.Message}");
                return new List<string>();
            }
        }


        public void Run() //заполняет поле List <string>
        {
            List<string> rates = ReadJsonFileSync();

            if (rates.Any())
            {
                StringListOfDollars = rates; // Сохраняем результат в поле jsonStrings
                Console.WriteLine("\nСписок курсов успешно обновлен:\n");
            }
            else
            {
                Console.WriteLine("Не удалось загрузить данные.");
            }
        }

        public void SetListOfDollars()
        {
            foreach (var dollar in StringListOfDollars)
            {
                int x = dollar.IndexOf("exchangedate\":\"", 0) + 15;
                string s = dollar.Substring(x, 10);
                int y = dollar.IndexOf("rate\":", 0) + 6;
                string rstr = dollar.Substring(y, 7);
                double r = Convert.ToDouble(rstr, System.Globalization.CultureInfo.InvariantCulture);
                ListOfDollars.Add(new Dollar(s, r));
            }
        }

       public int FindRate ()
        {
            Console.WriteLine("\nВведите текущую сумму");
            string tempCurrentSum = Console.ReadLine();
            CurrentSum = Convert.ToDouble(tempCurrentSum, System.Globalization.CultureInfo.InvariantCulture);
            Console.WriteLine("\nВведите желаемую сумму");
            string tempRequiredSum = Console.ReadLine();
            RequiredSum = Convert.ToDouble(tempRequiredSum, System.Globalization.CultureInfo.InvariantCulture);
            double currentRate = ListOfDollars[0].Rate;

            double temp = 100000000;
            int index = 0;

            //Console.WriteLine(Convert.ToString(currentRate));
            //Console.WriteLine(Convert.ToString(CurrentSum));
            //Console.WriteLine(Convert.ToString(RequiredSum));




            for (int i = 0; i < ListOfDollars.Count; i++)
            {
                //Console.WriteLine(Convert.ToDouble(RequiredSum - (CurrentSum / currentRate * ListOfDollars[i].Rate)) + " --- " + ListOfDollars[i].Exchangedate);
                if (RequiredSum - (CurrentSum / currentRate * ListOfDollars[i].Rate) < temp && RequiredSum - (CurrentSum / currentRate * ListOfDollars[i].Rate) >= 0)
                {
                    temp = RequiredSum - (CurrentSum / currentRate * ListOfDollars[i].Rate);
                    index = i;
                }
            }

            return index;
        }
    }
}
