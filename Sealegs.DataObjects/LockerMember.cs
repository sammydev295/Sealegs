using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class LockerMember : BaseDataObject
    {
        [Display(Name = "LockerMemberID")]
        public String LockerMemberID { get; set; }

        [Display(Name = "DisplayName")]
        public string DisplayName { get; set; }

        //[Display(Name = "Role")]
        //public SealegsUserRole Role { get; set; }

        [Display(Name = "MemberName")]
        public string MemberName { get; set; }

        [Display(Name = "UserID")]
        public String UserID { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }

        [Display(Name = "HomePhone")]
        public string HomePhone { get; set; }

        [Display(Name = "WorkPhone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }

        [Display(Name = "EmailAddress2")]
        public string EmailAddress2 { get; set; }

        [Display(Name = "LockerTypeID")]
        public String LockerTypeID { get; set; }

        [Display(Name = "CreditCardNumber")]
        public string CreditCardNumber { get; set; }

        [Display(Name = "CreditCardType")]
        public string CreditCardType { get; set; }

        [Display(Name = "ExpireyDate")]
        public DateTime? ExpireyDate { get; set; }

        [Display(Name = "NameOnCard")]
        public string NameOnCard { get; set; }

        [Display(Name = "SecurityCode")]
        public string SecurityCode { get; set; }

        [Display(Name = "TileColor")]
        public string TileColor { get; set; }

        string _profilePicture;
        [Display(Name = "ProfilePicture")]
        public string ProfilePicture
        {
            get
            {
                return _profilePicture;
            }
            set
            {
                if (_profilePicture == value)
                    return;

                _profilePicture = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }


        [Display(Name = "EmailAlerts")]
        public bool? EmailAlerts { get; set; }

        [Display(Name = "InventoryAlerts")]
        public bool? InventoryAlerts { get; set; }

        [Display(Name = "LastCheckedOutDate")]
        public DateTime? LastCheckedOutDate { get; set; }

        [Display(Name = "NoOfBottles")]
        public int? NoOfBottles { get; set; }

        [Display(Name = "CurrentRenewalDate")]
        public DateTime? CurrentRenewalDate { get; set; }

        [Display(Name = "NextRenewalDate")]
        public DateTime? NextRenewalDate { get; set; }

        string _profileImage;
        [Display(Name = "ProfileImage")]
        public string ProfileImage
        {
            get
            {
                return _profileImage;
            }
            set
            {
                if (_profileImage == value)
                    return;

                _profileImage = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        [Display(Name = "SignatureImage")]
        public string SignatureImage { get; set; }

        [Display(Name = "FacebookID")]
        public string FacebookID { get; set; }

        [Display(Name = "FacebookUserID")]
        public string FacebookUserID { get; set; }

        [Display(Name = "FacebookAccessToken")]
        public string FacebookAccessToken { get; set; }

        [Display(Name = "FacebookTokenExpired")]
        public bool? FacebookTokenExpired { get; set; }

        [Display(Name = "FacebookTokenExpirationDate")]
        public DateTime? FacebookTokenExpiretionDate { get; set; }

        [Display(Name = "TwitterID")]
        public string TwitterID { get; set; }

        [Display(Name = "OAuthToken")]
        public string OAuthToken { get; set; }

        [Display(Name = "AccessToken")]
        public string AccessToken { get; set; }

        [Display(Name = "IsAllowFacebookUpdate")]
        public bool? IsAllowFacebookUpdate { get; set; }

        [Display(Name = "IsAllowTwitterUpdate")]
        public bool? IsAllowTwitterUpdate { get; set; }

        [Display(Name = "Promotions")]
        public bool? Promotions { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "IsStaff")]
        public bool? IsStaff { get; set; }

        bool? _isFavorite = false;
        [Display(Name = "IsFavorite")]
        public bool? IsFavorite
        {
            get
            {
                return _isFavorite;
            }
            set
            {
                if (_isFavorite == value)
                    return;

                _isFavorite = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        bool? _isActive = false;
        [Display(Name = "IsActive")]
        public bool? IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive == value)
                    return;

                _isActive = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        #region LockerImageUri

        [Newtonsoft.Json.JsonIgnore]
        public const string DefaultProfileImage = "profile_generic.png";

        #endregion

        [Newtonsoft.Json.JsonIgnore]
        public static LockerMember Defaults => new LockerMember() { Id = new Guid().ToString(), IsActive = true, IsFavorite = false, InventoryAlerts = true, EmailAlerts = true, IsStaff = false, IsAllowFacebookUpdate = false, IsAllowTwitterUpdate = false, ProfileImage = DefaultProfileImage };
    }

    public class LockerMemberShort
    {
        public System.Guid LockerMemberID { get; set; }

        public string DisplayName { get; set; }

        public string MemberName { get; set; }

        public bool IsActive { get; set; }
    }
}