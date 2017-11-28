using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sealegs.DataObjects
{
    /// <summary>
    /// http://www.entityframeworktutorial.net/code-first/configure-entity-mappings-using-fluent-api.aspx
    /// </summary>
    public class SealegsUserRole : BaseDataObject
    {
        public const string AdminRoleId = "C5AEB9A8-5E2F-475B-910E-A0DD068D38C7";
        public const string AdminRole = "Admin";
        public static Guid Admin = new Guid(AdminRoleId);

        public const string BartenderRoleId = "264C3808-216F-4E62-8459-8DFC0885BE33";
        public const string BartenderRole = "Bartender";
        public static Guid Bartender = new Guid(BartenderRoleId);

        public const string CustomerRoleId =  "934B07D1-71FC-4528-B880-11EEFFF4B4D1";
        public const string CustomerRole = "Customer";
        public static Guid Customer = new Guid(CustomerRoleId);

        public const string ManagerRoleId = "36A301B4-6F66-4C44-8E19-65B39CE2544C";
        public const string ManagerRole = "Manager";
        public static Guid Manager = new Guid(ManagerRoleId);

        public const string ServerRoleId = "DCA9FCEA-2888-49F9-98A0-ABE0C96C4CE9";
        public const string ServerRole = "Server";
        public static Guid Server = new Guid(ServerRoleId);

        public const string EmployeeRoleId = "DA73B2EA-C554-4D3C-A9A6-81EBF689BB3C";
        public const string EmployeeRole = "Employee";
        public static Guid Employee = new Guid(EmployeeRoleId);

        public const string LockerMemberRoleId = "B152B160-D7A0-4EC0-9C71-B7F389FC7986";
        public const string LockerMemberRole = "LockerMember";
        public static Guid LockerMember = new Guid(LockerMemberRoleId);

        public const string LockerMemberFriendRoleId = "0BC79E2A-D31E-42E7-9F5A-37D443222412";
        public const string LockerMemberFriendRole = "LockerMemberFriend";
        public static Guid LockerMemberFriend = new Guid(LockerMemberFriendRoleId);

        public static SealegsUserRole[] RoleHierarchy = new SealegsUserRole[] 
                                                        {
                                                            new SealegsUserRole() { RoleName = AdminRole, Id = AdminRoleId },
                                                            new SealegsUserRole() { RoleName = ManagerRole, Id = ManagerRoleId },
                                                            new SealegsUserRole() { RoleName = ServerRole, Id = ServerRoleId },
                                                            new SealegsUserRole() { RoleName = EmployeeRole, Id = EmployeeRoleId },
                                                            new SealegsUserRole() { RoleName = LockerMemberRole, Id = LockerMemberRoleId },
                                                            new SealegsUserRole() { RoleName = LockerMemberFriendRole, Id = LockerMemberFriendRoleId },
                                                            new SealegsUserRole() { RoleName = CustomerRole, Id = CustomerRoleId }
                                                        };

        /// <summary>
        /// Gets or sets the role name.
        /// Such as Bartender or Customer....
        /// </summary>
        /// <value>The role name.</value>
        String _roleName = String.Empty;
        public String RoleName
        {
            get { return _roleName; }
            set
            {
                if (_roleName == value)
                    return;

                _roleName = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        /// <summary>
        /// Gets or sets the role description.
        /// </summary>
        /// <value>The description.</value>
        string _description = String.Empty;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                _description = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

#if MOBILE

        bool _isSelected = false;
        
        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                OnPropertyChanged();
            }
        }

#endif
    }
}