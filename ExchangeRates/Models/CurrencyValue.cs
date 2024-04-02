using System;

namespace ExchangeRates.Models;

public class CurrencyValue
{
    public Currencies CurrencyType { get; init; }
    public DateTime Date { get; init; }
    public int Nominal { get; init; }
    public double Value { get; init; }
    public double VunitRate { get; init; }
}