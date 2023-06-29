using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.DB.Specifications.ServiceOrder;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Orders;
using FBOLinx.ServiceLayer.BusinessServices.ServiceOrders;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Mail
{
    public interface IOrderConfirmationService
    {
        Task<bool> SendEmailConfirmation(int fuelerLinxId);
    }
    public class OrderConfirmationService : IOrderConfirmationService
    {
        private readonly IMailService _mailService;
        private readonly IFuelReqService _fuelReqService;
        private readonly IServiceOrderService _serviceOrderService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IFboService _fboService;
        private readonly IAirportTimeService _airportTimeService;

        public OrderConfirmationService(IMailService mailService, IFuelReqService fuelReqService, IServiceOrderService serviceOrderService, IOrderDetailsService orderDetailsService, IFboService fboService, IAirportTimeService airportTimeService)
        {
            _mailService = mailService;
            _fuelReqService = fuelReqService;
            _serviceOrderService = serviceOrderService;
            _orderDetailsService = orderDetailsService;
            _fboService = fboService;
            _airportTimeService = airportTimeService;
        }

        public async Task<bool> SendEmailConfirmation(int fuelerLinxId)
        {
            var TailNumber = "";
            var Fbo = "";
            var Icao = "";
            var Eta = "";
            var FuelVendor = "";
            var FuelerLinxTransactionId = "";

            var fuelOrder = await _fuelReqService.GetSingleBySpec(new FuelReqBySourceIdSpecification(fuelerLinxId));
            var serviceOrder = new DTO.ServiceOrderDto();
            var fbo = new FbosDto();

            if (fuelOrder == null || fuelOrder.Oid == 0)
            {
                serviceOrder = await _serviceOrderService.GetSingleBySpec(new ServiceOrderByFuelerLinxTransactionIdSpecification(fuelerLinxId));
                fbo = await _fboService.GetFbo(serviceOrder.Fboid);
                await serviceOrder.PopulateLocalTimes(_airportTimeService);

                TailNumber = serviceOrder.CustomerAircraft.TailNumber;
                Fbo = fbo.Fbo;
                Icao = fbo.FboAirport.Icao;
                Eta = serviceOrder.ArrivalDateTimeLocal.ToString();
                FuelerLinxTransactionId = fuelerLinxId.ToString();
            }
            else
            {
                fbo = await _fboService.GetFbo(fuelOrder.Fboid.GetValueOrDefault());
                await fuelOrder.PopulateLocalTimes(_airportTimeService);

                TailNumber = fuelOrder.TailNumber;
                Fbo = fuelOrder.Fbo.Fbo;
                Icao = fuelOrder.Icao;
                Eta = fuelOrder.ArrivalDateTimeLocal.ToString();
                FuelerLinxTransactionId = fuelerLinxId.ToString();
            }

            var orderDetails = await _orderDetailsService.GetSingleBySpec(new OrderDetailsByFuelerLinxTransactionIdSpecification(fuelerLinxId));
            if (orderDetails != null && orderDetails.Oid > 0)
            {
                FuelVendor = orderDetails.FuelVendor;
            }

            var dynamicTemplateData = new OrderConfirmationTemplateData
            {
                aircraftTailNumber = TailNumber,
                fboName = Fbo.ToString(),
                airportICAO = Icao,
                arrivalDate = Eta.ToString(),
                fuelVendor = FuelVendor,
                fuelerLinxId = FuelerLinxTransactionId
            };

            await SendEmail(dynamicTemplateData, fbo.FuelDeskEmail, orderDetails.ConfirmationEmail);

            return true;
        }

        private async Task<bool> SendEmail(OrderConfirmationTemplateData dynamicTemplateData, string fuelDeskEmail, string confirmationEmail)
        {
            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
            mailMessage.From = new MailAddress(fuelDeskEmail);
            foreach (string email in confirmationEmail.Split(";"))
            {
                if (_mailService.IsValidEmailRecipient(email))
                    mailMessage.To.Add(email);
            }

            mailMessage.OrderConfirmationTemplateData = dynamicTemplateData;

            //Send email
            var result = await _mailService.SendAsync(mailMessage);

            return true;
        }
    }
}
