using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using NSubstitute;
using NUnit.Framework;
using PhotoOrganizer.Core.Imaging;
using PhotoOrganizer.Core.Manipulators;

namespace PhotoOrganizer.Core.Tests
{
    public class ImageRotatorTests
    {
        [Test]
        public void GetRotationParametersForImage_ImageNull_ThrowsArgumentException()
        {
            // Arrange
            var factory = Fake.ImageFactory(Fake.Image());
            var rotator = new ImageRotator(factory);

            // Act, Assert
            Assert.Throws<ArgumentException>(() => rotator.GetRotationParameters((IImage)null));
        }

        [Test]
        public void GetRotationParametersForImage_OrientationIsTopLeft_ReturnsNull()
        {
            // Arrange
            var image = Fake.Image(ImageOrientation.TopLeft);
            var factory = Fake.ImageFactory(image);
            var rotator = new ImageRotator(factory);

            // Act
            var parameters = rotator.GetRotationParameters(image);

            // Assert
            Assert.IsNull(parameters);
        }

        [TestCase(ImageOrientation.BottomLeft, new[] { EncoderValue.TransformFlipVertical })]
        [TestCase(ImageOrientation.BottomRight, new[] { EncoderValue.TransformRotate180 })]
        [TestCase(ImageOrientation.LeftBottom, new[] { EncoderValue.TransformRotate270 })]
        [TestCase(ImageOrientation.LeftTop, new[] { EncoderValue.TransformRotate90, EncoderValue.TransformFlipHorizontal })]
        [TestCase(ImageOrientation.RightBottom, new[] { EncoderValue.TransformRotate90, EncoderValue.TransformFlipVertical })]
        [TestCase(ImageOrientation.RightTop, new[] { EncoderValue.TransformRotate90 })]
        [TestCase(ImageOrientation.TopRight, new[] { EncoderValue.TransformFlipHorizontal })]
        public void GetRotationParametersForImage_ImageMisOriented_ReturnsCorrectEncoderParameters(ImageOrientation orientation, EncoderValue[] expectedValues)
        {
            // Arrange
            var image = Fake.Image(orientation);
            var factory = Fake.ImageFactory(image);
            var rotator = new ImageRotator(factory);

            // Act
            var parameters = rotator.GetRotationParameters(image);

            // Assert
            Assert.IsNotNull(parameters);
            Assert.AreEqual(expectedValues.Length, parameters.Param.Length);

            foreach (var val in expectedValues)
            {
                Assert.IsTrue(parameters.Param.Any(param =>
                {
                    var valueField = param.GetType().GetField("_parameterValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var valuePointer = (IntPtr)valueField.GetValue(param);
                    var value = (EncoderValue)Marshal.ReadInt32(valuePointer);

                    return param.Encoder.Guid == Encoder.Transformation.Guid
                           && param.NumberOfValues == 1
                           && param.ValueType == EncoderParameterValueType.ValueTypeLong
                           && value == val;
                }));
            }
        }

        private static class Fake
        {
            public static IImage Image(ImageOrientation orientation = ImageOrientation.TopLeft)
            {
                var image = Substitute.For<IImage>();

                image.Orientation.Returns(orientation);

                return image;
            }

            public static IImageFactory ImageFactory(IImage image)
            {
                var factory = Substitute.For<IImageFactory>();

                factory.OpenImage(Arg.Any<string>()).Returns(image);

                return factory;
            }
        }
    }
}