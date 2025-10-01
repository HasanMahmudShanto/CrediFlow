using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interfaces
{
    public interface INotificationRepo
    {
        void Create(Notification s);
        List<Notification> Get();
        Notification Get(int id);
        bool Update(Notification s);
        bool Delete(int id);
    }
}
