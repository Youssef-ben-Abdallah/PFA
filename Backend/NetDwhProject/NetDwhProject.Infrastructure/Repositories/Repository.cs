using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetDwhProject.Core.Interfaces;
using NetDwhProject.Infrastructure.Data;
using System.Linq.Expressions;

namespace NetDwhProject.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly OltpDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(OltpDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
}