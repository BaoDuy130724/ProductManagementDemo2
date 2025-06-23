using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        void SaveProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Product GetProductById(int id);
    }
}
