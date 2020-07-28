using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentsBook
{
    class SubjectsViewModel : NotifyPropertyChanged
    {
        private DateTime dateTime;
        private Subject selectedSubject;

        private RelayCommand addCommand;
        private RelayCommand removeCommand;
        private RelayCommand editHomeworkCommand;
        private RelayCommand saveCommand;

        private StudentRepository studentRepository;
        private HomeworkRepository homeworkRepository;
        private SubjectRepository subjectRepository;

        public SubjectsViewModel()
        {
            studentRepository = new StudentRepository();
            Students = new ObservableCollection<Student>(studentRepository.GetAllStudents());
            homeworkRepository = new HomeworkRepository();
            subjectRepository = new SubjectRepository();
            Subjects = new ObservableCollection<Subject>(subjectRepository.GetAllSubjects());
            Homeworks = new ObservableCollection<Homework>(homeworkRepository.GetAllHomeworks());
        }

        public ObservableCollection<Subject> Subjects { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Homework> Homeworks { get; set; }

        public DateTime PickedDate
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                OnPropertyChanged("PickedDate");
            }
        }

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
                      Subject student = new Subject() { };
                      Subjects.Add(student);
                      SelectedSubject = student;
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
                            Subjects.Remove(student);
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
                            new HomeworkWindow().Show();
                        }
                    }));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        
                    }));
            }
        }
    }
}
