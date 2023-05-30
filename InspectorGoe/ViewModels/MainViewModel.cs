using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents;

namespace InspectorGoe.ViewModels;
public partial class MainViewModel : ObservableObject
{
    private Controller controller = Controller.GetInstance();
    private List<Player> detectives;

    public MainViewModel()
    {
        controller.Initialize(4);

        detectives = new List<Player>(controller.Detectives);

        var first = detectives.First();
        first.ScooterTicket = 3;
        first.BikeTicket = 3;
        first.BusTicket = 3;
        first.Name = "1. TestSpieler";
        first.AvatarImagePath = "dotnet_bot.png";

        var second= detectives[1];
        second.ScooterTicket = 6;
        second.BikeTicket = 6;
        second.BusTicket = 6;
        second.Name = "2. TestSpieler";
        second.AvatarImagePath = "dotnet_bot.png";

    }

    public List<Player> DetectivesCollection
    {
        get { return detectives; }
        set { detectives = value; }
    }




}
