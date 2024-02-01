using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompStoreConsole
{
    internal class Computer
    {
        public string ComputerType { get; set; }
        public double Price { get; set; }
        public int Ram { get; set; }
        public double DiskSize { get; set; }
        public int ProcessorCores { get; set; }
        public string Brand { get; set; }

        /// <summary>
        /// Allows for empty instantiation
        /// </summary>
        public Computer() { }

        /// <summary>
        /// Second overloaded constructor method.
        /// </summary>
        /// <param name="computerType">Must be entered</param>
        /// <param name="price">Must be entered</param>
        /// <param name="ram">Optional</param>
        /// <param name="diskSize">Optional</param>
        /// <param name="processorCores">Optional</param>
        public Computer(string computerType, double price, int ram = 512,
            double diskSize = .5, int processorCores = 4)
        {
            this.ComputerType = computerType;
            this.Price = price;
            this.Ram = ram;
            this.DiskSize = diskSize;
            this.ProcessorCores = processorCores;
            //InstanceDesciption(ram, diskSize, processorCores);
        }

        public Computer(string computerType, string brand, double price, 
            int ram = 512, double diskSize = .5)
        {
            this.ComputerType = computerType;
            this.Brand = brand;
            this.Ram = ram;
            this.DiskSize = diskSize;
            this.Price= price;
        }

        /// <summary>
        /// Formatted display of Computer object attributes.
        /// </summary>
        public void Display()
        {
            Console.WriteLine(String.Format("{0, -10}{1, -15}{2, -12}{3, -17}{4, -15}",
                $"{this.ComputerType}", $"Price: ${this.Price}", $"RAM: {this.Ram}",
                $"Disk Size: {this.DiskSize}", $"Processor Cores: {this.ProcessorCores}"));
        }

        /// <summary>
        /// Method that comments on each instance of argument combination of
        /// overloaded Computer object.
        /// </summary>
        /// <param name="ram"></param>
        /// <param name="diskSize"></param>
        /// <param name="processorCores"></param>
        private void InstanceDesciption(int ram, double diskSize, int processorCores)
        {
            if (ram == 512 && diskSize == .5 && processorCores == 4)
                Console.WriteLine("This instance was given a computer type and price arguments.");
            else if (diskSize == .5 && processorCores == 4)
                Console.WriteLine("This instance was given all arguments except disk size and" +
                    " processor cores.");
            else if (ram == 512 && diskSize == .5)
                Console.WriteLine("This instance was given all arguments except RAM" +
                    " and disk size.");
            else if (ram == 512 && processorCores == 4)
                Console.WriteLine("This instance was given all arguments except RAM " +
                    "and processor cores.");
            else if (ram == 512)
                Console.WriteLine("This instance was given all arguments except RAM.");
            else if (diskSize == .5)
                Console.WriteLine("This instance was given all arguments except disk size.");
            else if (processorCores == 4)
                Console.WriteLine("This instance was given all arguments except processor cores.");
            else
                Console.WriteLine("This instance was given all arguments.");
        }
    }
}
