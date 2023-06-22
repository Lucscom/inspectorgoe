namespace InspectorGoe;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

public partial class RegisterPage : Popup
{
    MainViewModel MVVM = MainViewModel.GetInstance();

    public RegisterPage()
	{
        InitializeComponent();
        BindingContext = MVVM;
    }

    private void Button_Clicked_Register(object sender, EventArgs e)
    {
        Close();
    }

    private void RegisterActivation(object sender, TextChangedEventArgs e)
    {
        if (MVVM.Usernameregister != string.Empty && MVVM.Userpasswordregister != string.Empty && MVVM.Userpasswordregister2 != string.Empty) { RegisterButton.IsEnabled = true; }
        else { RegisterButton.IsEnabled = false; }
    }

}