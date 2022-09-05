using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CurrencyService: Repository<ConversionLog>, ICurrencyService
    {
        public CurrencyService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> AddConversionLog(ConversionLog model) { 
            try
            {                

                var result = await Add(model);
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ConversionLog>> GetList()
        {
            return await GetAll();
        }

        public async Task<int> DeleteConversionLog(int Id)
        {
            try
            {
                var item = await GetById(Id);

                var result = await Remove(item);
                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ConversionLog> GetConversionLog(int Id)
        {
            var item = await GetById(Id);
            return item;
        }

        public async Task<int> UpdateConversionLog(ConversionLog model)
        {
            try
            {
                var item = await GetById(model.Id);
                item.Currency1 = model.Currency1;
                item.Currency2 = model.Currency2;
                item.Amount = model.Amount;
                item.Result = model.Result;
                item.Createdate = DateTime.Now;
                var result = await UpdateAsync(item);
                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }
    }


}
