using Microsoft.Extensions.Configuration;
using MyChat.Model;
using MyChat.Properties;
using MyChat.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string u)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(u);

            if (Settings.Default.IsMaximized == true)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.Height = Settings.Default.Height;
                this.Width = Settings.Default.Width;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                Settings.Default.IsMaximized = true;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.Width = this.Width;
                Settings.Default.Height = this.Height;
                Settings.Default.IsMaximized = false;
                Settings.Default.Save();
            }
            
        }
    }
}
