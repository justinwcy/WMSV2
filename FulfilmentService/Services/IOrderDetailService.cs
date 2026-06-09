using FulfilmentService.Models;
using WMSCommon.Results;
using WMSCommon.Services;

namespace FulfilmentService.Services
{
    public interface IOrderDetailService : ITenantSyncService<OrderDetail>
    {
    }
}