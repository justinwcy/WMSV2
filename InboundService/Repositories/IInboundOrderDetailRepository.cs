using InboundService.Models;

using WMSCommon.Repositories;

namespace InboundService.Repositories
{
    public interface IInboundOrderDetailRepository : ITenantRepository<InboundOrderDetail>
    {
    }
}
