using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace StudentsBook
{
    /// <summary>
    /// Логика взаимодействия для HomeworkWindow.xaml
    /// </summary>
    public partial class HomeworkWindow : Window
    {
        public HomeworkWindow()
        {
            InitializeComponent();
            this.DataContext = new
            {
                Homeworks = new HomeworksSelectViewModel()
            };
        }
    }
}
