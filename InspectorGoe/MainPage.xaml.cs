using InspectorGoe.Container;
using InspectorGoe.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Layouts;

namespace InspectorGoe;

public partial class MainPage : ContentPage
{
    private double Width = 2600;
    private double Height = 1458;


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
            Width += 100;
            Height = Width / 1.7828;

            setSize();

        }
        else if (tempButton.Text == "-" && map.WidthRequest - 100 >= mapContainer.Width)
        {
            Width -= 100;
            Height = Width / 1.7828;

            setSize();

        }

        var mapContainerTemp = (PanContainer)mapContainer;

        mapContainerTemp.OnPanUpdated(mapContainerTemp, new PanUpdatedEventArgs(GestureStatus.Running, 0, 0, 0));
        mapContainerTemp.OnPanUpdated(mapContainerTemp, new PanUpdatedEventArgs(GestureStatus.Completed, 0, 0, 0));

    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > height)
        {
            Width = width;
            Height = width / 1.7828;
        }
        else
        {
            Height = height;
            Width = Height * 1.7828;
        }
        setSize();
    }

    private void setSize()
    {
        absoluteMap.WidthRequest = Width;
        absoluteMap.HeightRequest = Height;

        map.WidthRequest = Width;
        map.HeightRequest = Height;

        absolutButtons.WidthRequest = Width;
        absolutButtons.HeightRequest = Height;

        absolutMisterX.WidthRequest = Width;
        absolutMisterX.HeightRequest = Height;
    }


}

