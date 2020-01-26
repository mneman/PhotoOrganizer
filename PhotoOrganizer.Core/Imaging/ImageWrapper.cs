using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// The wrapper for the <see cref="Image"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ImageWrapper : IImage
    {
        /// <summary>
        /// The <see cref="Image"/> wrappee.
        /// </summary>
        private readonly Image wrappee;

        /// <summary>
        /// The <see cref="IImageMetadataConverter"/> instance
        /// </summary>
        private readonly IImageMetadataConverter metadataConverter;

        /// <summary>
        /// The getter of the MIME type of the image.
        /// </summary>
        private readonly Lazy<string> mimeTypeProvider;

        /// <summary>
        /// The getter of the <see cref="ImageCodecInfo"/> of the image.
        /// </summary>
        private readonly Lazy<ImageCodecInfo> codecInfoProvider;

        /// <summary>
        /// The getter of the image orientation.
        /// </summary>
        private readonly Lazy<ImageOrientation> orientationProvider;

        /// <summary>
        /// The getter of the date-time when the image was taken.
        /// </summary>
        private readonly Lazy<DateTime?> dateTimeTakenProvider;

        /// <summary>
        /// The getter of the date-time when the image was digitized
        /// </summary>
        private readonly Lazy<DateTime?> dateTimeDigitizedProvider;

        /// <summary>
        /// The getter of the original date-time of the image.
        /// </summary>
        private readonly Lazy<DateTime?> dateTimeOriginalProvider;

        /// <summary>
        /// Creates an instance of the <see cref="ImageWrapper"/>.
        /// </summary>
        /// <param name="wrappee">The <see cref="Image"/> wrappee.</param>
        /// <param name="metadataParser"></param>
        internal ImageWrapper(Image wrappee, IImageMetadataConverter metadataParser)
        {
            this.wrappee = wrappee;
            this.metadataConverter = metadataParser;

            this.orientationProvider = new Lazy<ImageOrientation>(() => this.metadataConverter.ToOrientation(this.GetMetadata(ImageMetadataType.Orientation)));
            this.dateTimeTakenProvider = new Lazy<DateTime?>(() => this.metadataConverter.ToDateTime(this.GetMetadata(ImageMetadataType.DateTime)));
            this.dateTimeOriginalProvider = new Lazy<DateTime?>(() => this.metadataConverter.ToDateTime(this.GetMetadata(ImageMetadataType.DateTimeOriginal)));
            this.dateTimeDigitizedProvider = new Lazy<DateTime?>(() => this.metadataConverter.ToDateTime(this.GetMetadata(ImageMetadataType.DateTimeDigitized)));

            this.mimeTypeProvider = new Lazy<string>(() => this.metadataConverter.ToMimeType(this.wrappee.RawFormat.Guid));
            this.codecInfoProvider = new Lazy<ImageCodecInfo>(() => ImageCodecInfo.GetImageEncoders().Where(encoder => encoder.MimeType == this.MimeType).FirstOrDefault());
        }

        /// <summary>
        /// Gets the image MIME type.
        /// </summary>
        public string MimeType => this.mimeTypeProvider.Value;

        /// <summary>
        /// Gets the codec information (<see cref="ImageCodecInfo"/>).
        /// </summary>
        public ImageCodecInfo CodecInfo => this.codecInfoProvider.Value;

        /// <summary>
        /// Gets the image orientation.
        /// </summary>
        public ImageOrientation Orientation
        {
            get 
            { 
                return this.orientationProvider.Value; 
            }
            set 
            { 
                this.SetMetadata(ImageMetadataType.Orientation, this.metadataConverter.ToBytes(value)); 
            }
        }

        /// <summary>
        /// Gets the date time the image was taken at. 
        /// </summary>
        public DateTime? DateTimeTaken => this.dateTimeTakenProvider.Value;

        /// <summary>
        /// Gets the date time when the image was originally created at.
        /// </summary>
        public DateTime? DateTimeOriginal => this.dateTimeOriginalProvider.Value;

        /// <summary>
        /// Gets the date time the image was digitized at.
        /// </summary>
        public DateTime? DateTimeDigitized => this.dateTimeDigitizedProvider.Value;

        /// <summary>
        /// Saves this System.Drawing.Image to the specified file, with the specified encoder and image-encoder parameters.
        /// </summary>
        /// <param name="targetFile">A string that contains the name of the file to which to save this System.Drawing.Image.</param>
        /// <param name="codec">The System.Drawing.Imaging.ImageCodecInfo for this System.Drawing.Image.</param>
        /// <param name="parameters">An System.Drawing.Imaging.EncoderParameters to use for this System.Drawing.Image.</param>
        public void Save(string targetFile, ImageCodecInfo codec, EncoderParameters parameters)
        {
            this.wrappee.Save(targetFile, codec, parameters);
        }

        /// <summary>
        /// Disposes the underlying <see cref="Image"/> wrappee.
        /// </summary>
        public void Dispose()
        {
            this.wrappee.Dispose();
        }

        /// <summary>
        /// Gets the image metadata item.
        /// </summary>
        /// <param name="id">The metadata identifier.</param>
        /// <returns>The metadata value (byte array)</returns>
        private byte[] GetMetadata(int id)
        {
            var item = this.wrappee.PropertyItems.FirstOrDefault(item => item.Id == id);
            return item == null ? new byte[] { } : item.Value;
        }

        /// <summary>
        /// Gets the image metadata item.
        /// </summary>
        /// <param name="type">The metadata type.</param>
        /// <returns>The metadata value (byte array)</returns>
        private byte[] GetMetadata(ImageMetadataType type)
        {
            return this.GetMetadata((int)type);
        }

        /// <summary>
        /// Sets the image metadata item.
        /// </summary>
        /// <param name="id">The metadata identifier.</param>
        /// <param name="value">The metadat bytes.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void SetMetadata(int id, byte[] value)
        {
            var item = this.wrappee.PropertyItems.FirstOrDefault(item => item.Id == id);
            if (item != null)
            {
                item.Value = value;
                this.wrappee.SetPropertyItem(item);
            }
        }

        /// <summary>
        /// Sets the image metadata item.
        /// </summary>
        /// <param name="type">The metadata type.</param>
        /// <param name="value">The metadat bytes.</param>
        private void SetMetadata(ImageMetadataType type, byte[] value)
        {
            this.SetMetadata((int)type, value);
        }
    }
}
