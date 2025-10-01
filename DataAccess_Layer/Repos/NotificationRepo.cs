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
        public void Create(Notification s)
        {
            db.Notifications.Add(s);
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
        
        public List<Notification> Get()
        {
            return db.Notifications.ToList();
        }

        public Notification Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Notification s)
        {
            throw new NotImplementedException();
        }
    }
}
