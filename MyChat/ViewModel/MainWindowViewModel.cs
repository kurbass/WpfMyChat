using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using MyChat.Infrastructure;
using MyChat.Model;
using MyChat.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace MyChat.ViewModel
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        ChatDbContext db;
        IConfigurationRoot configuration;
        IPAddress multicastAddress = null;
        int remotePort = 0;
        int localPort = 0;

        public MainWindowViewModel(string u)
        {
            User = u;
            configuration = new ConfigurationBuilder().AddJsonFile("conf.json").Build();
            multicastAddress = IPAddress.Parse(configuration["Address"].Trim());
            remotePort = Convert.ToInt32(configuration["RemotePort"].Trim());
            localPort = Convert.ToInt32(configuration["LocalPort"].Trim());

            Thread t = new Thread(Listen);
            t.IsBackground = true;
            t.Start();
        }

        void Listen()
        {
            try
            {
                UdpClient receiver = new UdpClient();
                IPEndPoint localEp = new IPEndPoint(IPAddress.Any, localPort);

                receiver.ExclusiveAddressUse = false;
                receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                receiver.Client.Bind(localEp);
                receiver.JoinMulticastGroup(multicastAddress);

                while (true)
                {
                    IPEndPoint ep = null;
                    byte[] resp = receiver.Receive(ref ep);

                    ChatData cdata = JsonSerializer.Deserialize<ChatData>(resp);

                    TbChat += cdata.NickName + " " + DateTime.Now.ToString() + "\r\n" + cdata.Message + "\r\n\r\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string _user;

        public string User
        {
            get => _user;
            set
            {
                if (value != _user)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }

        string _tbMessage;

        public string TbMessage
        {
            get => _tbMessage;
            set
            {
                if (value != _tbMessage)
                {
                    _tbMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        string _tbChat;

        public string TbChat
        {
            get => _tbChat;
            set
            {
                if (value != _tbChat)
                {
                    _tbChat = value;
                    OnPropertyChanged();
                }
            }
        }

        RelayCommand _sendCommand;

        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new RelayCommand(SendCommandMethod));
            }
        }

        void SendCommandMethod(object param)
        {
            ChatData cdata = new ChatData()
            {
                NickName = User,
                Message = TbMessage
            };
            Thread t = new Thread(SendMessage);
            t.IsBackground = true;
            t.Start(cdata);
        }

        void SendMessage(object obj)
        {
            if (!string.IsNullOrEmpty(_tbMessage))
            {
                ChatData cdata = (ChatData)obj;
                try
                {
                    UdpClient client = new UdpClient();
                    client.JoinMulticastGroup(multicastAddress);
                    IPEndPoint sendEP = new IPEndPoint(multicastAddress, remotePort);

                    string json = JsonSerializer.Serialize<ChatData>(cdata);

                    byte[] buf = Encoding.UTF8.GetBytes(json);
                    client.Send(buf, buf.Length, sendEP);
                    client.Close();
                    TbMessage = String.Empty;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        RelayCommand _changeStyleCommand;

        public RelayCommand ChangeStyleCommand
        {
            get
            {
                return _changeStyleCommand ?? (_changeStyleCommand = new RelayCommand(ChangeStyleCommandMethod));
            }
        }

        void ChangeStyleCommandMethod(object param)
        {
            if (Settings.Default.ifLightTheme)
            {
                var uri = new Uri(@"DarkTheme.xaml", UriKind.Relative);
                ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                Settings.Default.ifLightTheme = false;
            }
            else
            {
                var uri = new Uri(@"LightTheme.xaml", UriKind.Relative);
                ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                Settings.Default.ifLightTheme = true;
            }    
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
