using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.ViewModels;
using Automate.Views;
using Moq;
using System.Windows;

namespace Automate.TestsNUnit.ViewModels
{
    [Apartment(ApartmentState.STA)]
    public class HomeViewModelTests
    {
        private const string alertMessage = "ATTENTION - Il y a un événement critique prévu aujourd'hui.";

        private HomeViewModel homeViewModel;
        private Mock<Window> mockWindow;
        private Mock<INavigationUtils> mockNavigationUtils;
        private Mock<ITasksServices> tasksServices;

        [SetUp]
        public void Setup()
        {
            mockWindow = new Mock<Window>();
            mockNavigationUtils = new Mock<INavigationUtils>();
            tasksServices = new Mock<ITasksServices>();

            homeViewModel = new HomeViewModel(mockWindow.Object, mockNavigationUtils.Object, tasksServices.Object);
        }

        [Test]
        public void GoToCalendar_NavigateToCalendar()
        {
            homeViewModel.GoToCalendar();

            mockNavigationUtils.Verify(
                x => x.NavigateToAndCloseCurrentWindow<CalendarWindow>(It.IsAny<Window>()), Times.Once());
        }

        [Test]
        public void CriticalTaskMessage_NoCriticalTask_ReturnEmptyString()
        {
            tasksServices.Setup(x => x.DoesTodayHasCriticalTask()).Returns(false);

            Assert.That(homeViewModel.CriticalTaskMessage, Is.EqualTo(""));
        }

        [Test]
        public void CriticalTaskMessage_HasCriticalTask_ReturnAlertMessage()
        {
            tasksServices.Setup(x => x.DoesTodayHasCriticalTask()).Returns(true);

            Assert.That(homeViewModel.CriticalTaskMessage, Is.EqualTo(alertMessage));
        }
    }
}
