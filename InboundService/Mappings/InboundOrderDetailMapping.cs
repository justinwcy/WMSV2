using InboundService.DTOs;
using InboundService.Models;

namespace InboundService.Mappings
{
    public static class InboundOrderDetailMapping
    {
        public static InboundOrderDetail ToModel(this InboundOrderDetailCreateDTO inboundOrderDetailCreateDTO)
        {
            return new InboundOrderDetail()
            {
                Status = inboundOrderDetailCreateDTO.Status,
                Quantity = inboundOrderDetailCreateDTO.Quantity,
                ProductDetailId = inboundOrderDetailCreateDTO.ProductDetailId,
                InboundOrderId = inboundOrderDetailCreateDTO.InboundOrderId,
            };
        }

        public static InboundOrderDetail ToModel(this InboundOrderDetailUpdateDTO inboundOrderDetailUpdateDTO)
        {
            return new InboundOrderDetail()
            {
                Status = inboundOrderDetailUpdateDTO.Status,
                Quantity = inboundOrderDetailUpdateDTO.Quantity,
                ProductDetailId = inboundOrderDetailUpdateDTO.ProductDetailId,
                InboundOrderId = inboundOrderDetailUpdateDTO.InboundOrderId,
            };
        }

        public static InboundOrderDetailReadDTO ToReadDTO(this InboundOrderDetail inboundOrderDetail)
        {
            return new InboundOrderDetailReadDTO()
            {
                Id = inboundOrderDetail.Id,
                CompanyId = inboundOrderDetail.CompanyId,
                Status = inboundOrderDetail.Status,
                Quantity = inboundOrderDetail.Quantity,
                ProductDetailId = inboundOrderDetail.ProductDetailId,
                InboundOrderId = inboundOrderDetail.InboundOrderId,
                ProductDetail = inboundOrderDetail.ProductDetail.ToReadDTO(),
            };
        }
    }
}
