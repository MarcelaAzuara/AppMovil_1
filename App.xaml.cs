using TiAppMovi.Views.Legal;

namespace TiAppMovi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            bool hasAcceptedTerms = Preferences.Get("HasAcceptedTerms", false);

            if (!hasAcceptedTerms)
            {

                MainPage = new NavigationPage(new TermsAndConditionsPage());
            }
            else
            {

                MainPage = new NavigationPage(new Views.LoginPage());
            }
        }
    }
}