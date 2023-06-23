using InspectorGoe.Container;
using InspectorGoe.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Layouts;

namespace InspectorGoe;

public partial class MainPage : ContentPage
{
    private double width = 2600;
    private double height = 1463;


    public MainPage()
	{
		InitializeComponent();

        BindingContext = ViewModels.MainViewModel.GetInstance();
    }



    private void OnButtonZoom(object sender, EventArgs args)
    {

        var tempButton = (Button)sender;

        if (tempButton.Text == "+")
        {
            width += 100;
            height = width / 1.77;

            setSize();

        }
        else if (tempButton.Text == "-" && map.WidthRequest - 100 >= mapContainer.Width)
        {
            width -= 100;
            height = width / 1.77;

            setSize();

        }

        var mapContainerTemp = (PanContainer)mapContainer;

        mapContainerTemp.OnPanUpdated(mapContainerTemp, new PanUpdatedEventArgs(GestureStatus.Running, 0, 0, 0));
        mapContainerTemp.OnPanUpdated(mapContainerTemp, new PanUpdatedEventArgs(GestureStatus.Completed, 0, 0, 0));

    }

    private void setSize()
    {
        absoluteMap.WidthRequest = width;
        absoluteMap.HeightRequest = height;

        map.WidthRequest = width;
        map.HeightRequest = height;

        absolutButtons.WidthRequest = width;
        absolutButtons.HeightRequest = height;
    }


}

