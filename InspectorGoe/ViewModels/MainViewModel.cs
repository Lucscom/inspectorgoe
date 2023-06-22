using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents;
using GameComponents.Model;
using Microsoft.Maui.Controls.Shapes;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using Client;

namespace InspectorGoe.ViewModels;
public partial class MainViewModel : ObservableObject
{
    #region Singleton

    private static MainViewModel _instance;
    /// <summary>
    /// Creates one instance of MainVieModel or returns it
    /// </summary>
    /// <returns>Unique MainVieModel instance</returns>
    public static MainViewModel GetInstance() { return _instance ??= new MainViewModel(); }
    #endregion

    private Communicator _com;

    //Variablen für Login
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(Button_Clicked_LogInCommand))]
    string username = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(Button_Clicked_LogInCommand))]
    string userpassword = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(Button_Clicked_LogInCommand))]
    string userseverip = string.Empty;

    //Variablen für Register
    [ObservableProperty]
    private string usernameregister = string.Empty;
    [ObservableProperty]
    private string userpasswordregister = string.Empty;
    [ObservableProperty]
    private string userpasswordregister2 = string.Empty;

    // Variablen für GameStartPage
    [ObservableProperty]
    private ImageButton choice;


    private MainViewModel()
    {
        // hier startet die connection mit der Logik und dem Server
        _com = new Communicator();

        //Player mrX = new Player();

        //mrX.AvatarImagePath = "dotnet_bot.png";

        //mrX.BikeTicket = 3;
        //mrX.BusTicket = 6;
        //mrX.ScooterTicket = 5;
        //mrX.UserName = "MisterX";

        ////_com.GameState.MisterX = mrX;

        //Player first = new Player();
        //first.ScooterTicket = 3;
        //first.BikeTicket = 3;
        //first.BusTicket = 3;
        //first.UserName = "1. TestSpieler";
        //first.AvatarImagePath = "hawk_hirsch.jpg";

        //Player second = new Player();
        //second.ScooterTicket = 6;
        //second.BikeTicket = 6;
        //second.BusTicket = 6;
        //second.UserName = "2. TestSpieler";
        //second.AvatarImagePath = "hawk_nietert.jpg";

        //Player third = new Player();
        //third.ScooterTicket = 6;
        //third.BikeTicket = 6;
        //third.BusTicket = 6;
        //third.UserName = "3. Testspieler";
        //third.AvatarImagePath = "hawk_koch.jpg";

        //List<Player> detectives = new List<Player>();
        //detectives.Add(first);
        //detectives.Add(second);
        //detectives.Add(third);

        //_com.GameState.Detectives = detectives;

        ////Hier muss eine Klasse aufgesetzt werden um das Databinding verwenden zu können.

        //string ticketBusPath = "ticket_bus.png";
        //string ticketScooterPath = "ticket_scooter.png";
        //string ticketBikePath = "ticket_bike.png";
        //string ticketblackPath = "ticket_black.png";
        //string ticket2xPath = "ticket_2x.png";


        //List<String> mrXtickets = new List<String>();
        //mrXtickets.Add(ticketBikePath);
        //mrXtickets.Add(ticketBusPath);
        //mrXtickets.Add(ticketScooterPath);
        //mrXtickets.Add(ticketblackPath);
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

    public List<PointOfInterest> AllPointOfInterest
    {
        get { return _com.GameState.PointsOfInterest; }
    }

    /// <summary>
    /// Take the login credentials and hand them to the communicator
    /// </summary>
    public void loginPlayer()
    {
        _com.initClient(Userseverip);
        var player = new Player(Username, Userpassword);
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

    /// <summary>
    /// Send start game to server
    /// </summary>
    public void startGame()
    {
        var status = _com.StartGameAsync();
    }





    #region Pages

    #region LogIn

    /// <summary>
    /// LogIn Logic
    /// </summary>
    /// <returns></returns>
    [RelayCommand(CanExecute = nameof(LogInActivation))]
    async Task Button_Clicked_LogIn()
    {
        await App.Current.MainPage.Navigation.PushAsync(new Menu());
    }

    /// <summary>
    /// Logic for LogIn Activation
    /// </summary>
    /// <returns>Bool</returns>
    private bool LogInActivation()
    {
        if (Username != string.Empty && Userpassword != string.Empty && Userseverip != string.Empty) { return true; }
        else { return false; }
    }

    /// <summary>
    /// Register Logic
    /// </summary>
    [RelayCommand]
    private void Button_Clicked_Register()
    {
        Shell.Current.ShowPopup(new RegisterPage());
    }

    #endregion

    #region MenuPage
    /// <summary>
    /// Navigation from MenuPage to GameStartPage
    /// </summary>
    [RelayCommand]
    private void CreateNewGame()
    {
        Shell.Current.ShowPopup(new AvaterPage());
    }

    /// <summary>
    /// Navigation from MenuPage to MainPage
    /// </summary>
    [RelayCommand]
    private void JoinGame()
    {
        Shell.Current.ShowPopup(new AvaterPage());
    }

    /// <summary>
    /// Navigation from MenuPage to LogInPage
    /// </summary>
    [RelayCommand]
    async Task LogOut()
    {
        await App.Current.MainPage.Navigation.PushAsync(new LogIn());
    }

    #endregion

    #region AvatarPage

    /// <summary>
    /// Navigation from GameStartPage to LobbyPage
    /// </summary>
    [RelayCommand]
    private void StartLobby()
    {
        Shell.Current.ShowPopup(new LobbyPage());
    }

    // hier muss noch die Logik für die Avatare rein


    #endregion

    #region LobbyPage

    [RelayCommand]
    async Task StartGame()
    {
        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
    }

    #endregion

    #endregion
}
