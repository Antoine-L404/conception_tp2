using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Models;
using Automate.Utils.Enums;
using Moq;
using System.Windows.Controls;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class CalendarViewModelTests
    {
        private readonly string noEvenTitle = "Aucun événement";

        private CalendarViewModel? calendarViewModel;
        private Mock<ITasksServices>? tasksServicesMock;
        private Mock<Calendar>? calendarMock;
        private Mock<INavigationUtils>? navigationUtilsMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                tasksServicesMock = new Mock<ITasksServices>();
                tasksServicesMock.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

                calendarMock = new Mock<Calendar>();
                navigationUtilsMock = new Mock<INavigationUtils>();

                calendarViewModel = new CalendarViewModel(
                    calendarMock.Object, tasksServicesMock.Object, navigationUtilsMock.Object);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void DateSelected_SelectedDateInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.DateSelected();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsNoTask_EventTitlesCountIsOne()
        {
            const int expectedCount = 1;
            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

            calendarViewModel.DateSelected();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsNoTask_EventTitlesValueIsCorrect()
        {
            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

            calendarViewModel.DateSelected();

            Assert.AreEqual(noEvenTitle, calendarViewModel.EventTitles[0]);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsOneTask_EventTitlesCountIsOne()
        {
            const int expectedCount = 1;
            UpcomingTask returnedTask = new UpcomingTask() { Title = EventType.Semis };
            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask });

            calendarViewModel.DateSelected();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsOneTask_EventTitlesValueIsCorrect()
        {
            string expectedValue = EventType.Semis.ToString();
            UpcomingTask returnedTask = new UpcomingTask() { Title = EventType.Semis };
            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask });

            calendarViewModel.DateSelected();

            Assert.AreEqual(expectedValue, calendarViewModel.EventTitles[0]);
        }
    }
}
