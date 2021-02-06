using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.Utilities.Extensions.String;
using CelloStore.ViewModels;
using CelloStore.ViewModels.MailTemplate;
using CelloStore.ViewModels.Sales.Order;

namespace CelloStore.Providers
{

    public class OrderProvider : BaseProvider
    {
        private OrderStatusProvider orderStatusProvider;
        private DeliveryTimeProvider deliveryTimeProvider;
        public OrderProvider(EntitiesModel context, IPrincipal principal, OrderStatusProvider orderStatusProvider, DeliveryTimeProvider deliveryTimeProvider)
            : base(context, principal)
        {
            this.orderStatusProvider = orderStatusProvider;
            this.deliveryTimeProvider = deliveryTimeProvider;
        }

        public OrderStatuses TranslateOrderStatus(int orderStatusId)
        {
            return (OrderStatuses) Enum.Parse(typeof (OrderStatuses), orderStatusId.ToString());
        }

        public string CreateOrderFromCart(Order order)
        {
            var autoNumber = context.AutoNumbers.FirstOrDefault(num => num.FormCode == "Order" && num.Year == DateTime.Today.Year) ??
                             new AutoNumber { FormCode = "Order", Year = DateTime.Today.Year, LastNumber = 0 };

            if (autoNumber.LastNumber == 0)
            {
                context.Add(autoNumber);
            }
            autoNumber.LastNumber++;

            context.Add(order);
            string prefix = String.Empty;
            order.OrderNo = String.Format("{0}{1}{2}", prefix.RandomLetters(3).ToUpper(),
                DateTime.Today.Year, autoNumber.LastNumber.ToString("00000"));

            SetAuditFields(order);

            var orderStatusHistory = new OrderStatusHistory();
            orderStatusHistory.Order = order;
            orderStatusHistory.IsActive = true;
            orderStatusHistory.OrderStatusId = (int)OrderStatuses.Unpaid;
            orderStatusHistory.CreatedBy = principal.Identity.Name;
            orderStatusHistory.CreatedDate = DateTime.Now;
            orderStatusHistory.EffectiveDate = DateTime.Today;
            context.Add(orderStatusHistory);

            context.SaveChanges();

            return order.OrderNo;
        }

        public void SetStatusOrder(int orderId, int orderStatusId, string notes, bool sendMail)
        {
            var order = context.Orders.SingleOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.OrderStatusId = orderStatusId;

                var orderStatusHistory = new OrderStatusHistory();
                orderStatusHistory.OrderId = orderId;
                orderStatusHistory.IsActive = true;
                orderStatusHistory.Notes = notes;
                orderStatusHistory.SendMail = sendMail;
                orderStatusHistory.OrderStatusId = orderStatusId;
                orderStatusHistory.CreatedBy = principal.Identity.Name;
                orderStatusHistory.EffectiveDate = DateTime.Today;
                orderStatusHistory.CreatedDate = DateTime.Now;
                context.Add(orderStatusHistory);

                context.SaveChanges();
            }
        }

