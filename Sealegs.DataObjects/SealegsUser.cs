using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sealegs.DataObjects
{
    /// <summary>
    /// http://www.entityframeworktutorial.net/code-first/configure-entity-mappings-using-fluent-api.aspx
    /// </summary>
	public class SealegsUser : BaseDataObject
	{
#if BACKEND
        [Index("IdxUserID")]   
#endif
        public Guid UserID { get; set; }


        [StringLength(256)]
#if BACKEND
        [Index("IdxEmail")]
#endif
        public string Email { get; set; }

		[StringLength(256)]
        public string FirstName { get; set; }

		[StringLength(256)]
        public string LastName { get; set; }

		[StringLength(128)]
        public string Password { get; set; }

		[StringLength(128)]
        public string PasswordSalt { get; set; }

        public int PasswordFormat { get; set; }

        public SealegsUserRole Role { get; set; }

        public bool IsApproved { get; set; }

		public bool IsAnnonymous { get; set; }

		public DateTime? LastLoginDate { get; set; }

        public string AvatarImage { get; set; }
    }
}
