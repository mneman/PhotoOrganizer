namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Image metadata type enumeration
    /// More info: https://docs.microsoft.com/en-us/windows/win32/gdiplus/-gdiplus-constant-property-tags-in-alphabetical-order
    /// </summary>
    public enum ImageMetadataType
    {
        /// <summary>
        /// Image orientation viewed in terms of rows and columns.
        /// </summary>
        Orientation = 274,

        /// <summary>
        /// Date and time the image was created.
        /// </summary>
        DateTime = 306,

        /// <summary>
        /// Date and time when the original image data was generated.
        /// </summary>
        DateTimeOriginal = 36867,

        /// <summary>
        /// Date and time when the image was stored as digital data.
        /// </summary>
        DateTimeDigitized = 36868
    }
}
