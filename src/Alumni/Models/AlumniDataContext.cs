using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Alumni.Models
{
    public partial class AlumniDataContext : DbContext
    {
        public AlumniDataContext(DbContextOptions<AlumniDataContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserRoles_UserId");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<DBChatMessage>(entity =>
            {
                entity.HasKey(e => e.MessageID)
                    .HasName("PK_DBChatMessage");

                entity.Property(e => e.Color).HasColumnType("varchar(50)");

                entity.Property(e => e.Text).HasColumnType("varchar(500)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.DBChatMessage)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_DBChatMessage_DBChatRoom");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.DBChatMessageToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK_DBChatMessage_DBUser1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DBChatMessageUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_DBChatMessage_DBUser");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.PostID)
                    .HasName("PK_Post");                

                entity.Property(e => e.Message).HasColumnType("varchar(500)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Post_DBUser");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentID)
                    .HasName("PK_Comment");

                entity.Property(e => e.Message).HasColumnType("varchar(500)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Comment_DBUser");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Comment_Post");
            });

            modelBuilder.Entity<DBChatRoom>(entity =>
            {
                entity.HasKey(e => e.RoomID)
                    .HasName("PK_DBChatRoom");

                entity.Property(e => e.Name).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<DBDegree>(entity =>
            {
                entity.HasKey(e => e.DegreeID)
                    .HasName("PK_DBDegree");

                entity.Property(e => e.DegreeName).HasColumnType("varchar(50)");

                entity.Property(e => e.DegreeType).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<DBLoggedInUser>(entity =>
            {
                entity.HasKey(e => e.LoggedInUserID)
                    .HasName("PK_DBLoggedInUser");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.DBLoggedInUser)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_DBLoggedInUser_DBChatRoom");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DBLoggedInUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_DBLoggedInUser_DBUser");

                entity.Property(e => e.ConnectionId).HasColumnType("nvarchar(50)").HasMaxLength(450);
            });

            modelBuilder.Entity<DBPrivateMessage>(entity =>
            {
                entity.HasKey(e => e.PrivateMessageID)
                    .HasName("PK_DBPrivateMessage");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.DBPrivateMessageToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK_DBPrivateMessage_DBUser1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DBPrivateMessageUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_DBPrivateMessage_DBUser");

                entity.Property(e => e.Color).HasColumnType("varchar(50)");

                entity.Property(e => e.Text).HasColumnType("varchar(500)");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<DBUser>(entity =>
            {
                entity.HasKey(e => e.UserID)
                    .HasName("PK_DBPerson");

                entity.Property(e => e.Adress).HasColumnType("varchar(150)");

                entity.Property(e => e.AspNetUser).HasMaxLength(450);

                entity.Property(e => e.CNP).HasColumnType("varchar(50)");

                entity.Property(e => e.City).HasColumnType("varchar(50)");

                entity.Property(e => e.Country).HasColumnType("varchar(50)");

                entity.Property(e => e.Email).HasColumnType("varchar(50)");

                entity.Property(e => e.Sex).HasColumnType("varchar(50)");

                entity.Property(e => e.Telephone_Number).HasColumnType("varchar(50)");

                entity.Property(e => e.fName).HasColumnType("varchar(50)");

                entity.Property(e => e.lName).HasColumnType("varchar(50)");

                entity.HasOne(d => d.AspNetUserNavigation)
                    .WithMany(p => p.DBUser)
                    .HasForeignKey(d => d.AspNetUser)
                    .HasConstraintName("FK_DBUser_AspNetUsers");

                entity.HasOne(d => d.Degree)
                    .WithMany(p => p.DBUser)
                    .HasForeignKey(d => d.DegreeId)
                    .HasConstraintName("FK_DBUser_DBDegree");
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.Property(e => e.DateEarned).HasColumnType("date");

                entity.Property(e => e.EducationDescription).HasColumnType("varchar(255)");

                entity.Property(e => e.EducationTitle).HasColumnType("varchar(150)");

                entity.Property(e => e.EducationType).HasColumnType("varchar(100)");

                entity.Property(e => e.GrantingInstitution).HasColumnType("varchar(200)");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.City).HasColumnType("varchar(150)");

                entity.Property(e => e.Country)
                    .HasColumnType("varchar(150)")
                    .HasDefaultValueSql("'Romania'");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.StateOrProvince).HasColumnType("varchar(200)");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationName).HasColumnType("varchar(100)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Publication>(entity =>
            {
                entity.Property(e => e.PublicationDescription).HasColumnType("varchar(255)");

                entity.Property(e => e.PublicationLocation).HasColumnType("varchar(200)");

                entity.Property(e => e.PublicationTopic).HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Reference>(entity =>
            {
                entity.Property(e => e.ReferenceDetail).HasColumnType("varchar(255)");

                entity.Property(e => e.ReferenceType).HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.SkillArea).HasColumnType("varchar(150)");

                entity.Property(e => e.SkillDescription).HasColumnType("varchar(255)");

                entity.Property(e => e.SkillName).HasColumnType("varchar(150)");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<UserCVLink>(entity =>
            {
                entity.HasOne(d => d.Education)
                    .WithMany(p => p.UserCVLink)
                    .HasForeignKey(d => d.EducationId)
                    .HasConstraintName("FK_UserCVLink_Education");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.UserCVLink)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_UserCVLink_Location");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.UserCVLink)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_UserCVLink_Organization");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.UserCVLink)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_UserCVLink_Publication");

                entity.HasOne(d => d.Referenece)
                    .WithMany(p => p.UserCVLink)
                    .HasForeignKey(d => d.RefereneceId)
                    .HasConstraintName("FK_UserCVLink_Reference");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.UserCVLink)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("FK_UserCVLink_Skill");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCVLink)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserCVLink_DBUser");
            });

            modelBuilder.Entity<sysdiagrams>(entity =>
            {
                entity.HasKey(e => e.diagram_id)
                    .HasName("PK__sysdiagr__C2B05B61762C64D2");

                entity.HasIndex(e => new { e.principal_id, e.name })
                    .HasName("UK_principal_name")
                    .IsUnique();

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnType("sysname");
            });
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<DBChatMessage> DBChatMessage { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<DBChatRoom> DBChatRoom { get; set; }
        public virtual DbSet<DBDegree> DBDegree { get; set; }
        public virtual DbSet<DBLoggedInUser> DBLoggedInUser { get; set; }
        public virtual DbSet<DBPrivateMessage> DBPrivateMessage { get; set; }
        public virtual DbSet<DBUser> DBUser { get; set; }
        public virtual DbSet<Education> Education { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Publication> Publication { get; set; }
        public virtual DbSet<Reference> Reference { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<UserCVLink> UserCVLink { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}