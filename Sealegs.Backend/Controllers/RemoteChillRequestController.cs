using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

using Microsoft.Azure.Mobile.Server;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;
using System.Collections.Generic;
using System.Collections;

namespace Sealegs.Backend.Controllers
{
    public class RemoteChillRequestController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public IList<RemoteChillRequestExteneded> GetAll()
        {
            IList<RemoteChillRequestExteneded> result = (from chillRequest in Context.RemoteChillRequest
                                                         join wine in Context.Wine on chillRequest.MemberBottleID equals wine.Id
                                                         join locker in Context.LockerMember on chillRequest.LockerMemberID equals locker.Id
                                                         where chillRequest.IsCompleted == false
                                                         select new RemoteChillRequestExteneded
                                                         {
                                                             ChillRequest = chillRequest,
                                                             Wine = wine,
                                                             Locker = locker
                                                         }).ToList();

            return result;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public IList<RemoteChillRequest> GetAllByLocker(string lockerId)
        {
            return Context.RemoteChillRequest.Where(item => item.Deleted == false && item.LockerMemberID == lockerId).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public RemoteChillRequest Get(string id)
        {
            return Context.RemoteChillRequest.Where(item => item.Id.Replace("\r\n", string.Empty).ToLower() == id.ToLower()).ToList().FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpPost]
        public bool Update(RemoteChillRequest item)
        {
            item.UpdatedAt = DateTime.Now;
            RemoteChillRequest original = Context.RemoteChillRequest.Find(item.Id);

            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(item);
                int result = Context.SaveChanges();
                return result < 0 ? false : true;
            }
            return false;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpPost]
        public bool Insert(RemoteChillRequest item)
        {
            DateTimeOffset date = DateTime.Now;

            item.CreatedAt = item.UpdatedAt = DateTime.Now;
            Context.RemoteChillRequest.Add(item);
            int result = Context.SaveChanges();

            // Push notification time...
            RemoteChillRequestExteneded request = (from chillRequest in Context.RemoteChillRequest
                                                         join wine in Context.Wine.Include("WineVarietal") on chillRequest.MemberBottleID equals wine.Id
                                                         join locker in Context.LockerMember on chillRequest.LockerMemberID equals locker.Id
                                                         where chillRequest.Id == item.Id
                                                         select new RemoteChillRequestExteneded
                                                         {
                                                             ChillRequest = chillRequest,
                                                             Wine = wine,
                                                             Locker = locker
                                                         }).First();

            if (result < 0)
                return false;

            CustomNotificationController notifications = new CustomNotificationController();
            var requestPNS = $"Remote chill request {request.ChillRequest?.Description} for wine {request?.Wine?.WineTitle}({request?.Wine?.Vintage} - {request.Wine?.WineVarietal?.VarietalName}) from locker member {request?.Locker?.DisplayName} for {request?.ChillRequest?.RequestDate.ToString()}";
            notifications.DispatchNotification(NotificationType.APNS, $"{SealegsUserRole.AdminRole},{SealegsUserRole.ManagerRole}", requestPNS);
            notifications.DispatchNotification(NotificationType.GCM, $"{SealegsUserRole.AdminRole},{SealegsUserRole.ManagerRole}", requestPNS);

            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            RemoteChillRequest original = Context.RemoteChillRequest.Find(id);

            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(original.Deleted = true);
                int result = Context.SaveChanges();
                return result < 0 ? false : true;
            }
            return false;
        }
    }
}