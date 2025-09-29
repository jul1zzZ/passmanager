using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace pass.Models;

public class Account : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string ServiceName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Notes { get; set; }
    public string Icon { get; set; } = "other.png";

    // Связь с категорией
    public int? CategoryId { get; set; }

    private bool isPasswordVisible;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsPasswordVisible
    {
        get => isPasswordVisible;
        set
        {
            if (isPasswordVisible != value)
            {
                isPasswordVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayPassword));
            }
        }
    }

    public string DisplayPassword => IsPasswordVisible ? Password : "••••••";

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
