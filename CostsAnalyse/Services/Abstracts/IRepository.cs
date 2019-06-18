using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Abstracts
{
    public interface IRepository<T>
    {
        bool Add(T item);
        T Get();
        bool Update(T item, T updateItem);
        bool Delete(T item);
    }
}
