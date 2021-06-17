using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Eventive.Models.Users
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "There needs to be a name!")]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "There needs to be a last name!")]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }

        [Display(Name = "Profile image")]
        public IFormFile ProfileImage { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please input your country")]
        [Display(Name = "Country *")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please input your city")]
        [Display(Name = "City *")]
        public string City { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNo { get; set; }

        [Display(Name = "Link to a social media account")]
        public string LinkToSocialM { get; set; }
    }
}
