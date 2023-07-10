using Client;
using Client.Events;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameComponents;
using GameComponents.Model;
using InspectorGoe.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
    private LobbyPage _lobby;
    private bool _gameInProgress = false;

    //Variablen für Login
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(Button_Clicked_LogInCommand))]
    #if DEBUG
    string username = DateTime.Now.ToString("HHmmss");
    #else
    string username = string.Empty;
    #endif

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(Button_Clicked_LogInCommand))]
    #if DEBUG
    string userpassword = "test";
    #else
    string userpassword = string.Empty;
    #endif

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(Button_Clicked_LogInCommand))]
    #if DEBUG
    string userseverip = "https://localhost:5000";
#else
    string userseverip = string.Empty;
#endif

    [ObservableProperty]
    private bool isCreator = false;

    // ########## Variablen für Register ##########
    [ObservableProperty]
    private string usernameregister = string.Empty;

    [ObservableProperty]
    private string userpasswordregister = string.Empty;

    [ObservableProperty]
    private string userpasswordregister2 = string.Empty;


    // ########## Variablen für AvatarPage ##########
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    private string avatarImagePath;

    [ObservableProperty]
    private string avatarButton;


    // ########## Variablen für MainPage ##########

    [ObservableProperty]
    private double widthMap = 2600;

    [ObservableProperty]
    private double heightMap = 1458.38;

    [ObservableProperty]
    private ObservableCollection<PointOfInterestView> playerLocation = new ObservableCollection<PointOfInterestView>();

    [ObservableProperty]
    private ObservableCollection<PlayerView> detectives = new ObservableCollection<PlayerView>();

    [ObservableProperty]
    private PlayerView misterX = new PlayerView();

    [ObservableProperty]
    private bool isMisterX = false;

    private Player ownPlayer = new Player();

    [ObservableProperty]
    private ObservableCollection<PlayerView> allPlayers = new ObservableCollection<PlayerView>();

    [ObservableProperty]
    private ObservableCollection<TicketsView> mrXticketHistory = new ObservableCollection<TicketsView>();


    // POIS
    [ObservableProperty]
    private ObservableCollection<PointOfInterestView> poiButtons = new ObservableCollection<PointOfInterestView>();

    [ObservableProperty]
    private ObservableCollection<PointOfInterestView> poiFrames = new ObservableCollection<PointOfInterestView>();

    private TicketSelectionPage ticketSelectionPage;
    private TicketSelectionPageMisterX ticketSelectionPageMisterX;

    [ObservableProperty]
    private ObservableCollection<TicketSelection> ticketSelection = new ObservableCollection<TicketSelection>();

    [ObservableProperty]
    private bool isDoubleTicketPossible = false;

    [ObservableProperty]
    private bool doubleTicketSelected = false;

    #endregion

    private MainViewModel()
    {
        // hier startet die connection mit der Logik und dem Server
        _com = new Communicator();

        _com.GameEndEvent += async (s, e) => await Shell.Current.Dispatcher.DispatchAsync(async () => await ComGameEnd(s, e));

        
        //signalr initiates updates on a seperate thread
        //use the dispatcher to shedule the update on the UI thread instead
        //therefore signalr and ui thread will not access the properties/variables at the same time
        _com.UpdateGameStateEvent += async (s,e) => await Shell.Current.Dispatcher.DispatchAsync(async () => await ComUpdateGameState(s,e));
        _com.GameStartedEvent += async (s,e) => await Shell.Current.Dispatcher.DispatchAsync(async () => await ComGameStarted(s,e));
    }

    private async Task ComGameStarted(object s, EventArgs e)
    {
        if (!_gameInProgress)
        {
            _gameInProgress = true;
            await Shell.Current.Dispatcher.DispatchAsync(async () => await Shell.Current.Navigation.PushAsync(new MainPage()));
            _lobby?.Close();
        }
    }


    #region GameLogic

    private async Task ComUpdateGameState(object sender, EventArgs e)
    {
        try
        {
            // Fill Lobby Playerlist
            if (!_com.GameState.GameStarted)
            {
                fillPlayerLobby();
            }

            // Fill all Game Data
            else
            {

                if (ownPlayer.UserName == _com.GameState.MisterX.UserName) IsMisterX = true; else IsMisterX = false;

                // Set Player Card
                fillPlayerCards();


                // Set Player Position
                fillPlayerLocation();


                // Ticket History from Mister
                fillTicketHistoryList();


                // Point of Interest Buttons
                fillPoiObjects();

                if (DoubleTicketSelected)
                {
                    _com.GameState.ActivePlayer.usingDoubleTicket = true;
                    DoubleTicketSelected = false;
                }

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
#if DEBUG
            throw;
#endif
        }
    }


    /// <summary>
    /// Handles game end event
    /// </summary>
    /// <param name="player">Player(group) that won the game</param>
    /// <param name="e"></param>
    /// <exception cref="Exception"></exception>
    private async Task ComGameEnd(object player, GameEndEventArgs e)
    {
        string winMessage= string.Empty;
        if(IsMisterX && e.Player == "MisterX")
        {
            winMessage = "You won the game!";
        }
        else if(IsMisterX && e.Player == "Detectives")
        {
            winMessage = "You lost the game!";
        }
        else if(!IsMisterX && e.Player == "MisterX")
        {
            winMessage = "You lost the game!";
        }
        else if(!IsMisterX && e.Player == "Detectives")
        {
            winMessage = "You won the game!";
        }
        else
        {
            throw new Exception("GameEndEventArgs.Player is not valid");
        }
 
       await Shell.Current.DisplayAlert(winMessage, "", "OK");   

       PoiButtons = new ObservableCollection<PointOfInterestView>();
       PoiFrames = new ObservableCollection<PointOfInterestView>();
    }


    /// <summary>
    /// Fill all Player List for Lobby
    /// </summary>
    private void fillPlayerLobby()
    {

        // Add Detectives
        AllPlayers = new ObservableCollection<PlayerView>();
        foreach (Player detective in _com.GameState.Detectives)
        {
            PlayerView temp = new PlayerView();
            temp.UserName = detective.UserName;
            temp.AvatarImagePath = detective.AvatarImagePath;
            temp.IsMisterX = false;
            temp.Npc = detective.Npc;
            if(IsCreator && temp.Npc) temp.IsRemovable = true;
            else temp.IsRemovable = false;

            AllPlayers.Add(temp);
        }

        // Add MisterX
        if (_com.GameState.MisterX != null)
        {
            PlayerView temp = new PlayerView();
            temp.UserName = _com.GameState.MisterX.UserName;
            temp.AvatarImagePath = _com.GameState.MisterX.AvatarImagePath;
            temp.IsMisterX = true;
            temp.Npc = _com.GameState.MisterX.Npc;
            if (IsCreator && temp.Npc) temp.IsRemovable = true;
            else temp.IsRemovable = false;

            AllPlayers.Add(temp);
        }

    }


    /// <summary>
    /// Fill the Player Cards from detectives and MisterX
    /// </summary>
    private void fillPlayerCards()
    {
        // Set  Player Cards
        Detectives = new ObservableCollection<PlayerView>();

        List<Color> colorList = new List<Color>
            {
                Colors.Aqua,
                Colors.LimeGreen,
                Colors.Turquoise,
                Colors.Orange,
                Colors.Purple
            };
        int colorCounter = 0;

        foreach (Player detective in _com.GameState.Detectives)
        {
            PlayerView temp = new();
            temp.AvatarImagePath = detective.AvatarImagePath;
            if(ownPlayer.UserName == detective.UserName) temp.UserName = detective.UserName + " (You)"; else temp.UserName = detective.UserName;
            temp.BikeTicket = detective.BikeTicket;
            temp.ScooterTicket = detective.ScooterTicket;
            temp.BusTicket = detective.BusTicket;
            temp.BlackTicket = detective.BlackTicket;
            temp.DoubleTicket = detective.DoubleTicket;
            temp.Position = detective.Position;

            if (_com.GameState.ActivePlayer.UserName == detective.UserName)
                temp.BorderThickness = 4;
            else
                temp.BorderThickness = 0;

            Detectives.Add(temp);

            temp.PlayerColor = colorList[colorCounter];
            colorCounter++;

        }

        // MisterX
        MisterX.AvatarImagePath = _com.GameState.MisterX.AvatarImagePath;
        if(IsMisterX) MisterX.UserName = _com.GameState.MisterX.UserName + " (You)"; else MisterX.UserName = _com.GameState.MisterX.UserName;
        MisterX.BikeTicket = _com.GameState.MisterX.BikeTicket;
        MisterX.ScooterTicket = _com.GameState.MisterX.ScooterTicket;
        MisterX.BusTicket = _com.GameState.MisterX.BusTicket;
        MisterX.BlackTicket = _com.GameState.MisterX.BlackTicket;
        MisterX.DoubleTicket = _com.GameState.MisterX.DoubleTicket;
        MisterX.Position = _com.GameState.MisterX.Position;
        MisterX.PlayerColor = Colors.Red;
        if (_com.GameState.ActivePlayer.UserName == _com.GameState.MisterX.UserName)
            MisterX.BorderThickness = 4;
        else
            MisterX.BorderThickness = 0;

    }

    /// <summary>
    /// fill up the player location list
    /// </summary>
    private void fillPlayerLocation()
    {
        // Detectives
        PlayerLocation = new ObservableCollection<PointOfInterestView>();
        foreach (PlayerView detective in Detectives)
        {
            PlayerLocation.Add(PoiConverter(detective.Position, 210, detective.PlayerColor, detective.Position.Name));
        }

        if(IsMisterX)
            PlayerLocation.Add(PoiConverter(_com.GameState.MisterX?.Position, 210, MisterX.PlayerColor, _com.GameState.MisterX.Position.Name));

        else if(_com.GameState.MisterXLastKnownPOI != null)
            PlayerLocation.Add(PoiConverter(_com.GameState.MisterXLastKnownPOI, 210, MisterX.PlayerColor, _com.GameState.MisterX.Position.Name));
    }


    /// <summary>
    /// Fill up the Point of Interest Buttons List
    /// </summary>
    private void fillPoiObjects()
    {
        // Point of Interest Buttons if active player = this client
        Dictionary<PointOfInterest, List<TicketTypeEnum>> temp = new Dictionary<PointOfInterest, List<TicketTypeEnum>>();
        temp = Validator.GetValidMoves(_com.GameState, _com.GameState.ActivePlayer);

        PoiButtons = new ObservableCollection<PointOfInterestView>();
        PoiFrames = new ObservableCollection<PointOfInterestView>();

        foreach (PointOfInterest poi in temp.Keys)
        {
            PointOfInterestView tempPOIV = PoiConverter(poi, 220);
            tempPOIV.PointOfInterest = poi;
            tempPOIV.Name = poi.Name;

            if (_com.GameState.ActivePlayer.UserName == ownPlayer.UserName)
                PoiButtons.Add(tempPOIV);
            else if(_com.GameState.ActivePlayer.UserName != _com.GameState.MisterX.UserName)
                PoiFrames.Add(tempPOIV);    
        }


    }


    /// <summary>
    /// Fill up the ticket history list from MisterX
    /// </summary>
    private void fillTicketHistoryList()
    {
        MrXticketHistory = new ObservableCollection<TicketsView>();
        foreach (TicketTypeEnum ticket in _com.GameState.TicketHistoryMisterX)
        {
            TicketsView tempTicket = new();
            tempTicket.ImagePath = "ticket_" + ticket.ToString().ToLower() + ".png";
            MrXticketHistory.Add(tempTicket);
        }

        // Fill up with dummys
        for (int i = _com.GameState.TicketHistoryMisterX.Count; i < 24; i++)
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
            }
            else
            {
                ticket.NumberColor = Colors.White;
            }

            // setting up MisterX discover round
            if (number == 2 || number == 7 || number == 12 || number == 17 || number == 23)
                //ticket.BorderThickness = 4;
                ticket.BorderColor = new Color (247, 181, 72 );
            else
                //ticket.BorderThickness = 0;
                ticket.BorderColor = Colors.Transparent;

            number++;
        }
        //MrXticketHistory.Reverse();
    }


    /// <summary>
    /// Convert from POI in the model to POI in the view
    /// </summary>
    /// <param name="poi">Point of interes Object</param>
    /// <returns>PointOfInterestView</returns>
    private PointOfInterestView PoiConverter(PointOfInterest poi, double size, Color objectColor = null, string name = null)
    {
        if (poi != null)
        {
            double zoomFactor = WidthMap / 8914;

            PointOfInterestView temp = new PointOfInterestView();
            temp.Location = new Rect(zoomFactor * poi.LocationX - (zoomFactor * size) / 2, zoomFactor * poi.LocationY - (zoomFactor * size)/2, zoomFactor *  size , zoomFactor * size);
            temp.ObjectColor = objectColor ?? Colors.Transparent;
            temp.Name = name ?? poi.Name;
            return temp;
        }
        else
            return null;
        

    }


    /// <summary>
    /// Move a player to a destination poi
    /// </summary>
    /// <param name="poi">Point of interes Object</param>
    /// <param name="ticket">ticket Type</param>
    public void movePlayer(int poi, TicketTypeEnum ticket)
    {
        var move = new MovePlayerDto(poi, ticket, doubleTicketSelected);
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

    #endregion

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

        HttpResponseMessage response = null;
        try
        {
            response = await _com.CreatePlayerAsync(player);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            var content = JsonConvert.DeserializeObject<List<IdentityError>>(await response.Content.ReadAsStringAsync());
            await Shell.Current.DisplayAlert($"{content?.First()?.Code}", $"{content?.First()?.Description}", "OK");
            return;
        }

        try
        {
            await _com.LoginAsync(player);
            ownPlayer = await GetOwnPlayer();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
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
    private async Task CreateNewGame()
    {
        IsCreator = true;
        AvatarButton = "Create Game";
        await Shell.Current.ShowPopupAsync(new AvatarPage());
    }

    /// <summary>
    /// Navigation from MenuPage to AvatarPage
    /// </summary>
    [RelayCommand]
    private async Task JoinGame()
    {
        IsCreator = false;
        AvatarButton = "Join Game";
        await Shell.Current.ShowPopupAsync(new AvatarPage());
    }

    /// <summary>
    /// Navigation from MenuPage to MainPage
    /// </summary>
    [RelayCommand]
    private async Task Tutorial()
    {
        await Shell.Current.ShowPopupAsync(new TutorialPage());
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
    private async Task Start(AvatarPage popup) //todo: rename to create game
    {
        //Spiel erstellen wenn Creator
        if (IsCreator == true)
        {
            try
            {
                await _com.CreateGameAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
        }

        //Spiel beitreten immer
        try
        {

            await _com.JoinGameAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }

        //senden des ausgewälten Avatars an den Server
        try
        {
            var avatarPathObj = new StringDto(avatarImagePath);
            await _com.UpdateAvatar(avatarPathObj);

            //debug info
            var player = await _com.GetPlayerAsync();

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }
        _lobby = new LobbyPage();
        await _com.newGameStateEvent.WaitAsync();

        await Shell.Current.ShowPopupAsync(_lobby);
    }


    /// <summary>
    /// Überprüfe ob ein Avatar ausgewählt wurde
    /// </summary>
    /// <returns></returns>

    private bool AvatarIsSelected()
    {

        if (AvatarImagePath != "")
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
    async Task StartGame(LobbyPage popup)
    {
        try
        {
            await _com.StartGameAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }
    }

    [RelayCommand]
    async Task AddNPC(LobbyPage popup)
    {
        try
        {
            await _com.AddNpcAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }
    }

    [RelayCommand]
    async Task RemovePlayer(string player)
    {
        try
        {
            await _com.RemoveAsync(player);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }
    }
    #endregion

    #region MainPage

    [RelayCommand]
    private async Task Button_Clicked_Zoom(string zoomType)
    { 
        if (zoomType == "plus" && WidthMap <= 8914 && HeightMap <= 5000)
        {
            WidthMap += 100;
            HeightMap = WidthMap / 1.7828;
        }
        else if (zoomType == "minus")//&& WidthMap - 100 >= DeviceDisplay.MainDisplayInfo.Width && HeightMap - 100 >= DeviceDisplay.MainDisplayInfo.Height
        {
            WidthMap -= 100;
            HeightMap = WidthMap / 1.7828;
        }

        fillPoiObjects();
        fillPlayerLocation();

    }


    // Tickselection PopUp
   [RelayCommand]
    private void Button_Clicked_Poi(PointOfInterest poi)
    {
        TicketSelection = new ObservableCollection<TicketSelection>();

        Dictionary<PointOfInterest, List<TicketTypeEnum>> temp = new Dictionary<PointOfInterest, List<TicketTypeEnum>>();
        temp = Validator.GetValidMoves(_com.GameState, _com.GameState.ActivePlayer);


        if (temp[poi].Contains(TicketTypeEnum.doubleTicket))
            IsDoubleTicketPossible = true;
        
        else
            IsDoubleTicketPossible = false;
        

        foreach (TicketTypeEnum ticket in Enum.GetValues(typeof(TicketTypeEnum)))
        {
            if(IsMisterX == false && (ticket == TicketTypeEnum.Black || ticket == TicketTypeEnum.doubleTicket))
                continue;

            if (ticket == TicketTypeEnum.doubleTicket)
                continue;


            TicketSelection tempTicket = new TicketSelection();
            tempTicket.PointOfInterest = poi;
            tempTicket.TicketType = ticket;

            if (temp[poi].Contains(ticket))
            {
                tempTicket.IsEnabled = true;
                tempTicket.TicketImagePath = "ticket_" + ticket.ToString().ToLower() + ".png";
            }
            else
            {
                tempTicket.IsEnabled = false;
                tempTicket.TicketImagePath = "ticket_" + ticket.ToString().ToLower() + "_sw.png";
            }            
            TicketSelection.Add(tempTicket);
        }

        DoubleTicketSelected = false;
        if (!IsMisterX)
        {
            ticketSelectionPage = new TicketSelectionPage();
            Shell.Current.ShowPopup(ticketSelectionPage);
        }
        else
        {
            ticketSelectionPageMisterX = new TicketSelectionPageMisterX();
            Shell.Current.ShowPopup(ticketSelectionPageMisterX);
        }
    }

    // After Click on Ticket
    [RelayCommand]
    private void Button_Clicked_Ticket(TicketSelection ticket)
    {

        movePlayer(ticket.PointOfInterest.Number, ticket.TicketType);

        ticketSelectionPage?.Close();
        ticketSelectionPageMisterX?.Close();
        
    }


    #endregion

    #endregion
}
