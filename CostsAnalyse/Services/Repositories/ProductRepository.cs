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
        private readonly ApplicationContext _context;
        public ProductRepository(ApplicationContext applicationContext){
            _context = applicationContext;
        }
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                bool IsAdding = false;
                Product productFromContext;
                if (product.Index != null)
                {
                    productFromContext = await _context.Products
                           .Include(m => m.LastPrice)
                           .ThenInclude(lp => lp.Company)
                           .Include(p => p.Price)
                           .ThenInclude(p => p.Company)
                           .FirstOrDefaultAsync(m => m.Index == product.Index);
                }
                else
                {
                     productFromContext = await _context.Products
                           .Include(m => m.LastPrice)
                           .ThenInclude(lp => lp.Company)
                           .Include(p => p.Price)
                           .ThenInclude(p => p.Company)
                           .FirstOrDefaultAsync(m => m.Name == product.Name);
                }

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
                return false;
            }
        }

        

        public async Task<bool> UpdatePriceAsync(Product product, Product productFromContext)
        {
            try
            {
                var currentCost = product.LastPrice[0].Cost;
                var lastPrice = productFromContext.LastPrice.SingleOrDefault(m => m.Company.Equals(product.LastPrice[0].Company));

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
                return false;
            }
        }

         

        public async Task<Product> GetAsync(object id)
        { 
            return  await _context.Products
                       .Include(m=>m.Subscribers)
                       .Include(m=>m.LastPrice)
                       .ThenInclude(lp=>lp.Company)
                       .Include(p=>p.Price)
                       .ThenInclude(p=>p.Company)
                       .FirstAsync(m=>m.Id==(int)id);
        }

         
    }
}
