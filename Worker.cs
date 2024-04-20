namespace DeepDiveIntoOOPPart2
{
    internal class Worker
    {
        public string Position { get; set; }              

        public virtual ClientListViewModel GetClientListVM()
        {
            ClientListViewModel clientListVM = new ClientListViewModel
            {
                CreateButtonIsEnabled = false,
                DeleteButtonIsEnabled = false
            };

            return clientListVM;
        }

        public virtual ClientFormViewModel GetClientFormVM()
        {
            ClientFormViewModel clientFormVM = new ClientFormViewModel
            {
                SurnameTextBoxIsEnabled = false,
                NameTextBoxIsEnabled = false,
                PatronymicTextBoxIsEnabled = false,
                PhoneNumberTextBoxIsEnabled = false,
                PassportSeriesTextBoxIsEnabled = false,
                PassportNumberTextBoxIsEnabled = false,                

                WriteButtonIsEnabled = false,
                OKButtonIsEnabled = false,
                HistoryButtonIsEnabled = false
            };                        

            return clientFormVM;
        }
    }
}
