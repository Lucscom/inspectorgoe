namespace InspectorGoe;
using CommunityToolkit.Maui.Views;

public partial class GameStartPage : Popup
{
    ImageButton Choice;

    public GameStartPage()
	{
		InitializeComponent();
	}

    void Button_Clicked(object sender, EventArgs e) => Close(true);


    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (Choice == grothausmann) { Choice = null; } else { Choice = grothausmann; }
        Choosing();
    }

    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        if (Choice == hadeler) { Choice = null; } else { Choice = hadeler; }
        Choosing();
    }

    private void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        if (Choice == hirsch) { Choice = null; } else { Choice = hirsch; }
        Choosing();
    }

    private void ImageButton_Clicked_3(object sender, EventArgs e)
    {
        if (Choice == ibental) { Choice = null; } else { Choice = ibental; }
        Choosing();
    }

    private void ImageButton_Clicked_4(object sender, EventArgs e)
    {
        if (Choice == koch) { Choice = null; } else { Choice = koch; }
        Choosing();
    }

    private void ImageButton_Clicked_5(object sender, EventArgs e)
    {
        if (Choice == neunheuser) { Choice = null; } else { Choice = neunheuser; }
        Choosing();
    }

    private void ImageButton_Clicked_6(object sender, EventArgs e)
    {
        if (Choice == nietert) { Choice = null; } else { Choice = nietert; }
        Choosing();
    }

    private void ImageButton_Clicked_7(object sender, EventArgs e)
    {
        if (Choice == wienecke) { Choice = null; } else { Choice = wienecke; }
        Choosing();
    }

    private void Choosing()
    { 
        
        if (Choice != null)
        {
            grothausmann.Opacity = 0.5;
            hadeler.Opacity = 0.5;
            hirsch.Opacity = 0.5;
            ibental.Opacity = 0.5;
            koch.Opacity = 0.5;
            neunheuser.Opacity = 0.5;
            nietert.Opacity = 0.5;
            wienecke.Opacity = 0.5;
            Choice.Opacity = 1;
        }
        
        else
        {
            grothausmann.Opacity = 1;
            hadeler.Opacity = 1;
            hirsch.Opacity = 1;
            ibental.Opacity = 1;
            koch.Opacity = 1;
            neunheuser.Opacity = 1;
            nietert.Opacity = 1;
            wienecke.Opacity = 1;
        }




          
    
    
    }



}