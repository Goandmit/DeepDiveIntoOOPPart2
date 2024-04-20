using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeepDiveIntoOOPPart2
{
    internal class Manager : Worker
    {
        private readonly string position = "Менеджер";

        public Manager()
        {
            Position = this.position;
        }

        public override ClientListViewModel GetClientListVM()
        {
            ClientListViewModel clientListVM = base.GetClientListVM();

            clientListVM.CreateButtonIsEnabled = true;
            clientListVM.DeleteButtonIsEnabled = true;

            return clientListVM;
        }

        public override ClientFormViewModel GetClientFormVM()
        {
            ClientFormViewModel clientFormVM = base.GetClientFormVM();

            clientFormVM.SurnameTextBoxIsEnabled = true;
            clientFormVM.NameTextBoxIsEnabled = true;
            clientFormVM.PatronymicTextBoxIsEnabled = true;
            clientFormVM.PhoneNumberTextBoxIsEnabled = true;
            clientFormVM.PassportSeriesTextBoxIsEnabled = true;
            clientFormVM.PassportNumberTextBoxIsEnabled = true;

            clientFormVM.WriteButtonIsEnabled = true;
            clientFormVM.OKButtonIsEnabled = true;
            clientFormVM.HistoryButtonIsEnabled = true;           

            return clientFormVM;
        }

        public void WriteClient(Client client)
        {
            string clientInString = Repository.ConvertClientToString(client);

            using (StreamWriter streamWriter =
                new StreamWriter(Repository.ClientFilePath, true, Encoding.Unicode))
            {
                streamWriter.WriteLine(clientInString);
            }
        }

        private string ProcessChangedFields(string changedFields)
        {
            changedFields = changedFields.Trim();
            changedFields = changedFields.TrimEnd(';');

            return changedFields;
        }

        public string OwerwriteClient(Client client)
        {
            string changedFields = String.Empty;

            List<Client> clients = Repository.GetAllClientsFromFile();

            for (int i = 0; i < clients.Count; i++)
            {
                if (client.ClientId == clients[i].ClientId)
                {
                    if (client.Surname != clients[i].Surname)
                    {
                        changedFields = $"Фамилия " +
                            $"(было \"{clients[i].Surname}\", " +
                            $"стало \"{client.Surname}\"); ";
                        clients[i].Surname = client.Surname;
                    }
                    if (client.Name != clients[i].Name)
                    {
                        changedFields += $"Имя " +
                            $"(было \"{clients[i].Name}\", " +
                            $"стало \"{client.Name}\"); ";
                        clients[i].Name = client.Name;
                    }
                    if (client.Patronymic != clients[i].Patronymic)
                    {
                        changedFields += $"Отчество " +
                            $"(было \"{clients[i].Patronymic}\", " +
                            $"стало \"{client.Patronymic}\"); ";
                        clients[i].Patronymic = client.Patronymic;
                    }
                    if (client.PhoneNumber != clients[i].PhoneNumber)
                    {
                        changedFields += $"Номер телефона " +
                            $"(было \"{clients[i].PhoneNumber}\", " +
                            $"стало \"{client.PhoneNumber}\"); ";
                        clients[i].PhoneNumber = client.PhoneNumber;
                    }
                    if (client.PassportSeries != clients[i].PassportSeries)
                    {
                        changedFields += $"Серия паспорта " +
                            $"(было \"{clients[i].PassportSeries}\", " +
                            $"стало \"{client.PassportSeries}\"); ";
                        clients[i].PassportSeries = client.PassportSeries;
                    }
                    if (client.PassportNumber != clients[i].PassportNumber)
                    {
                        changedFields += $"Номер паспорта " +
                            $"(было \"{clients[i].PassportNumber}\", " +
                            $"стало \"{client.PassportNumber}\")";
                        clients[i].PassportNumber = client.PassportNumber;
                    }

                    if (changedFields.Length != 0)
                    {
                        Repository.WriteClientsToFile(clients);
                        ProcessChangedFields(changedFields);
                    }
                    else
                    {
                        changedFields = "Перезапись без изменения полей";
                    }

                    break;
                }
            }

            if (changedFields.Length == 0)
            {
                changedFields = "Идентификатор не найден";
            }

            return changedFields;
        }

        public void DeleteClient()
        {
            List<Client> clients = Repository.GetAllClientsFromFile();

            Client client = Repository.FindClient(clients, Repository.CurrentId);
            
            if (client != null)
            {
                clients.Remove(client);

                Repository.WriteClientsToFile(clients);

                File.Delete(Repository.GenerateHistoryFilePath(Repository.CurrentId));
            }            
        }
    }
}
