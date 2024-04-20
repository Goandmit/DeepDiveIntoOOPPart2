using System.Collections.Generic;

namespace DeepDiveIntoOOPPart2
{
    internal class Consultant : Worker
    {
        private readonly string position = "Консультант";

        public Consultant()
        {
            Position = this.position;
        }

        public override ClientFormViewModel GetClientFormVM()
        {
            ClientFormViewModel clientFormVM = base.GetClientFormVM();

            clientFormVM.PhoneNumberTextBoxIsEnabled = true;

            clientFormVM.WriteButtonIsEnabled = true;
            clientFormVM.OKButtonIsEnabled = true;              

            return clientFormVM;
        }

        public string OwerwriteClient(Client client)
        {
            string changedFields = "Перезапись без изменения полей";

            List<Client> clients = Repository.GetAllClientsFromFile();

            for (int i = 0; i < clients.Count; i++)
            {
                if (client.ClientId == clients[i].ClientId)
                {
                    if (client.PhoneNumber != clients[i].PhoneNumber)
                    {
                        changedFields = $"Номер телефона " +
                            $"(было \"{clients[i].PhoneNumber}\", " +
                            $"стало \"{client.PhoneNumber}\"); ";
                        clients[i].PhoneNumber = client.PhoneNumber;

                        Repository.WriteClientsToFile(clients);
                    }

                    break;
                }
            }

            return changedFields;
        }
    }
}
