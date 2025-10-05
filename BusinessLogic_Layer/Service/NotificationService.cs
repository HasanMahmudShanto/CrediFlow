using AutoMapper;
using BusinessLogic_Layer.DTOs;
using DataAccess_Layer;
using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.Service
{
    public class NotificationService
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Notification, NotificationDTO>().ReverseMap();
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
            });
            return new Mapper(config);
        }
        public static bool Create(NotificationDTO NotificationDTO_Data)
        {
            bool is_Create = DataAccessFactory.NotificationData().Create(GetMapper().Map<Notification>(NotificationDTO_Data));
            return is_Create;
        }

        public static List<NotificationDTO> Get()
        {
            var NotificationDTO_Data = DataAccessFactory.NotificationData().Get();
            return GetMapper().Map<List<NotificationDTO>>(NotificationDTO_Data);
        }
        public static List<NotificationDTO> Get_By_Customer(int customer_id)
        {
            List<NotificationDTO> NotificationDTO_Data = GetMapper().Map < List < NotificationDTO >> (DataAccessFactory.NotificationData().Get_By_Customer(customer_id));

            //fetching Customer details for each notification
            foreach(var notification in NotificationDTO_Data)
            {
                var customer = CustomerService.Get(notification.CustomerId);
                notification.CustomerDTO = GetMapper().Map<CustomerDTO>(customer);
            }

            return NotificationDTO_Data;
        }
    }
}
