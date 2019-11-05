namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Describes the orientation of an image.
    /// </summary>
    public enum ImageOrientation : short
    {
        /// <summary>
        /// The (0,0) point is at the top left.
        /// </summary>
        TopLeft = 1,

        /// <summary>
        /// The (0,0) point is at the top right.
        /// </summary>
        TopRight = 2,

        /// <summary>
        /// The (0,0) point is at the bottom right.
        /// </summary>
        BottomRight = 3,

        /// <summary>
        /// The (0,0) point is at the bottom left.
        /// </summary>
        BottomLeft = 4,

        /// <summary>
        /// The (0,0) point is at the left top.
        /// </summary>
        LeftTop = 5,

        /// <summary>
        /// The (0,0) point is at the right top.
        /// </summary>
        RightTop = 6,

        /// <summary>
        /// The (0,0) point is at the right bottom.
        /// </summary>
        RightBottom = 7,

        /// <summary>
        /// The (0,0) point is at the left bottom.
        /// </summary>
        LeftBottom = 8,
    }
}
