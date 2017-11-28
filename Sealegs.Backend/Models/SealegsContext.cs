using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Sealegs.DataObjects;
using System.Collections.Generic;

namespace Sealegs.Backend.Models
{
    public class SealegsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public SealegsContext() : base(connectionStringName)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<FeaturedEvent> FeaturedEvent { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<LockerMember> LockerMember { get; set; }

        public DbSet<LockerType> LockerType { get; set; }

        public DbSet<MemberFriend> MemberFriend { get; set; }

        public DbSet<MemberSuggestion> MemberSuggestion { get; set; }

        public DbSet<MiniHack> MiniHacks { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<RemoteChillRequest> RemoteChillRequest { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<SealegsUserRole> SealegsUserRole { get; set; }

        public DbSet<SealegsUser> SealegsUser { get; set; }

        public DbSet<Wine> Wine { get; set; }

        public DbSet<WineVarietal> WineVarietal { get; set; }

        public DbSet<News> News { get; set; }
    }

}
