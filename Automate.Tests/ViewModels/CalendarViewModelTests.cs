using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Abstract.ViewModels;
using Automate.Models;
using Automate.Utils.Enums;
using Automate.ViewModels;
using Moq;
using System.Windows;
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
        private Mock<Window>? windowMock;
        private Mock<ITaskFormViewModel>? taskFormViewModelMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                tasksServicesMock = new Mock<ITasksServices>();
                tasksServicesMock.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>())).Returns(new List<UpcomingTask>());

                calendarMock = new Mock<Calendar>();
                navigationUtilsMock = new Mock<INavigationUtils>();
                windowMock = new Mock<Window>();
                taskFormViewModelMock = new Mock<ITaskFormViewModel>();

                calendarViewModel = new CalendarViewModel(
                    null, tasksServicesMock.Object, navigationUtilsMock.Object);
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

        [TestMethod]
        public void DateSelected_SelectedDateContainsManyTasks_EventTitlesCountIsCorrect()
        {
            const int expectedCount = 2;
            UpcomingTask returnedTask1 = new UpcomingTask() { Title = EventType.Semis };
            UpcomingTask returnedTask2 = new UpcomingTask() { Title = EventType.Entretien };

            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask1, returnedTask2 });

            calendarViewModel.DateSelected();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void DateSelected_SelectedDateContainsManyTasks_EventTitlesValuesAreCorrect()
        {
            UpcomingTask returnedTask1 = new UpcomingTask() { Title = EventType.Semis };
            UpcomingTask returnedTask2 = new UpcomingTask() { Title = EventType.Entretien };

            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask1, returnedTask2 });

            calendarViewModel.DateSelected();

            Assert.AreEqual(returnedTask1.Title.ToString(), calendarViewModel.EventTitles[0]);
            Assert.AreEqual(returnedTask2.Title.ToString(), calendarViewModel.EventTitles[1]);
        }

        [TestMethod]
        public void AddTask_SelectedDateInvalid_AddErrorToErrorsCollection()
        {
            calendarViewModel!.AddTask();

            Assert.IsTrue(calendarViewModel!.HasErrors);
        }

        [TestMethod]
        public void AddTask_SelectedDateIsValid_CallTasksServices()
        {
            DateTime selectedDate = DateTime.Today;
            calendarViewModel!.SelectedDate = selectedDate;

            taskFormViewModelMock!.Setup(x => x.SelectedEventType).Returns(EventType.Semis);
            navigationUtilsMock!.Setup(x => x.GetTaskFormValues(selectedDate, null)).Returns(taskFormViewModelMock.Object);

            calendarViewModel!.AddTask();

            tasksServicesMock!.Verify(x => x.CreateTask(It.IsAny<UpcomingTask>()), Times.Once());
        }

        [TestMethod]
        public void AddTask_SelectedDateIsValid_EventTitlesCountIsOne()
        {
            const int expectedCount = 1;
            UpcomingTask returnedTask = new UpcomingTask() { Title = EventType.Semis };

            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask });

            DateTime selectedDate = DateTime.Today;
            calendarViewModel!.SelectedDate = selectedDate;

            taskFormViewModelMock!.Setup(x => x.SelectedEventType).Returns(EventType.Semis);
            navigationUtilsMock!.Setup(x => x.GetTaskFormValues(selectedDate, null)).Returns(taskFormViewModelMock.Object);

            calendarViewModel.AddTask();

            Assert.AreEqual(expectedCount, calendarViewModel.EventTitles.Count);
        }

        [TestMethod]
        public void AddTask_SelectedDateIsValid_EventTitlesValueIsCorrect()
        {
            string expectedValue = EventType.Semis.ToString();
            UpcomingTask returnedTask = new UpcomingTask() { Title = EventType.Semis };

            calendarViewModel!.SelectedDate = DateTime.Today;
            tasksServicesMock!.Setup(x => x.GetTasksByDate(It.IsAny<DateTime>()))
                .Returns(new List<UpcomingTask>() { returnedTask });

            DateTime selectedDate = DateTime.Today;
            calendarViewModel!.SelectedDate = selectedDate;

            taskFormViewModelMock!.Setup(x => x.SelectedEventType).Returns(EventType.Semis);
            navigationUtilsMock!.Setup(x => x.GetTaskFormValues(selectedDate, null)).Returns(taskFormViewModelMock.Object);

            calendarViewModel.AddTask();

            Assert.AreEqual(expectedValue, calendarViewModel.EventTitles[0]);
        }

    }
}
