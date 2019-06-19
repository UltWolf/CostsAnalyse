using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly Logging.FileLogging fl = new Logging.FileLogging();
        private readonly ApplicationContext _context;
        public ProductRepository(ApplicationContext applicationContext){
            _context = applicationContext;
        }
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                bool IsAdding = false;
                var productFromContext = await _context.Products.FirstOrDefaultAsync(m=> m.Index == product.Index);

                if (productFromContext == null)
                {
                    return await AddAsync(product);
                }
                else
                {
                    return await UpdatePriceAsync(product, productFromContext);
                }

            }
            catch(Exception ex)
            {
                fl.LogAsync(ex, product);
                return false;
            }
           
        }
        public async Task<bool> AddAsync(Product item)
        {
            try
            {
                var currentCost = item.LastPrice[0].Cost;
                item.Price = item.LastPrice;
                item.Min = currentCost;
                item.Max = currentCost;
                await _context.AddAsync(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await fl.LogAsync(ex, item);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Product item)
        {
            try
            {
                _context.Products.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                this.fl.LogAsync(ex, item);
                return false;
            }
        }

         
        public  bool Update(Product product)
        {
            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                 fl.LogAsync(ex, product);
                return false;
            }
        }

        

        public async Task<bool> UpdatePriceAsync(Product product, Product productFromContext)
        {
            try
            {
                var currentCost = product.LastPrice[0].Cost;
                var lastPrice = productFromContext.LastPrice.Single(m => m.Company.Equals(product.LastPrice[0].Company));

                if (lastPrice.Cost != product.LastPrice[0].Cost)
                {
                    productFromContext.LastPrice.Remove(lastPrice);
                    productFromContext.LastPrice.Add(product.LastPrice[0]);
                    if (currentCost > productFromContext.Max)
                    {
                        productFromContext.Max = currentCost;
                    }
                    else if (currentCost < productFromContext.Min)
                    {
                        productFromContext.Min = currentCost;
                    }
                    productFromContext.Price.Add(product.LastPrice[0]);
                    _context.Products.Update(productFromContext);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
                
            }
            catch(Exception ex)
            {
                await fl.LogAsync(ex, product);
                return false;
            }
        }

         

        public async Task<Product> GetAsync(object id)
        { 
            return  await _context.Products.Include(m=>m.Subscribers)
                       .FirstAsync(m=>m.Id==(int)id);
        }

         
    }
}
