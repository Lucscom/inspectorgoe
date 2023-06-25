

namespace InspectorGoe.Model
{
    /// <summary>
    /// Class for the POI on the map
    /// </summary>
    public class PointOfInterestView 

    {
        /// <summary>
        /// Position on the map
        /// </summary>
        public Rect Location { get; set; }

        /// <summary>
        /// Number of the POI
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// If an object need special color on the map (e.g. PlayerColor
        /// </summary>
        public Color ObjectColor { get; set; }


    }
}
