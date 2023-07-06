using CommunityToolkit.Maui.Views;

namespace InspectorGoe;

public partial class GameEndPage : Popup
{
	public GameEndPage()
	{
		InitializeComponent();

        BindingContext = ViewModels.MainViewModel.GetInstance();
    }
}