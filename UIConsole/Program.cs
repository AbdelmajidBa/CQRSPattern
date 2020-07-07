using Application.Commands;
using Application.Interfaces;
using Application.Queries;
using Domaine.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test CQRS Pattern!");

            var serviceProvider = new ServiceCollection()
                            // Add data base context
                            .AddSingleton<IApplicationContext, ApplicationContext>()
                            // Add commands handlers
                            .AddScoped<ICommandHandler<AddNewProductCommand>, AddNewProductCommandHandler>()
                            .AddScoped<ICommandHandler<DeleteProductCommand>, DeleteProductCommandHandler>()
                            .AddScoped<ICommandHandler<UpdateProductCurrentStockCommand>, UpdateProductCurrentStockCommandHandler>()
                            .AddScoped<ICommandHandler<UpdateProductUnitPriceCommand>, UpdateProductUnitPriceCommandHandler>()
                            // Add queries handlers
                            .AddScoped<IQueryHandler<FindOutOfStockProductsQuery>, FindOutOfStockProductsQueryHandler>()
                            .AddScoped<IQueryHandler<GetProductsByNameQuery>, GetProductsByNameQueryHandler>()

                            //Creat service
                            .BuildServiceProvider();

            try
            {
                var commandDispatcher = new CommandDispatcher(serviceProvider);
                var queryDispatcher = new QueryDispatcher(serviceProvider);

                //Add new Product
                var product = new AddNewProductCommand { Id = Guid.NewGuid(), Name = "iPhone 11", Description = "Apple iphone 11" };
                commandDispatcher.Send(product);
                
                //Update Product Unit Price
                commandDispatcher.Send(new UpdateProductUnitPriceCommand { Id = product.Id, UnitPrice = 800 });

                //Update Product Current Stock
                commandDispatcher.Send(new UpdateProductCurrentStockCommand { Id = product.Id, CurrentStock = 500 });


                //Fine Products By Name
                var productsByName = queryDispatcher.Send(new GetProductsByNameQuery { Name = "iPhone" });
                foreach (var item in productsByName)
                {
                    Console.WriteLine(item.ToString());
                }

                //Fine Products By Name
                var outOfStockProducts = queryDispatcher.Send(new FindOutOfStockProductsQuery());
                foreach (var item in outOfStockProducts)
                {
                    Console.WriteLine(item.ToString());
                }

                //Delete Product
                commandDispatcher.Send(new DeleteProductCommand { Id = product.Id});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }


            Console.ReadKey();   

        }
    }
}
