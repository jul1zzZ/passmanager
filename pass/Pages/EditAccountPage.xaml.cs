using pass.Models;
using System.Text;

namespace pass.Pages;

public partial class EditAccountPage : ContentPage
{
    private Account _account;
    private List<Category> _categories = new();
    public List<string> IconList { get; set; } = new()
    {
        "gmail.png", "facebook.png", "instagram.png", "vk.png", "steam.png", "other.png"
    };

    private string _selectedIcon = "other.png";

    public EditAccountPage(Account account)
    {
        InitializeComponent();
        _account = account;
        BindingContext = this;

        LoadCategories();
        LoadData();
    }

    private void LoadData()
    {
        ServiceNameEntry.Text = _account.ServiceName;
        UsernameEntry.Text = _account.Username;
        PasswordEntry.Text = _account.Password;
        NotesEditor.Text = _account.Notes;

        _selectedIcon = string.IsNullOrEmpty(_account.Icon) ? "other.png" : _account.Icon;

        SelectedIconPreview.Source = _selectedIcon;
        IconCollection.SelectedItem = _selectedIcon;
    }

    private async void LoadCategories()
    {
        _categories = (await App.Database.GetCategoriesAsync()).ToList();

        if (!_categories.Any())
        {
            var defaults = new[] { "Работа", "Личное", "Соцсети" };
            foreach (var name in defaults)
                await App.Database.SaveCategoryAsync(new Category { Name = name });

            _categories = (await App.Database.GetCategoriesAsync()).ToList();
        }

        CategoryPicker.ItemsSource = _categories.Select(c => c.Name).ToList();

        var currentCategory = _categories.FirstOrDefault(c => c.Id == _account.CategoryId);
        if (currentCategory != null)
            CategoryPicker.SelectedItem = currentCategory.Name;
    }

    private void OnIconSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string icon)
        {
            _selectedIcon = icon;
            SelectedIconPreview.Source = _selectedIcon;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServiceNameEntry.Text) ||
            string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
            CategoryPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Ошибка", "Заполните все поля", "OK");
            return;
        }

        var selectedCategory = _categories[CategoryPicker.SelectedIndex];

        _account.ServiceName = ServiceNameEntry.Text;
        _account.Username = UsernameEntry.Text;
        _account.Password = PasswordEntry.Text;
        _account.Notes = NotesEditor.Text;
        _account.Icon = _selectedIcon;
        _account.CategoryId = selectedCategory.Id;

        await App.Database.SaveAccountAsync(_account);

        await DisplayAlert("Успех", "Запись обновлена", "OK");
        await Navigation.PopAsync();
    }

    private void OnGeneratePasswordClicked(object sender, EventArgs e)
    {
        int length = (int)LengthSlider.Value;
        PasswordEntry.Text = GenerateSecurePassword(length);
    }

    private string GenerateSecurePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+[]{}";
        StringBuilder res = new();
        Random rnd = new();

        for (int i = 0; i < length; i++)
            res.Append(valid[rnd.Next(valid.Length)]);

        return res.ToString();
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        int value = (int)e.NewValue;
        LengthLabel.Text = $"{value} символов";
    }
}

