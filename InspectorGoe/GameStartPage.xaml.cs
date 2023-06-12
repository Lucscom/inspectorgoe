namespace InspectorGoe;
using CommunityToolkit.Maui.Views;

public partial class GameStartPage : Popup
{
	public GameStartPage()
	{
		InitializeComponent();
	}

    void Button_Clicked(object sender, EventArgs e) => Close(true);

}