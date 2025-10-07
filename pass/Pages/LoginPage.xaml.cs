using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Material.Components.Maui;
using System.Text;

namespace pass.Pages;

public partial class LoginPage : ContentPage
{
    private const string PinKey = "UserPIN";
    private StringBuilder _enteredPin = new();
    private int _pinLength = 4;

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

        if (sender is Material.Components.Maui.Button btn) 
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
            var toast = Toast.Make($"Введите {_pinLength}-значный PIN", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        var enteredPin = _enteredPin.ToString();
        var savedPin = await SecureStorage.GetAsync(PinKey);

        if (string.IsNullOrEmpty(savedPin))
        {
            await SecureStorage.SetAsync(PinKey, enteredPin);
            var toast = Toast.Make("PIN создан", ToastDuration.Short, 14);
            await toast.Show();
        }
        else
        {
            if (enteredPin != savedPin)
            {
                var toast = Toast.Make("Неверный PIN", ToastDuration.Short, 14);
                await toast.Show();

                await ShakeAnimation(PinDisplay);

                _enteredPin.Clear();
                UpdatePinDisplay();
                return;
            }
        }

        App.IsAuthenticated = true;
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

    private async void UpdatePinDisplay()
    {
        Frame[] pins = { Pin1, Pin2, Pin3, Pin4 };
        for (int i = 0; i < _pinLength; i++)
        {
            var isFilled = i < _enteredPin.Length;
            var frame = pins[i];

            if (isFilled)
            {
                await frame.ScaleTo(1.2, 80, Easing.CubicOut);
                frame.BackgroundColor = Colors.Black;
                await frame.ScaleTo(1.0, 80, Easing.CubicIn);
            }
            else
            {
                frame.BackgroundColor = Colors.Transparent;
            }
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
