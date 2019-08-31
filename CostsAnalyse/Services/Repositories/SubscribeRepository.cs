using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Repositories
{
    public class SubscribeRepository : IManyToManyRepository<UserApp, Product>
    {
        private readonly ApplicationContext _context; 
        public SubscribeRepository(ApplicationContext applicationContext)
        {
            _context = applicationContext;


        }
        public async Task<bool> Add(UserApp item, Product secondItem)
        {
            UserProduct UP = new UserProduct();
            try
            {

                UP.Products = secondItem;
                UP.IdProduct = secondItem.Id;
                UP.Users = item;
                UP.IdUserapp = item.Id;
                item.products.Add(UP);
                secondItem.Subscribers.Add(UP);
                _context.Update(secondItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            { 
                return false;
            }
        }

        

       
        

        public async Task<bool>  Delete(UserApp item, Product secondItem)
        {
            try
            {
                
                var productWithUser = secondItem.Subscribers.First(m => m.IdUserapp == item.Id);
                secondItem.Subscribers.Remove(productWithUser);  
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            { 
                return false;
            }
        }

        public Task<bool> Update(UserApp item, UserApp updateItem)
        {
            throw new NotImplementedException();
        }
    }
}
