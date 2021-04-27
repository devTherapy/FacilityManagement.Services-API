using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FacilityManagement.Services.Data.PreSeeder
{
    public partial class Seeder
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DataContext appDbContext;

        public Seeder(DataContext appDbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appDbContext = appDbContext;
        }

        private static List<TModel> TransformJsonToPoco<TModel>(DataContext _appDbContext,string json, bool tryLoadfromDb = true) where TModel : class
        {
            // Converts the Records in a *.json file in to a CLR types
            try
            {
                if (_appDbContext.Set<TModel>().Any() & tryLoadfromDb)
                {
                    Console.WriteLine($"Your database contains instances of {typeof(TModel)}, Using Models from Db");
                    return _appDbContext.Set<TModel>().ToList();
                }
                var path = Path.GetFullPath(@"../FacilityManagement.Services.Data/PreSeeder/data/" + json);
                List<TModel> lists = JsonConvert.DeserializeObject<List<TModel>>(File.ReadAllText(path));
                return lists;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Is the Model Added to DBset in DbContext class?");
                return null;
            }
        }

        public void HasMany<T, T1>(string one, string many, Action<dynamic, dynamic> relation) where T : class where T1 : class
        {
            //A wrapper method for exposing the HasMany class
            new _HasMany<T, T1>(one, many, appDbContext).SetFK(relation);
        }

        public void HasOne<T, T1>(string FromOne, string ToOne, Action<dynamic, dynamic> relation) where T : class where T1 : class
        {
            //A wrapper method for exposing the HasOne class
            new _HasOne<T, T1>(FromOne, ToOne, appDbContext).SetFK(relation);
    
        }

        public async Task SeedIdentityUsers(string jsonStr)
        {
            //Seeds Users into the database
            var users = TransformJsonToPoco<User>(appDbContext,jsonStr, false);
            foreach (User user in users)
            {
                var result = await userManager.CreateAsync(user, "P@ssW0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "decadev");
                }
            }
        }

        public async Task IdentityUserHasMany<T>(string many, Action<User, dynamic> predicate) where T : class
        {
            var obj = TransformJsonToPoco<T>(appDbContext,many);
            var usersList = userManager.Users.Skip(2);
            var users = usersList.ToList();
            var skip = obj.Count / users.Count;
            var cursor = 0;
            for (int i = 0; i < users.Count; i++)
            {
                User user = users[i];
                int pos = 0;
                List<T> temp = new List<T>();
                while (pos < skip)
                {
                    T t = obj[pos + cursor];
                    temp.Add(t);
                    predicate(user, temp);
                    await userManager.UpdateAsync(user);
                    pos++;
                }
                cursor += skip;
            }
        }

        public async Task SeedAdminWithRoles()
        {
            // pre-load data to roles table
            appDbContext.Database.EnsureCreated();
            if (!roleManager.Roles.Any())
            {
                var listOfRoles = new List<IdentityRole>
                {
                    new IdentityRole("admin"),
                    new IdentityRole("decadev"),
                    new IdentityRole("facility"),
                    new IdentityRole("vendor"),
                };
                foreach (var role in listOfRoles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            // pre-load data of admin type to the users table
            List<User> listOfUsers = null;
            if (!userManager.Users.Any())
            {
                listOfUsers = new List<User>
                {
                    new User{ UserName="randomuser1@sample.com",
                        Email = "randomuser1@sample.com",
                        LastName="RandomUser",
                        FirstName="James" ,
                        Gender="m",
                        PhoneNumber = "+2349876543212",
                        IsActive = true
                    },
                    new User{ UserName="randomuser2@sample.com",
                        Email = "randomuser2@sample.com",
                        LastName="RandomUser",
                        FirstName="John",
                        Gender="m",
                        PhoneNumber = "+2349876543211",
                        IsActive = true,
                    }
                };

                foreach (var user in listOfUsers)
                {
                    var result = await userManager.CreateAsync(user, "P@ssW0rd");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                }
            }
        }
    }
}