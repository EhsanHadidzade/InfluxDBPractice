namespace InfuxDBPractice.DTOs
{


    public class CreateProductDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime DateAdded { get; set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
            DateAdded = DateTime.Now;
        }
    }
}
