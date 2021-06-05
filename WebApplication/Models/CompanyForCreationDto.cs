using System.Collections.Generic;

namespace WebApplication.Models
{
    public class CompanyForCreationDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
