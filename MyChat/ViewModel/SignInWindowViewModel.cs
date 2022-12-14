using MyChat.Infrastructure;
using MyChat.Model;
using MyChat.View;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MyChat.ViewModel
{
    internal class SignInWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        ChatDbContext db;
        SignInWindow window;

        RelayCommand _logInCommand;
        public RelayCommand LogInCommand
        {
            get
            {
                return _logInCommand ?? (_logInCommand = new RelayCommand(LogInCommandMethod, CanLogInMethod));
            }
        }

        void LogInCommandMethod(object param)
        {
            try
            {
                var list = db.Users.ToList<User>();
                var user = list.Find(x => x.Login.Equals(_login));
                if (user == null)
                {
                    UserNotExistWindow nw = new UserNotExistWindow(_login, _password);
                    nw.ShowDialog();
                }
                else if (user.Password == _password && user.Login == _login)
                {
                    var mainWindow = new MainWindow(user.Login);
                    mainWindow.Show();
                    window.Close();
                    MessageBox.Show("You have successfully log in");
                }
                else if (user.Login == _login && user.Password != _password)
                {
                    MessageBox.Show("Wrong password");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        bool CanLogInMethod(object param)
        {
            if (Login.Length < 5 || Password.Length < 5)
            {
                return false;
            }
            return true;
        }

        string _login;

        public string Login
        {
            get => _login;
            set
            {
                if (value != _login)
                {
                    _login = value.Trim();
                    OnPropertyChanged();
                }
            }
        }

        string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {

                    _password = value.Trim();
                    OnPropertyChanged();
                }
            }
        }

        public SignInWindowViewModel(SignInWindow w)
        {
            db = new ChatDbContext();
            window = w;
            _login = string.Empty;
            _password = string.Empty;
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
