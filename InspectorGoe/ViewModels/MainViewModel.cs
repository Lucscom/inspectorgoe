using Client;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameComponents;
using GameComponents.Model;
using InspectorGoe.Model;

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


    // Variablen für MainPage
    [ObservableProperty]
    private List<Player> detectives = new List<Player>();

    [ObservableProperty]
    private List<PointOfInterest> pois = new List<PointOfInterest>();

    [ObservableProperty]
    public List<PointOfInterestView> poisLocation = new List<PointOfInterestView>();

    [ObservableProperty]
    private Player misterX = null;

    [ObservableProperty]
    private List<TicketsView> mrXticketHistory = new List<TicketsView>();

    [ObservableProperty]
    private List<PointOfInterestView> mrXLastKnownPoi = new List<PointOfInterestView>();


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

        // ##### For Debugging #####
        _com.GameState.MisterXLastKnownPOI = _com.GameState.PointsOfInterest[0];
        // ##### For Debugging #####

        mrXLastKnownPoi.Add(PoiConverter(_com.GameState.MisterXLastKnownPOI, 170, Colors.Purple));

        // Ticket History from Mister
        mrXticketHistory = new List<TicketsView>();
        foreach (TicketTypeEnum ticket in _com.GameState.TicketHistoryMisterX)
        {
            TicketsView tempTicket = new();
            switch (ticket)
            {
                case TicketTypeEnum.Bus:
                    tempTicket.ImagePath = "ticket_bus.png";
                    mrXticketHistory.Add(tempTicket);
                    break;
                case TicketTypeEnum.Bike:
                    tempTicket.ImagePath = "ticket_bike.png";
                    mrXticketHistory.Add(tempTicket);
                    break;
                case TicketTypeEnum.Scooter:
                    tempTicket.ImagePath = "ticket_scooter.png";
                    mrXticketHistory.Add(tempTicket);
                    break;
                case TicketTypeEnum.Black:
                    tempTicket.ImagePath = "ticket_black.png";
                    mrXticketHistory.Add(tempTicket);
                    break;
                default:
                    tempTicket.ImagePath = "";
                    mrXticketHistory.Add(tempTicket);
                    break;

            }
        }

        // Fill up with dummys
        for(int i = _com.GameState.TicketHistoryMisterX.Count-1; i<23; i++)
        {
            TicketsView tempTicket = new();
            tempTicket.ImagePath = "ticket_placeholder.png";
            mrXticketHistory.Add(tempTicket);

        }

        // setting up all other variables
        int number = 0;
        foreach(TicketsView ticket in mrXticketHistory)
        {
            ticket.NumberRound = number + 1;
            if (number < _com.GameState.TicketHistoryMisterX.Count)
            {
                ticket.NumberColor = Colors.Transparent;
                ticket.NumberFrameColor = Colors.Transparent;
            }
            else
            {
                ticket.NumberColor = Colors.White;
                ticket.NumberFrameColor = Colors.Gray;
            }


            if (number == 2 || number == 7 || number == 12 || number == 17 || number == 23)
                ticket.BorderThickness = 4;
            else
                ticket.BorderThickness = 0;


            number++;
        }

        MrXticketHistory.Reverse();


        // Buttons POIS
        PoisLocation = new List<PointOfInterestView>();
        foreach (PointOfInterest poi in pois)
        {
            PoisLocation.Add(PoiConverter(poi, 150));
        }
    }


    /// <summary>
    /// Convert from POI in the model to POI in the view
    /// </summary>
    /// <param name="poi">Point of interes Object</param>
    /// <returns>PointOfInterestView</returns>
    private PointOfInterestView PoiConverter(PointOfInterest poi, int size,  Color objectColor = null)
    {
        PointOfInterestView temp = new PointOfInterestView();
        temp.Location = new Rect(poi.Location.X / 8914, poi.Location.Y / 5000, size / 8914.00, size / 5000.00);
        temp.Number = poi.Number;
        temp.ObjectColor = objectColor ?? Colors.Transparent;
        return temp;
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
