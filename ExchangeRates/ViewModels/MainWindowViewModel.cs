﻿using System;
using ExchangeRates.Commands;
using ExchangeRates.Models;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using ReactiveUI;
using SkiaSharp;

namespace ExchangeRates.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    [Obsolete("Obsolete")]
    public ISeries[] Series { get; set; } =
    [
        new LineSeries<CurrencyValue>
        {
            Values = new CurrencyValue[]{},
            Mapping = (value, i) => new Coordinate(i, value.Value),
            YToolTipLabelFormatter = point => $"Курс: {point.PrimaryValue.ToString("0.##")} руб. \n Разница: {point.Model!.PrevDiff.ToString("0.##")} руб."
        },
    ];
    
    public Axis[] XAxis { get; set; } =
    [
        new Axis
        {
            Labels = new string[]{ DateTime.Now.ToString("dd.MM.yyyy") }
        }
    ];

    public Axis[] YAxis { get; set; } =
    [
        new Axis
        {
            Labeler = x => $"{x} руб.",
        }
    ];
        
    public LabelVisual Title { get; set; } = new LabelVisual
    {
        Text = "Динамика валюты",
        TextSize = 25,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint(SKColors.DarkSlateGray)
    };

    // Observable Properties

    private int _selectedCurrency;
    public int SelectedCurrency { get => _selectedCurrency; set => this.RaiseAndSetIfChanged(ref _selectedCurrency, value); }

    private DateTime _startDate = DateTime.Now.AddMonths(-1);
    public DateTime StartDate { get => _startDate; set => this.RaiseAndSetIfChanged(ref _startDate, value); }
    
    private DateTime _endDate = DateTime.Now;
    public DateTime EndDate { get => _endDate; set => this.RaiseAndSetIfChanged(ref _endDate, value); }

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => this.RaiseAndSetIfChanged(ref _isLoading, value); }
    
    private string? _errorMessage;
    public string ErrorMessage { get => _errorMessage!; set { this.RaiseAndSetIfChanged(ref _errorMessage, value); this.RaisePropertyChanged(nameof(HasErrorMessage)); } }

    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    // Commands

    private SearchDataCommand? _searchDataCommand;
    [Obsolete("Obsolete")] public SearchDataCommand SearchDataCommand => _searchDataCommand ??= new SearchDataCommand(this);

    private SaveJsonDataCommand? _saveJsonDataCommand;
    [Obsolete("Obsolete")] public SaveJsonDataCommand SaveJsonDataCommand => _saveJsonDataCommand ??= new SaveJsonDataCommand(this);
}