using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents;
using GameComponents.Model;
using Microsoft.Maui.Controls.Shapes;

namespace InspectorGoe.ViewModels;
public partial class MainViewModel : ObservableObject
{
    private GameState gameState;
    private List<Player> detectives;
    private Player mrX;
    private List<String> mrXtickets;

    public MainViewModel()
    {
        detectives = new List<Player>(gameState.Detectives);
        mrX = gameState.MisterX;
        mrX.AvatarImagePath = "dotnet_bot.png";

        mrX.BikeTicket = 3;
        mrX.BusTicket = 6;
        mrX.ScooterTicket = 5;
        mrX.UserName = "MisterX";

        var first = detectives.First();
        first.ScooterTicket = 3;
        first.BikeTicket = 3;
        first.BusTicket = 3;
        first.UserName = "1. TestSpieler";
        first.AvatarImagePath = "dotnet_bot.png";

        var second = detectives[1];
        second.ScooterTicket = 6;
        second.BikeTicket = 6;
        second.BusTicket = 6;
        second.UserName = "2. TestSpieler";
        second.AvatarImagePath = "dotnet_bot.png";

        var third = detectives[2];
        third.ScooterTicket = 6;
        third.BikeTicket = 6;
        third.BusTicket = 6;
        third.UserName = "3. Testspieler";
        third.AvatarImagePath = "dotnet_bot.png";

        //Hier muss eine Klasse aufgesetzt werden um das Databinding verwenden zu können.

        string ticketBusPath = "ticket_bus.png";
        string ticketScooterPath = "ticket_scooter.png";
        string ticketBikePath = "ticket_bike.png";
        string ticketblackPath = "ticket_black.png";
        string ticket2xPath = "ticket_2x.png";


        mrXtickets = new List<String>();
        mrXtickets.Add(ticketBikePath);
        mrXtickets.Add(ticketBusPath);
        mrXtickets.Add(ticketScooterPath);
        mrXtickets.Add(ticketblackPath);
        //mrXtickets.Add(ticket2xPath);
        //mrXtickets.Add(ticket2xPath);
        //mrXtickets.Add(ticketScooterPath);
        //mrXtickets.Add(ticketScooterPath);
        //mrXtickets.Add(ticketBusPath);
        //mrXtickets.Add(ticketBikePath);
        //mrXtickets.Add(ticketBusPath);
        //mrXtickets.Add(ticketBikePath);
        //mrXtickets.Add(ticketBikePath);

    }


    public List<Player> DetectivesCollection
    {
        get { return detectives; }
        set { detectives = value; }
    }

    public Player MisterX
    {
        get { return mrX; }
    }

    public List<String> MrXticketsCollection
    {
        get { return mrXtickets; }
    }
 }
