using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using YHRSys.Models;
using Microsoft.AspNet.Identity;
namespace YHRSys.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private const string InitialUserName = "kenora";
        private const string InitialUserFirstName = "Kenneth";
        private const string InitialUserLastName = "Oraegbunam";
        private const string InitialUserEmail = "k.oraegbunam@cgiar.org";
        private const string InitialUserPassword = "test11";
        //private const string VaniallaUserName = "VanillaUser";
        //private const string VaniallaUserFirstName = "VanillaFirstName";
        //private const string VaniallaUserLastName = "VanillaLastName";
        //private const string VaniallaUserEmail = "vanilla@test.com";
        //private const string VaniallaUserPassword = "Password1";
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly string[] _groupAdminRoleNames = { "CanEditUser", "CanEditGroup", "User" };
        private readonly IdentityManager _idManager = new IdentityManager();
        private readonly string[] _initialGroupNames = { "SuperAdmins", "GroupAdmins", "UserAdmins", "Users" };
        private readonly string[] _superAdminRoleNames = { "Admin", "CanEditUser", "CanEditGroup", "CanEditRole", "User", "CanEditLocation", "CanAddLocation", "CanDeleteLocation", "CanViewLocation", "CanEditLocationUser", "CanAddLocationUser", "CanDeleteLocationUser", "CanViewLocationUser", "CanEditActivity", "CanAddActivity", "CanDeleteActivity", "CanViewActivity", "CanEditInventory", "CanAddInventory", "CanDeleteInventory", "CanViewInventory", "CanEditMediumPrepType", "CanAddMediumPrepType", "CanDeleteMediumPrepType", "CanViewMediumPrepType", "CanEditPartner", "CanAddPartner", "CanDeletePartner", "CanViewPartner", "CanEditPartnerActivity", "CanAddPartnerActivity", "CanDeletePartnerActivity", "CanViewPartnerActivity", "CanEditPartnerContactPerson", "CanAddPartnerContactPerson", "CanDeletePartnerContactPerson", "CanViewPartnerContactPerson", "CanEditReagent", "CanAddReagent", "CanDeleteReagent", "CanViewReagent", "CanEditVariety", "CanAddVariety", "CanDeleteVariety", "CanViewVariety", "CanEditVarietyProcessFlow", "CanAddVarietyProcessFlow", "CanDeleteVarietyProcessFlow", "CanViewVarietyProcessFlow", "CanEditActivityDefinitions", "CanAddActivityDefinitions", "CanDeleteActivityDefinitions", "CanViewActivityDefinitions", "CanEditVarietyDefinitions", "CanAddVarietyDefinitions", "CanDeleteVarietyDefinitions", "CanViewVarietyDefinitions", "CanEditSpecies", "CanAddSpecies", "CanDeleteSpecies", "CanViewSpecies", "CanEditInternalReagentUsage", "CanAddInternalReagentUsage", "CanDeleteInternalReagentUsage", "CanViewInternalReagentUsage", "CanEditActivityWorkplan", "CanAddActivityWorkplan", "CanDeleteActivityWorkplan", "CanViewActivityWorkplan", "CanEditActivityAchievement", "CanAddActivityAchievement", "CanDeleteActivityAchievement", "CanViewActivityAchievement", "CanAddWeeklyActivityLog", "CanEditWeeklyActivityLog", "CanDeleteWeeklyActivityLog", "CanViewWeeklyActivityLog", "CanAddSiteContents", "CanEditSiteContents", "CanDeleteSiteContents", "CanViewSiteContents", "CanAddForms", "CanEditForms", "CanDeleteForms", "CanViewForms", "CanAddRanks", "CanEditRanks", "CanDeleteRanks", "CanViewRanks", "CanAddOrder", "CanEditOrder", "CanDeleteOrder", "CanViewOrder", "CanAddSeedling", "CanEditSeedling", "CanDeleteSeedling", "CanViewSeedling", "CanAddReport", "CanEditReport", "CanDeleteReport", "CanViewReport", "CanViewCummulativeReport" };
        private readonly string[] _userAdminRoleNames = { "CanEditUser", "User" };
        private readonly string[] _userRoleNames = { "User" };
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(ApplicationDbContext context)
        {
            AddGroups();
            AddRoles();
            AddUsers();
            AddRolesToGroups();
            AddUsersToGroups();
        }
        public void AddGroups()
        {
            foreach (var groupName in _initialGroupNames)
            {
                try
                {
                    _idManager.CreateGroup(groupName);
                }
                catch (GroupExistsException)
                {
                    // intentionally catched for seeding
                }
            }
        }
        private void AddRoles()
        {
            // Some example initial roles. These COULD BE much more granular:
            _idManager.CreateRole("Admin", "Global Access");
            _idManager.CreateRole("CanEditUser", "Add, modify, and delete Users");
            _idManager.CreateRole("CanEditGroup", "Add, modify, and delete Groups");
            _idManager.CreateRole("CanEditRole", "Add, modify, and delete roles");
            _idManager.CreateRole("User", "Restricted to business domain activity");
            _idManager.CreateRole("CanEditLocation", "Modify Locations");
            _idManager.CreateRole("CanAddLocation", "Add Locations");
            _idManager.CreateRole("CanDeleteLocation", "Delete Locations");
            _idManager.CreateRole("CanViewLocation", "View Location details");
            _idManager.CreateRole("CanEditLocationUser", "Modify Location OiCs");
            _idManager.CreateRole("CanAddLocationUser", "Add Location OiCs");
            _idManager.CreateRole("CanDeleteLocationUser", "Delete Location OiCs");
            _idManager.CreateRole("CanViewLocationUser", "View Location OiC details");
            _idManager.CreateRole("CanEditActivity", "Modify Activity");
            _idManager.CreateRole("CanAddActivity", "Add Activity");
            _idManager.CreateRole("CanDeleteActivity", "Delete Activity");
            _idManager.CreateRole("CanViewActivity", "View Activity");
            _idManager.CreateRole("CanEditInventory", "Modify Inventory");
            _idManager.CreateRole("CanAddInventory", "Add Inventory");
            _idManager.CreateRole("CanDeleteInventory", "Delete Inventory");
            _idManager.CreateRole("CanViewInventory", "View Inventory");
            _idManager.CreateRole("CanEditMediumPrepType", "Modify MediumPrepType");
            _idManager.CreateRole("CanAddMediumPrepType", "Add MediumPrepType");
            _idManager.CreateRole("CanDeleteMediumPrepType", "Delete MediumPrepType");
            _idManager.CreateRole("CanViewMediumPrepType", "View MediumPrepType");
            _idManager.CreateRole("CanEditPartner", "Modify Partner");
            _idManager.CreateRole("CanAddPartner", "Add Partner");
            _idManager.CreateRole("CanDeletePartner", "Delete Partner");
            _idManager.CreateRole("CanViewPartner", "View Partner");
            _idManager.CreateRole("CanEditPartnerActivity", "Modify Partner Activity");
            _idManager.CreateRole("CanAddPartnerActivity", "Add Partner Activity");
            _idManager.CreateRole("CanDeletePartnerActivity", "Delete Partner Activity");
            _idManager.CreateRole("CanViewPartnerActivity", "View Partner Activity");
            _idManager.CreateRole("CanEditPartnerContactPerson", "Modify Partner Contact Person");
            _idManager.CreateRole("CanAddPartnerContactPerson", "Add Partner Contact Person");
            _idManager.CreateRole("CanDeletePartnerContactPerson", "Delete Partner Contact Person");
            _idManager.CreateRole("CanViewPartnerContactPerson", "View Partner Contact Person");
            _idManager.CreateRole("CanEditReagent", "Modify Reagent");
            _idManager.CreateRole("CanAddReagent", "Add Reagent");
            _idManager.CreateRole("CanDeleteReagent", "Delete Reagent");
            _idManager.CreateRole("CanViewReagent", "View Reagent");
            _idManager.CreateRole("CanEditVariety", "Modify Variety");
            _idManager.CreateRole("CanAddVariety", "Add Variety");
            _idManager.CreateRole("CanDeleteVariety", "Delete Variety");
            _idManager.CreateRole("CanViewVariety", "View Variety");
            _idManager.CreateRole("CanEditVarietyProcessFlow", "Modify Variety Process Flow");
            _idManager.CreateRole("CanAddVarietyProcessFlow", "Add Variety Process Flow");
            _idManager.CreateRole("CanDeleteVarietyProcessFlow", "Delete Variety Process Flow");
            _idManager.CreateRole("CanViewVarietyProcessFlow", "View Variety Process Flow");
            _idManager.CreateRole("CanEditActivityDefinitions", "Modify Activity Definition");
            _idManager.CreateRole("CanAddActivityDefinitions", "Add Activity Definition");
            _idManager.CreateRole("CanDeleteActivityDefinitions", "Delete Activity Definition");
            _idManager.CreateRole("CanViewActivityDefinitions", "View Activity Definition");

            _idManager.CreateRole("CanEditVarietyDefinitions", "Modify Variety Definition");
            _idManager.CreateRole("CanAddVarietyDefinitions", "Add Variety Definition");
            _idManager.CreateRole("CanDeleteVarietyDefinitions", "Delete Variety Definition");
            _idManager.CreateRole("CanViewVarietyDefinitions", "View Variety Definition");

            _idManager.CreateRole("CanEditSpecies", "Modify Species");
            _idManager.CreateRole("CanAddSpecies", "Add Species");
            _idManager.CreateRole("CanDeleteSpecies", "Delete Species");
            _idManager.CreateRole("CanViewSpecies", "View Species");

            _idManager.CreateRole("CanEditInternalReagentUsage", "Modify Internal Reagent Usage");
            _idManager.CreateRole("CanAddInternalReagentUsage", "Add Internal Reagent Usage");
            _idManager.CreateRole("CanDeleteInternalReagentUsage", "Delete Internal Reagent Usage");
            _idManager.CreateRole("CanViewInternalReagentUsage", "View Internal Reagent Usage");

            _idManager.CreateRole("CanEditWeeklyActivityLog", "Modify Weekly Activity Log");
            _idManager.CreateRole("CanAddWeeklyActivityLog", "Add Weekly Activity Log");
            _idManager.CreateRole("CanDeleteWeeklyActivityLog", "Delete Weekly Activity Log");
            _idManager.CreateRole("CanViewWeeklyActivityLog", "View Weekly Activity Log");

            _idManager.CreateRole("CanEditSiteContents", "Modify Site Contents");
            _idManager.CreateRole("CanAddSiteContents", "Add/Publish Site Contents");
            _idManager.CreateRole("CanDeleteSiteContents", "Delete Site Contents");
            _idManager.CreateRole("CanViewSiteContents", "View Site Contents");


            _idManager.CreateRole("CanEditActivityWorkplan", "Modify Activity Workplans");
            _idManager.CreateRole("CanAddActivityWorkplan", "Add Activity Workplans");
            _idManager.CreateRole("CanDeleteActivityWorkplan", "Delete Activity Workplans");
            _idManager.CreateRole("CanViewActivityWorkplan", "View Activity Workplans");

            _idManager.CreateRole("CanEditActivityAchievement", "Modify Activity Achievements");
            _idManager.CreateRole("CanAddActivityAchievement", "Add/Publish Activity Achievements");
            _idManager.CreateRole("CanDeleteActivityAchievement", "Delete Activity Achievements");
            _idManager.CreateRole("CanViewActivityAchievement", "View Activity Achievements");

            _idManager.CreateRole("CanEditForms", "Modify Forms");
            _idManager.CreateRole("CanAddForms", "Add Forms");
            _idManager.CreateRole("CanDeleteForms", "Delete Forms");
            _idManager.CreateRole("CanViewForms", "View Forms");

            _idManager.CreateRole("CanEditRanks", "Modify Ranks");
            _idManager.CreateRole("CanAddRanks", "Add Ranks");
            _idManager.CreateRole("CanDeleteRanks", "Delete Ranks");
            _idManager.CreateRole("CanViewRanks", "View Ranks");

            _idManager.CreateRole("CanEditOrder", "Modify Variety Order");
            _idManager.CreateRole("CanAddOrder", "Add Variety Order");
            _idManager.CreateRole("CanDeleteOrder", "Delete Variety Ranks");
            _idManager.CreateRole("CanViewOrder", "View Variety Order");

            _idManager.CreateRole("CanEditSeedling", "Modify Seedling");
            _idManager.CreateRole("CanAddSeedling", "Add Seedling");
            _idManager.CreateRole("CanDeleteSeedling", "Delete Seedling");
            _idManager.CreateRole("CanViewSeedling", "View Seedling");

            _idManager.CreateRole("CanEditReport", "Modify Report");
            _idManager.CreateRole("CanAddReport", "Add Report");
            _idManager.CreateRole("CanDeleteReport", "Delete Report");
            _idManager.CreateRole("CanViewReport", "View Report");

            _idManager.CreateRole("CanEditOwnPartner", "Modify Own Partner");
            _idManager.CreateRole("CanAddOwnPartner", "Add Own Partner");
            _idManager.CreateRole("CanDeleteOwnPartner", "Delete Own Partner");
            _idManager.CreateRole("CanViewOwnPartner", "View Own Partner");

            _idManager.CreateRole("CanEditOwnPartnerContact", "Modify Own Partner Contact");
            _idManager.CreateRole("CanAddOwnPartnerContact", "Add Own Partner Contact");
            _idManager.CreateRole("CanDeleteOwnPartnerContact", "Delete Own Partner Contact");
            _idManager.CreateRole("CanViewOwnPartnerContact", "View Own Partner Contact");

            _idManager.CreateRole("CanEditOwnPartnerActivity", "Modify Own Partner Activity");
            _idManager.CreateRole("CanAddOwnPartnerActivity", "Add Own Partner Activity");
            _idManager.CreateRole("CanDeleteOwnPartnerActivity", "Delete Own Partner Activity");
            _idManager.CreateRole("CanViewOwnPartnerActivity", "View Own Partner Activity");

            _idManager.CreateRole("CanEditOwnReport", "Modify Own Partner Activity Report");
            _idManager.CreateRole("CanAddOwnReport", "Add Own Partner Activity Report");
            _idManager.CreateRole("CanDeleteOwnReport", "Delete Own Partner Activity Report");
            _idManager.CreateRole("CanViewOwnReport", "View Own Partner Activity Report");

            _idManager.CreateRole("CanViewCummulativeReport", "View Cummulative Partner Activity Reports");

        }
        private void AddRolesToGroups()
        {
            // Add the Super-Admin Roles to the Super-Admin Group:
            IDbSet<Group> allGroups = _db.Groups;
            Group superAdmins = allGroups.First(g => g.Name == "SuperAdmins");
            foreach (string name in _superAdminRoleNames)
            {
                _idManager.AddRoleToGroup(superAdmins.Id, name);
            }
            // Add the Group-Admin Roles to the Group-Admin Group:
            Group groupAdmins = _db.Groups.First(g => g.Name == "GroupAdmins");
            foreach (string name in _groupAdminRoleNames)
            {
                _idManager.AddRoleToGroup(groupAdmins.Id, name);
            }
            // Add the User-Admin Roles to the User-Admin Group:
            Group userAdmins = _db.Groups.First(g => g.Name == "UserAdmins");
            foreach (string name in _userAdminRoleNames)
            {
                _idManager.AddRoleToGroup(userAdmins.Id, name);
            }
            // Add the User Roles to the Users Group:
            Group users = _db.Groups.First(g => g.Name == "Users");
            foreach (string name in _userRoleNames)
            {
                _idManager.AddRoleToGroup(users.Id, name);
            }
        }
        // Change these to your own:
        private void AddUsers()
        {
            var newUser = new ApplicationUser
            {
                UserName = InitialUserName,
                FirstName = InitialUserFirstName,
                LastName = InitialUserLastName,
                Email = InitialUserEmail
            };
            // Be careful here - you will need to use a password which will
            // be valid under the password rules for the application,
            // or the process will abort:
            var userCreationResult = _idManager.CreateUser(newUser, InitialUserPassword);
            if (!userCreationResult.Succeeded)
            {
                // warn the user that it's seeding went wrong
                throw new DbEntityValidationException("Could not create InitialUser because: " + String.Join(", ", userCreationResult.Errors));
            }
            //var newVaniallaUser = new ApplicationUser
            //{
            //    UserName = VaniallaUserName,
            //    FirstName = VaniallaUserFirstName,
            //    LastName = VaniallaUserLastName,
            //    Email = VaniallaUserEmail
            //};
            // Be careful here - you will need to use a password which will
            // be valid under the password rules for the application,
            // or the process will abort:
            //userCreationResult = _idManager.CreateUser(newVaniallaUser, VaniallaUserPassword);
            //if (!userCreationResult.Succeeded)
            //{
                // warn the user that it's seeding went wrong
            //    throw new DbEntityValidationException("Could not create VaniallaUser because: " + String.Join(", ", userCreationResult.Errors));
            //}
        }
        // Configure the initial Super-Admin user:
        private void AddUsersToGroups()
        {
            Console.WriteLine(String.Join(", ", _db.Users.Select(u => u.Email)));
            ApplicationUser user = _db.Users.First(u => u.UserName == InitialUserName);
            IDbSet<Group> allGroups = _db.Groups;
            foreach (Group group in allGroups)
            {
                _idManager.AddUserToGroup(user.Id, group.Id);
            }
            //user = _db.Users.FirstOrDefault(u => u.UserName == VaniallaUserName);
            //var plainUsers = allGroups.FirstOrDefault(g => g.Name == "Users");
            //_idManager.AddUserToGroup(user.Id, plainUsers.Id);
        }
    }
}