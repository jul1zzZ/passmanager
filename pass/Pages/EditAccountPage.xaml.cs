using pass.Models;

namespace pass.Pages;

public partial class EditAccountPage : ContentPage
{
    private Account _account;

    public EditAccountPage(Account account)
    {
        InitializeComponent();
        _account = account;

        // Заполняем поля текущими данными
        ServiceNameEntry.Text = _account.ServiceName;
        UsernameEntry.Text = _account.Username;
        PasswordEntry.Text = _account.Password;
        NotesEditor.Text = _account.Notes;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _account.ServiceName = ServiceNameEntry.Text;
        _account.Username = UsernameEntry.Text;
        _account.Password = PasswordEntry.Text;
        _account.Notes = NotesEditor.Text;

        await App.Database.SaveAccountAsync(_account);

        await DisplayAlert("Успех", "Запись обновлена", "OK");
        await Navigation.PopAsync();
    }
}