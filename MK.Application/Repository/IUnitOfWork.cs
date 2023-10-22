
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<Location> Location { get; }
        IGenericRepository<User> User { get; }
        IGenericRepository<Role> Role { get; }
        IGenericRepository<Customer> Customer { get; }
        IGenericRepository<Notification> Notification { get; }
        IGenericRepository<Kitchen> Kitchen { get; }

        IGenericRepository<FavouriteKitchen> FavouriteKitchen { get; }
        IGenericRepository<Area> Area { get; }
        IGenericRepository<Dish> Dish { get; }
        IGenericRepository<Tray> Tray { get; }
        IGenericRepository<Meal> Meal { get; }
        IGenericRepository<PaymentType> PaymentType { get; }
        IGenericRepository<OrderPayment> OrderPayment { get; }
        IGenericRepository<Order> Order { get; }

        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RolebackTransactionAsync();
    }
}
