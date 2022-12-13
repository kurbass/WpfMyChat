using MyChat.Infrastructure;
using MyChat.Model;
using MyChat.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace MyChat.ViewModel
{
    internal class UserNotExistWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        string user;
        string password;
        UserNotExistWindow window;

        public UserNotExistWindowViewModel(string u, string p, UserNotExistWindow w) 
        {
            user = u;
            password = p;
            window = w;
        }

        RelayCommand _noCommand;

        public RelayCommand NoCommand
        {
            get
            {
                return _noCommand ?? (_noCommand = new RelayCommand(NoCommandMethod));
            }
        }

        void NoCommandMethod(object param)
        {
            window.Close();
        }

        RelayCommand _yesCommand;

        public RelayCommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand(YesCommandMethod));
            }
        }

        void YesCommandMethod(object param)
        {
            try
            {
                if (user.Length < 5 || password.Length < 5)
                {
                    MessageBox.Show("login and password must be more than 4 characters");
                    return;
                }
                ChatDbContext db = new ChatDbContext();
                db.Users.Add(new User { Login = user, Password = password });
                db.SaveChanges();
                window.Close();
                MessageBox.Show("User successfully registered");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
