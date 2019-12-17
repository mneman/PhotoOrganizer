using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PhotoOrganizer.Core.Imaging;

namespace PhotoOrganizer.Core.Tests
{
    [TestFixture]
    public sealed class ImageMetadataConverterTests
    {
        private static IEnumerable<(Guid formatId, string mimeType)> Codecs => ImageCodecInfo.GetImageDecoders().Select(codec => (codec.FormatID, codec.MimeType));

        [Test]
        public void ToOrientation_MetadataBytesNullOrEmpty_ReturnsTopLeft([Values(null, new byte[] { })] byte[] emptyBytes)
        {
            // Arrange
            var converter = new ImageMetadataConverter();

            // Act, Assert
            Assert.AreEqual(ImageOrientation.TopLeft, converter.ToOrientation(emptyBytes));
        }

        [Test]
        public void ToOrientation_InvalidBytes_ReturnsTopLeft()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act
            var orientation = parser.ToOrientation(new byte[] { 1 });

            // Assert
            Assert.AreEqual(ImageOrientation.TopLeft, orientation);
        }

        [TestCase(ImageOrientation.TopLeft)]
        [TestCase(ImageOrientation.TopRight)]
        [TestCase(ImageOrientation.BottomRight)]
        [TestCase(ImageOrientation.BottomLeft)]
        [TestCase(ImageOrientation.LeftTop)]
        [TestCase(ImageOrientation.RightTop)]
        [TestCase(ImageOrientation.RightBottom)]
        [TestCase(ImageOrientation.LeftBottom)]
        public void ToOrientation_ImageWithDifferentOrientationProperty_ReturnsCorrectValue(ImageOrientation expectedOrientation)
        {
            // Arrange
            var orientationByteArray = BitConverter.GetBytes((short)expectedOrientation);

            var parser = new ImageMetadataConverter();

            // Act
            var orientation = parser.ToOrientation(orientationByteArray);

            // Assert
            Assert.AreEqual(expectedOrientation, orientation);
        }

        [Test]
        public void ToDateTime_MetadataBytesNullOrEmpty_ReturnsNull([Values(null, new byte[] { })] byte[] emptyBytes)
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act, Assert
            Assert.IsNull(parser.ToDateTime(emptyBytes));
        }

        [Test]
        public void ToDataTime_FailsToConvertToDateTime_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act
            var dt = parser.ToDateTime(new byte[] { 1 });

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void ToDateTime_SuccessfullyConvertsToDateTime_ReturnsDateTimeTaken()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            var dt = DateTime.Today.AddHours(9).AddMinutes(1).AddSeconds(15);
            var dtString = dt.ToString("yyyy:MM:d H:m:s");
            var dtBytes = Encoding.ASCII.GetBytes(dtString);

            // Act
            var dtActual = parser.ToDateTime(dtBytes);

            // Assert
            Assert.AreEqual(dt, dtActual);
        }

        [TestCase(ImageOrientation.BottomLeft)]
        [TestCase(ImageOrientation.BottomRight)]
        [TestCase(ImageOrientation.LeftBottom)]
        [TestCase(ImageOrientation.LeftTop)]
        [TestCase(ImageOrientation.RightBottom)]
        [TestCase(ImageOrientation.RightTop)]
        [TestCase(ImageOrientation.TopLeft)]
        [TestCase(ImageOrientation.TopRight)]
        public void ToBytes_CalledWithImageOrientation_ReturnsBytes(ImageOrientation orientation)
        {
            // Arrange
            var expected = BitConverter.GetBytes((short)orientation);
            var converter = new ImageMetadataConverter();

            // Act, Assert
            CollectionAssert.AreEqual(expected, converter.ToBytes(orientation));
        }

        [Test]
        public void ToBytes_CalledWithDateTime_ReturnsBytes()
        {
            // Arrange
            var now = DateTime.Now;
            var nowString = now.ToString("yyyy:MM:d H:m:s", CultureInfo.InvariantCulture);
            var expected = Encoding.ASCII.GetBytes(nowString);

            var converter = new ImageMetadataConverter();

            // Act, Assert
            CollectionAssert.AreEqual(expected, converter.ToBytes(now));
        }

        [TestCaseSource("Codecs")]
        public void ToMimeType_CalledWithValidGuid_ReturnMimeType((Guid formatId, string mimeType) codec)
        {
            // Arrange
            var converter = new ImageMetadataConverter();

            // Act
            var actualMimeType = converter.ToMimeType(codec.formatId);

            // Assert
            Assert.That(actualMimeType, Is.EqualTo(codec.mimeType));
        }

        [Test]
        public void ToMimeType_CalledWithInvalidGuid_ThrowsException()
        {
            // Arrange
            var converter = new ImageMetadataConverter();

            // Act, Assert
            Assert.That(() => converter.ToMimeType(Guid.Empty), Throws.TypeOf<NotSupportedException>());
        }
    }
}
