using Automate.Abstract.Utils;
using Automate.ViewModels;
using Automate.Views;
using Moq;
using System.Windows;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class HomeViewModelTests
    {
        private HomeViewModel? homeViewModel;
        private Mock<Window>? mockWindow;
        private Mock<INavigationUtils>? mockNavigationUtils;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                mockWindow = new Mock<Window>();
                mockNavigationUtils = new Mock<INavigationUtils>();
                homeViewModel = new HomeViewModel(mockWindow.Object, mockNavigationUtils.Object);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void GoToCalendar_NavigateToCalendar()
        {
            homeViewModel!.GoToCalendar();

            mockNavigationUtils!.Verify(x => x.NavigateToAndCloseCurrentWindow<CalendarWindow>(It.IsAny<Window>()), Times.Once());
        }
    }
}
