using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface ICurrencyService : IRepository<ConversionLog>
    {
        Task<IEnumerable<ConversionLog>> GetList();
        Task<ConversionLog> GetConversionLog(int Id);
        Task<int> AddConversionLog(ConversionLog model);
        Task<int> UpdateConversionLog(ConversionLog model);
        Task<int> DeleteConversionLog(int Id);
    }    
}
