using InspectorGoe.Container;
using InspectorGoe.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Layouts;

namespace InspectorGoe;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();

        BindingContext = ViewModels.MainViewModel.GetInstance();
    }




    //protected override void OnSizeAllocated(double width, double height)
    //{
    //    base.OnSizeAllocated(width, height);

    //    if (width > height)
    //    {
    //        Width = width;
    //        Height = width / 1.7828;
    //    }
    //    else
    //    {
    //        Height = height;
    //        Width = Height * 1.7828;
    //    }
    //    setSize();
    //}


}

