using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StudentsBook
{
    class SubjectsViewModel : NotifyPropertyChanged
    {
        private Subject selectedSubject;

        private RelayCommand addCommand;
        private RelayCommand removeCommand;
        private RelayCommand datesChanged;
        private RelayCommand googleLoad;
        private RelayCommand editHomeworkCommand;

        private SelectedDatesCollection dates;

        SubjectModel subjectModel;

        public SubjectsViewModel(StudentModel studentModel, SubjectModel subjectModel)
        {
            Subjects = new ObservableCollection<Subject>();
            Students = studentModel.Items;
            this.subjectModel = subjectModel;
        }

        public SelectedDatesCollection Dates
        {
            get { return dates; }
            set
            {
                dates = value;
                OnPropertyChanged("Dates");
            }
        }

        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Subject> Subjects { get; set; }

        public Subject SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                selectedSubject = value;
                OnPropertyChanged("SelectedSubject");
            }
        }

        public RelayCommand DatesChanged
        {
            get
            {
                return datesChanged ??
                  (datesChanged = new RelayCommand(obj =>
                  {
                      SelectedDatesCollection dates = obj as SelectedDatesCollection;

                      DateTime start = dates[0];
                      DateTime end = dates[dates.Count - 1].AddHours(23);

                      Subjects.Clear();
                      foreach(var x in subjectModel.Items.OrderBy(x=> x.From))
                      {
                          if (x.From >= start && x.From <= end)
                              Subjects.Add(x);
                      }
                      Dates = dates;
                  }));
            }
        }
        
        public RelayCommand GoogleLoad
        {
            get
            {
                return googleLoad ??
                    (googleLoad = new RelayCommand(obj =>
                    {
                        SelectedDatesCollection dates = obj as SelectedDatesCollection;
                        var loaded = GoogleCalendar.GetSubjects(dates[0], dates[dates.Count - 1]);
                        var existed = Subjects.Where(x => x.From >= dates[0] && x.From <= dates[dates.Count - 1]);

                        foreach (var e in loaded)
                        {
                            if(!existed.Any(x => x.From == e.From && x.Student.Name == e.Student.Name))
                                Subjects.Add(e);
                        }
                    }));
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Subject subject = new Subject() { From = Dates[0], To = Dates[0] };
                      subjectModel.Add(subject);
                      Subjects.Add(subject);
                      SelectedSubject = subject;
                  }));
            }
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                    (removeCommand = new RelayCommand(obj =>
                    {
                        Subject subject = obj as Subject;
                        if (subject != null)
                        {
                            subjectModel.Remove(subject);
                            Subjects.Remove(subject);
                        }
                    },
                    (obj) => Subjects.Count > 0));
            }
        }

        public RelayCommand EditHomeworkCommand
        {
            get
            {
                return editHomeworkCommand ??
                    (editHomeworkCommand = new RelayCommand(obj =>
                    {
                        Subject subject = obj as Subject;
                        if (subject != null)
                        {
                            new HomeworkWindow(SelectedSubject).Show();
                        }
                    }));
            }
        }
    }
}
