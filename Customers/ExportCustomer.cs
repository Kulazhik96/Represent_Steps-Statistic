namespace AndreiKulazhin_PRE_task
{
    public class ExportCustomer
    {
        public ExportCustomer() { }
        public ExportCustomer(Customer customer)
        {
            Rank = customer.Rank;
            Status = customer.Status;
            Name = customer.Name;
            AverageSteps = customer.AverageSteps;
            MinSteps = customer.MinSteps;
            MaxSteps = customer.MaxSteps;
        }

        public int Rank { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public int AverageSteps { get; set; }
        public int MinSteps { get; set; }
        public int MaxSteps { get; set; }
    }
}
