using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ExchangeRates.Extensions;
using ExchangeRates.Models;
using ExchangeRates.ViewModels;
using ReactiveUI;

namespace ExchangeRates.Commands;

public class SearchDataCommand : ReactiveCommand<Unit, Task>
{
    private static string TranslateCurrency(Currencies currency)
    {
        switch (currency)
        {
            case Currencies.USD: 
                return "Доллар";
            
            case Currencies.EUR:
                return "Евро";
            
            default:
                return "-";
        }
    }
    
    private static async Task SearchDataAsync(MainWindowViewModel mainWindowViewModel)
    {
        var fileName = "cbr.xml";
        
        var currencyCode = ((Currencies)mainWindowViewModel.SelectedCurrency).ToEnumString();
        
        var startDate = mainWindowViewModel.StartDate.ToString("dd/MM/yyyy");
        var endDate = mainWindowViewModel.EndDate.ToString("dd/MM/yyyy");
        
        var url = $"https://www.cbr.ru/scripts/XML_dynamic.asp?date_req1={startDate}&date_req2={endDate}&VAL_NM_RQ={currencyCode}";
        
        try
        {
            mainWindowViewModel.IsLoading = true;

            using (var client = new HttpClient())
            {
                await client.DownloadFileTaskAsync(new Uri(url), fileName);
                var data = new CbrXmlDeserializer(fileName).Deserialize();

                mainWindowViewModel.Title.Text = $"Динамика валюты - {TranslateCurrency((Currencies)mainWindowViewModel.SelectedCurrency)}";
                mainWindowViewModel.Series[0].Values = data.Select(x => x.Value);
                mainWindowViewModel.XAxis[0].Labels = data.Select(x => x.Date.ToString("dd.MM.yyyy")).ToImmutableList();
            }
        }
        catch (Exception e)
        {
            mainWindowViewModel.IsLoading = false;
            mainWindowViewModel.ErrorMessage = $"Произошла ошибка: ({e.Message})";
            
            Console.WriteLine(e.StackTrace);
        }
        mainWindowViewModel.IsLoading = false;
    }
    
    public SearchDataCommand(MainWindowViewModel mainWindowViewModel) : 
        base(_ => Observable.Start(async () => await SearchDataAsync(mainWindowViewModel)), canExecute: Observable.Return(true))
    {
        
    }
}