using Client;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameComponents;
using GameComponents.Model;
using InspectorGoe.Model;
using System.Collections.ObjectModel;

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
    string userseverip = "https://localhost:5000";

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
    private double widthMap = 2600;

    [ObservableProperty]
    private double heightMap = 1458.38;

    [ObservableProperty]
    private ObservableCollection<PointOfInterestView> playerLocation = new ObservableCollection<PointOfInterestView>();

    [ObservableProperty]
    private ObservableCollection<Player> detectives = new ObservableCollection<Player>();

    [ObservableProperty]
    private Player misterX = new Player();

    [ObservableProperty]
    private ObservableCollection<TicketsView> mrXticketHistory = new ObservableCollection<TicketsView>();


    // POIS
    [ObservableProperty]
    public ObservableCollection<PointOfInterestView> poiButtons = new ObservableCollection<PointOfInterestView>();

    [ObservableProperty]
    public ObservableCollection<PointOfInterestView> poiFrames = new ObservableCollection<PointOfInterestView>();


    private TicketSelectionPage ticketSelectionPage;

    [ObservableProperty]
    private ObservableCollection<TicketSelection> ticketSelection = new ObservableCollection<TicketSelection>();


    #endregion


    private MainViewModel()
    {
        // hier startet die connection mit der Logik und dem Server
        _com = new Communicator();
        _com.UpdateGameStateEvent += ComUpdateGameState;
    }


    private void ComUpdateGameState(object sender, EventArgs e)
    {
        // Set  Player Cards
        Detectives = new ObservableCollection<Player>();
        foreach (Player detective in _com.GameState.Detectives)
        {
            Detectives.Add(detective);
        }

        // MisterX
        MisterX = _com.GameState.MisterX;


        // Set Player Position
        fillPlayerLocation();


        // Ticket History from Mister
        fillTicketHistoryList();


        // Point of Interest Buttons
        fillPoiObjects();
    }


    /// <summary>
    /// fill up the player location list
    /// </summary>
    private void fillPlayerLocation()
    {
        // Detectives
        PlayerLocation = new ObservableCollection<PointOfInterestView>();
        foreach (Player detective in _com.GameState.Detectives)
        {
            PlayerLocation.Add(PoiConverter(detective.Position, 210, Colors.Red));
        }

        // Abfrage ob dieser Client misterX ist wenn ja dann wird die Position angezeigt
        PlayerLocation.Add(PoiConverter(_com.GameState.MisterX.Position, 210, Colors.Purple));

        // Wenn nicht dann wird die letzte bekannte Position angezeigt
        //PlayerLocation.Add(PoiConverter(_com.GameState.MisterXLastKnownPOI, 170, Colors.Purple));
    }


    /// <summary>
    /// Fill up the Point of Interest Buttons List
    /// </summary>
    private async Task fillPoiObjects()
    {
        // Point of Interest Buttons if active player = this client
        Player ownPlayer = await GetOwnPlayer();

        Dictionary<PointOfInterest, List<TicketTypeEnum>> temp = new Dictionary<PointOfInterest, List<TicketTypeEnum>>();
        temp = Validator.GetValidMoves(_com.GameState, _com.GameState.ActivePlayer);

        PoiButtons = new ObservableCollection<PointOfInterestView>();
        PoiFrames = new ObservableCollection<PointOfInterestView>();

        foreach (PointOfInterest poi in temp.Keys)
        {
            PointOfInterestView tempPOIV = PoiConverter(poi, 200);
            tempPOIV.PointOfInterest = poi;

            if (_com.GameState.ActivePlayer.UserName == ownPlayer.UserName)
                PoiButtons.Add(tempPOIV);
            else
                PoiFrames.Add(tempPOIV);    
        }


    }


    /// <summary>
    /// Fill up the ticket history list from MisterX
    /// </summary>
    private void fillTicketHistoryList ()
    {
        MrXticketHistory = new ObservableCollection<TicketsView>();
        foreach (TicketTypeEnum ticket in _com.GameState.TicketHistoryMisterX)
        {
            TicketsView tempTicket = new();
            tempTicket.ImagePath = "ticket_" + ticket.ToString().ToLower() + ".png";
            MrXticketHistory.Add(tempTicket);
        }

        // Fill up with dummys
        for (int i = _com.GameState.TicketHistoryMisterX.Count - 1; i < 23; i++)
        {
            TicketsView tempTicket = new();
            tempTicket.ImagePath = "ticket_placeholder.png";
            MrXticketHistory.Add(tempTicket);

        }

        // setting up all other variables
        int number = 0;
        foreach (TicketsView ticket in MrXticketHistory)
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

            // setting up MisterX discover round
            if (number == 2 || number == 7 || number == 12 || number == 17 || number == 23)
                ticket.BorderThickness = 4;
            else
                ticket.BorderThickness = 0;

            number++;
        }
        MrXticketHistory.Reverse();
    }


    /// <summary>
    /// Convert from POI in the model to POI in the view
    /// </summary>
    /// <param name="poi">Point of interes Object</param>
    /// <returns>PointOfInterestView</returns>
    private PointOfInterestView PoiConverter(PointOfInterest poi, double size, Color objectColor = null)
    {
        if (poi != null)
        {
            double zoomFactor = widthMap / 8914;

            PointOfInterestView temp = new PointOfInterestView();
            temp.Location = new Rect(zoomFactor * poi.LocationX - (zoomFactor * size) / 2, zoomFactor * poi.LocationY - (zoomFactor * size)/2, zoomFactor *  size , zoomFactor * size);
            temp.ObjectColor = objectColor ?? Colors.Transparent;
            return temp;
        }
        else
        {
            return null;
        }

    }


    /// <summary>
    /// Move a player to a destination poi
    /// </summary>
    /// <param name="poi">Point of interes Object</param>
    /// <param name="ticket">ticket Type</param>
    public void movePlayer(int poi, TicketTypeEnum ticket)
    {
        var move = new MovePlayerDto(poi, ticket);
        try
        {
            var moveStatus = _com.MovePlayerAsync(move);
        }
        catch (Exception ex)
        {
            // bei fail gamestate neu bekommen und wiederholen
        }


    }

    /// <summary>
    /// Get own player Object from the server
    /// </summary>
    /// <returns>Player player</returns>
    public async Task<Player> GetOwnPlayer()
    {
        try
        {
            var player = await _com.GetPlayerAsync();
            return player;
        }
        catch (Exception ex)
        {
            return null;

        }
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
        _com.initClient(Userseverip);
        var player = new Player(Username, Userpassword);
        player.Position = null;
        try
        {
            var statusCrate = await _com.CreatePlayerAsync(player);
        }
        catch (Exception ex)
        {
            // TODO: hier einfügen, was passieren soll, wenn Username schon vergeben ist!
        }
        try
        {
            var statusLogin = await _com.LoginAsync(player);
        }
        catch (Exception ex)
        {
            // hier dürfte normalerweise kein Fehler auftreten xD
        }



        var statusHub = _com.RegisterGameHubAsync();


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
    private async void JoinGame()
    {
        _com.gameStateInitEvent.WaitOne();
        await App.Current.MainPage.Navigation.PushAsync(new MainPage());

        //Shell.Current.ShowPopup(new AvatarPage());
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
        var status = await _com.StartGameAsync();

        _com.gameStateInitEvent.WaitOne();
        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
    }

    #endregion

    #region MainPage

    [RelayCommand]
    private void Button_Clicked_Zoom(string zoomType)
    { 
        if (zoomType == "plus" && WidthMap <= 8914 && HeightMap <= 5000)
        {
            WidthMap += 200;
            HeightMap = WidthMap / 1.7828;
        }
        else if (zoomType == "minus" && WidthMap - 200 >= DeviceDisplay.MainDisplayInfo.Width && HeightMap - 200 >= DeviceDisplay.MainDisplayInfo.Height)
        {
            WidthMap -= 200;
            HeightMap = WidthMap / 1.7828;
        }

        fillPoiObjects();
        fillPlayerLocation();

    }


   [RelayCommand]
    private void Button_Clicked_Poi(PointOfInterest poi)
    {
        TicketSelection.Clear();

        Dictionary<PointOfInterest, List<TicketTypeEnum>> temp = new Dictionary<PointOfInterest, List<TicketTypeEnum>>();
        temp = Validator.GetValidMoves(_com.GameState, _com.GameState.ActivePlayer);

        if (temp.ContainsKey(poi))
        {
            foreach (TicketTypeEnum ticket in temp[poi])
            {
                TicketSelection tempTicket = new TicketSelection();
                tempTicket.PointOfInterest = poi;
                tempTicket.TicketType = ticket;
                tempTicket.TicketImagePath = "ticket_" + ticket.ToString().ToLower() + ".png";
                TicketSelection.Add(tempTicket);
            }
        }
        ticketSelectionPage = new TicketSelectionPage();

        Shell.Current.ShowPopup(ticketSelectionPage);
    }

    [RelayCommand]
    private void Button_Clicked_Ticket(TicketSelection ticket)
    {
        movePlayer(ticket.PointOfInterest.Number, ticket.TicketType);

        ticketSelectionPage.Close();
    }

    #endregion

    #endregion
}
