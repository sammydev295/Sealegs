#define DEBUG // Hockeyapp environment. Force variables in DEBUG mode
//#define SHAREPOINT2013A // Sammy machine

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sealegs.Clients.Portable
{
    public static class ApiKeys
    {
        public const string HockeyAppiOS = "f2a8232898614f11b6ebd1118804fc1d";
        public const string HockeyAppAndroid = "22249b4d15bb455eb65f67649a939fab"; //f2a8232898614f11b6ebd1118804fc1d  old key
        public const string HockeyAppUWP = "HockeyAppUWP";

        public const string AzureKey = "AzureKey";
        public const string GoogleSenderId = "381575223842";
        public const string AzureHubName = "SealegsNotificationHub";
        public const string AzureListenConnection = "Endpoint=sb://sealegsnotificationnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=You1oo+IYd0LRLpPm3GMWck0D+Yac/rt2o5im2H5WHo=";
        //                                                     Endpoint=sb://sealegsnotificationnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=k5OLJ2rSasiAJtUsFRcsf+JC0EKQHZNTGfQwDWh1iH4=
    }

    public static class Addresses
    {
        public static  string Token = String.Empty;


        public static readonly string App = "com.sealegs.lockerapp";
        public static readonly string AppId = "554a02aaa96a40569716e98903ee7c4a"; // internal number
        public static readonly string GoogleProjectID = "140656397139";
        public static readonly string GoogleAppID = "140656397139-6dvne7fqlql6cp0beg54sr5q7utdefuk.apps.googleusercontent.com";
        //#if DEBUG
        //        public static readonly string FacebookAppID = "1633725579977014"; // -Dev app
        //#else
        public static readonly string FacebookAppID = "1140874385928805";
        //#endif
        public static readonly string HockeyappID = "d70eb3f4b69a42ae96cb4ffc777c7fa1";

        public static readonly string SealegsAzureStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=sealegsblob;AccountKey=wEwu+oboEGrqO7GoyIMMowTLc1uxOwnav4UtPHSQFdIm1QRqdCNtWSzjixkq5TASVdYMmy0xlQzHFEFnW60U6Q==;EndpointSuffix=core.windows.net";

        public static readonly string COMPUTER_VISION_API_KEY = "3aacede42d224136b4bc203059e9b749";

        public static readonly string WinesStorage = "resources-wines";

        public static readonly string LockersStorage = "resources-lockers";

        public static readonly string MembershipStorage = "Membership";

        public static readonly string SignatureStorage = "signature";

        public static readonly string WinesStorageBaseAddress =
            "https://sealegsblob.blob.core.windows.net/sealegsblobcontainer/resources-wines/";

        public static readonly string LockersStorageBaseAddress =
            "https://sealegsblob.blob.core.windows.net/sealegsblobcontainer/resources-lockers/";

        public static readonly string MembershipStorageBaseAddress =
            "https://sealegsblob.blob.core.windows.net/sealegsblobcontainer/Membership/";

        public static readonly string SignatureStorageBaseAddress =
            "https://sealegsblob.blob.core.windows.net/sealegsblobcontainer/Signature/";
#if DEBUG

#if SHAREPOINT2013A
        public static readonly string HttpWebDevAddress = "http://192.168.1.104:81";
        public static readonly string ApiAddress = "http://192.168.1.104";
        public static readonly string AccountAddress = "http://192.168.1.104:83";

        public static readonly string DomainAPI = "drdev.com";
        public static readonly string DomainAccount = "azurewebsites.net";
        public static readonly string DomainStorage = "drdev.com";
        public static readonly string DomainCDN = "drdev.com";

        public static readonly string SubDomainApi = "sealegs-backend";
        public static readonly string SubDomainAccount = "sealegs";
        public static readonly string SubDomainStorage = "sealegs-backend";
        public static readonly string SubDomainCDN = "sealegs-backend";

        public static readonly string ApiDomain = SubDomainApi + "." + DomainAPI;
        public static readonly string AccountDomain = SubDomainAccount + "." + DomainAccount;
        public static readonly string StorageDomain = SubDomainStorage + "." + DomainStorage;
        public static readonly string CdnDomain = SubDomainCDN + DomainCDN;

        public static readonly string ApiOnline = @"https://sealegs.azurewebsites.net";
        public static readonly string AccountOnline = "https://" + AccountDomain;
        public static readonly string StorageOnline = "https://" + StorageDomain;
        public static readonly string CDNOnline = "https://" + CdnDomain;

        public static readonly string AzureStorageBlobContainer = "sealegsblobcontainer";

        public static readonly string AzureStorageDirectAddress = StorageOnline + "/" + AzureStorageBlobContainer; // SSL with custom domain not supported in Azure.
        public static string ResoucessImagesDirectAddress => AzureStorageDirectAddress + "/resources";
        public static string LockersImagesDirectAddress => ResoucessImagesDirectAddress + "-lockers";
        public static string ResourcesImagesDirectAddress => ResoucessImagesDirectAddress + "-images";
        public static string WinesImagesDirectAddress => ResoucessImagesDirectAddress + "-wines";

#else
        public static readonly string DomainAPI = "azurewebsites.net";
        public static readonly string DomainAccount = "azurewebsites.net";
        public static readonly string DomainStorage = "blob.core.windows.net";
        public static readonly string DomainCDN = "azureedge.net";

        public static readonly string SubDomainApi = "sealegs";
        public static readonly string SubDomainAccount = "sealegs";
        public static readonly string SubDomainStorage = "sealegsblob";
        public static readonly string SubDomainCDN = "sealegs";

        public static readonly string ApiDomain = SubDomainApi + "." + DomainAPI;
        public static readonly string AccountDomain = SubDomainAccount + "." + DomainAccount;
        public static readonly string StorageDomain = SubDomainStorage + "." + DomainStorage;
        public static readonly string CdnDomain = SubDomainCDN + DomainCDN;

        public static readonly string ApiOnline = "https://" + ApiDomain;
        public static readonly string AccountOnline = "https://" + AccountDomain;
        public static readonly string StorageOnline = "https://" + StorageDomain;
        public static readonly string CDNOnline = "https://" + CdnDomain;

        public static readonly string AzureStorageBlobContainer = "sealegsblobcontainer"; 

        public static readonly string AzureStorageDirectAddress = StorageOnline  + "/" + AzureStorageBlobContainer; // SSL with custom domain not supported in Azure.

        public static string ResoucessImagesDirectAddress => AzureStorageDirectAddress + "/resources";

        public static string LockersImagesDirectAddress => ResoucessImagesDirectAddress + "-lockers";
        public static string ResourcesImagesDirectAddress => ResoucessImagesDirectAddress + "-images";
        public static string WinesImagesDirectAddress => ResoucessImagesDirectAddress + "-wines";
#endif
#else
        public static readonly string DomainAPI = "azurewebsites.net";
        public static readonly string DomainAccount = "azurewebsites.net";
        public static readonly string DomainStorage = "core.windows.net";
        public static readonly string DomainCDN = "azureedge.net";

        public static readonly string SubDomainApi = "sealegs";
        public static readonly string SubDomainAccount = "sealegs";
        public static readonly string SubDomainStorage = "blob";
        public static readonly string SubDomainCDN = "sealegs";

        public static readonly string ApiDomain = SubDomainApi + "." + DomainAPI;
        public static readonly string AccountDomain = SubDomainAccount + "." + DomainAccount;
        public static readonly string StorageDomain = SubDomainStorage + "." + DomainStorage;
        public static readonly string CdnDomain = SubDomainCDN + DomainCDN;

        public static readonly string ApiOnline = "https://" + ApiDomain;
        public static readonly string AccountOnline = "https://" + AccountDomain;
        public static readonly string StorageOnline = "https://" + StorageDomain;
        public static readonly string CDNOnline = "https://" + CdnDomain;

        public static readonly string AzureStorageBlobContainer = "sealegsblobcontainer"; 

        public static readonly string AzureStorageDirectAddress = StorageOnline  + "/" + AzureStorageBlobContainer; 

        public static string ResoucessImagesDirectAddress => AzureStorageDirectAddress + "/resources";

        public static string LockersImagesDirectAddress => ResoucessImagesDirectAddress + "-lockers";
        public static string ResourcesImagesDirectAddress => ResoucessImagesDirectAddress + "-images";
        public static string WinesImagesDirectAddress => ResoucessImagesDirectAddress + "-wines";
#endif
    }


}
