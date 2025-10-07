using pass.Pages;
using pass.Models;
using pass.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Storage;

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
        SortPicker.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _categories = (await App.Database.GetCategoriesAsync()).ToList();
        CategoryFilterPicker.ItemsSource = new[] { "Все" }.Concat(_categories.Select(c => c.Name)).ToList();
        CategoryFilterPicker.SelectedIndex = 0;

        _allAccounts = (await App.Database.GetAccountsAsync()).ToList();
        ApplySortAndFilter();
    }

    private void ApplySortAndFilter()
    {
        IEnumerable<Account> filtered = _allAccounts;

        var search = SearchBar.Text?.Trim().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(search))
            filtered = filtered.Where(a =>
                (a.ServiceName ?? "").ToLowerInvariant().Contains(search) ||
                (a.Username ?? "").ToLowerInvariant().Contains(search));

        if (CategoryFilterPicker.SelectedIndex > 0)
        {
            var categoryName = CategoryFilterPicker.SelectedItem.ToString();
            var category = _categories.FirstOrDefault(c => c.Name == categoryName);
            if (category != null)
                filtered = filtered.Where(a => a.CategoryId == category.Id);
        }

        if (SortPicker.SelectedIndex == 0)
            filtered = filtered.OrderBy(a => a.ServiceName);
        else if (SortPicker.SelectedIndex == 1)
            filtered = filtered.OrderByDescending(a => a.CreatedAt);

        Accounts.Clear();
        foreach (var a in filtered)
            Accounts.Add(a);
    }


    private async void OnEditSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account)
            await Navigation.PushAsync(new EditAccountPage(account));
    }

    private async void OnDeleteSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account)
        {
            var confirm = await DisplayAlert("Удалить?", $"Удалить {account.ServiceName}?", "Да", "Нет");
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
            await Toast.Make("Пароль скопирован", ToastDuration.Short, 14).Show();
        }
    }

    private async void OnCopyUsernameSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is Account account && !string.IsNullOrEmpty(account.Username))
        {
            await Clipboard.Default.SetTextAsync(account.Username);
            await Toast.Make("Логин скопирован", ToastDuration.Short, 14).Show();
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


    private async void OnExportClicked(object sender, EventArgs e)
    {
        try
        {
            App.IsPickingFile = true;

            string password = await DisplayPromptAsync(
                "Экспорт",
                "Введите пароль для шифрования:",
                accept: "OK",
                cancel: "Отмена",
                placeholder: "Пароль",
                maxLength: 100,
                keyboard: Keyboard.Text);

            if (string.IsNullOrWhiteSpace(password))
                return;

            var accounts = await App.Database.GetAccountsAsync();
            var path = await BackupService.ExportAsync(password, accounts);

            await DisplayAlert("Успех", $"Бэкап сохранён:\n{path}", "OK");

            try
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Поделиться бэкапом",
                    File = new ShareFile(path)
                });
            }
            catch
            {
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка экспорта", ex.Message, "OK");
        }
        finally
        {
            App.IsPickingFile = false;
        }
    }

    private async void OnImportClicked(object sender, EventArgs e)
    {
        try
        {
            App.IsPickingFile = true; 

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите файл резервной копии"
            });

            if (result == null) return;

            if (!result.FileName.EndsWith(".enc", StringComparison.OrdinalIgnoreCase))
            {
                var proceed = await DisplayAlert("Внимание",
                    "Выбран файл без расширения .enc. Продолжить попытку импорта?",
                    "Продолжить", "Отмена");
                if (!proceed) return;
            }

            string password = await DisplayPromptAsync(
                "Импорт",
                "Введите пароль для расшифровки:",
                accept: "OK",
                cancel: "Отмена",
                placeholder: "Пароль",
                maxLength: 100,
                keyboard: Keyboard.Text);

            if (string.IsNullOrWhiteSpace(password)) return;

            using var stream = await result.OpenReadAsync();
            var imported = await BackupService.ImportAsync(password, stream);

            if (imported == null || imported.Count == 0)
            {
                await DisplayAlert("Импорт", "Файл пуст или не содержит записей.", "OK");
            }
            else
            {
                int count = 0;
                foreach (var acc in imported)
                {
                    // сбрасываем Id, чтобы сохранить как новую запись (или адаптируй логику под себя)
                    acc.Id = 0;
                    await App.Database.SaveAccountAsync(acc);
                    count++;
                }

                // обновляем список в UI
                _allAccounts = (await App.Database.GetAccountsAsync()).ToList();
                ApplySortAndFilter();

                await DisplayAlert("Готово", $"Импортировано {count} записей", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка импорта", ex.Message, "OK");
        }
        finally
        {
            App.IsPickingFile = false;
        }
    }




}
