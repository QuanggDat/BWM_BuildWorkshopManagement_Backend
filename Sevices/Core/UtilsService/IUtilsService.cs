using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.UtilsService
{
    public interface IUtilsService
    {
        string GenerateItemCode(List<string> listCodeDB, List<string> excludeCodes);
    }
}
