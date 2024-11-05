using Automate.Utils;
using System.Globalization;
using System.Windows.Media;

namespace Automate.Tests.Utils
{
    [TestClass]
    public class BoolToColorConverterTests
    {
        private BoolToColorConverter boolToColorConverter;

        private readonly Type DEFAULT_TYPE = typeof(int);
        private readonly object DEFAULT_PARAMETER = new object();
        private readonly CultureInfo DEFAULT_CULTURE = CultureInfo.InvariantCulture;
        private readonly SolidColorBrush? RED_BRUSH = new BrushConverter().ConvertFrom("#c50500") as SolidColorBrush;

        public BoolToColorConverterTests()
        {
            boolToColorConverter = new BoolToColorConverter();
        }

        [TestMethod]
        public void Convert_ValueNotBool_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.Convert("", DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.AreEqual(Brushes.Transparent, result);
        }

        [TestMethod]
        public void Convert_ValueFalse_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.Convert(false, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.AreEqual(Brushes.Transparent, result);
        }

        [TestMethod]
        public void Convert_ValueTrue_ReturnRedBrush()
        {
            SolidColorBrush? result = boolToColorConverter.Convert(true, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE) as SolidColorBrush;

            Assert.AreEqual(RED_BRUSH!.Color, result!.Color);
        }

        [TestMethod]
        public void ConvertBack_ValueNotBool_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.ConvertBack("", DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.AreEqual(Brushes.Transparent, result);
        }

        [TestMethod]
        public void ConvertBack_ValueFalse_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.ConvertBack(false, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.AreEqual(Brushes.Transparent, result);
        }

        [TestMethod]
        public void ConvertBack_ValueTrue_ReturnTransparentBrush()
        {
            var result = boolToColorConverter.ConvertBack(true, DEFAULT_TYPE, DEFAULT_PARAMETER, DEFAULT_CULTURE);

            Assert.AreEqual(Brushes.Transparent, result);
        }
    }
}
