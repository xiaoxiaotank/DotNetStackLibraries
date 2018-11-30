﻿using AspNet.WebApi.OData.Client.AspNet.WebApi.OData.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.WebApi.OData.Client
{
    class Program
    {
        // Get an entire entity set.
        static void ListAllProducts(Default.Container container)
        {
            foreach (var p in container.Products)
            {
                Console.WriteLine("{0} {1} {2}", p.Name, p.Price, p.Category);
            }
        }

        static void AddProduct(Default.Container container, Product product)
        {
            container.AddToProducts(product);
            var serviceResponse = container.SaveChanges();
            foreach (var operationResponse in serviceResponse)
            {
                Console.WriteLine("Response: {0}", operationResponse.StatusCode);
            }
        }

        static void Main(string[] args)
        {
            // TODO: Replace with your local URI.
            string serviceUri = "http://localhost:55880/";
            var container = new Default.Container(new Uri(serviceUri));

            var product = new Product()
            {
                Name = "Yo-yo",
                Category = "Toys",
                Price = 4.95M
            };

            AddProduct(container, product);
            ListAllProducts(container);

            Console.ReadKey();
        }
    }
}
