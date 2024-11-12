using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.ViewModels;
using Moq;
using System.Windows;
using System.Windows.Controls;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class CalendarViewModelTests
    {
        private CalendarViewModel calendarViewModel;
        private Mock<ITasksServices> tasksServicesMock;
        private Mock<Calendar> calendarMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Thread thread = new(() =>
            {
                tasksServicesMock = new Mock<ITasksServices>();
                calendarMock = new Mock<Calendar>();
                calendarViewModel = new CalendarViewModel(calendarMock.Object, tasksServicesMock.Object);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void test()
        {
            Assert.IsTrue(true);
        }
    }
}
