using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class ContactDetails
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string LinkToSocialM { get; set; }

        public ContactDetails(string email, string country, string city)
        {
            Id = Guid.NewGuid();
            Email = email;
            Country = country;
            City = city;
        }

        public ContactDetails UpdateDetails(string address,
                                    string city,
                                    string country,
                                    string phoneNo,
                                    string linkToSocialM) {

            Address = address;
            City = city;
            Country = country;
            PhoneNo = phoneNo;
            LinkToSocialM = linkToSocialM;

            return this;
        }
    }
}
