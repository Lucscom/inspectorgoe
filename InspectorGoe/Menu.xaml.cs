using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

namespace InspectorGoe;

public partial class Menu : ContentPage
{
	public Menu()
	{
		InitializeComponent();
        BindingContext = MainViewModel.GetInstance();
	}

}