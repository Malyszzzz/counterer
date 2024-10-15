using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace CounterApp;

public partial class MainPage : ContentPage
{
    private ObservableCollection<Counter> _counters;

    public MainPage()
    {
        InitializeComponent();
        _counters = new ObservableCollection<Counter>();
        LoadCounters();
        CounterList.ItemsSource = _counters;
    }

    // Dodawanie nowego licznika
    private async void OnAddCounterClicked(object sender, EventArgs e)
    {
        var counterName = $"Licznik {_counters.Count + 1}";

        // Wybieranie początkowej wartości
        string initialValueStr = await DisplayPromptAsync("Ustaw wartość", "Podaj początkową wartość:", initialValue: "0");
        int initialValue = int.TryParse(initialValueStr, out var value) ? value : 0;

        // Wybieranie koloru
        var color = await DisplayActionSheet("Wybierz kolor", "Anuluj", null, "Czarny", "Czerwony", "Zielony", "Niebieski");

        var newCounter = new Counter
        {
            Name = counterName,
            Value = initialValue,
            InitialValue = initialValue,
            Color = color switch
            {
                "Czarny" => Colors.Black,
                "Czerwony" => Colors.Red,
                "Zielony" => Colors.Green,
                "Niebieski" => Colors.Blue,
                _ => Colors.Black // Domyślny kolor
            }
        };

        _counters.Add(newCounter);
        SaveCounters();
    }

    // Zwiększanie wartości licznika
    private void OnPlusClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var counter = button?.BindingContext as Counter;
        if (counter != null)
        {
            counter.Value++;
            SaveCounters();  // Auto-zapis
        }
    }

    // Zmniejszanie wartości licznika
    private void OnMinusClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var counter = button?.BindingContext as Counter;
        if (counter != null)
        {
            counter.Value--;
            SaveCounters();  // Auto-zapis
        }
    }

    // Resetowanie wartości licznika do początkowej
    private void OnResetClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var counter = button?.BindingContext as Counter;
        if (counter != null)
        {
            counter.Value = counter.InitialValue;
            SaveCounters();  // Auto-zapis
        }
    }

    // Usuwanie licznika
    private void OnDeleteCounterClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var counter = button?.BindingContext as Counter;
        if (counter != null)
        {
            _counters.Remove(counter);
            SaveCounters();  // Auto-zapis
        }
    }

    // Zapisywanie stanu liczników (np. do JSON)
    private void SaveCounters()
    {
        var json = JsonSerializer.Serialize(_counters);
        var path = Path.Combine(FileSystem.AppDataDirectory, "CountersData.json");
        File.WriteAllText(path, json);  // Zapis do pliku JSON
    }

    // Wczytywanie stanu liczników
    private void LoadCounters()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "CountersData.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var loadedCounters = JsonSerializer.Deserialize<ObservableCollection<Counter>>(json);
            if (loadedCounters != null)
            {
                _counters = loadedCounters;
                CounterList.ItemsSource = _counters;
            }
        }
    }
}


