using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _dbContext;

        public EmployeeRepository(MVCAppDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        => _dbContext.Employees.Where(E  => E.Address == address);

        public IQueryable<Employee> GetEmployeeByName(string SearchValue)
        {
          return _dbContext.Employees.Where(E =>E.Name.ToLower().Contains(SearchValue.ToLower()));
        }
        /*private readonly MVCAppDbContext _dbContext;

public EmployeeRepository(MVCAppDbContext dbContext)
{
   _dbContext = dbContext;
}

public int Add(Employee employee)
{
   _dbContext.Add(employee);
   return _dbContext.SaveChanges();
}

public int Delete(Employee employee)
{
   _dbContext.Remove(employee);
   return _dbContext.SaveChanges();
}

public IEnumerable<Employee> GetAll()
=> _dbContext.Employees.ToList();

public Employee GetById(int Id)
=> _dbContext.Employees.Find(Id);

public int Update(Employee employee)
{
   _dbContext.Employees.Update(employee);
   return _dbContext.SaveChanges();
}*/

    }
}
