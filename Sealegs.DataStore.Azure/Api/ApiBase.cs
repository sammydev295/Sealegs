using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Sealegs.DataStore.Azure.Api
{
    public class ApiBase
    {
        public readonly HttpClientBase ClientBase = new HttpClientBase();
        public static MobileServiceClient MobileService { get; set; }

        // public static string Token =String.Empty;

        #region API End Points

        #region Auth
       
        protected const string GetUserUri = "/api/SealegsUser/Get?userId={0}";
        protected const string UpdateUserImageUri = "/api/SealegsUser/Update";
        protected const string InsertUserApi = "/api/SealegsUser/Insert";

        #endregion


        #region News

        protected const string GetAllNewsUri = "/api/News/GetAll";
        protected const string DeleteNewsUri = "/api/News/Delete?id={0}";
        protected const string InsertNewsUri = "/api/News/Insert";
        protected const string UpdateNewsUri = "/api/News/Update";


        #endregion

        #region Featured Events

        protected const string GetAllEventsUri = "/api/FeaturedEvent/GetAll";
        protected const string DeleteEventsUri = "/api/FeaturedEvent/Delete?id={0}";
        protected const string UpdateEventsUri = "/api/FeaturedEvent/Update";
        protected const string InsertEventsUri = "/api/FeaturedEvent/Insert";

        #endregion

        #region Locker Member 

        protected const string GetByMemberIdUri = "/api/LockerMember/GetByMemberId?id={0}";
        protected const string GetAllNonStaffUri = "/api/LockerMember/GetAllNonStaff";
        protected const string DeleteLockerMemberUri = "/api/LockerMember/Delete?id={0}";
        protected const string UpdateLockerMemberUri = "/api/LockerMember/Update";
        protected const string InsertLockerMemberUri = "/api/LockerMember/Insert";

        #endregion

        #region Wines

        protected const string GetAllWinesByLockerIdUri = "/api/Wine/GetAll?lockerId={0}";
        protected const string GetWineByIdUri = "/api/Wine/GetAll?id={0}";
        protected const string UpdateWineUri = "/api/Wine/Update";
        protected const string InsertWineUri = "/api/Wine/Insert";
        protected const string DeleteWineUri = "/api/Wine/Delete?id={0}";

        #endregion

        #region Employees

        protected const string GetAllStaffUri = "/api/LockerMember/GetAllStaff";

        #endregion

        #region Remote Chill Request

        protected const string GetAllRemoteChillRequestUri = "/api/RemoteChillRequest/GetAll";
        protected const string GetRemoteChillRequestByIdUri = "/api/RemoteChillRequest/Get?id={0}";
        protected const string DeleteRemoteChillRequestUri = "/api/RemoteChillRequest/Delete?id={0}";
        protected const string InsertRemoteChillRequestUri = "/api/RemoteChillRequest/Insert";
        protected const string UpdateRemoteChillRequestUri = "/api/RemoteChillRequest/Update";
        protected const string GetAllRemoteChillRequestByLockerIdUri = "/api/RemoteChillRequest/GetAllByLocker?lockerId={0}";

        #endregion

        #region Wine Varietal

        protected const string GetAllWineVarietalByLockerIdUri = "/api/RemoteChillRequest/GetAllByLocker";
        protected const string GetAllWineVarietalUri = "/api/WineVarietal/GetAll";
        protected const string GetWineVarietalByIdUri = "/api/WineVarietal/Get?id={0}";
        protected const string UpdateWineVarietalUri = "/api/WineVarietal/Update";
        protected const string InsertWineVarietalUri = "/api/WineVarietal/Insert";
        protected const string DeleteWineVarietalUri = "/api/WineVarietal/Delete?id={0}";

        protected const string GetAllLockerTypesUri = "/api/LockerType/GetAll";
        protected const string GetLockerTypesByIdUri = "/api/LockerType/Get?id={0}";

        #endregion

        #region Notifications

        protected const string GetAllNotificationsUri = "/api/CustomNotification/GetAll";
        protected const string UpdateNotificationsUri = "/api/CustomNotification/Update";
        protected const string InsertNotificationsUri = "/api/CustomNotification/Insert";
        protected const string DeleteNotificationsUri = "/api/CustomNotification/Delete?id={0}";
        protected const string SendNotificationsUri = "/api/CustomNotification/Send";

        #endregion

        #region Feeback

        protected const string GetAllFeedbackUri = "/api/Feedback/GetAll";
        protected const string GetAllFeedbackByWineUri = "/api/Feedback/GetAllByWine?wineId={0}";
        protected const string GetSingleFeedbackUri = "/api/Feedback/Get?id={0}";
        protected const string InsertFeedbackUri = "/api/Feedback/Insert";
        protected const string DeleteFeedbackUri = "/api/Feedback/Delete?id={0}";
        protected const string UpdateFeedbackUri = "/api/Feedback/Update";

        #endregion

        #region Favorite

        protected const string GetAllFavoriteUri = "/api/Favorite/GetAll";
        protected const string GetAllFavoriteByLockerUri = "/api/Favorite/GetAll?Id={0}";
        protected const string InsertFavoriteUri = "/api/Favorite/Insert";
        protected const string DeleteFavoriteUri = "/api/Favorite/Delete?id={0}";
        protected const string UpdateFavoriteUri = "/api/Favorite/Update";

        #endregion

        #region UserRole
        protected const string GetAllRolesUri = "/api/SealegsUserRole/GetAll";
        #endregion

        #endregion
    }
}
