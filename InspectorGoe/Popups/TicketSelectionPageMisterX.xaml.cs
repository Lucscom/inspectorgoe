using CommunityToolkit.Maui.Views;

namespace InspectorGoe;

public partial class TicketSelectionPageMisterX : Popup
{
	public TicketSelectionPageMisterX()
	{
		InitializeComponent();

        BindingContext = ViewModels.MainViewModel.GetInstance();
    }
}