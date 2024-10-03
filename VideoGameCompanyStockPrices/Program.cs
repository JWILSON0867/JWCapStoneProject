using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace VideoGameCompanyStockPrices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var apiKey = "0NGRHE8PEQBF33VY";  // Your API key

            // Stock symbols
            var nintendoSymbol = "7974.T";  // Nintendo
            var microsoftSymbol = "MSFT";   // Microsoft
            var sonySymbol = "6758.T";      // Sony

            // Get stock data and store in database
            await GetAndSaveStockData(client, apiKey, nintendoSymbol);
            await GetAndSaveStockData(client, apiKey, microsoftSymbol);
            await GetAndSaveStockData(client, apiKey, sonySymbol);
        }

        static async Task GetAndSaveStockData(HttpClient client, string apiKey, string symbol)
        {
            // Get stock data from Alpha Vantage
            var response = await client.GetStringAsync($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}");
            var stockData = JObject.Parse(response);

            // Parse the stock prices
            var firstDate = stockData["Time Series (Daily)"].First;
            var date = DateTime.Parse(firstDate.Path.Split('.').Last());
            var dailyData = firstDate.First;

            var openPrice = (decimal)dailyData["1. open"];
            var highPrice = (decimal)dailyData["2. high"];
            var lowPrice = (decimal)dailyData["3. low"];
            var closePrice = (decimal)dailyData["4. close"];
            var volume = (decimal)dailyData["5. volume"];

            // Save to database
            using (var context = new ProductContext())
            {
                var stockEntity = new ProductEntity
                {
                    Symbol = symbol,
                    Date = date,
                    OpenPrice = openPrice,
                    HighPrice = highPrice,
                    LowPrice = lowPrice,
                    ClosePrice = closePrice,
                    Volume = volume
                };

                context.Products.Add(stockEntity);
                await context.SaveChangesAsync();
            }
        }
    }

}
