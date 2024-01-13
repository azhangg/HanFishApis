using Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sysyem
{
    public interface IDataAnalysisService
    {
        Task<DataAnalysisModel> GetDataAnalysisAsync();
    }
}
