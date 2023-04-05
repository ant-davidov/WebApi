using Domain.Entities.Secondary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAnimalsAnalyticsRepository
    {
        public Task<AnalyticsResponse> GetAnalyticsAsync(long areaId, DateTime startDate, DateTime endDate);
    }
}
