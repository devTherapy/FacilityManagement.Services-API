using FacilityManagement.Services.Models;
using System.Collections.Generic;

namespace FacilityManagement.Services.Test.Helpers
{
    public class ModelReturnHelper
    {
        public static List<Category> ReturnCategories()
        {
            return new List<Category>
            {
               new Category
               {
                   Description ="aenean fermentum donec ut mauris eget massa tempor convallis nulla",
                   Name = "Food",
                   Id = "abcdef"
               },
               new Category
               {
                   Description = "maecenas ut massa quis augue luctus tincidunt nulla mollis molestie lorem quisque ut erat curabitur gravida nisi at nibh in",
                   Name = "Facility"
               },

            };
        }

        public static Complaint ReturnComplaint()
        {
            return
                new Complaint
                {
                    Question = "Digitized upward-trending adapter",
                    Type = "View to a Kill, A",
                    Image = "http://dummyimage.com/134x188.jpg/5fa2dd/ffffff",
                    IsTask = false,
                    Comments = new List<Comments>
                    {
                    new Comments
                    {
                        Comment = "semper porta volutpat quam pede lobortis ligula sit amet eleifend pede libero quis orci nullam molestie nibh in lectus pellentesque",
                        Replies =  new List<Replies>
                        {
                            new Replies
                            {
                                Reply = "est risus auctor sed tristique in tempus sit amet sem fusce consequat nulla nisl nunc"
                            }
                        }
                        
                    }
                    },
                    User = new User
                    {
                        FirstName = "Pasquale",
                        LastName = "Puttnam",
                        Gender = "M",
                        AvatarUrl = "https://robohash.org/quistemporenostrum.png?size=50x50&set=set1",
                        Squad = "sq-002",
                        UserName = "pputtnam0@livejournal.com",
                        Email = "pputtnam0@merriam-webster.com"
                    }
                };
        }

        public static List<Complaint> ReturnComplaints()
        {
            return new List<Complaint>
            {
                  new Complaint
                {
                    Question = "Digitized upward-trending adapter",
                    Type = "View to a Kill, A",
                    Image = "http://dummyimage.com/134x188.jpg/5fa2dd/ffffff",
                    IsTask = false,
                    Comments = new List<Comments>
                    {
                    new Comments
                    {
                        Comment = "semper porta volutpat quam pede lobortis ligula sit amet eleifend pede libero quis orci nullam molestie nibh in lectus pellentesque",
                        Replies =  new List<Replies>
                        {
                            new Replies
                            {
                                Reply = "est risus auctor sed tristique in tempus sit amet sem fusce consequat nulla nisl nunc"
                            }
                        }

                    }
                    },
                    User = new User
                    {
                        FirstName = "Pasquale",
                        LastName = "Puttnam",
                        Gender = "M",
                        AvatarUrl = "https://robohash.org/quistemporenostrum.png?size=50x50&set=set1",
                        Squad = "sq-002",
                        UserName = "pputtnam0@livejournal.com",
                        Email = "pputtnam0@merriam-webster.com"
                    }
            }
            };
        }
    }
}
