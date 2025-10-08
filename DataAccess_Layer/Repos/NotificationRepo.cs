using DataAccess_Layer.EF;
using DataAccess_Layer.EF.Tables;
using DataAccess_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Repos
{
    public class NotificationRepo : INotificationRepo
    {
        CrediFlowContext db;
        public NotificationRepo()
        {
            db = new CrediFlowContext();
        }
        public bool Create(Notification s)
        {
            db.Notifications.Add(s);
            db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var ex = db.Notifications.Find(id);
            db.Notifications.Remove(ex);
            db.SaveChanges();
            return true;
        }
        
        public List<Notification> Get()
        {
            return db.Notifications.ToList();
        }

        public Notification Get(int id)
        {
            return db.Notifications.Find(id);
        }

        public bool Update(Notification s)
        {
            var Data = db.Notifications.Find(s.Notification_Id);
            db.Entry(Data).CurrentValues.SetValues(s);
            db.SaveChanges();
            return true;
        }
        public List<Notification> Get_By_Customer(int customer_id)
        {
            return db.Notifications.Where(n => n.Customer_Id == customer_id).ToList();
        }
    }
}
