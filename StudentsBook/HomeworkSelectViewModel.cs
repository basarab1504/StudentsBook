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
    class HomeworksSelectViewModel : NotifyPropertyChanged
    {
        private HomeworkRepository homeworkRepository;

        private Homework selectedPickedHomework;
        private Homework selectedSettedHomework;

        private RelayCommand addCommand;
        private RelayCommand removeCommand;

        public HomeworksSelectViewModel()
        { 
            homeworkRepository = new HomeworkRepository();
            AllHomeworks = new ObservableCollection<Homework>(homeworkRepository.GetAllHomeworks());
            SettedHomeworks = new ObservableCollection<Homework>();
        }

        public ObservableCollection<Homework> AllHomeworks { get; set; }
        public ObservableCollection<Homework> SettedHomeworks { get; set; }

        public Homework SelectedPickedHomework
        {
            get { return selectedPickedHomework; }
            set
            {
                selectedPickedHomework = value;
                OnPropertyChanged("SelectedPickedHomework");
            }
        }
        public Homework SelectedSettedHomework
        {
            get { return selectedSettedHomework; }
            set
            {
                selectedSettedHomework = value;
                OnPropertyChanged("SelectedSettedHomework");
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Homework homework = obj as Homework;
                      if (homework != null)
                      {
                          //foreach(var h in homework)
                            SettedHomeworks.Add(homework);
                      }
                  }/*,
                  (obj) => (obj as Homework)?.Count() > 0*/));
            }
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      Homework homework = obj as Homework;
                      if (homework != null)
                      {
                          //foreach (var h in homework)
                              SettedHomeworks.Remove(homework);
                      }
                  }/*,
                  (obj) => (obj as Homework)?.Count() > 0*/));
            }
        }
    }
}
