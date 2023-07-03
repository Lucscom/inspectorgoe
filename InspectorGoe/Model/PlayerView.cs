using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents.Model;

namespace InspectorGoe.Model
{
    /// <summary>
    /// Class for display the player on the playercard
    /// </summary>
    public class PlayerView : ObservableObject
    {

        private string userName;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }

        private Color playerColor;
        public Color PlayerColor { get => playerColor; set => SetProperty(ref playerColor, value); }

        private string avatarImagePath;
        public string AvatarImagePath { get => avatarImagePath; set => SetProperty(ref avatarImagePath, value); }

        private int busTicket;
        public int BusTicket { get => busTicket; set => SetProperty(ref busTicket, value); }

        private int scooterTicket;
        public int ScooterTicket { get => scooterTicket; set => SetProperty(ref scooterTicket, value); }

        private int bikeTicket;
        public int BikeTicket { get => bikeTicket; set => SetProperty (ref bikeTicket, value); }

        private int blackTicket;
        public int BlackTicket { get => blackTicket; set => SetProperty(ref blackTicket, value); }

        private int doubleTicket;
        public int DoubleTicket { get => doubleTicket; set => SetProperty(ref doubleTicket, value); }

        private bool npc;
        public bool Npc { get => npc; set => SetProperty(ref npc, value); }

        private PointOfInterest position;
        public PointOfInterest Position { get => position; set => SetProperty(ref position, value); }


        /// <summary>
        /// Color for top level Frame in Player Cards to signal if the player is active
        /// </summary>
        private int borderThickness;
        public int BorderThickness { get => borderThickness; set => SetProperty(ref borderThickness, value); }
    }
}
