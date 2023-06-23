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
using System.Numerics;
using Microsoft.Maui.Controls.Xaml;
using System.Windows.Input;

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

    #region Variables

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

    // Variablen für AvatarPage
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    private ImageButton choice;

    [ObservableProperty]
    private List<Player> detectives = new List<Player>();

    [ObservableProperty]
    private List<PointOfInterest> pois = new List<PointOfInterest>();

    [ObservableProperty]
    public List<PointsOfInterestView> poisLocation = new List<PointsOfInterestView>();

    [ObservableProperty]
    private Player misterX = null;

    [ObservableProperty]
    private List<TicketsView> mrXticketHistory = new List<TicketsView>();

    [ObservableProperty]
    private PointOfInterest mrXLastKnownPoi = null;

    #endregion


    private MainViewModel()
    {
        // hier startet die connection mit der Logik und dem Server
        _com = new Communicator();
        _com.UpdateGameStateEvent += ComUpdateGameState;
    }


    private void ComUpdateGameState(object sender, EventArgs e)
    {
        detectives = _com.GameState.Detectives;
        pois = _com.GameState.PointsOfInterest;
        misterX = _com.GameState.MisterX;
        mrXLastKnownPoi = _com.GameState.MisterXLastKnownPOI;

        // Change List MisterX-Tickethistory in image-pahts
        _com.GameState.TicketHistoryMisterX.Add(TicketTypeEnum.Bus);
        _com.GameState.TicketHistoryMisterX.Add(TicketTypeEnum.Scooter);

        mrXticketHistory = new List<TicketsView>();
        foreach (TicketTypeEnum ticket in _com.GameState.TicketHistoryMisterX)
        {
            TicketsView tempTicket = new();
            switch (ticket)
            {
                case TicketTypeEnum.Bus:
                    tempTicket.ImagePath = "ticket_bus.png";
                    tempTicket.MisterXDiscoverPosition = false;
                    mrXticketHistory.Add(tempTicket);
                    break;
                case TicketTypeEnum.Bike:
                    tempTicket.ImagePath = "ticket_bike.png";
                    tempTicket.MisterXDiscoverPosition = false;
                    mrXticketHistory.Add(tempTicket);
                    break;
                case TicketTypeEnum.Scooter:
                    tempTicket.ImagePath = "ticket_scooter.png";
                    tempTicket.MisterXDiscoverPosition = false;
                    mrXticketHistory.Add(tempTicket);
                    break;
                case TicketTypeEnum.Black:
                    tempTicket.ImagePath = "ticket_black.png";
                    tempTicket.MisterXDiscoverPosition = false;
                    mrXticketHistory.Add(tempTicket);
                    break;
                default:
                    tempTicket.ImagePath = "";
                    tempTicket.MisterXDiscoverPosition = false;
                    mrXticketHistory.Add(tempTicket);
                    break;

            }
        }

        // Add 24 dummys
        for(int i = _com.GameState.TicketHistoryMisterX.Count-1; i<24; i++)
        {
            TicketsView tempTicket = new();
            tempTicket.ImagePath = "ticket_placeholder.png";
            tempTicket.MisterXDiscoverPosition = false;
            mrXticketHistory.Add(tempTicket);
        }
        int number = 0;
        foreach(TicketsView ticket in mrXticketHistory)
        {
            if (number == 3 || number == 8 || number == 13 || number == 18 || number == 24)
                ticket.MisterXDiscoverPosition = true;
            number++;
        }


        MrXticketHistory.Reverse();


        

        PoisLocation = new List<PointsOfInterestView>();
        foreach (PointOfInterest poi in pois)
        {
            PointsOfInterestView temp = new PointsOfInterestView();
            temp.Location = new Rect(poi.Location.X / 8914, poi.Location.Y / 5000, 0.0168, 0.03);
            temp.Number = poi.Number;
            PoisLocation.Add(temp);
        }
    }

    /// <summary>
    /// Take the login credentials and hand them to the communicator
    /// </summary>
    public async void loginPlayer()
    {
        _com.initClient(Userseverip);
        var player = new Player(Username, Userpassword);
        var statusCrate = await _com.CreatePlayerAsync(player);
        var statusLogin = await _com.LoginAsync(player);
        // todo: status code checken -->
        // bei fail gamestate neu bekommen und wiederholen
        var statusHub = _com.RegisterGameHubAsync();

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
        loginPlayer();
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
        Shell.Current.ShowPopup(new AvatarPage());
    }

    /// <summary>
    /// Navigation from MenuPage to MainPage
    /// </summary>
    [RelayCommand]
    private void JoinGame()
    {
        Shell.Current.ShowPopup(new AvatarPage());
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
    [RelayCommand(CanExecute = nameof(AvatarIsSelected))]
    private void Start()
    {

        //senden des ausgewälten Avatars an den Server

        Shell.Current.ShowPopup(new LobbyPage());
    }

    /// <summary>
    /// Überprüfe ob ein Avatar ausgewählt wurde
    /// </summary>
    /// <returns></returns>

    private bool AvatarIsSelected()
    {

        if (Choice!= null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #endregion

    #region LobbyPage

    [RelayCommand]
    async Task StartGame()
    {
        startGame();
        _com.gameStateInitEvent.WaitOne();
        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
    }

    #endregion

    #region MainPage

    [RelayCommand]
    private void Button_Clicked_Poi(int number)
    {
        Console.Write(number);
    }

    #endregion

    #endregion
}
