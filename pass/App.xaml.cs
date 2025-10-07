using pass.Services;
using pass.Pages;

namespace pass
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public static bool IsAuthenticated { get; set; } = false;
        public static bool IsPickingFile { get; set; } = false;

        public App(string dbPath)
        {
            InitializeComponent();

            Database = new DatabaseService(dbPath);

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnSleep()
        {
            base.OnSleep();

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
