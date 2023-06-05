using CommunityToolkit.Maui.Views;

namespace InspectorGoe;

public partial class LogIn : ContentPage
{
    public LogIn()
    {
        InitializeComponent();
    }

    private void Button_Clicked_LogIn(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Menu());
    }

    private void Button_Clicked_SignUp(object sender, EventArgs e)
    {
        // Show the popup window
        this.ShowPopup(new RegisterPage());

    }
}