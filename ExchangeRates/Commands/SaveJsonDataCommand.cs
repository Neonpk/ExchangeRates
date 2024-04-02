using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ExchangeRates.Models;
using ExchangeRates.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;

namespace ExchangeRates.Commands;

public class SaveJsonDataCommand : ReactiveCommand<Unit, Task>
{
    [Obsolete("Obsolete")]
    public static async Task SaveJsonData(MainWindowViewModel mainWindowViewModel)
    {
        try
        {
            
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var file = await provider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Save Json File",
                DefaultExtension = ".json"
            });

            if (file is null) return;

            mainWindowViewModel.IsLoading = true;
            
            var stream = new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(mainWindowViewModel.Series[0].Values)));
            await using var writeStream = await file.OpenWriteAsync();
            await stream.CopyToAsync(writeStream);

        }
        catch (Exception e)
        {
            mainWindowViewModel.IsLoading = false;
            mainWindowViewModel.ErrorMessage = e.Message;
            
            Console.WriteLine(e.StackTrace);
        }
        mainWindowViewModel.IsLoading = false;
    }
    
    [Obsolete("Obsolete")]
    public SaveJsonDataCommand(MainWindowViewModel mainWindowViewModel) :
        base(_ => Observable.Start(async () => await SaveJsonData(mainWindowViewModel)),
            canExecute: mainWindowViewModel.WhenAnyValue(x => x.IsLoading, x => !x).ObserveOn(AvaloniaScheduler.Instance))
    {
    }
}