namespace InspectorGoe;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

public partial class AvatarPage : Popup
{
    MainViewModel MVM = MainViewModel.GetInstance();

    public AvatarPage()
    {
        InitializeComponent();
        BindingContext = MVM;
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (MVM.Choice == grothausmann) { MVM.Choice = null; } else { MVM.Choice = grothausmann; }
        Choosing();
    }

    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        if (MVM.Choice == hadeler) { MVM.Choice = null; } else { MVM.Choice = hadeler; }
        Choosing();
    }

    private void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        if (MVM.Choice == hirsch) { MVM.Choice = null; } else { MVM.Choice = hirsch; }
        Choosing();
    }

    private void ImageButton_Clicked_3(object sender, EventArgs e)
    {
        if (MVM.Choice == ibental) { MVM.Choice = null; } else { MVM.Choice = ibental; }
        Choosing();
    }

    private void ImageButton_Clicked_4(object sender, EventArgs e)
    {
        if (MVM.Choice == koch) { MVM.Choice = null; } else { MVM.Choice = koch; }
        Choosing();
    }

    private void ImageButton_Clicked_5(object sender, EventArgs e)
    {
        if (MVM.Choice == neunheuser) { MVM.Choice = null; } else { MVM.Choice = neunheuser; }
        Choosing();
    }

    private void ImageButton_Clicked_6(object sender, EventArgs e)
    {
        if (MVM.Choice == nietert) { MVM.Choice = null; } else { MVM.Choice = nietert; }
        Choosing();
    }

    private void ImageButton_Clicked_7(object sender, EventArgs e)
    {
        if (MVM.Choice == wienecke) { MVM.Choice = null; } else { MVM.Choice = wienecke; }
        Choosing();
    }

    private void Choosing()
    { 
        
        if (MVM.Choice != null)
        {
            grothausmann.Opacity = 0.5;
            hadeler.Opacity = 0.5;
            hirsch.Opacity = 0.5;
            ibental.Opacity = 0.5;
            koch.Opacity = 0.5;
            neunheuser.Opacity = 0.5;
            nietert.Opacity = 0.5;
            wienecke.Opacity = 0.5;
            MVM.Choice.Opacity = 1;
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