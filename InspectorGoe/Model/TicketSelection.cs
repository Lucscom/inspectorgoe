using GameComponents;
using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents.Model;

namespace InspectorGoe.Model
{
    /// <summary>
    /// Class for plotting the tickets in the Ticket Selection 
    /// </summary>
    public class TicketSelection : ObservableObject
    {
        /// <summary>
        /// Set Ticket enabled or disabled
        /// </summary>
        private bool isEnabled;
        public bool IsEnabled { get => isEnabled; set => SetProperty(ref isEnabled, value); }

        /// <summary>
        /// selected POI
        /// </summary>
        private PointOfInterest pointOfInterest;
        public PointOfInterest PointOfInterest { get => pointOfInterest; set => SetProperty(ref pointOfInterest, value); }

        /// <summary>
        /// Ticket Type
        /// </summary>
        private TicketTypeEnum ticketType;
        public TicketTypeEnum TicketType { get => ticketType; set => SetProperty(ref ticketType, value); }

        /// <summary>
        /// Ticket Image Path for displaying the Ticket
        /// </summary>
        private string ticketImagePath;
        public string TicketImagePath { get => ticketImagePath; set => SetProperty<string>(ref ticketImagePath, value); }

    }
}
