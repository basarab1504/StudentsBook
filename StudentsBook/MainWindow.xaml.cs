using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.UI.Xaml.Schedule;

namespace StudentsBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StudentModel studentModel = new StudentModel();
        HomeworkModel homeworkModel = new HomeworkModel();
        SubjectModel subjectModel = new SubjectModel();

        SubjectsViewModel subjectsViewModel;
        StatisticViewModel statisticViewModel;

        public MainWindow()
        {
            InitializeComponent();
            subjectsViewModel = new SubjectsViewModel(studentModel, subjectModel);
            statisticViewModel = new StatisticViewModel();
            this.DataContext = new
            {
                Students = new StudentsViewModel(studentModel),
                Homeworks = new HomeworksViewModel(homeworkModel),
                Subjects = subjectsViewModel,
                Statistic = statisticViewModel
            };
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FakeDB.Save();
        }

        private void SyncItem_Click(object sender, RoutedEventArgs e)
        {
            subjectModel.Items.Clear();
            foreach(var i in GoogleCalendar.GetSubjects(new DateTime(DateTime.Today.Year, 1, 1), new DateTime(DateTime.Today.Year, 12, 31)))
            {
                subjectModel.Items.Add(i);
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            subjectsViewModel.DatesChanged.Execute(calendar.SelectedDates);
        }

        private void Calendar_StatsSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            statisticViewModel.DatesChanged.Execute(statsCalendar.SelectedDates);
        }
    }
}
