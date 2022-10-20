using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AndreiKulazhin_PRE_task
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Customer selectedCustomer;
        private DialogService dialogService;
        private ICommand saveCommand;

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPorpertyChanged("SelectedCustomer");
            }
        }
        public List<Customer> SelectedCustomers { get; set; }
        public ICommand SaveCommand
        {
            get
            {
                return saveCommand ??= new TransferCommand(obj =>
                {
                    try
                    {
                        dialogService = new(selectedCustomer);
                        if (dialogService.SaveFileDialog())
                        {
                            dialogService.Save();
                            MessageBox.Show("Saved successfully!", "Saved", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Choose the customer first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public ViewModel()
        {
            SelectedCustomers = GetEveryCustomerData();
        }

        // Assistive methods.
        private List<Customer> GetEveryCustomerData()
        {
            string textFromJSON = "";
            List<Customer> selectedCustomers = new();

            for (int day = 1; day <= 30; day++)
            {
                textFromJSON = File.ReadAllText($@"{Environment.CurrentDirectory}\TestData\day{day}.json");
                JArray customers = JsonConvert.DeserializeObject<JArray>(textFromJSON);

                foreach (var customer in customers)
                {
                    if (selectedCustomers.Exists(name => name.Name == (string)customer["User"]))
                    {
                        selectedCustomers.Find(name => name.Name == (string)customer["User"]).AddSteps(day, (int)customer["Steps"]);
                    }
                    else
                    {
                        selectedCustomers.Add(new Customer((string)customer["User"], day, (int)customer["Steps"], (int)customer["Rank"], (string)customer["Status"]));
                    }
                }
            }
            return selectedCustomers;
        }

        // Events & handlers.
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPorpertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
