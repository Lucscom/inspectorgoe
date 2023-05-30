﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents;
using Microsoft.Maui.Controls.Shapes;

namespace InspectorGoe.ViewModels;
public partial class MainViewModel : ObservableObject
{
    private Controller controller = Controller.GetInstance();
    private List<Player> detectives;
    private List<String> mrXtickets;

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

        //Hier muss eine Klasse aufgesetzt werden um das Databinding verwenden zu können.

        string BusTicketURL = "dotnet_bot.png";
        string ScooterTicketURL = "dotnet_bot.png";
        string BikeTicketURL = "dotnet_bot.png";

        mrXtickets = new List<String>();
        mrXtickets.Add(BusTicketURL);
        mrXtickets.Add(BikeTicketURL);
        mrXtickets.Add(ScooterTicketURL);
        mrXtickets.Add(ScooterTicketURL);
        mrXtickets.Add(BikeTicketURL);
        mrXtickets.Add(BusTicketURL);
    }

    public List<Player> DetectivesCollection
    {
        get { return detectives; }
        set { detectives = value; }
    }

    public List<String> MrXticketsCollection
    {
        get { return mrXtickets; }
        set { mrXtickets = value; }
    }


}
