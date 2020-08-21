using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace StudentsBook
{
    public class StatisticViewModel : NotifyPropertyChanged
    {
        private int income;
        private int lessons;
        private RelayCommand datesChanged;

        public StatisticViewModel()
        {
            SeriesCollection = new SeriesCollection();
            ColumnSeriesCollection = new SeriesCollection();

            foreach (Student s in FakeDB.Students)
            {
                ColumnSeriesCollection.Add(new ColumnSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name).Count() } });
            }

            foreach (Student s in FakeDB.Students)
            {
                SeriesCollection.Add(new PieSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name).Count() } });
            }
            Formatter = value => value.ToString("N");
        }

        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection ColumnSeriesCollection { get; set; }
        public int Lessons
        {
            get { return lessons; }
            set
            {
                lessons = value;
                OnPropertyChanged("Lessons");
            }
        }
        public int Income
        {
            get { return income; }
            set
            {
                income = value;
                OnPropertyChanged("Income");
            }
        }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public RelayCommand DatesChanged
        {
            get
            {
                return datesChanged ??
                  (datesChanged = new RelayCommand(obj =>
                  {
                      SeriesCollection.Clear();
                      ColumnSeriesCollection.Clear();
                      SelectedDatesCollection dates = obj as SelectedDatesCollection;
                      foreach (Student s in FakeDB.Students)
                      {
                          SeriesCollection.Add(new PieSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name && x.From >= dates[0] && x.From <= dates[dates.Count - 1]).Count() } });
                      }
                      foreach (Student s in FakeDB.Students)
                      {
                          ColumnSeriesCollection.Add(new ColumnSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name && x.From >= dates[0] && x.From <= dates[dates.Count - 1]).Sum(x => x.Student.Payment) } });
                      }
                      Income = FakeDB.Subjects.Where(x => x.From >= dates[0] && x.From <= dates[dates.Count - 1]).Sum(x => x.Student.Payment);
                      Lessons = FakeDB.Subjects.Where(x => x.From >= dates[0] && x.From <= dates[dates.Count - 1]).Count();
                  }));
            }
        }
    }
}
