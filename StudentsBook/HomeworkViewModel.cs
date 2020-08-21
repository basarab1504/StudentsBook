using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsBook
{
    class HomeworksViewModel : NotifyPropertyChanged
    {
        private Homework selectedHomework;

        private RelayCommand addCommand;
        private RelayCommand removeCommand;
        private RelayCommand saveCommand;

        public HomeworksViewModel(HomeworkModel model)
        {
            Homeworks = model.Items;
        }

        public ObservableCollection<Homework> Homeworks { get; set; }
        public Homework SelectedHomework
        {
            get { return selectedHomework; }
            set
            {
                selectedHomework = value;
                OnPropertyChanged("SelectedHomework");
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Homework homework = new Homework() { Title = "Домашнее задание" };
                      Homeworks.Add(homework);
                      SelectedHomework = homework;
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
                        Homework homework = obj as Homework;
                        if (homework != null)
                        {
                            Homeworks.Remove(homework);
                        }
                    },
                    (obj) => Homeworks.Count > 0));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        //repository.Save();
                    }));
            }
        }
    }
}
