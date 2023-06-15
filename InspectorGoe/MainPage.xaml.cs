using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;

namespace InspectorGoe;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();

        BindingContext = new ViewModels.MainViewModel();

    }

}

