using System.Windows;

namespace DeepDiveIntoOOPPart2
{
    /// <summary>
    /// Логика взаимодействия для ClientForm.xaml
    /// </summary>
    public partial class ClientForm : Window
    {
        internal ClientForm()
        {
            InitializeComponent();
            DataContext = Repository.CurrentUser.GetClientFormVM();
        }
    }
}
