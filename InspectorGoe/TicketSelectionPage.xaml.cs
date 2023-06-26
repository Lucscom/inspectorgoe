using CommunityToolkit.Maui.Views;

namespace InspectorGoe;

public partial class TicketSelectionPage : Popup
{
	public TicketSelectionPage()
	{
		InitializeComponent();

        BindingContext = ViewModels.MainViewModel.GetInstance();
    }
}