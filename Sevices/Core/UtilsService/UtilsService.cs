using Data.DataAccess;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.UtilsService
{
    public class UtilsService : IUtilsService
    {
        private readonly AppDbContext _dbContext;

        public UtilsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GenerateItemCode(List<string> listCodeDB, List<string> excludeCodes)
        {
            var randomCode = FnUtil.GenerateCode();
            if (excludeCodes.Contains(randomCode) || listCodeDB.Contains(randomCode))
            {
                return GenerateItemCode(listCodeDB, excludeCodes);
            }
            return randomCode;
        }
    }
}
