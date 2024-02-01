/*
 * Author: David Beltran
 * This file contains the Computer class. 
 */
namespace CompStoreWeb
{
    public class Computer
    {
        public string computerType;
        public double price;
        public int ram;
        public double diskSize;
        public int processorCores;
        public Computer(string computerType)
        {
            this.computerType = computerType;
        }
        public Computer(string computerType, double price, int ram = 512,
            double diskSize = .5, int processorCores = 4)
        {
            this.computerType = computerType;
            this.price = price;
            this.ram = ram;
            this.diskSize = diskSize;
            this.processorCores = processorCores;
        }
    }
}
