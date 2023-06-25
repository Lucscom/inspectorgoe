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
    private double widthMap = 2600;

    [ObservableProperty]
    private double heightMap = 1458.38;

    [ObservableProperty]
    private ObservableCollection<Player> detectives = new ObservableCollection<Player>();

    [ObservableProperty]
    private Player misterX = new Player();

    [ObservableProperty]
    public ObservableCollection<PointOfInterestView> poisLocation = new ObservableCollection<PointOfInterestView>();

    [ObservableProperty]
    private ObservableCollection<TicketsView> mrXticketHistory = new ObservableCollection<TicketsView>();

    [ObservableProperty]
    private ObservableCollection<PointOfInterestView> mrXLastKnownPoi = new ObservableCollection<PointOfInterestView>();

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
        // Dtectives
        Detectives.Clear();
        foreach (Player detective in _com.GameState.Detectives)
        {
            Detectives.Add(detective);
        }

        // MisterX
        misterX = _com.GameState.MisterX;

        // MisterX Last Known POI
        mrXLastKnownPoi.Add(PoiConverter(_com.GameState.MisterXLastKnownPOI, 170, Colors.Purple));


        // Ticket History from Mister
        mrXticketHistory = new ObservableCollection<TicketsView>();
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

            // setting up MisterX discover round
            if (number == 2 || number == 7 || number == 12 || number == 17 || number == 23)
                ticket.BorderThickness = 4;
            else
                ticket.BorderThickness = 0;

            number++;
        }
        MrXticketHistory.Reverse();


        // Buttons POIS
        PoisLocation = new ObservableCollection<PointOfInterestView>();
        foreach (PointOfInterest poi in _com.GameState.PointsOfInterest)
        {
            PoisLocation.Add(PoiConverter(poi, 200));
        }
    }


    /// <summary>
    /// Convert from POI in the model to POI in the view
    /// </summary>
    /// <param name="poi">Point of interes Object</param>
    /// <returns>PointOfInterestView</returns>
    private PointOfInterestView PoiConverter(PointOfInterest poi, double size,  Color objectColor = null)
    {
        if (poi != null)
        {
            double zoomFactor = widthMap / 8914;

            PointOfInterestView temp = new PointOfInterestView();
            temp.Location = new Rect(zoomFactor * poi.Location.X - (zoomFactor * size) / 2, zoomFactor * poi.Location.Y - (zoomFactor * size)/2, zoomFactor *  size , zoomFactor * size);
            temp.Number = poi.Number;
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
    public void movePlayer(PointOfInterest poi, TicketTypeEnum ticket)
    {
        var move = new MovePlayerDto(poi, ticket);
        var moveStatus =_com.MovePlayerAsync(move);
        // todo: status code checken -->
        // bei fail gamestate neu bekommen und wiederholen
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
        var statusCrate = await _com.CreatePlayerAsync(player);
        var statusLogin = await _com.LoginAsync(player);
        // todo: status code checken -->
        // bei fail gamestate neu bekommen und wiederholen
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
        var status = _com.StartGameAsync();

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

        PoisLocation.Clear();
        //Button Position and Size Update
        foreach (PointOfInterest poi in _com.GameState.PointsOfInterest)
        {
            PoisLocation.Add(PoiConverter(poi, 200));
        }

    }


   [RelayCommand]
    private void Button_Clicked_Poi(int number)
    {
        TicketSelection.Clear();

        TicketSelection temp = new TicketSelection();
        temp.PointOfInterest = _com.GameState.PointsOfInterest.Find(x => x.Number == number);
        temp.TicketType = TicketTypeEnum.Bike;
        temp.TicketImagePath = "ticket_bike.png";
        TicketSelection.Add(temp);

        temp = new TicketSelection();
        temp.PointOfInterest = _com.GameState.PointsOfInterest.Find(x => x.Number == number);
        temp.TicketType = TicketTypeEnum.Bus;
        temp.TicketImagePath = "ticket_bus.png";
        TicketSelection.Add(temp);

        temp = new TicketSelection();
        temp.PointOfInterest = _com.GameState.PointsOfInterest.Find(x => x.Number == number);
        temp.TicketType = TicketTypeEnum.Scooter;
        temp.TicketImagePath = "ticket_scooter.png";
        TicketSelection.Add(temp);

        ticketSelectionPage = new TicketSelectionPage();

        Shell.Current.ShowPopup(ticketSelectionPage);
    }

    [RelayCommand]
    private void Button_Clicked_Ticket(TicketSelection ticket)
    {
        ticketSelectionPage.Close();
        Console.Write("Test");
    }

    #endregion

    #endregion
}
