using CommunityToolkit.Mvvm.ComponentModel;


namespace GameComponents.Model
{
    /// <summary>
    /// Class to display the tickets 
    /// </summary>
    public class TicketsView : ObservableObject
    {
        /// <summary>
        /// Ticket Imagepath
        /// </summary>
        private string imagePath;
        public string ImagePath { get => imagePath; set => SetProperty(ref imagePath, value); }

        /// <summary>
        /// Number of the Round
        /// </summary>
        private int numberRound ;
        public int NumberRound { get => numberRound; set => SetProperty(ref numberRound, value); }

        /// <summary>
        /// Borderthickness for the border of the ticket if Misterx Position is discovered
        /// </summary>

        private Color borderColor;
        public Color BorderColor { get => borderColor; set => SetProperty(ref borderColor, value); }

        private int borderThickness;
        public int BorderThickness { get => borderThickness; set => SetProperty(ref borderThickness, value); }

        /// <summary>
        /// Color of the Roundnumber
        /// </summary>
        private Color numberColor;
        public Color NumberColor { get => numberColor; set => SetProperty (ref numberColor, value); }

    }
}
