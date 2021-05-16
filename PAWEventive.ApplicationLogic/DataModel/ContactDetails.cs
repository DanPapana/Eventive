using System;
using System.Collections.Generic;
using System.Text;

namespace PAWEventive.ApplicationLogic.DataModel
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


        public ContactDetails(string email)
        {
            Id = Guid.NewGuid();
            Email = email;
        }

        public ContactDetails UpdateDetails(string address,
                                    string city,
                                    string country,
                                    string phoneNo,
                                    string email,
                                    string linkToSocialM) {

            Address = address;
            City = city;
            Country = country;
            PhoneNo = phoneNo;
            Email = email;
            LinkToSocialM = linkToSocialM;

            return this;
        }
    }
}
