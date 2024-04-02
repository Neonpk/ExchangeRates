using System.Runtime.Serialization;

namespace ExchangeRates.Models;

[DataContract]
public enum Currencies
{
    [EnumMember(Value = "R01235")]
    USD = 0,
    
    [EnumMember(Value = "R01239")]
    EUR
}