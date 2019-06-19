using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Abstracts
{
    public interface IRepository<T>
    {
        Task<bool> AddAsync(T item); 
        Task<T> GetAsync(object id);
        bool Update(T item); 
        Task<bool> DeleteAsync(T item); 
    }
}
