using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Collections.Generic;

namespace FacilityManagement.Services.Test.TestsForUserController
{
    class Helper
    {
        public static Response<Pagination<UserDTO>> GetPaginatedUsers
        {
            get
            {
                return new Response<Pagination<UserDTO>>
                {
                    Success = true,
                    Data = new Pagination<UserDTO>
                    {
                        CurrentPage = 1,
                        TotalNumberOfItems = 3,
                        TotalNumberOfPages = 1,
                        ItemsPerPage = 10,
                        Items = new List<UserDTO>
                        {
                            new UserDTO { FirstName = "aaa", LastName = "admin", Squad = "sq-001"},
                            new UserDTO { FirstName = "aaa", LastName = "admin", Squad = "sq-001"},
                            new UserDTO { FirstName = "aaa", LastName = "admin", Squad = "sq-001"},

                        }
                    }
                };

            }
        }

        public static Response<Pagination<UserDTO>> GetPaginatedUsersInvalid
        {
            get
            {
                return new Response<Pagination<UserDTO>>
                {
                    Success = false,
                    Data = new Pagination<UserDTO>
                    {
                        CurrentPage = 1,
                        TotalNumberOfItems = 3,
                        TotalNumberOfPages = 1,
                        ItemsPerPage = 10,
                        Items = null
                    }
                };

            }
        }
    }
}
