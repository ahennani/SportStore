using SportStore.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider service)
        {
            var context = service.GetRequiredService<AppDbContext>();

            await TestDataAsync(context);
        }

        public static async Task TestDataAsync(AppDbContext context)
        {
            var isChanged = false;

            if (context.Users.Any() is false)
            {
                var users = new List<User>
                {
                    new User // Password: user01
                    {
                        UserId = new Guid("cd707f02-c08e-47ba-a57f-47921ea64b08"),
                        UserName = "user01",
                        Email = "user01@gmail.com",
                        Passwordhash = Convert.FromBase64String("UHdwil/0K4kMqsd3c4yy0kCgMYPLFD0IV3nq937749q4t5U5spc2hDBI8JZNLulLSQCZ+nUv3MXJ3h3u1rBvhQ=="),
                        Passwordsalt = Convert.FromBase64String("l/vZ/a9UP5fNYzjZCSsFkrb6c82iT2Cw8rZPUbDuf1+wtQMqBgyPDebyQXhhx/Icg1+6aBr8b91Nm859wSlV5uhTWz6YZRd28TQY90dn3z0xGcysKTs7djFvgI0JQZxz3bh64Nra0MMZOgiTFuuP7NmhMDZ3mfundtlZU79gBZU=")
                    },
                    new User // Password: user02
                    {
                        UserId = new Guid("b779bf0e-ebe8-4395-ad28-390633782d03"),
                        UserName = "user02",
                        Email = "user02@gmail.com",
                        Passwordhash = Convert.FromBase64String("kMMzec974S2SrLyDU28ldi6h92zMjcX8Qs5HSiryYf19YsjFM2oQI04uso93d1uM4VSp2mpboA5VuYWmIv6w/A=="),
                        Passwordsalt = Convert.FromBase64String("FSlFdR8YF92F5rWgyIedgg9JHnPMg3MidM+sBvwCsLsCruAEx/XDyvaSzHN8P6xmOPuzAU/VeCVRAN+cg3UfVanISSUXE5vvLVfdmnX37OuqDyMjJYH8s9Di70iA06svbWCYLhAFq/fxv0kP0S1w2mcBtt7AhUFeWGIjk89BYVU=")
                    }
                };

                context.Users.AddRange(users);

                isChanged = true;
            }

            if (context.Orders.Any() is false)
            {
                var orders = new List<Order>()
                {
                    new Order()
                    {
                        OrderId = new Guid("e8de5d4f-adda-4e3e-2d7e-08d998b8c8ca"),
                        OrderDate = new DateTime(2020, 12, 30),
                        UserId = new Guid("b779bf0e-ebe8-4395-ad28-390633782d03")
                    },
                    new Order()
                    {
                        OrderId = new Guid("4cb2ee1c-262d-4a7a-2d7f-08d998b8c8ca"),
                        OrderDate = new DateTime(2021, 05, 11),
                        UserId = new Guid("cd707f02-c08e-47ba-a57f-47921ea64b08")
                    },
                    new Order()
                    {
                        OrderId = new Guid("07e145a6-855a-45c7-b7f3-58384661df88"),
                        OrderDate = DateTime.Now,
                        UserId = new Guid("cd707f02-c08e-47ba-a57f-47921ea64b08")
                    }
                };

                context.Orders.AddRange(orders);

                isChanged = true;
            }

            if (context.Categories.Any() is false)
            {
                var cats = new List<Category>
                {
                    new Category {CategoryId = new Guid("8fa555bc-0cd2-4ed4-9fa1-dc9219aba9ce"),Name = "Cat-001"},
                    new Category {CategoryId = new Guid("6ba57619-bf6e-44a6-bb25-9efcdc65f4be"),Name = "Cat-002"},
                    new Category {CategoryId = new Guid("51b18a17-e4cb-4da6-8820-bb9cdb997627"),Name = "Cat-003"},
                    new Category {CategoryId = new Guid("b58c78ef-8cac-4b6d-9696-d512e87ba4db"),Name = "Cat-004"}
                };

                context.Categories.AddRange(cats);

                isChanged = true;
            }

            if (context.Products.Any() is false)
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        ProductId = new Guid("715c517e-c0d9-4dd2-bdd7-0276767b56b2"),
                        SKU ="DGTPERB",
                        Descriptio ="Descriptio-001",
                        Name="Product-001",
                        Price=9.99M,
                        CategoryId = new Guid("b58c78ef-8cac-4b6d-9696-d512e87ba4db"),
                        Quantity = 15,
                        OrderId = new Guid("4cb2ee1c-262d-4a7a-2d7f-08d998b8c8ca")
                    },
                    new Product
                    {
                        ProductId = new Guid("98b9ba8b-ad26-4623-b923-b94bc0389ff9"),
                        SKU ="GPRTCDB",
                        Descriptio ="Descriptio-002",
                        Name="Product-002",
                        Price=12.49M,
                        CategoryId = new Guid("51b18a17-e4cb-4da6-8820-bb9cdb997627"),
                        Quantity = 29
                    },
                    new Product
                    {
                        ProductId = new Guid("79b35a1c-9e52-4039-b7a1-6dc40276fa28"),
                        SKU = "VRTYSDE",
                        Descriptio = "Descriptio-003",
                        Name = "Product-003",
                        Price = 20M,
                        CategoryId = new Guid("6ba57619-bf6e-44a6-bb25-9efcdc65f4be"),
                        Quantity = 30,  
                        OrderId = new Guid("e8de5d4f-adda-4e3e-2d7e-08d998b8c8ca")
                    },
                    new Product
                    {
                        ProductId = new Guid("30b66bbc-2692-4cd8-b761-d6c0c6bebd41"),
                        SKU ="PORTYNG",
                        Descriptio ="Descriptio-004",
                        Name="Product-004",
                        Price=95.99M,
                        CategoryId = new Guid("8fa555bc-0cd2-4ed4-9fa1-dc9219aba9ce"),
                        Quantity = 0
                    },
                    new Product
                    {
                        ProductId = new Guid("e1a01603-272e-49d6-aadc-0d6367b61da2"),
                        SKU ="TRNGJHY",
                        Descriptio ="Descriptio-005",
                        Name="Product-005",
                        Price=19M,
                        CategoryId = new Guid("b58c78ef-8cac-4b6d-9696-d512e87ba4db"),
                        Quantity = 9,
                        OrderId = new Guid("4cb2ee1c-262d-4a7a-2d7f-08d998b8c8ca")
                    },
                    new Product
                    {
                        ProductId = new Guid("ed4194a3-1061-4b94-83cd-c66078399638"),
                        SKU ="XERVGYM",
                        Descriptio ="Descriptio-006",
                        Name="Product-006",
                        Price=9.99M,
                        CategoryId = new Guid("b58c78ef-8cac-4b6d-9696-d512e87ba4db"),
                        Quantity = 37
                    },
                    new Product
                    {
                        ProductId = new Guid("dd97da25-8904-4493-a2b9-f329f259644b"),
                        SKU ="QZRVGTS",
                        Descriptio ="Descriptio-008",
                        Name="Product-008",
                        Price=2.99M,
                        CategoryId = new Guid("b58c78ef-8cac-4b6d-9696-d512e87ba4db"),
                        Quantity = 11
                    },
                    new Product
                    {
                        ProductId = new Guid("e5cf8fc8-d835-458b-8d45-ae3cb6236a43"),
                        SKU ="VFDRETY",
                        Descriptio ="Descriptio-007",
                        Name="Product-007",
                        Price=100M,
                        CategoryId = new Guid("b58c78ef-8cac-4b6d-9696-d512e87ba4db"),
                        Quantity = 0
                    }
                };

                context.Products.AddRange(products);

                isChanged = true;
            }

            if (isChanged) { var result = await context.SaveChangesAsync(); }
        }
    }
}
