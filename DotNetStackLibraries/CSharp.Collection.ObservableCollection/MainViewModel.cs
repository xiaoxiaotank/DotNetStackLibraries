using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace CSharp.Collection.ObservableCollection
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1),
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Users = new ObservableCollection<User>()
            {
                new User(){Id = 1, Name = "1"},
                new User(){Id = 2, Name = "2"},
                new User(){Id = 3, Name = "3"},
                new User(){Id = 4, Name = "4"},
                new User(){Id = 5, Name = "5"},
                new User(){Id = 6, Name = "6"},
            };
            _timer.Tick += (s, e) =>
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    foreach (var user in Users)
                    {
                        user.Count++;
                    }
                    var lastUser = Users.Last();
                    Users.Add(new User() { Id = lastUser.Id + 1, Name = (lastUser.Id + 1).ToString() });
                });
            };
            _timer.Start();
        }

        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Users)));
            }
        }

    }

    class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        public int Id 
        {
            get => _id;
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }

    }
}
