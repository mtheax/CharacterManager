namespace CharacterManager.Models;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class Character : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    
    private string _localImagePath = string.Empty;
    public string LocalImagePath
    {
        get => _localImagePath;
        set
        {
            if (_localImagePath != value)
            {
                _localImagePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ImagePathForWpf)); 
            }
        }
    }
    
    public string ImagePathForWpf => string.IsNullOrEmpty(LocalImagePath) 
        ? ImageUrl
        : LocalImagePath;

    public string ImageHash { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsImageDownloading { get; set; }

    public Character()
    {
        CreatedDate = DateTime.Now;
        IsImageDownloading = false;
    }

    public void DisplayInfo()
    {
        Console.WriteLine("--- Персонаж ---");
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Ім'я: {Name}");
        Console.WriteLine($"Клас: {Class}");
        Console.WriteLine($"Рівень: {Level}");
        Console.WriteLine($"Опис: {Description}");
        Console.WriteLine($"Графіка: {ImagePathForWpf}");
        Console.WriteLine();
    }

}