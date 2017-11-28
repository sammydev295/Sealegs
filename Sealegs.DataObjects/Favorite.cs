using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    /// <summary>
    /// This is per user
    /// </summary>
    public class Favorite : BaseDataObject
    {
        public const String Locker = "locker";
        public const String Wine = "wine";

        public string UserId { get; set; }
        public string FavoriteTypeId { get; set; }
        public string FavoriteType { get; set; }
    }
}