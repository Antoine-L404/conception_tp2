using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Abstract.ViewModels;
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
        private Mock<ITasksServices>? tasksServices;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                mockWindow = new Mock<Window>();
                mockNavigationUtils = new Mock<INavigationUtils>();
                tasksServices = new Mock<ITasksServices>();

                homeViewModel = new HomeViewModel(mockWindow.Object, mockNavigationUtils.Object, tasksServices.Object);
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

        [TestMethod]
        public void DoesTodayHasCriticalTask_NoCriticalTask_ReturnFalse()
        {
            tasksServices!.Setup(x => x.DoesTodayHasCriticalTask()).Returns(false);

            Assert.IsFalse(homeViewModel!.DoesTodayHasCriticalTask);
        }

        [TestMethod]
        public void DoesTodayHasCriticalTask_HasCriticalTask_ReturnTrue()
        {
            tasksServices!.Setup(x => x.DoesTodayHasCriticalTask()).Returns(true);

            Assert.IsTrue(homeViewModel!.DoesTodayHasCriticalTask);
        }
    }
}
