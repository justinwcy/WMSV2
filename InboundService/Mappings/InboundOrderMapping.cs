using InboundService.DTOs;
using InboundService.Models;

namespace InboundService.Mappings
{
    public static class InboundOrderMapping
    {
        public static InboundOrder ToModel(this InboundOrderCreateDTO inboundOrderCreateDTO)
        {
            return new InboundOrder()
            {
                ReceivedDate = inboundOrderCreateDTO.ReceivedDate,
                IssuedDate = inboundOrderCreateDTO.IssuedDate,
                EstimatedReceivedDate = inboundOrderCreateDTO.EstimatedReceivedDate,
                Source = inboundOrderCreateDTO.Source,
                VendorId = inboundOrderCreateDTO.VendorId,
                PONumber = inboundOrderCreateDTO.PONumber,
            };
        }

        public static InboundOrder ToModel(this InboundOrderUpdateDTO inboundOrderUpdateDTO)
        {
            return new InboundOrder()
            {
                ReceivedDate = inboundOrderUpdateDTO.ReceivedDate,
                IssuedDate = inboundOrderUpdateDTO.IssuedDate,
                EstimatedReceivedDate = inboundOrderUpdateDTO.EstimatedReceivedDate,
                Source = inboundOrderUpdateDTO.Source,
                VendorId = inboundOrderUpdateDTO.VendorId,
                PONumber = inboundOrderUpdateDTO.PONumber,
            };
        }

        public static InboundOrderReadDTO ToReadDTO(this InboundOrder inboundOrder)
        {
            return new InboundOrderReadDTO()
            {
                Id = inboundOrder.Id,
                ReceivedDate = inboundOrder.ReceivedDate,
                IssuedDate = inboundOrder.IssuedDate,
                Source = inboundOrder.Source,
                EstimatedReceivedDate = inboundOrder.EstimatedReceivedDate,
                VendorId = inboundOrder.VendorId,
                PONumber = inboundOrder.PONumber,
                CompanyId = inboundOrder.CompanyId,
                IncomingDetails = inboundOrder.IncomingDetails?.Select(detail => detail.ToReadDTO()) ?? new List<InboundOrderDetailReadDTO>()
            };
        }
    }
}
