using System.Collections.Generic;

namespace FacilityManagement.Services.Models
{
    public class Pagination<TModel>
    {
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public ICollection<TModel> Items { get; set; }
    }
}
