using Automate.Utils;
using System.Globalization;
using System.Windows.Media;

namespace Automate.TestsNUnit.Utils
{
    public class BoolToColorConverterTests
    {
        private BoolToColorConverter boolToColorConverter;

        private readonly Type DEFAULT_TYPE = typeof(int);
        private readonly object DEFAULT_PARAMETER = new object();
        private readonly CultureInfo DEFAULT_CULTURE = CultureInfo.InvariantCulture;
        private readonly SolidColorBrush? RED_BRUSH = new BrushConverter().ConvertFrom("#c50500") as SolidColorBrush;

        [SetUp]
        public void SetUp()
        {
            boolToColorConverter = new BoolToColorConverter();
        }

        [Test]
        public void Convert_ValueNotBool_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.Convert("", DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.That(result, Is.EqualTo(Brushes.Transparent));
        }

        [Test]
        public void Convert_ValueFalse_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.Convert(false, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.That(result, Is.EqualTo(Brushes.Transparent));
        }

        [Test]
        public void Convert_ValueTrue_ReturnRedBrush()
        {
            SolidColorBrush? result = boolToColorConverter.Convert(true, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE) as SolidColorBrush;

            Assert.That(result!.Color, Is.EqualTo(RED_BRUSH!.Color));
        }

        [Test]
        public void ConvertBack_ValueNotBool_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.ConvertBack("", DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.That(result, Is.EqualTo(Brushes.Transparent));
        }

        [Test]
        public void ConvertBack_ValueFalse_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.ConvertBack(false, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.That(result, Is.EqualTo(Brushes.Transparent));
        }

        [Test]
        public void ConvertBack_ValueTrue_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.ConvertBack(true, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.That(result, Is.EqualTo(Brushes.Transparent));
        }
    }
}
