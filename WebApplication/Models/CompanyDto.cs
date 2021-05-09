using System;

namespace WebApplication.Models
{
    public class CompanyDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public string FullAddress { get; set; } 
    }
}