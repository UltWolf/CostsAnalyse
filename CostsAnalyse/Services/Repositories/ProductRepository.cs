using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Abstracts;
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
        public bool AddProduct(Product product)
        {
            if (!product.IsNull())
            {
                bool IsAdding = false;
                var productFromContext = _context.Products.FirstOrDefault(m => m.Index == product.Index);
                
                if (productFromContext == null)
                {
                    IsAdding =  Add(product);
                }
                else
                {
                    IsAdding = Update(product,productFromContext);  
                }
                if (IsAdding == true)
                {
                    try
                    {
                        _context.SaveChanges();
                        return true;
                    }catch(Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public bool Add(Product item)
        {
            try
            {
                var currentCost = item.LastPrice[0].Cost;
                item.Price = item.LastPrice;
                item.Min = currentCost;
                item.Max = currentCost;
                _context.Add(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(Product item)
        {
            throw new NotImplementedException();
        }

        public Product Get()
        {
            throw new NotImplementedException();
        }
        

        

        public bool Update(Product product, Product productFromContext)
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
                    return true;
                }
                return false;
                
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
