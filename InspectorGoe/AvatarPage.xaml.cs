namespace InspectorGoe;
using CommunityToolkit.Maui.Views;
using InspectorGoe.ViewModels;

public partial class AvatarPage : Popup
{
    MainViewModel MVM = MainViewModel.GetInstance();

    ImageButton Choice;

    public AvatarPage()
    {
        InitializeComponent();
        BindingContext = MVM;
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (Choice == grothausmann) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = grothausmann; MVM.AvatarImagePath = "hawk_grothausmann.jpg"; }
        Choosing();
    }

    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        if (Choice == hadeler) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = hadeler; MVM.AvatarImagePath = "hawk_hadeler.jpg"; }
        Choosing();
    }

    private void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        if (Choice == hirsch) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = hirsch; MVM.AvatarImagePath = "hawk_hirsch.jpg"; }
        Choosing();
    }

    private void ImageButton_Clicked_3(object sender, EventArgs e)
    {
        if (Choice == ibental) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = ibental; MVM.AvatarImagePath = "hawk_ibental.jpg"; }
        Choosing();
    }

    private void ImageButton_Clicked_4(object sender, EventArgs e)
    {
        if (Choice == koch) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = koch; MVM.AvatarImagePath = "hawk_koch.jpg"; }
        Choosing();
    }

    private void ImageButton_Clicked_5(object sender, EventArgs e)
    {
        if (Choice == neunheuser) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = neunheuser; MVM.AvatarImagePath = "hawk_neunheuser.jpg"; }
        Choosing();
    }

    private void ImageButton_Clicked_6(object sender, EventArgs e)
    {
        if (Choice == nietert) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = nietert; MVM.AvatarImagePath = "hawk_nietert.jpg"; }
        Choosing();
    }

    private void ImageButton_Clicked_7(object sender, EventArgs e)
    {
        if (Choice == wienecke) { Choice = null; MVM.AvatarImagePath = ""; } else { Choice = wienecke; MVM.AvatarImagePath = "hawk_wienecke.jpg"; }
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