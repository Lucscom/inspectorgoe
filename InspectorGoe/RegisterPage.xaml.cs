namespace InspectorGoe;
using CommunityToolkit.Maui.Views;

public partial class RegisterPage : Popup
{
	public RegisterPage()
	{
        InitializeComponent();
    }

    private void Button_Clicked_Register(object sender, EventArgs e)
    {
        Close();
    }
}