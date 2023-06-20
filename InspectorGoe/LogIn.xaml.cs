using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;
using System.Runtime.CompilerServices;

namespace InspectorGoe;

public partial class LogIn : ContentPage
{
    MainViewModel MVM = new MainViewModel();

    public LogIn()
    {
        InitializeComponent();

        BindingContext = MVM;
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

    // m�sste in das MainViewModel aber ich hab kp wie ich die Aktivierung des Buttons da reinbekomme
    private void LogInActivation(object sender, EventArgs e)
    {
        if (MVM.Username != string.Empty && MVM.Userpassword != string.Empty && MVM.Userseverip != string.Empty) { LogInButton.IsEnabled = true; }
        else { LogInButton.IsEnabled = false; }
    }

} 