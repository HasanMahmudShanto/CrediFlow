using AutoMapper;
using BusinessLogic_Layer.DTOs;
using DataAccess_Layer;
using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
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
            });
            return new Mapper(config);
        }
        public static void Create(NotificationDTO data)
        {
            DataAccessFactory.NotificationData().Create(GetMapper().Map<Notification>(data));
        }

        public static List<NotificationDTO> Get()
        {
            var NotificationDTO_Data = DataAccessFactory.NotificationData().Get();
            return GetMapper().Map<List<NotificationDTO>>(NotificationDTO_Data);
        }
    }
}
