using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.Model.Extensions
{
    public static  class ChillRequestExtensions
    {
        public static IEnumerable<Grouping<string, RemoteChillRequestExteneded>> GroupByName(this IEnumerable<RemoteChillRequestExteneded> chillRequests)
        {           
            return from request in chillRequests orderby request.Locker.DisplayName
                   group request by request.Locker.DisplayName
                   into requestGroup
                   select new Grouping<string, RemoteChillRequestExteneded>(requestGroup.Key, requestGroup);
        }
    }
}
