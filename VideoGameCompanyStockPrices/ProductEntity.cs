using System;

namespace VideoGameCompanyStockPrices
{
    public class ProductEntity
    {
        public int Id { get; set; }                      // Primary key

        // Stock-specific fields
        public string Symbol { get; set; }               // Stock symbol (e.g., Nintendo, Sony, etc.)
        public DateTime Date { get; set; }               // Date of the stock price data
        public decimal OpenPrice { get; set; }           // Opening price
        public decimal ClosePrice { get; set; }          // Closing price
        public decimal HighPrice { get; set; }           // Highest price of the day
        public decimal LowPrice { get; set; }            // Lowest price of the day
        public decimal Volume { get; set; }              // Trading volume
    }
}
