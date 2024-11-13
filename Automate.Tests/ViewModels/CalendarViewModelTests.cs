using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Moq;
using System.Windows.Controls;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class CalendarViewModelTests
    {
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
                calendarMock = new Mock<Calendar>();
                navigationUtilsMock = new Mock<INavigationUtils>();

                calendarViewModel = new CalendarViewModel(
                    calendarMock.Object, tasksServicesMock.Object, navigationUtilsMock.Object);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        //[TestMethod]
        //public void test()
        //{
        //    Assert.IsTrue(true);
        //}
    }
}
