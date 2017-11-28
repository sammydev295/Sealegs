using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;
using Sealegs.DataStore.Mock;

namespace Sealegs.DataStore.Mock
{
    public class LockerMemberStore : BaseStore<LockerMember>, ILockerMemberStore
    {
        #region Properties

        ObservableCollection<LockerMember> lockers;
        
        public override string Identifier => "Lockers";

        #region Mock data

        /// <summary>
        /// Mock data
        /// </summary>
        string[] images = {
            "05b06e15-4d88-4e9a-86bc-891045102727_a6186584-e9ac-43fc-adbe-96a7552cba8a.jpg",
            "09f61a23-3d0a-423d-8f67-62fbffb9c260.jpg",
            "0aa6743c-df50-4784-b179-449f3e82f145_590d584a-f5ea-487e-9e8a-0068f1ccf324.jpg",
            "2c58f09d-ba62-48d3-be49-c6022dd72033.jpg",
            "311837c8-bace-4b27-9014-6e998135ad64_49f27484-99a3-426a-8b37-5b75142e0763.jpg",
            "323fe83c-8f86-49a0-b3a4-12bf91d017ca.jpg",
            "3f705b4a-c943-4cf1-98a3-c3ca3437502c.JPG",
            "4286651e-2503-4ddd-b472-ecff2afe7b24.JPG",
            "53c8ab8f-6786-4530-b09a-e960aac188d8.JPG",
            "7d86355c-a817-4802-a3b0-9ea2d7bb0818.jpg",
            "820f5473-9ae2-4269-9ee4-2a58042f125e_f8b7672e-ec4a-48cc-8df5-397d033d3cc6.jpg",
            "9f1de418-96d2-49f2-8705-48f586d4f9dc_35a0f45c-53ef-4435-b40a-1ef9f3d7cc8a.jpg",
            "9f1de418-96d2-49f2-8705-48f586d4f9dc_68db0458-615a-484a-aece-3e50f3559a55.jpg",
            "9f1de418-96d2-49f2-8705-48f586d4f9dc_bb151ec4-8511-4854-8b2a-03608713e0fc.jpg",
            "c621056c-22b9-49d1-a5ad-df711e4d4827_15abafa8-a15e-4a6d-bbbe-67f7964bff58.jpg",
            "caad2056-4617-4d82-8e07-48cd81990e13.jpg",
            "cf97cb0e-efd7-4825-9f79-08c0d0ef4b0a_a42261e5-7310-4b29-a905-03fee378b013.jpg",
            "d43c4b3a-010b-49e1-b6a3-560d0083174e_79391e6d-8862-4a31-b2b3-9876b03ce85c.jpg",
            "d6c376fb-a68a-4c96-b3d4-1cfa0f09f482.jpg",
            "d9ca770f-29fb-45de-b556-300177abf76d_3bbbef52-5a5b-49e6-8006-68f96ca11377.jpg",
            "dd4143d5-5996-40db-887e-3eb6a40d95a6_9b553b64-c5a1-485e-a64b-f1f4b1a7e9e7.jpg",
            "eabdc233-e566-4cb0-981a-d9d993b3c929.jpg",
            "ecd6f541-bd47-4508-9fdc-9cda5ea0dbd2.jpg",
            "fcf7e0a1-864f-4c5e-b52b-6ca950346330_14a6d101-66ad-4dd6-a4f8-0c4ccf842464.jpg"
            };

        #endregion

        #endregion

        #region CTOR

        public LockerMemberStore()
        {
        }

        #endregion 

        #region ILockerMemberStore implementation

        public async Task<IEnumerable<LockerMember>> GetAllLockerMembers(bool force)
        {
            if (!initialized)
                await InitializeStore();


            lockers = new ObservableCollection<LockerMember>();

            int number = 0;
            for (int n = 0; n < 3; n++)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    number++;
                    var item = new LockerMember()
                    {
                        LockerMemberID = "dd6f146c-c10b-46fe-8b94-72f6ac9c5d0a",
                        DisplayName = $"Locker {number}",
                        ProfileImage = ImagesURI + images[i],
                    };

                    lockers.Add(item);
                }
            }

            return lockers;
        }

        public async Task<LockerMember> GetSingleLockerMember(int id)
        {
            return new LockerMember()
            {
                DisplayName = "Member 10",
                ProfileImage = "Sealegs.Clients.UI.Images.Locker.png"
            };
        }

        #endregion

        #region IBaseStore implementation

        bool initialized = false;
        public async override Task InitializeStore()
        {
            if (initialized)
                return;

            initialized = true;
        }

        #endregion
    }
}
