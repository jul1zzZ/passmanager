using pass.Services;
using pass.Pages;

namespace pass
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        // Флаг: вошёл ли пользователь
        public static bool IsAuthenticated { get; set; } = false;
        public static bool IsPickingFile { get; set; } = false;

        public App(string dbPath)
        {
            InitializeComponent();

            Database = new DatabaseService(dbPath);

            // При запуске всегда начинаем с авторизации
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            // Если мы не выбираем файл — сбрасываем авторизацию
            if (!IsPickingFile)
                IsAuthenticated = false;
        }

        protected override void OnResume()
        {
            base.OnResume();
            RequireAuth();
        }

        private void RequireAuth()
        {
            if (!IsAuthenticated)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
