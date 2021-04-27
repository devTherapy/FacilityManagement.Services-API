using FacilityManagement.Services.Data;
using FacilityManagement.Services.Data.PreSeeder;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacilityManagement.Common.Utilities.Helpers
{
    public static class PreSeederSetup
    {
        /// <summary>
        /// Configure preseeder
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public static async Task ConfigurePreSeeder(DataContext ctx,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            await ctx.Database.EnsureCreatedAsync();

            if (await userManager.Users.AnyAsync()) return;
            Seeder seeder = new Seeder(ctx, userManager, roleManager);
            // Create DefaultAdmin With Roles
            seeder.SeedAdminWithRoles().Wait();
            seeder.SeedIdentityUsers("Users.json").Wait();
            seeder.IdentityUserHasMany<Replies>("Replies.json", (user, reply) => user.Replies = reply).Wait();
            seeder.IdentityUserHasMany<Comments>("Comments.json", (user, comments) => user.Comments = comments).Wait();
            seeder.IdentityUserHasMany<Ratings>("Ratings.json", (User, ratings) => User.Ratings = ratings).Wait();
            seeder.IdentityUserHasMany<Complaint>("Complaints.json", (User, complaints) => User.Complaints = complaints).Wait();
            seeder.HasMany<Category, Complaint>("Categories.json", "Complaints.json", (c, v) => c.Complaints = v);
            seeder.HasMany<Complaint, Comments>("Complaints.json", "Comments.json", (c, v) => c.Comments = v);
            seeder.HasMany<Complaint, Ratings>("Complaints.json", "Ratings.json", (c, r) => c.Ratings = r);
            seeder.HasMany<Comments, Replies>("Comments.json", "Replies.json", (c, r) => c.Replies = r);
        }
    }
}
