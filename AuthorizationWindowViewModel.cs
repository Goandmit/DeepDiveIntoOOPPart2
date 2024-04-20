using System.Collections.ObjectModel;
using System.Windows;

namespace DeepDiveIntoOOPPart2
{
    internal class AuthorizationWindowViewModel
    {
        public ObservableCollection<Worker> Workers { get; set; }
        public Worker SelectedWorker { get; set; }        

        public AuthorizationWindowViewModel()
        {
            Workers = new ObservableCollection<Worker>();

            Consultant consultant = new Consultant();
            Workers.Add(consultant);

            Manager manager = new Manager();
            Workers.Add(manager);
        }

        private RelayCommand oKCommand;
        public RelayCommand OKCommand
        {
            get
            {
                return oKCommand ??
                  (oKCommand = new RelayCommand(obj =>
                  {
                      if (SelectedWorker is Worker worker)
                      {
                          Repository.CurrentUser = worker;

                          ClientList clientList = new ClientList();

                          clientList.Show();

                          foreach (Window window in App.Current.Windows)
                          {
                              if (window is AuthorizationWindow)
                              {
                                  window.Close();
                              }
                          }
                      }
                  }));
            }
        }        
    }
}
