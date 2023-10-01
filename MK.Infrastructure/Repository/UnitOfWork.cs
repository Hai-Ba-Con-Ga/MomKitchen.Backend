
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


        private IGenericRepository<Location> _locationRepository;
        public IGenericRepository<Location> Location
        {
            get
            {
                if (_locationRepository == null)
                {
                    _locationRepository = new GenericRepository<Location>(_dbContext);
                }
                return _locationRepository;
            }
        }



        private IGenericRepository<User> _userRepository;
        public IGenericRepository<User> User
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<User>(_dbContext);
                }
                return _userRepository;
            }
        }

        private IGenericRepository<Role> _roleRepository;
        public IGenericRepository<Role> Role
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new GenericRepository<Role>(_dbContext);
                }
                return _roleRepository;
            }
        }

        private IGenericRepository<Customer> _customerRepository;
        public IGenericRepository<Customer> Customer
        {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new GenericRepository<Customer>(_dbContext);
                }
                return _customerRepository;
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
