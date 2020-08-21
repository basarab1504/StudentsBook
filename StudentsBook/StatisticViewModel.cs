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
        private int pIncome;
        private int fIncome;
        private int lessons;
        private RelayCommand datesChanged;

        public StatisticViewModel()
        {
            SeriesCollection = new SeriesCollection();
            ColumnSeriesCollection = new SeriesCollection();
            YearSeriesCollection = new SeriesCollection();

            //foreach (Student s in FakeDB.Students)
            //{
            //    ColumnSeriesCollection.Add(new ColumnSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name).Count() } });
            //}


            //foreach (Student s in FakeDB.Students)
            //{
            //    SeriesCollection.Add(new PieSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name).Count() } });
            //}

            MonthLabels = new[] { "Январь", "Ферваль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

            Formatter = value => value.ToString("N");
        }

        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection ColumnSeriesCollection { get; set; }
        public SeriesCollection YearSeriesCollection { get; set; }

        public int Lessons
        {
            get { return lessons; }
            set
            {
                lessons = value;
                OnPropertyChanged("Lessons");
            }
        }

        public int PIncome
        {
            get { return pIncome; }
            set
            {
                pIncome = value;
                OnPropertyChanged("PIncome");
            }
        }

        public int FIncome
        {
            get { return fIncome; }
            set
            {
                fIncome = value;
                OnPropertyChanged("FIncome");
            }
        }

        public string[] Labels { get; set; }
        public string[] MonthLabels { get; set; }
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

                      DateTime start = dates[0];
                      DateTime end = dates[dates.Count - 1].AddHours(23);

                      foreach (Student s in FakeDB.Students)
                      {
                          SeriesCollection.Add(new PieSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name && x.From >= start && x.From <= end).Count() } });
                      }

                      foreach (Student s in FakeDB.Students)
                      {
                          ColumnSeriesCollection.Add(new ColumnSeries() { Title = s.Name, Values = new ChartValues<int> { FakeDB.Subjects.Where(x => x.Student.Name == s.Name && x.From >= start && x.From <= end).Sum(x => x.Student.Payment) } });
                      }

                      DateTime _start = new DateTime(DateTime.Today.Year, 1, 1);
                      DateTime _end = new DateTime(DateTime.Today.Year, 12, 31);

                      if(YearSeriesCollection.Count == 0)
                      {
                          LineSeries ls = new LineSeries();
                          ls.Title = "Потенциальный доход";
                          ls.Values = new ChartValues<int>();
                          while (_start < _end)
                          {
                              ls.Values.Add(FakeDB.Subjects.Where(x => x.From.Month == _start.Month).Sum(x => x.Student.Payment));
                              _start = _start.AddMonths(1);
                          }

                          _start = new DateTime(DateTime.Today.Year, 1, 1);

                          LineSeries fls = new LineSeries();
                          fls.Title = "Фактический доход";
                          fls.Values = new ChartValues<int>();
                          while (_start < _end)
                          {
                              fls.Values.Add(FakeDB.Subjects.Where(x => x.From.Month == _start.Month && x.IsPaid).Sum(x => x.Student.Payment));
                              _start = _start.AddMonths(1);
                          }

                          YearSeriesCollection.Add(ls);
                          YearSeriesCollection.Add(fls);
                      }

                      PIncome = FakeDB.Subjects.Where(x => x.From >= start && x.From <= end).Sum(x => x.Student.Payment);
                      FIncome = FakeDB.Subjects.Where(x => x.From >= start && x.From <= end).Sum(x => x.Student.Payment);
                      Lessons = FakeDB.Subjects.Where(x => x.From >= start && x.From <= end).Count();
                  }));
            }
        }
    }
}
