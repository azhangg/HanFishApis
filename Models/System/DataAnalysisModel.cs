using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.System
{
    public class DataAnalysisModel
    {
        public int TodayDeals { get; set; }

        public decimal TodayTransactionAmount { get; set; }

        public int TodayRegisterNum { get; set; }

        public int TodayPostPublishNum { get; set; }

        public int TodayGoodPublishNum { get; set; }

        public int TotalDeals { get; set; }

        public decimal TotalTransactions { get; set; }

        public int TotalRegisterNum { get; set; }

        public int TotalPostPublishNum { get; set; }

        public int TotalGoodPublishNum { get; set; }
    }
}
