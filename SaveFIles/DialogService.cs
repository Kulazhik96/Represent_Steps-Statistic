using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows;
using CsvHelper;
using System.Globalization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace AndreiKulazhin_PRE_task
{
    public class DialogService
    {
        private ExportCustomer exportCustomer;
        private string filterString = "JSON (*.json)|*.json|XML (*.xml)|*.xml|CSV (*.csv)|*.csv";

        public string FilePath { get; set; }

        public DialogService(Customer customer)
        {
            exportCustomer = new(customer);
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFile = new();
            saveFile.FileName = $"{exportCustomer.Name}";
            saveFile.InitialDirectory = Environment.CurrentDirectory;
            saveFile.Filter = filterString;

            if ((bool)saveFile.ShowDialog())
            {
                FilePath = saveFile.FileName;
                return true;
            }
            return false;
        }
        public void Save()
        {
            if (FilePath.EndsWith(".json"))
            {
                SaveJSON(FilePath, exportCustomer);
            }
            if (FilePath.EndsWith(".xml"))
            {
                SaveXML(FilePath, exportCustomer);
            }
            if (FilePath.EndsWith(".csv"))
            {
                SaveCSV(FilePath, exportCustomer);
            }
        }

        private static void SaveCSV(string fileName, ExportCustomer selectedCustomer)
        {
            // CsvHelper doesn't want to write a single record, so I decided to create a list
            // of single customer and parse it to the WriteRecords().
            List<ExportCustomer> exportCustomerList = new();
            exportCustomerList.Add(selectedCustomer);

            using (StreamWriter sw = new(fileName))
            {
                CsvWriter csvw = new(sw, CultureInfo.CurrentCulture);
                csvw.WriteHeader<ExportCustomer>();
                csvw.NextRecord();
                csvw.WriteRecords(exportCustomerList);
                sw.Flush();
            }
        }
        private static void SaveXML(string fileName, ExportCustomer selectedCustomer)
        {
            XmlSerializer serializer = new(typeof(ExportCustomer));

            using (StreamWriter sw = new(fileName))
            {
                serializer.Serialize(sw, selectedCustomer);
            }
        }
        private static void SaveJSON(string fileName, ExportCustomer selectedCustomer)
        {
            using (StreamWriter sw = new(fileName))
            {
                sw.WriteLine(JsonConvert.SerializeObject(selectedCustomer, Formatting.Indented));
            }
        } 
    }
}
