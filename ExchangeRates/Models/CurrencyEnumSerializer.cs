using System;
using System.Linq;
using System.Runtime.Serialization;

namespace ExchangeRates.Models;

public static class CurrencyEnumSerializer
{
    public static string ToEnumString<T>(this T type) where T : Enum
    {
        var enumType = typeof(T);
        var name = Enum.GetName(enumType, type);
        var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name!)!.GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
        return enumMemberAttribute.Value!;
    }
    
    public static T? ToEnum<T>(this string str)
        where T : Enum
    {
        var enumType = typeof(T);
        foreach (var name in Enum.GetNames(enumType))
        {
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
        }
        //throw exception or whatever handling you want or
        return default;
    }
    
}