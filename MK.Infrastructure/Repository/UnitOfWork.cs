
using Microsoft.EntityFrameworkCore.Storage;
using MK.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;
        private IDbContextTransaction _transaction;


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Repository 
        //Demo for Singleton pattern in UnitOfWork
        //IGenericRepository<Account> _accountRepository;
        //    public IGenericRepository<Account> AcccountRepository
        //    {
        //        get
        //        {
        //            if (_accountRepository == null)
        //            {
        //                _accountRepository = new GenericRepository<Account>(_dbContext);
        //            }
        //            return _accountRepository;
        //        }
        //    }


        private IGenericRepository<Location> _location;
        public IGenericRepository<Location> Location
        {
            get
            {
                if (_location == null)
                {
                    _location = new GenericRepository<Location>(_dbContext);
                }
                return _location;
            }
        }

        private IGenericRepository<User> _user;
        public IGenericRepository<User> User
        {
            get
            {
                if (_user == null)
                {
                    _user = new GenericRepository<User>(_dbContext);
                }
                return _user;
            }
        }

        private IGenericRepository<Role> _role;
        public IGenericRepository<Role> Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new GenericRepository<Role>(_dbContext);
                }
                return _role;
            }
        }

        private IGenericRepository<Customer> _customer;
        public IGenericRepository<Customer> Customer
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new GenericRepository<Customer>(_dbContext);
                }
                return _customer;
            }
        }

        private IGenericRepository<Notification> _notification;
        public IGenericRepository<Notification> Notification
        {
            get
            {
                if (_notification == null)
                {
                    _notification = new GenericRepository<Notification>(_dbContext);
                }
                return _notification;
            }
        }

        private IGenericRepository<Kitchen> _kitchen;
        public IGenericRepository<Kitchen> Kitchen
        {
            get
            {
                if (_kitchen == null)
                {
                    _kitchen = new GenericRepository<Kitchen>(_dbContext);
                }
                return _kitchen;
            }
        }

        private IGenericRepository<Area> _area;
        public IGenericRepository<Area> Area
        {
            get
            {
                if (_area == null)
                {
                    _area = new GenericRepository<Area>(_dbContext);
                }
                return _area;
            }
        }

        public IGenericRepository<FavouriteKitchen> _favouriteKitchen;
        public IGenericRepository<FavouriteKitchen> FavouriteKitchen
        {
            get
            {
                if (_favouriteKitchen == null)
                {
                    _favouriteKitchen = new GenericRepository<FavouriteKitchen>(_dbContext);
                }
                return _favouriteKitchen;
            }
        }

        private IGenericRepository<Dish> _dish;
        public IGenericRepository<Dish> Dish
        {
            get
            {
                if (_dish == null)
                {
                    _dish = new GenericRepository<Dish>(_dbContext);
                }
                return _dish;
            }
        }

        public IGenericRepository<Tray> _tray;
        public IGenericRepository<Tray> Tray
        {
            get
            {
                if (_tray == null)
                {
                    _tray = new GenericRepository<Tray>(_dbContext);
                }
                return _tray;
            }
        }

        public IGenericRepository<Meal> _meal;
        public IGenericRepository<Meal> Meal
        {
            get
            {
                if (_meal == null)
                {
                    _meal = new GenericRepository<Meal>(_dbContext);
                }
                return _meal;
            }
        }

        public IGenericRepository<PaymentType> _paymentType;
        public IGenericRepository<PaymentType> PaymentType
        {
            get
            {
                if (_paymentType == null)
                {
                    _paymentType = new GenericRepository<PaymentType>(_dbContext);
                }
                return _paymentType;
            }
        }

        public IGenericRepository<OrderPayment> _orderPayment;
        public IGenericRepository<OrderPayment> OrderPayment
        {
            get
            {
                if (_orderPayment == null)
                {
                    _orderPayment = new GenericRepository<OrderPayment>(_dbContext);
                }
                return _orderPayment;
            }
        }

        #endregion Repository


        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _dbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction is null)
                return;

            try
            {
                await _dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            catch
            {
                await RolebackTransactionAsync();
            }
        }

        public async Task RolebackTransactionAsync()
        {
            if (_transaction is null)
                return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
