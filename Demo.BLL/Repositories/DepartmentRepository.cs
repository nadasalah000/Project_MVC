using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> ,IDepartmentRepository
    {
        public DepartmentRepository(MVCAppDbContext dbContext):base(dbContext)
        { }

        
        /*private readonly MVCAppDbContext _dbContext;
public DepartmentRepository(MVCAppDbContext dbContext)
{
   _dbContext = dbContext;
}
public int Add(Department department)
{
   _dbContext.Add(department);
   return _dbContext.SaveChanges();
}

public int Delete(Department department)
{
   _dbContext.Remove(department);
   return _dbContext.SaveChanges();
}

public IEnumerable<Department> GetAll()
=> _dbContext.Department.ToList();


public Department GetById(int id)
{
   return _dbContext.Department.Find(id);
}

public int Update(Department department)
{
   _dbContext.Update(department);
   return _dbContext.SaveChanges();
}*/
    }
}
