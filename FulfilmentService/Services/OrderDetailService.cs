using FulfilmentService.Models;
using FulfilmentService.Repositories;
using WMSCommon.Contracts.FulfilmentService;
using WMSCommon.Results;
using WMSCommon.Services;
using Wolverine;

namespace FulfilmentService.Services
{
    public class OrderDetailService(
        IOrderDetailRepository repository,
        IMessageContext messageContext) : 
        TenantSyncService<OrderDetail>(repository, messageContext), 
        IOrderDetailService
    {
    }
}
