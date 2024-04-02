using System;
using System.Collections.Generic;
using System.Xml;

namespace ExchangeRates.Models;

public class CbrXmlDeserializer
{
    private string _fileName;
    public IEnumerable<CurrencyValue> Deserialize()
    {
        var currencyCollection = new List<CurrencyValue>();
        double prevValue = 0;
        
        XmlDocument doc = new XmlDocument();
        doc.Load(_fileName);
        
        XmlNode currencyListNode = doc.SelectSingleNode("/ValCurs")!;
        XmlNodeList currencyNodeList = currencyListNode.SelectNodes("Record")!;
        
        foreach (XmlNode node in currencyNodeList)
        {
            CurrencyValue currencyValue = new CurrencyValue
            {
                Value = Convert.ToDouble(node.SelectSingleNode(".//Value")!.InnerText),
                VunitRate = Convert.ToDouble(node.SelectSingleNode(".//VunitRate")!.InnerText),
                Nominal = Convert.ToInt32(node.SelectSingleNode(".//Nominal")!.InnerText),
                CurrencyType = currencyListNode.Attributes!["ID"]!.InnerText.ToEnum<Currencies>(),
                Date = DateTime.Parse(node.Attributes!["Date"]!.InnerText),
                PrevDiff = prevValue != 0 ? Convert.ToDouble(node.SelectSingleNode(".//Value")!.InnerText) - prevValue : 0
            };
            currencyCollection.Add(currencyValue);

            prevValue = Convert.ToDouble(node.SelectSingleNode(".//Value")!.InnerText);
        }
        
        return currencyCollection;
    }
    
    public CbrXmlDeserializer(string fileName)
    {
        _fileName = fileName;
    }
}