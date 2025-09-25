using System.Text;

namespace pass.Pages;

public partial class LoginPage : ContentPage
{
    private const string PinKey = "UserPIN";
    private StringBuilder _enteredPin = new();
    private int _pinLength = 4; // PIN из 4 цифр

    public LoginPage()
    {
        InitializeComponent();
        CheckPinExists();
        UpdatePinDisplay();
    }

    private async void CheckPinExists()
    {
        var savedPin = await SecureStorage.GetAsync(PinKey);
        TitleLabel.Text = string.IsNullOrEmpty(savedPin) ? "Создайте новый PIN" : "Введите PIN";
    }

    private void OnDigitClicked(object sender, EventArgs e)
    {
        if (_enteredPin.Length >= _pinLength) return;

        if (sender is Button btn)
        {
            _enteredPin.Append(btn.Text);
            UpdatePinDisplay();
        }
    }

    private void OnBackspaceClicked(object sender, EventArgs e)
    {
        if (_enteredPin.Length > 0)
        {
            _enteredPin.Remove(_enteredPin.Length - 1, 1);
            UpdatePinDisplay();
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (_enteredPin.Length < _pinLength)
        {
            await DisplayAlert("Ошибка", $"Введите {_pinLength}-значный PIN", "OK");
            return;
        }

        var enteredPin = _enteredPin.ToString();
        var savedPin = await SecureStorage.GetAsync(PinKey);

        if (string.IsNullOrEmpty(savedPin))
        {
            // Создание нового PIN
            await SecureStorage.SetAsync(PinKey, enteredPin);
            await DisplayAlert("Успех", "PIN создан", "OK");
        }
        else
        {
            // Проверка PIN
            if (enteredPin != savedPin)
            {
                await DisplayAlert("Ошибка", "Неверный PIN", "OK");

                // 🔥 Анимация "shake"
                await ShakeAnimation(PinDisplay);

                _enteredPin.Clear();
                UpdatePinDisplay();
                return;
            }
        }

        App.IsAuthenticated = true;
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

    private void UpdatePinDisplay()
    {
        Frame[] pins = { Pin1, Pin2, Pin3, Pin4 };
        for (int i = 0; i < _pinLength; i++)
        {
            pins[i].BackgroundColor = i < _enteredPin.Length ? Colors.Black : Colors.Transparent;
        }
    }

    private async Task ShakeAnimation(View view)
    {
        uint duration = 50;
        await view.TranslateTo(-15, 0, duration);
        await view.TranslateTo(15, 0, duration);
        await view.TranslateTo(-10, 0, duration);
        await view.TranslateTo(10, 0, duration);
        await view.TranslateTo(0, 0, duration);
    }
}
