namespace InspectorGoe;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

public partial class RegisterPage : Popup
{
    MainViewModel MVM = new MainViewModel();

    public RegisterPage()
	{
        InitializeComponent();
        BindingContext = MVM;
    }

    private void Button_Clicked_Register(object sender, EventArgs e)
    {
        Close();
    }

    private void RegisterActivation(object sender, TextChangedEventArgs e)
    {
        if (MVM.Usernameregister != string.Empty && MVM.Userpasswordregister != string.Empty && MVM.Userpasswordregister2 != string.Empty) { RegisterButton.IsEnabled = true; }
        else { RegisterButton.IsEnabled = false; }
    }

}