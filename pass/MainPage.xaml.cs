using pass.Pages;
using pass.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace pass;

public partial class MainPage : ContentPage
{
    private List<Account> _allAccounts = new();
    private List<Category> _categories = new();

    public ObservableCollection<Account> Accounts { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        SortPicker.SelectedIndex = 0; // По алфавиту по умолчанию
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Загружаем категории
        _categories = (await App.Database.GetCategoriesAsync()).ToList();
        CategoryFilterPicker.ItemsSource = new[] { "Все" }.Concat(_categories.Select(c => c.Name)).ToList();
        CategoryFilterPicker.SelectedIndex = 0;

        // Загружаем аккаунты
        _allAccounts = (await App.Database.GetAccountsAsync()).ToList();
        ApplySortAndFilter();
    }

    private void ApplySortAndFilter()
    {
        IEnumerable<Account> filtered = _allAccounts;

        // Фильтр поиска
        var search = SearchBar.Text?.Trim().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(search))
        {
            filtered = filtered.Where(a =>
                (a.ServiceName ?? "").ToLowerInvariant().Contains(search) ||
                (a.Username ?? "").ToLowerInvariant().Contains(search));
        }

        // Фильтр категории
        if (CategoryFilterPicker.SelectedIndex > 0)
        {
            var categoryName = CategoryFilterPicker.SelectedItem.ToString();
            var category = _categories.FirstOrDefault(c => c.Name == categoryName);
            if (category != null)
                filtered = filtered.Where(a => a.CategoryId == category.Id);
        }

        // Сортировка
        if (SortPicker.SelectedIndex == 0)
            filtered = filtered.OrderBy(a => a.ServiceName);
        else if (SortPicker.SelectedIndex == 1)
            filtered = filtered.OrderByDescending(a => a.CreatedAt);

        // Обновляем ObservableCollection
        Accounts.Clear();
        foreach (var a in filtered)
            Accounts.Add(a);
    }

    // Swipe Handlers
    private async void OnEditSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account)
            await Navigation.PushAsync(new EditAccountPage(account));
    }

    private async void OnDeleteSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account)
        {
            var confirm = await DisplayAlert("Удалить?", $"Удалить запись {account.ServiceName}?", "Да", "Нет");
            if (confirm)
            {
                await App.Database.DeleteAccountAsync(account);
                _allAccounts.Remove(account);
                Accounts.Remove(account);
            }
        }
    }

    private async void OnCopyPasswordSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account && !string.IsNullOrEmpty(account.Password))
        {
            await Clipboard.Default.SetTextAsync(account.Password);
            var toast = Toast.Make("Пароль скопирован", ToastDuration.Short, 14);
            await toast.Show();
        }
    }

    private async void OnCopyUsernameSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account && !string.IsNullOrEmpty(account.Username))
        {
            await Clipboard.Default.SetTextAsync(account.Username);
            var toast = Toast.Make("Логин скопирован", ToastDuration.Short, 14);
            await toast.Show();
        }
    }

    private void OnTogglePasswordSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account)
            account.IsPasswordVisible = !account.IsPasswordVisible;
    }

    private async void OnAddClicked(object sender, EventArgs e) => await Navigation.PushAsync(new AddAccountPage());

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e) => ApplySortAndFilter();
    private void OnSortChanged(object sender, EventArgs e) => ApplySortAndFilter();
    private void OnCategoryFilterChanged(object sender, EventArgs e) => ApplySortAndFilter();
}
