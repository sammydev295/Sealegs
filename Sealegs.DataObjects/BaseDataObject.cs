using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#if BACKEND

using Microsoft.Azure.Mobile.Server;

#elif MOBILE

using MvvmHelpers;

#endif

namespace Sealegs.DataObjects
{
    public interface IBaseDataObject
    {
        String Id {get;set;}
    }

#if BACKEND
    public class BaseDataObject 
    {
        public BaseDataObject ()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
#else
    public class BaseDataObject : ObservableObject, IBaseDataObject
    {
        public BaseDataObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string RemoteId { get; set; }

        public String Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }
    }
#endif
}

