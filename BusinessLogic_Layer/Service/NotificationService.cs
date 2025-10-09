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
using System.Net;
using System.Net.Mail;

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
            NotificationDTO_Data.Date = DateTime.Now;
            NotificationDTO_Data.Is_Read = false;

            bool is_sent = Send_Mail(NotificationDTO_Data);

            bool is_Create = DataAccessFactory.NotificationData().Create(GetMapper().Map<Notification>(NotificationDTO_Data));
            return is_Create;
        }

        public static bool Send_Mail(NotificationDTO NotificationDTO_Data)
        {
            try
            {
                
                // Set up the SMTP client
                SmtpClient client = new SmtpClient("smtp.gmail.com") // Replace with your SMTP server
                {
                    Port = 587, // Replace with your SMTP port
                    Credentials = new NetworkCredential("hasanmahmudshanto100@gmail.com", "kwft nnxj vvgy fpba"), // Replace with your email and password
                    EnableSsl = true,
                };

                // Create the email message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("hasanmahmudshanto100@gmail.com"); // Sender
                mail.To.Add(NotificationDTO_Data.CustomerDTO.Email); // Recipient
                mail.Subject = NotificationDTO_Data.Title;
                mail.Body = NotificationDTO_Data.Message;
                mail.IsBodyHtml = false; // Set true if you want to send HTML email

                // Send the email
                client.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                Console.WriteLine("Exception caught in Send_Mail(): {0}", ex.ToString());
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
        

        public static List<NotificationDTO> Get()
        {
            var NotificationDTO_Data = DataAccessFactory.NotificationData().Get();
            return GetMapper().Map<List<NotificationDTO>>(NotificationDTO_Data);
        }
        public static NotificationDTO Get(int id)
        {
            var NotificationDTO_Data = DataAccessFactory.NotificationData().Get(id);
            return GetMapper().Map<NotificationDTO>(NotificationDTO_Data);
        }
        public static List<NotificationDTO> Get_By_Customer(int customer_id)
        {
            List<NotificationDTO> NotificationDTO_Data = GetMapper().Map < List < NotificationDTO >> (DataAccessFactory.NotificationData().Get_By_Customer(customer_id));

            //Setting is_read property to true for all fetched notifications
            foreach(var notification in NotificationDTO_Data)
            {
                if(notification.Is_Read == false)
                {
                    notification.Is_Read = true;
                    DataAccessFactory.NotificationData().Update(GetMapper().Map<Notification>(notification));
                }
            }

            //fetching Customer details for each notification
            foreach (var notification in NotificationDTO_Data)
            {
                var customer = CustomerService.Get(notification.Customer_Id);
                notification.CustomerDTO = GetMapper().Map<CustomerDTO>(customer);
            }

            return NotificationDTO_Data;
        }

        public static bool Delete(int id)
        {
            return DataAccessFactory.NotificationData().Delete(id);
        }

        public static NotificationDTO Merge_Update(NotificationDTO NotificationDTO_Data, NotificationDTO Prev_Data)
        {
            if(NotificationDTO_Data.Title == null) NotificationDTO_Data.Title = Prev_Data.Title;
            if(NotificationDTO_Data.Message == null) NotificationDTO_Data.Message = Prev_Data.Message;
            if(NotificationDTO_Data.Date == null) NotificationDTO_Data.Date = Prev_Data.Date;
            if(NotificationDTO_Data.Date == DateTime.MinValue) NotificationDTO_Data.Date = Prev_Data.Date;
            if (NotificationDTO_Data.Customer_Id == 0) NotificationDTO_Data.Customer_Id = Prev_Data.Customer_Id;
            return NotificationDTO_Data;

        }

        public static bool Update(NotificationDTO NotificationDTO_Data)
        {
            var Prev_Data = Get(NotificationDTO_Data.Notification_Id);
            if (Prev_Data == null) return false;
            NotificationDTO_Data = Merge_Update(NotificationDTO_Data, Prev_Data);
            return DataAccessFactory.NotificationData().Update(GetMapper().Map<Notification>(NotificationDTO_Data));
        }
    }
}
