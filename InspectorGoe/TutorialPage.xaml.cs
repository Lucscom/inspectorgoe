namespace InspectorGoe;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

public partial class TutorialPage : Popup
{

    public TutorialPage()
	{
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}