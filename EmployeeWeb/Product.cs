namespace EmployeeWeb
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }

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
