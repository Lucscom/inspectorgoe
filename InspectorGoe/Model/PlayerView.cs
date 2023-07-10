using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents.Model;

namespace InspectorGoe.Model
{
    /// <summary>
    /// Class for display the player on the playercard
    /// </summary>
    public class PlayerView : ObservableObject
    {
        /// <summary>
        /// Playername
        /// </summary>
        private string userName;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }

        /// <summary>
        /// The Color displayed in the GUI
        /// </summary>
        private Color playerColor;
        public Color PlayerColor { get => playerColor; set => SetProperty(ref playerColor, value); }

        /// <summary>
        /// Picture of player
        /// </summary>
        private string avatarImagePath;
        public string AvatarImagePath { get => avatarImagePath; set => SetProperty(ref avatarImagePath, value); }

        /// <summary>
        /// Number of bustickets available to the player
        /// </summary>
        private int busTicket;
        public int BusTicket { get => busTicket; set => SetProperty(ref busTicket, value); }

        /// <summary>
        /// Number of scootertickets available to the player
        /// </summary>
        private int scooterTicket;
        public int ScooterTicket { get => scooterTicket; set => SetProperty(ref scooterTicket, value); }

        /// <summary>
        /// Number of biketickets available to the player
        /// </summary>
        private int bikeTicket;
        public int BikeTicket { get => bikeTicket; set => SetProperty (ref bikeTicket, value); }

        /// <summary>
        /// Number of Black Tickets available for Mister X
        /// </summary>
        private int blackTicket;
        public int BlackTicket { get => blackTicket; set => SetProperty(ref blackTicket, value); }

        /// <summary>
        /// Number of Double Tickets available for Mister X
        /// </summary>
        private int doubleTicket;
        public int DoubleTicket { get => doubleTicket; set => SetProperty(ref doubleTicket, value); }

        /// <summary>
        /// True if this player is controlled by the computer
        /// </summary>
        private bool npc;
        public bool Npc { get => npc; set => SetProperty(ref npc, value); }

        /// <summary>
        /// True if this player is Mister X
        /// </summary>
        private bool isMisterX;
        public bool IsMisterX { get => isMisterX; set => SetProperty(ref isMisterX, value); }

        /// <summary>
        /// True if this player is the creator of the game
        /// </summary>
        private bool isRemovable;
        public bool IsRemovable { get => isRemovable; set => SetProperty(ref isRemovable, value); }

        /// <summary>
        /// The current position as point of interest
        /// </summary>
        private PointOfInterest position;
        public PointOfInterest Position { get => position; set => SetProperty(ref position, value); }


        /// <summary>
        /// Borderthickness of the playercard to display the current player
        /// </summary>
        private int borderThickness;
        public int BorderThickness { get => borderThickness; set => SetProperty(ref borderThickness, value); }
    }
}