        public IEnumerable<Order> ListOrders(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Orders.Where(order => order.OrderNo.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListOrderViewModel> ListOrders()
        {
            var query = from order in context.Orders
                        join status in context.OrderStatus on order.OrderStatusId equals status.Id
                        join person in context.People on order.PersonId equals person.Id
                        join deliveryTime in context.DeliveryTimes on order.DeliveryTime equals deliveryTime.Id
                        select new ListOrderViewModel
                        {
                            Id = order.Id,
                            OrderNo = order.OrderNo,
                            OrderDate = order.OrderDate,
                            DeliveryDate = order.DeliveryDate.GetValueOrDefault(),
                            DeliveryTime = order.DeliveryTime,
                            DeliveryTimeName = deliveryTime.Name,
                            OrderStatus = status.Name,
                            PersonName = person.FirstName.Trim() + " " + person.LastName.Trim(),
                            PhoneNumber = person.PhoneNumber
                        };
            return query;
        }

        public IEnumerable<Order> ListPersonOrders(int personId, int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Orders.Where(order => order.PersonId == personId && order.OrderNo.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListOrderViewModel> ListPersonOrders(int personId)
        {
            var query = from order in context.Orders
                        join status in context.OrderStatus on order.OrderStatusId equals status.Id
                        join person in context.People on order.PersonId equals person.Id
                        join deliveryTime in context.DeliveryTimes on order.DeliveryTime equals deliveryTime.Id
                        where person.Id == personId
                        orderby order.OrderDate descending 
                        select new ListOrderViewModel
                        {
                            Id = order.Id,
                            OrderNo = order.OrderNo,
                            OrderDate = order.OrderDate,
                            DeliveryDate = order.DeliveryDate.GetValueOrDefault(),
                            DeliveryTime = order.DeliveryTime,
                            DeliveryTimeName = deliveryTime.Name,
                            OrderStatus = status.Name,
                            PersonName = person.FirstName.Trim() + " " + person.LastName.Trim()
                        };
            return query;
        }

        public async Task NotifyOrderStatusUpdate(string smtpServer, int smtpPort, bool useSsl, bool defaultCredential, string userName, string password, string senderName, string senderMail, string destinationMail, string body)
        {
            var smtp = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = useSsl,
                UseDefaultCredentials = defaultCredential
            };
            if (!defaultCredential)
            {
                smtp.Credentials = new NetworkCredential(userName, password);
            }

            var mailMessage = new MailMessage();
            mailMessage.To.Add(destinationMail);
            mailMessage.From = new MailAddress(senderMail, senderName);
            mailMessage.IsBodyHtml = true;

            mailMessage.Body = body;

            await smtp.SendMailAsync(mailMessage);
        }


        public Order GetOrder(int id)
        {
            return context.CreateDetachedCopy(context.Orders.SingleOrDefault(cat => cat.Id == id));
        }

        public Order GetOrder(string orderNo)
        {
            return context.Orders.FirstOrDefault(o => o.OrderNo == orderNo);
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int orderId)
        {
            return context.OrderDetails.Where(o => o.OrderId == orderId).ToList();
        }

        public IQueryable<OrderStatusHistoryViewModel> GetOrderStatusHistories(int id, bool @descending = true)
        {
            var query = from statusHistory in context.OrderStatusHistories
                        join status in context.OrderStatus on statusHistory.OrderStatusId equals status.Id
                        where statusHistory.OrderId == id && statusHistory.IsActive
                        select new OrderStatusHistoryViewModel
                        {
                            CreatedDate = statusHistory.CreatedDate,
                            Date = statusHistory.EffectiveDate,
                            Status = status.Name,
                            Notes = statusHistory.Notes,
                            SendMail = statusHistory.SendMail
                        };

            query = @descending
                ? query.OrderByDescending(order => order.CreatedDate)
                : query.OrderBy(order => order.CreatedDate);

            return query;
        }

        public bool ValidateOrderOwner(int orderId, string currentUserName)
        {
            //TODO validate order and its owner
            return true;
        }

        public OrderMailViewModel GetOrderMail(Order order)
        {
            var model = new OrderMailViewModel();
            var person = order.Person;

            if(person != null)
            {
                var deliveryTime = deliveryTimeProvider.GetDeliveryTime(order.DeliveryTime);

                model.OrderNo = order.OrderNo;
                model.CustomerName = String.Format("{0} {1}", person.FirstName, person.LastName);
                model.DeliveryDate = order.DeliveryDate.GetValueOrDefault();
                model.DeliveryTimeName = deliveryTime.Name;
                model.OrderDate = order.OrderDate;
                model.CustomerMail = person.Email;                
            }

            return model;
        }

        public bool ValidateOrderPayment(string orderNo, decimal payAmount)
        {
            bool result = false;
            var validatedOrder = context.Orders.FirstOrDefault(order => order.OrderNo == orderNo);
            if (validatedOrder != null)
            {
                decimal orderAmount = validatedOrder.OrderDetails
                    .Sum(detail => detail.Qty * detail.UnitPrice - detail.DiscValue);
                result = orderAmount == payAmount;
            }
            return result;
        }

        public bool IsOrderPaymentConfirmed(string orderNo)
        {
            var query = from order in context.Orders
                        join payConf in context.PaymentConfirmations on order.Id equals payConf.OrderId
                        where order.OrderNo == orderNo
                        select payConf;
            return query.Any();
        }

        public void CreatePaymentConfirmation(string orderNo, int bankId, int paymentMethodId, DateTime paymentDate, 
            string senderName, decimal payAmount, string notes)
        {
            var order = GetOrder(orderNo);
            if(order != null)
            {
                var paymentConf = new PaymentConfirmation();
                paymentConf.OrderId = order.Id;
                paymentConf.PaymentMethodId = paymentMethodId;
                paymentConf.BankId = bankId;
                paymentConf.Amount = payAmount;
                paymentConf.Date = paymentDate;
                paymentConf.Notes = notes;
                SetAuditFields(paymentConf);
                context.Add(paymentConf);
                context.SaveChanges();

            }

        }

        public PaymentConfirmation GetPaymentConfirmation(int orderId)
        {
            var selectedOrder = context.Orders.SingleOrDefault(order => order.Id == orderId);
            if(selectedOrder != null)
            {
                return selectedOrder.PaymentConfirmations.FirstOrDefault();
            }
            return null;
        }

        public void VerifyPaymentConfirmation(int paymentConfirmationId)
        {
            var paymentConfirmation =
                context.PaymentConfirmations.SingleOrDefault(pay => pay.Id == paymentConfirmationId);
            var person = context.People.SingleOrDefault(p => p.Email == principal.Identity.Name);
            if(paymentConfirmation != null && person != null)
            {
                paymentConfirmation.VerifiedWhen = DateTime.Now;
                paymentConfirmation.VerifiedByPersonId = person.Id;
                context.SaveChanges();
            }

        }
    }
}
