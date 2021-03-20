using System;

namespace EFCoreExample
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    class Program
    {
        static void Main(string[] args)
        {
            //// Команда для создания новой миграции в Package Manager Console: "Add-Migration название_миграции"
            //// Команда для применения последних миграций к БД в Package Manager Console: "Update-Database"

            Console.WriteLine("Hello World!");

            using (var context = new ExampleContext())
            {
                var orderList = context.Set<Order>().Include(o => o.User).Where(x => x.User.FullName == "Иванов Иван Иванович").ToList();

                User user = null;

                //// user = context.Set<User>().First(user => user.FullName == "Иванов Иван Иванович");

                var order = new Order { User = user, CreateDate = DateTime.Now, State = OrderState.Created};

                //// Эмуляция добавления пользователем позиций в заказ.
                //// foreach (var posId in positionIdsSelectedByUser)
                ////    order.Positions.Add(context.Set<Position>().Find(posId));
                context.Set<Order>().Add(order);

                context.SaveChanges();

                context.Set<Position>().Add(new Position { Name = "Бананас африканский", PriceInEuro = 100 });
                context.SaveChanges();
            }
        }
    }
}
