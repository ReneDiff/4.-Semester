using System;
using System.Linq;
using BikeStore.Models;

class Program
{
    static void Main()
    {
        using(var context = new BikeStoreContext())
        {
            var products = context.Products.ToList();

            Console.WriteLine("Liste over produkter");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.ProductId}, Navn: {product.ProductName}, Pris: {product.ListPrice}");
            }
        }
    }
}
