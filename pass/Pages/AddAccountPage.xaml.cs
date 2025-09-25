using pass.Models;
using System.Text;

namespace pass.Pages;

public partial class AddAccountPage : ContentPage
{
    private List<Category> _categories = new();

    public AddAccountPage()
    {
        InitializeComponent();
        LoadCategories();
    }

    private async void LoadCategories()
    {
        _categories = (await App.Database.GetCategoriesAsync()).ToList();

        // если нет категорий — создаём несколько дефолтных
        if (!_categories.Any())
        {
            var defaults = new[] { "Работа", "Личное", "Соцсети" };
            foreach (var name in defaults)
                await App.Database.SaveCategoryAsync(new Category { Name = name });

            _categories = (await App.Database.GetCategoriesAsync()).ToList();
        }

        CategoryPicker.ItemsSource = _categories.Select(c => c.Name).ToList();
        if (_categories.Any()) CategoryPicker.SelectedIndex = 0;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Проверка полей
        if (string.IsNullOrWhiteSpace(ServiceNameEntry.Text) ||
            string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
            CategoryPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Ошибка", "Заполните все поля", "OK");
            return;
        }

        var selectedCategory = _categories[CategoryPicker.SelectedIndex];

        var account = new Account
        {
            ServiceName = ServiceNameEntry.Text,
            Username = UsernameEntry.Text,
            Password = PasswordEntry.Text,
            Notes = NotesEditor.Text,
            CategoryId = selectedCategory.Id
        };

        await App.Database.SaveAccountAsync(account);

        await DisplayAlert("Успех", "Запись добавлена", "OK");
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
