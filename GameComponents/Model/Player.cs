using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GameComponents.Model
{
    /// <summary>
    /// Contains information about player
    /// </summary>
    [DataContract]
    public class Player : IdentityUser
    {
        /// <summary>
        /// Playername
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "Username is required")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [DataMember]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
        /// <summary>
        /// The Color displayed in the GUI
        /// </summary>
        public string PlayerColor { get; set; }
        /// <summary>
        /// Picture of player
        /// </summary>
        public string AvatarImagePath { get; set; }
        /// <summary>
        /// The current position as point of interest
        /// </summary>
        public PointOfInterest Position { get; set; }
        /// <summary>
        /// Number of bustickets available to the player
        /// </summary>
        public int BusTicket { get; set; } = 4;
        /// <summary>
        /// Number of biketickets available to the player
        /// </summary>
        public int BikeTicket { get; set; } = 8;
        /// <summary>
        /// Number of scootertickets available to the player
        /// </summary>
        public int ScooterTicket { get; set; } = 11;
        /// <summary>
        /// Number of Black Tickets available for Mister X
        /// </summary>
        public int BlackTicket { get; set; } = 0;
        /// <summary>
        /// Number of Double Tickets available for Mister X
        /// </summary>
        public int DoubleTicket { get; set; } = 0;
        /// <summary>
        /// True if this player is controlled by the computer
        /// </summary>
        public bool Npc { get; set; } = false;

        [JsonConstructor]
        public Player() { }

        public Player(string Name, string pw)
        {
            UserName = Name;
            Password = pw;
        }

        /// <summary>
        /// Constructor to init player
        /// </summary>
        /// <param name="position">Start position as point of interest</param>
        public Player(PointOfInterest position)
        {
            Position = position;
        }
    }
}
