namespace CEGIS_Project_ProgressApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FullName = c.String(maxLength: 8000, unicode: false),
                        ShortName = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProjectInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DivisionId = c.Int(nullable: false),
                        Client = c.String(maxLength: 8000, unicode: false),
                        FocalPerson = c.String(maxLength: 8000, unicode: false),
                        ProgressTypeId = c.Int(nullable: false),
                        ContactValue = c.Double(nullable: false),
                        ProjectTypeId = c.Int(nullable: false),
                        Probility = c.Double(nullable: false),
                        Duration = c.Int(nullable: false),
                        ExpectedDateId = c.Int(nullable: false),
                        DateTimes = c.DateTime(nullable: false),
                        UserInitial = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .ForeignKey("dbo.ExpectedDates", t => t.ExpectedDateId, cascadeDelete: true)
                .ForeignKey("dbo.ProgressTypes", t => t.ProgressTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId, cascadeDelete: true)
                .Index(t => t.DivisionId)
                .Index(t => t.ProgressTypeId)
                .Index(t => t.ProjectTypeId)
                .Index(t => t.ExpectedDateId);
            
            CreateTable(
                "dbo.ExpectedDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Dates = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProgressTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Progress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 8000, unicode: false),
                        TypeFullName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UpdateHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        ProgressTypeId = c.Int(nullable: false),
                        UpdatedBy = c.String(),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectInfoes", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Initial = c.String(),
                        UserType = c.Int(nullable: false),
                        DivisionId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UpdateHistories", "ProjectId", "dbo.ProjectInfoes");
            DropForeignKey("dbo.ProjectInfoes", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.ProjectInfoes", "ProgressTypeId", "dbo.ProgressTypes");
            DropForeignKey("dbo.ProjectInfoes", "ExpectedDateId", "dbo.ExpectedDates");
            DropForeignKey("dbo.ProjectInfoes", "DivisionId", "dbo.Divisions");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.UpdateHistories", new[] { "ProjectId" });
            DropIndex("dbo.ProjectInfoes", new[] { "ExpectedDateId" });
            DropIndex("dbo.ProjectInfoes", new[] { "ProjectTypeId" });
            DropIndex("dbo.ProjectInfoes", new[] { "ProgressTypeId" });
            DropIndex("dbo.ProjectInfoes", new[] { "DivisionId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UpdateHistories");
            DropTable("dbo.ProjectTypes");
            DropTable("dbo.ProgressTypes");
            DropTable("dbo.ExpectedDates");
            DropTable("dbo.ProjectInfoes");
            DropTable("dbo.Divisions");
        }
    }
}
