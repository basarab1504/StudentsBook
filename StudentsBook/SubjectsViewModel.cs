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

        private IEnumerable<Subject> subjects;

        SubjectModel subjectModel;

        public SubjectsViewModel(StudentModel studentModel, SubjectModel subjectModel)
        {
            this.subjectModel = subjectModel;
            Students = studentModel.Items;
            //Subjects = subjectModel.Items;
        }


        public IEnumerable<Subject> Subjects
        {
            get { return subjects; }
            set
            {
                subjects = value;
                OnPropertyChanged("Subjects");
            }
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

        public Subject SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                selectedSubject = value;
                OnPropertyChanged("SelectedSubject");
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Subject student = new Subject() { From = DateTime.Now, To = DateTime.Now };
                      subjectModel.Add(student);
                      SelectedSubject = student;
                  }));
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

                      Subjects = subjectModel.Items.Where(x => x.From >= start && x.From <= end).OrderBy(x => x.From);
                      //MessageBox.Show(subjectModel.Items.Where(x => x.From >= dates[0] && x.From <= dates[dates.Count - 1]).Count().ToString());
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
                        var coll = GoogleCalendar.GetSubjects(dates[0], dates[dates.Count - 1]);
                        foreach (var s in coll)
                        {
                            subjectModel.Add(s);
                        }
                        Subjects = coll;
                        //MessageBox.Show(Subjects.ElementAt(0).From.ToString());
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
                        Subject student = obj as Subject;
                        if (student != null)
                        {
                            subjectModel.Items.Remove(student);
                        }
                    },
                    (obj) => subjectModel.Items.Count > 0));
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
