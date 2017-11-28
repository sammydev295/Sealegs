using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Sealegs.Clients.Portable;
using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Azure.Api
{
    public class LockerMember : ApiBase, ILockerMember
    {
        public async Task<DataObjects.LockerMember> GetByMemberId(string id)
        {
            var api = String.Format(GetByMemberIdUri, id);
            var result = await ClientBase.HttpGetRequest<DataObjects.LockerMember>(api);
            result.ProfileImage = GetFullImageName(result.ProfileImage);
            return result;
        }

        public async Task<IEnumerable<DataObjects.LockerMember>> GetAllNonStaff()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.LockerMember>>(GetAllNonStaffUri);
            result.Where(s => s.ProfileImage != null).ForEach(s=>s.ProfileImage = GetFullImageName(s.ProfileImage));
            return result;
        }

        public async Task<IEnumerable<DataObjects.LockerMember>> GetAllStaff()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.LockerMember>>(GetAllStaffUri);
            result.Where(s => s.ProfileImage != null).ForEach(s => s.ProfileImage = GetFullImageName(s.ProfileImage));
            return result;
        }

        public async Task<bool> InsertLocker(DataObjects.LockerMember locker)
        {
            var form = new Dictionary<string, string>
            {
                {"LockerMemberID",locker.LockerMemberID },{"DisplayName", locker.DisplayName},
                {"MemberName",locker.MemberName },{"UserID", locker.UserID},
                {"Street",locker.Street },{"City", locker.City},
                {"State",locker.State },{"ZipCode", locker.ZipCode},
                {"HomePhone",locker.HomePhone },{"WorkPhone", locker.WorkPhone},
                {"Mobile",locker.Mobile },{"EmailAddress", locker.EmailAddress},
                {"EmailAddress2",locker.EmailAddress2 },{"LockerTypeID", locker.LockerTypeID},
                {"CreditCardNumber",locker.CreditCardNumber },{"CreditCardType", locker.CreditCardType},
                {"ExpireyDate",Convert.ToString(locker.ExpireyDate) },{"NameOnCard", locker.NameOnCard},

                {"SecurityCode",locker.SecurityCode },{"TileColor", locker.TileColor},
                {"ProfilePicture",locker.ProfilePicture },{"EmailAlerts",Convert.ToString(locker.EmailAlerts)},
                {"InventoryAlerts",Convert.ToString(locker.InventoryAlerts) },{"LastCheckedOutDate", Convert.ToString(locker.LastCheckedOutDate)},
                {"NoOfBottles",Convert.ToString(locker.NoOfBottles) },{"CurrentRenewalDate", Convert.ToString(locker.CurrentRenewalDate)},
                {"NextRenewalDate",Convert.ToString(locker.NextRenewalDate) },{"ProfileImage", locker.ProfileImage},
                {"SignatureImage",locker.SignatureImage },{"FacebookID", locker.FacebookID},

                {"FacebookUserID",locker.FacebookUserID },{"FacebookAccessToken", locker.FacebookAccessToken},
                {"FacebookTokenExpired",Convert.ToString(locker.FacebookTokenExpired) },{"FacebookTokenExpiretionDate", Convert.ToString(locker.FacebookTokenExpiretionDate)},
                {"TwitterID",locker.TwitterID },{"OAuthToken", locker.OAuthToken},
                {"AccessToken",locker.AccessToken },{"IsAllowFacebookUpdate", Convert.ToString(locker.IsAllowFacebookUpdate)},
                {"IsAllowTwitterUpdate",Convert.ToString(locker.IsAllowTwitterUpdate) },{"Promotions", Convert.ToString(locker.Promotions)},
                {"Notes",locker.Notes },{"IsStaff", Convert.ToString(locker.IsStaff)},

                {"IsFavorite",Convert.ToString(locker.IsFavorite) },{"IsActive", Convert.ToString(locker.IsActive)},

            };
            var result = await ClientBase.HttpPostRequest<bool>(InsertLockerMemberUri, form);
            return result;
        }

        public async Task<bool> UpdateLocker(DataObjects.LockerMember locker)
        {
            var imagePath = locker.ProfileImage;
            locker.ProfileImage = GetBaseImageName(locker.ProfileImage);
            var form = new Dictionary<string, string>
            {
                {"Id",locker.Id },
                { "LockerMemberID",locker.LockerMemberID },{"DisplayName", locker.DisplayName},
                {"MemberName",locker.MemberName },{"UserID", locker.UserID},
                {"Street",locker.Street },{"City", locker.City},
                {"State",locker.State },{"ZipCode", locker.ZipCode},
                {"HomePhone",locker.HomePhone },{"WorkPhone", locker.WorkPhone},
                {"Mobile",locker.Mobile },{"EmailAddress", locker.EmailAddress},
                {"EmailAddress2",locker.EmailAddress2 },{"LockerTypeID", locker.LockerTypeID},
                {"CreditCardNumber",locker.CreditCardNumber },{"CreditCardType", locker.CreditCardType},
                {"ExpireyDate",Convert.ToString(locker.ExpireyDate) },{"NameOnCard", locker.NameOnCard},

                {"SecurityCode",locker.SecurityCode },{"TileColor", locker.TileColor},
                {"ProfilePicture",locker.ProfilePicture },{"EmailAlerts",Convert.ToString(locker.EmailAlerts)},
                {"InventoryAlerts",Convert.ToString(locker.InventoryAlerts) },{"LastCheckedOutDate", Convert.ToString(locker.LastCheckedOutDate)},
                {"NoOfBottles",Convert.ToString(locker.NoOfBottles) },{"CurrentRenewalDate", Convert.ToString(locker.CurrentRenewalDate)},
                {"NextRenewalDate",Convert.ToString(locker.NextRenewalDate) },{"ProfileImage", locker.ProfileImage},
                {"SignatureImage",locker.SignatureImage },{"FacebookID", locker.FacebookID},

                {"FacebookUserID",locker.FacebookUserID },{"FacebookAccessToken", locker.FacebookAccessToken},
                {"FacebookTokenExpired",Convert.ToString(locker.FacebookTokenExpired) },{"FacebookTokenExpiretionDate", Convert.ToString(locker.FacebookTokenExpiretionDate)},
                {"TwitterID",locker.TwitterID },{"OAuthToken", locker.OAuthToken},
                {"AccessToken",locker.AccessToken },{"IsAllowFacebookUpdate", Convert.ToString(locker.IsAllowFacebookUpdate)},
                {"IsAllowTwitterUpdate",Convert.ToString(locker.IsAllowTwitterUpdate) },{"Promotions", Convert.ToString(locker.Promotions)},
                {"Notes",locker.Notes },{"IsStaff", Convert.ToString(locker.IsStaff)},

                {"IsFavorite",Convert.ToString(locker.IsFavorite) },{"IsActive", Convert.ToString(locker.IsActive)}
            };
            var result = await ClientBase.HttpPostRequest<bool>(UpdateLockerMemberUri, form);
            locker.ProfileImage = imagePath; // must restore path else UI goes nuts
            return result;
        }

        public async Task<bool> DeleteLocker(string id)
        {
            var api = String.Format(DeleteLockerMemberUri, id);
            var result = await ClientBase.HttpGetRequest<bool>(api);
            return result;
        }

        public string GetBaseImageName(string imageUrl)
        {
            return new Uri(imageUrl).Segments.Last();
        }

        public string GetFullImageName(string imageName)
        {
            if (Uri.TryCreate(imageName, UriKind.Absolute, out var uri) && uri.Segments.Count() > 3)
                imageName = uri.Segments.Last();

            return (!String.IsNullOrEmpty(imageName)) ? String.Concat(Addresses.LockersStorageBaseAddress, imageName) : String.Concat(Addresses.LockersStorageBaseAddress, DataObjects.LockerMember.DefaultProfileImage);
        }
    }
}
