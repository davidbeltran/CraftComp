namespace EmployeeWeb
{
    [Serializable]
    public class Customer
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FavCategory { get; set; }

        public string Display()
        {
            return $"{Name} | {Address} | {Email} | {Phone} | {FavCategory}";
        }
    }
}
