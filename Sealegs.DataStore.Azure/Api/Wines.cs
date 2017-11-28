using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Sealegs.Clients.Portable;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure.Api
{
    public class Wines : ApiBase, IWines
    {
        public async Task<bool> DeleteWine(string id)
        {
            throw new NotImplementedException();
        }

        public  async Task<IEnumerable<Wine>> GetAllWinesById(string id)
        {
            var address = String.Format(GetAllWinesByLockerIdUri, id);
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.Wine>>(address);
            result.Where(s => s.ImagePath != null).ForEach(s => s.ImagePath = GetFullImageName(s.ImagePath));
            result.Where(s => s.CheckedOutMemberSignature != null).ForEach(s => s.CheckedOutMemberSignature = GetFullSignatureName(s.CheckedOutMemberSignature));
            result.Where(s => s.CheckedOutEmployeeSignature != null).ForEach(s => s.CheckedOutEmployeeSignature = GetFullSignatureName(s.CheckedOutEmployeeSignature));

            return result;
        }

        public async Task<Wine> GetWineById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertWine(Wine wine)
        {
            var form = new Dictionary<string, string>
            {
                {"MemberBottleID",wine.MemberBottleID },  {"LockerID",wine.LockerID }, {"WineTitle",wine.WineTitle },
                {"Vintage",wine.Vintage }, {"WineVarietalId",wine.WineVarietalId }, {"SpecialOccassion",Convert.ToString(wine.SpecialOccassion) },
                {"SpecialOccassionDescription",wine.SpecialOccassionDescription }, {"BottleSize",wine.BottleSize }, {"Notes",wine.Notes },
                {"Quantity",Convert.ToString(wine.Quantity) }, {"ImagePath",wine.ImagePath }, {"CheckedInDate",Convert.ToString(wine.CheckedInDate) },
                {"CheckedOutDate",Convert.ToString(wine.CheckedOutDate) }, {"CheckedOutLockerMemberID",wine.CheckedOutLockerMemberID }, {"CheckedOutMemberFriendID",wine.CheckedOutMemberFriendID },
                {"CheckedOutMemberSignature",wine.CheckedOutMemberSignature }, {"CheckedOutEmployeeSignature",wine.CheckedOutEmployeeSignature }, {"IsChecked",Convert.ToString(wine.IsChecked) },
                {"CheckoutQuantity",Convert.ToString(wine.CheckoutQuantity) }

            };
            var result = await ClientBase.HttpPostRequest<bool>(InsertWineUri, form);
            return result;
        }

        public async Task<bool> UpdateWine(Wine wine)
        {
            var imagePath = wine.ImagePath;
            wine.ImagePath = GetBaseImageName(wine.ImagePath);

            var form = new Dictionary<string, string>
            {
                {"Id",wine.Id},
                {"MemberBottleID",wine.MemberBottleID },  {"LockerID",wine.LockerID }, {"WineTitle",wine.WineTitle },
                {"Vintage",wine.Vintage }, {"WineVarietalId",wine.WineVarietalId }, {"SpecialOccassion",Convert.ToString(wine.SpecialOccassion) },
                {"SpecialOccassionDescription",wine.SpecialOccassionDescription }, {"BottleSize",wine.BottleSize }, {"Notes",wine.Notes },
                {"Quantity",Convert.ToString(wine.Quantity) }, {"ImagePath",wine.ImagePath }, {"CheckedInDate",Convert.ToString(wine.CheckedInDate) },
                {"CheckedOutDate",Convert.ToString(wine.CheckedOutDate) }, {"CheckedOutLockerMemberID",wine.CheckedOutLockerMemberID }, {"CheckedOutMemberFriendID",wine.CheckedOutMemberFriendID },
                {"CheckedOutMemberSignature",wine.CheckedOutMemberSignature }, {"CheckedOutEmployeeSignature",wine.CheckedOutEmployeeSignature }, {"IsChecked",Convert.ToString(wine.IsChecked) },
                {"CheckoutQuantity",Convert.ToString(wine.CheckoutQuantity) }

            };
            var result = await ClientBase.HttpPostRequest<bool>(UpdateWineUri, form);
            wine.ImagePath = imagePath; // must restore path else UI goes nuts
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

            return (!String.IsNullOrEmpty(imageName)) ? String.Concat(Addresses.WinesStorageBaseAddress, imageName) : String.Empty;
        }

        public string GetBaseSignatureName(string imageUrl)
        {
            return new Uri(imageUrl).Segments.Last();
        }

        public string GetFullSignatureName(string imageName)
        {
            if (Uri.TryCreate(imageName, UriKind.Absolute, out var uri) && uri.Segments.Count() > 3)
                imageName = uri.Segments.Last();

            return (!String.IsNullOrEmpty(imageName)) ? String.Concat(Addresses.SignatureStorageBaseAddress, imageName) : String.Empty;
        }
    }
}
