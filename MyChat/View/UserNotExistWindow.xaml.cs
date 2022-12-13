using MyChat.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyChat.View
{
    /// <summary>
    /// Логика взаимодействия для UserNotExistWindow.xaml
    /// </summary>
    public partial class UserNotExistWindow : Window
    {
        public UserNotExistWindow(string u, string p)
        {
            InitializeComponent();
            DataContext = new UserNotExistWindowViewModel(u, p, this); 
        }
    }
}
