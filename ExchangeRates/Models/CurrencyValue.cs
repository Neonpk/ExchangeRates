using System;
using Newtonsoft.Json;

namespace ExchangeRates.Models;

public class CurrencyValue
{
    private static string TranslateCurrency(Currencies currency)
    {
        switch (currency)
        {
            case Currencies.USD: 
                return "USD";
            
            case Currencies.EUR:
                return "EUR";
            
            default:
                return "-";
        }
    }
    
    [JsonIgnore]
    public Currencies CurrencyType { get; init; }
    public string CurrencyTypeString => TranslateCurrency(CurrencyType);
    public DateTime Date { get; init; }
    public int Nominal { get; init; }
    public double Value { get; init; }
    public double VunitRate { get; init; }
    public double PrevDiff { get; init; }
}