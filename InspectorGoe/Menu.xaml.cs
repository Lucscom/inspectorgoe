using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace InspectorGoe;

public partial class Menu : ContentPage
{
	public Menu()
	{
		InitializeComponent();
	}

    private async void Button_Clicked_Erstellen(object sender, EventArgs e)
    {
        var popup = new GameStartPage();
        var result = await this.ShowPopupAsync(popup);

        if (result is bool boolResult && boolResult)
        {
            StartGame_Clicked();
        }
        else
        {
            // No was tapped
        }

    }
    private void Button_Clicked_Betreten(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }

    private async void StartGame_Clicked()
    {
        var popup = new LobbyPage();
        var result = await this.ShowPopupAsync(popup);

        if (result is bool boolResult && boolResult)
        {
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            // No was tapped
        }
    }
}