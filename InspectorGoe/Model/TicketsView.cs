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

        private int borderThickness;
        public int BorderThickness { get => borderThickness; set => SetProperty(ref borderThickness, value); }

        /// <summary>
        /// Color for the Frame behind the Roundnumber (only if ther is no ticket)
        /// </summary>
        private Color numberFrameColor;
        public Color NumberFrameColor { get => numberFrameColor; set => SetProperty(ref numberFrameColor, value); }

        /// <summary>
        /// Color of the Roundnumber
        /// </summary>
        private Color numberColor;
        public Color NumberColor { get => numberColor; set => SetProperty (ref numberColor, value); }

    }
}
