using System;
using CoffeeMate.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoffeeMate.Infrastructure.Data;

public class AppUser : IdentityUser
{
    public required string DisplayName { get; set; }
    public string? ProfileImage {get;set;}
    public ICollection<Collaborator> Collaborations {get;set;} = [];
    
}
