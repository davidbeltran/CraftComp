/*
 * Author: David Beltran
 * This file contains the Product class. 
 */
namespace CompStoreWeb
{
    public class Product
    {
        public string? Name;
        public string? Description;
        public string? Brand;
        public double? Price;
        public string? Type;

        public Product(string Name)
        {
            this.Name = Name;
        }
        public Product(string Name, string Description, string Brand, double Price, string Type)
        {
            this.Name = Name;
            this.Description = Description;
            this.Brand = Brand;
            this.Price = Price;
            this.Type = Type;
        }

        public string GetInfo()
        {
            return $"{this.Name} | {this.Description} | {this.Brand} " +
                $"| ${this.Price} | {this.Type}";
        }
    }
}
