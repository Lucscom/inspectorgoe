namespace InspectorGoe;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

public partial class LobbyPage : Popup
{
    public LobbyPage()
	{
		InitializeComponent();
        BindingContext = MainViewModel.GetInstance();
    }

}