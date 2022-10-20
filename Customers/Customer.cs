using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.ComponentModel;

namespace AndreiKulazhin_PRE_task
{
    public class Customer : INotifyPropertyChanged
    {
        public Customer(string name, int day, int steps, int rank = 0, string status = "")
        {
            Name = name;
            Day = day;
            Steps = steps;
            dayStepsPair.Add(Day, Steps);
            Rank = rank;
            Status = status;
        }
        public Customer() { }

        private Dictionary<int, int> dayStepsPair = new();
        private int Day { get; set; }
        private int Steps { get; set; }

        public bool IsMoreThanTwentyPercentProperty
        {
            get
            {
                double difference = this.AverageSteps * 0.2;
                return difference <= (this.AverageSteps - this.MinSteps) || difference <= (this.MaxSteps - this.AverageSteps);
            }
        }
        public Dictionary<int, int> DayStepsPair
        {
            get { return dayStepsPair; }
            set
            {
                dayStepsPair = value;
                OnPropertyChanged("DayStepsPair");
            }
        }
        public int Rank { get; }
        public string Status { get; }
        public string Name { get; }
        public int AverageSteps
        {
            get
            {
                var query = dayStepsPair.Sum(steps => steps.Value);
                return query/30;
            }
        }
        public int MinSteps
        {
            get
            {
                var query = dayStepsPair.Values.Min();
                return query;
            }
        }
        public int MaxSteps
        {
            get
            {
                var query = dayStepsPair.Values.Max();
                return query;
            }
        }

        // Assistive methods.
        public void AddSteps(int day, int steps)
        {
            dayStepsPair.Add(day, steps);
        }
        // Events & handlers.
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));

        }
    }
}
