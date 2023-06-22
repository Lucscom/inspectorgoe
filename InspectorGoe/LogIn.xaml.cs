using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;
using System.Runtime.CompilerServices;

namespace InspectorGoe;

public partial class LogIn : ContentPage
{
    public LogIn()
    {
        InitializeComponent();
        BindingContext = MainViewModel.GetInstance();
    }
} 