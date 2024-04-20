using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DeepDiveIntoOOPPart2
{
    internal class ClientListViewModel
    {
        public ObservableCollection<ClientListItem> ClientListItems { get; set; }
        public ClientListItem SelectedСlientListItem { get; set; }

        public bool CreateButtonIsEnabled { get; set; }
        public bool DeleteButtonIsEnabled { get; set; }        

        private void FillClientList()
        {
            List<ClientListItem> list = Repository.GetСlientListItems();

            foreach (ClientListItem clientListItem in list)
            {
                ClientListItems.Add(clientListItem);
            }
        }        

        public ClientListViewModel()
        {
            ClientListItems = new ObservableCollection<ClientListItem>();            

            FillClientList();           
        }

        private void ClientForm_Closed(object sender, EventArgs e)
        {
            ClientListItems.Clear();
            FillClientList();
        }

        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty($"{SelectedСlientListItem as ClientListItem}"))
                      {
                          Repository.CurrentId = SelectedСlientListItem.ClientID;

                          ClientForm clientForm = new ClientForm();

                          foreach (Window window in App.Current.Windows)
                          {
                              if (window is ClientList)
                              {
                                  clientForm.Owner = window;
                              }
                          }

                          clientForm.Show();

                          clientForm.Closed += ClientForm_Closed;
                      }                      

                  }));
            }
        }

        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                  (createCommand = new RelayCommand(obj =>
                  {
                      Repository.CurrentId = String.Empty;

                      ClientForm clientForm = new ClientForm();

                      foreach (Window window in App.Current.Windows)
                      {
                          if (window is ClientList)
                          {
                              clientForm.Owner = window;
                          }
                      }

                      clientForm.Show();

                      clientForm.Closed += ClientForm_Closed;

                  }));
            }
        }        

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrEmpty($"{SelectedСlientListItem as ClientListItem}"))
                      {
                          Repository.CurrentId = SelectedСlientListItem.ClientID;

                          Repository.DeleteClient();

                          ClientListItems.Clear();
                          FillClientList();
                      }                         

                  }));
            }
        }
    }
}
