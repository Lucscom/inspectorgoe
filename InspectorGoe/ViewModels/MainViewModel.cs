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
    private Controller controller = Controller.GetInstance();
    private List<Player> detectives;
    private Player mrX;
    private List<String> mrXtickets;

    //Variablen für Login
    private string username = string.Empty;
    private string userpassword = string.Empty;
    private string severip = string.Empty;

    //Variablen für Register
    private string usernameregister = string.Empty;
    private string userpasswordregister = string.Empty;
    private string userpasswordregister2 = string.Empty;

    public MainViewModel()
    {
        controller.Initialize(4);
        detectives = new List<Player>(controller.Detectives);
        mrX = controller.MisterX;
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
        first.AvatarImagePath = "hawk_hirsch.jpg";

        var second = detectives[1];
        second.ScooterTicket = 6;
        second.BikeTicket = 6;
        second.BusTicket = 6;
        second.UserName = "2. TestSpieler";
        second.AvatarImagePath = "hawk_nietert.jpg";

        var third = detectives[2];
        third.ScooterTicket = 6;
        third.BikeTicket = 6;
        third.BusTicket = 6;
        third.UserName = "3. Testspieler";
        third.AvatarImagePath = "hawk_koch.jpg";

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
        mrXtickets.Add(ticket2xPath);
        mrXtickets.Add(ticket2xPath);
        mrXtickets.Add(ticketScooterPath);
        mrXtickets.Add(ticketScooterPath);
        mrXtickets.Add(ticketBusPath);
        mrXtickets.Add(ticketBikePath);
        mrXtickets.Add(ticketBusPath);
        mrXtickets.Add(ticketBikePath);
        mrXtickets.Add(ticketBikePath);

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

    //get&set für Register

    public string Usernameregister
    {
        get { return usernameregister; }
        set { usernameregister = value; }
    }

    public string Userpasswordregister
    {
        get { return userpasswordregister; }
        set { userpasswordregister = value; }
    }
    
    public string Userpasswordregister2
    {
        get { return userpasswordregister2; }
        set { userpasswordregister2 = value; }
    }

    //get&set für Login
    public string Username
    {
        get { return username; }
        set { username = value; }
    }

    public string Userpassword
    {
        get { return userpassword; }
        set { userpassword = value; }
    }

    public string Userseverip
    {
        get { return severip; }
        set { severip = value; }
    }

}
