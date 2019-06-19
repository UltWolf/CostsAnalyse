using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Abstracts
{
    interface IManyToManyRepository<T,D>
    {
        Task<bool> Add(T item,D secondItem); 
        Task<bool> Update(T item, T updateItem);
        Task<bool> Delete(T item,D secondItem);
    }
}
