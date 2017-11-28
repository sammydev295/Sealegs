using System.Linq;
using System.Web.Http;
using System.Collections.Generic;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;

namespace Sealegs.Backend.Controllers
{
    public class FeedbackController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        // GET api/Default
        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole)]
        [HttpGet]
        public IList<FeedbackExteneded> GetAll()
        {
            IList<FeedbackExteneded> result = (from feedback in Context.Feedbacks
             join wine in Context.Wine on feedback.WineId equals wine.Id
             join locker in Context.LockerMember on feedback.UserId equals locker.Id
             where feedback.Deleted == false
             select new FeedbackExteneded
             {
                 Feedback = feedback,
                 Wine = wine,
                 Locker = locker
             }).ToList();

            return result;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public IList<Feedback> GetAll(string userId)
        {
            return Context.Feedbacks.Where(item => item.UserId == userId && item.Deleted == false).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole)]
        [HttpGet]
        public IList<FeedbackExteneded> GetAllByWine(string wineId)
        {
            IList<FeedbackExteneded> result = (from feedback in Context.Feedbacks
                                               join wine in Context.Wine on feedback.WineId equals wine.Id
                                               join locker in Context.LockerMember on feedback.UserId equals locker.Id
                                               where feedback.Deleted == false && feedback.WineId == wineId
                                               select new FeedbackExteneded
                                               {
                                                   Feedback = feedback,
                                                   Wine = wine,
                                                   Locker = locker
                                               }).ToList();

            return result;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public Feedback Get(string id)
        {
            return Context.Feedbacks.Where(item => item.Id == id).FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Update(Feedback item)
        {
            Feedback original = Context.Feedbacks.Find(item.Id);

            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(item);
                int result = Context.SaveChanges();
                return result < 0 ? false : true;
            }
            return false;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Insert(Feedback item)
        {
            Context.Feedbacks.Add(item);
            int result = Context.SaveChanges();
            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            Feedback original = Context.Feedbacks.Find(id);

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