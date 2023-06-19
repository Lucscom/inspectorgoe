﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents;
using GameComponents.Model;
using Microsoft.Maui.Controls.Shapes;
using Client;

namespace InspectorGoe.ViewModels;
public partial class MainViewModel : ObservableObject
{
    private Communicator _com;
    private Validator _validator = new Validator();

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
        // hier startet die connection mit der Logik und dem Server
        _com = new Communicator();

        //init gamestate for test purposes
        //Will be removed in the future
        _com.GameState = _validator.InitPois();

        _com.GameState.MisterX.AvatarImagePath = "dotnet_bot.png";

        _com.GameState.MisterX.BikeTicket = 3;
        _com.GameState.MisterX.BusTicket = 6;
        _com.GameState.MisterX.ScooterTicket = 5;
        _com.GameState.MisterX.UserName = "MisterX";

        Player first = new Player();
        first.ScooterTicket = 3;
        first.BikeTicket = 3;
        first.BusTicket = 3;
        first.UserName = "1. TestSpieler";
        first.AvatarImagePath = "hawk_hirsch.jpg";

        Player second = new Player();
        second.ScooterTicket = 6;
        second.BikeTicket = 6;
        second.BusTicket = 6;
        second.UserName = "2. TestSpieler";
        second.AvatarImagePath = "hawk_nietert.jpg";

        Player third = new Player();
        third.ScooterTicket = 6;
        third.BikeTicket = 6;
        third.BusTicket = 6;
        third.UserName = "3. Testspieler";
        third.AvatarImagePath = "hawk_koch.jpg";

        List<Player> detectives = new List<Player>();
        detectives.Add(first);
        detectives.Add(second);
        detectives.Add(third);

        _com.GameState.Detectives = detectives;

        //Hier muss eine Klasse aufgesetzt werden um das Databinding verwenden zu können.

        string ticketBusPath = "ticket_bus.png";
        string ticketScooterPath = "ticket_scooter.png";
        string ticketBikePath = "ticket_bike.png";
        string ticketblackPath = "ticket_black.png";
        string ticket2xPath = "ticket_2x.png";


        List<String> mrXtickets = new List<String>();
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
        get { return _com.GameState.Detectives; }
    }

    public Player MisterX
    {
        get { return _com.GameState.MisterX; }
    }

    public List<TicketTypeEnum> MrXticketHistoryCollection
    {
        get { return _com.GameState.TicketHistoryMisterX; }
    }

    public PointOfInterest MrXLastKnownPoi
    {
        get { return _com.GameState.MisterXLastKnownPOI; }
    }

    /// <summary>
    /// Take the login credentials and hand them to the communicator
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="serverAdress"></param>
    public void loginPlayer(String userName, String password, String serverAdress)
    {
        _com.initClient(serverAdress);
        var player = new Player(userName, password);
        var statusCreate = _com.CreatePlayerAsync(player);
        var statusLogin = _com.LoginAsync(player);
        // todo: status code checken -->
        // bei fail gamestate neu bekommen und wiederholen
        var statusHub =  _com.RegisterGameHubAsync();

    }

    /// <summary>
    /// Move a player to a destination poi
    /// </summary>
    /// <param name="poi">Point of interes Object</param>
    /// <param name="ticket">ticket Type</param>
    public void movePlayer(PointOfInterest poi, TicketTypeEnum ticket)
    {
        var move = new MovePlayerDto(poi, ticket);
        var moveStatus =_com.MovePlayerAsync(move);
        // todo: status code checken -->
        // bei fail gamestate neu bekommen und wiederholen
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
