namespace InspectorGoe;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

public partial class LobbyPage : Popup
{
    MainViewModel MVM = MainViewModel.GetInstance();
    public LobbyPage()
	{
		InitializeComponent();
        BindingContext = MVM;
    }

    private void StartGame_Clicked(object sender, EventArgs e) => Close(true);
}