using InspectorGoe.Container;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;

namespace InspectorGoe;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();

        BindingContext = ViewModels.MainViewModel.GetInstance();

    }

    private void OnButtonZoom(object sender, EventArgs args)
    {

        var tempButton = (Button)sender;

        if (tempButton.Text == "+" )
        {
            map.WidthRequest += 100;
            map.HeightRequest = map.WidthRequest / 1.777;

            absoluteMap.WidthRequest = map.WidthRequest;
            absoluteMap.HeightRequest = map.HeightRequest;

        }
        else if (tempButton.Text == "-" && map.WidthRequest - 100 >= mapContainer.Width)
        {
            map.WidthRequest -= 100;
            map.HeightRequest = map.WidthRequest / 1.777;

            absoluteMap.WidthRequest = map.WidthRequest;
            absoluteMap.HeightRequest = map.HeightRequest;

        }

        var mapContainerTemp = (PinchPanContainer)mapContainer;

        mapContainerTemp.OnPanUpdated(mapContainerTemp, new PanUpdatedEventArgs(GestureStatus.Running, 0, 0, 0));
        mapContainerTemp.OnPanUpdated(mapContainerTemp, new PanUpdatedEventArgs(GestureStatus.Completed, 0, 0, 0));

    }

}

