using Microsoft.Maui.Controls;

namespace InspectorGoe;

public partial class MainPage : ContentPage
{


    public MainPage()
	{
		InitializeComponent();

        BindingContext = new ViewModels.MainViewModel();

    }


}

