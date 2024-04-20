using System.Windows;

namespace DeepDiveIntoOOPPart2
{
    /// <summary>
    /// Логика взаимодействия для ClientList.xaml
    /// </summary>
    public partial class ClientList : Window
    {
        public ClientList()
        {
            InitializeComponent();
            DataContext = Repository.CurrentUser.GetClientListVM();
        }
    }
}
