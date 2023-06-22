using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

namespace InspectorGoe;

public partial class Menu : ContentPage
{
    MainViewModel MVM = MainViewModel.GetInstance();

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
            StartLobby_Clicked();
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

    private async void StartLobby_Clicked()
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