using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DeepDiveIntoOOPPart2
{
    internal class ClientFormViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string idTextBox;

        public string IdTextBox
        {
            get { return idTextBox; }
            set
            {
                idTextBox = value;
                OnPropertyChanged("IdTextBox");
            }
        }

        public string SurnameTextBox { get; set; }
        public string NameTextBox { get; set; }
        public string PatronymicTextBox { get; set; }
        public string PhoneNumberTextBox { get; set; }
        public string PassportSeriesTextBox { get; set; }
        public string PassportNumberTextBox { get; set; }

        public bool IdTextBoxIsEnabled { get; set; }
        public bool SurnameTextBoxIsEnabled { get; set; }
        public bool NameTextBoxIsEnabled { get; set; }
        public bool PatronymicTextBoxIsEnabled { get; set; }
        public bool PhoneNumberTextBoxIsEnabled { get; set; }
        public bool PassportSeriesTextBoxIsEnabled { get; set; }
        public bool PassportNumberTextBoxIsEnabled { get; set; }

        public bool WriteButtonIsEnabled { get; set; }
        public bool OKButtonIsEnabled { get; set; }
        public bool HistoryButtonIsEnabled { get; set; }

        public ObservableCollection<string> HistoryListBox { get; set; }

        private string AssignOrReadId()
        {
            string id;

            if (IdTextBox != null)
            {
                id = IdTextBox;
            }
            else
            {
                id = (Repository.AssignId()).ToString();
                IdTextBox = id;
            }

            return id;
        }

        private Client GetClientFromForm()
        {
            Client client = new Client(IdTextBox,
                SurnameTextBox.Trim(),
                NameTextBox.Trim(),
                PatronymicTextBox.Trim(),
                String.Empty,
                PhoneNumberTextBox.Trim(),
                PassportSeriesTextBox.Trim(),
                PassportNumberTextBox.Trim());

            return client;
        }               

        private void OwerwriteOrWriteClient()
        {
            IdTextBox = AssignOrReadId();

            Client client = GetClientFromForm();            

            List<string> history = Repository.OwerwriteOrWriteClient(client);

            foreach (string item in history)
            {
                HistoryListBox.Add(item);
            }            
        }
        
        private void Message()
        {
            MessageBox.Show("Все поля должны быть заполнены",
                        "Операция не выполнена",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.OK,
                        MessageBoxOptions.DefaultDesktopOnly);
        }

        private bool CheckRequiredFields()
        {
            bool status = true;

            if (string.IsNullOrEmpty(SurnameTextBox) ||
                string.IsNullOrEmpty(NameTextBox) ||
                string.IsNullOrEmpty(PatronymicTextBox) ||
                string.IsNullOrEmpty(PhoneNumberTextBox) ||
                string.IsNullOrEmpty(PassportSeriesTextBox) ||
                string.IsNullOrEmpty(PassportNumberTextBox))
            {

                Message();
                status = false;               
            }
            else if (SurnameTextBox.Trim().Length == 0 ||
                NameTextBox.Trim().Length == 0 ||
                PatronymicTextBox.Trim().Length == 0 ||
                PhoneNumberTextBox.Trim().Length == 0 ||
                PassportSeriesTextBox.Trim().Length == 0 ||
                PassportNumberTextBox.Trim().Length == 0)
            {

                Message();
                status = false;                
            }

            return status;
        }

        public ClientFormViewModel()
        {
            if (Repository.CurrentId.Length != 0)
            {
                Client client = Repository.GetClientFromFile(Repository.CurrentId);

                if (client != null)
                {
                    IdTextBox = client.ClientId;
                    SurnameTextBox = client.Surname;
                    NameTextBox = client.Name;
                    PatronymicTextBox = client.Patronymic;
                    PhoneNumberTextBox = client.PhoneNumber;

                    if (Repository.CurrentUser.GetType() == typeof(Consultant))
                    {
                        PassportSeriesTextBox = Repository.HideString(client.PassportSeries);
                        PassportNumberTextBox = Repository.HideString(client.PassportNumber);
                    }
                    else
                    {
                        PassportSeriesTextBox = client.PassportSeries;
                        PassportNumberTextBox = client.PassportNumber;
                    }
                }                               
            }           

            IdTextBoxIsEnabled = false;

            HistoryListBox = new ObservableCollection<string>();
        }

        private RelayCommand writeCommand;
        public RelayCommand WriteCommand
        {
            get
            {
                return writeCommand ??
                  (writeCommand = new RelayCommand(obj =>
                  {
                      bool status = CheckRequiredFields();

                      if (status == true)
                      {
                          OwerwriteOrWriteClient();
                      }

                  }));
            }
        }

        private RelayCommand oKCommand;
        public RelayCommand OKCommand
        {
            get
            {
                return oKCommand ??
                  (oKCommand = new RelayCommand(obj =>
                  {
                      bool status = CheckRequiredFields();

                      if (status == true)
                      {
                          OwerwriteOrWriteClient();

                          foreach (Window window in App.Current.Windows)
                          {
                              if (window is ClientForm clientForm)
                              {
                                  clientForm.Close();
                              }
                          }                          
                      }

                  }));
            }
        }

        private RelayCommand historyCommand;
        public RelayCommand HistoryCommand
        {
            get
            {
                return historyCommand ??
                  (historyCommand = new RelayCommand(obj =>
                  {
                      HistoryListBox.Clear();

                      List<string> history = Repository.GetHistory(IdTextBox);

                      foreach (string item in history)
                      {
                          HistoryListBox.Add(item);
                      }                      

                  }));
            }
        }        
    }
}
