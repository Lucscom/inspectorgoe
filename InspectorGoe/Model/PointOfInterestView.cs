using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents.Model;

namespace InspectorGoe.Model
{
    /// <summary>
    /// Class for the POI on the map
    /// </summary>
    public class PointOfInterestView : ObservableObject

    {
        /// <summary>
        /// Number of the POI
        /// </summary>
        private int number;
        public int Number { get => number; set => SetProperty(ref number, value); }

        /// <summary>
        /// Position on the map
        /// </summary>
        private Rect location;
        public Rect Location { get => location; set => SetProperty(ref location, value); }

        /// <summary>
        /// Number of the POI
        /// </summary>
        private PointOfInterest pointOfIntereset;
        public PointOfInterest PointOfInterest { get => pointOfIntereset; set => SetProperty(ref pointOfIntereset, value); }

        /// <summary>
        /// If an object need special color on the map (e.g. PlayerColor
        /// </summary>
        private Color objectColor;
        public Color ObjectColor { get => objectColor; set => SetProperty(ref objectColor, value); }


    }
}
