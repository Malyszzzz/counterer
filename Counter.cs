
using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace CounterApp;

public class Counter : INotifyPropertyChanged
{
    public string Name { get; set; } = "Licznik";

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }

    public int InitialValue { get; set; }

    private Color _color = Colors.Black;
    public Color Color
    {
        get => _color;
        set
        {
            if (_color != value)
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

