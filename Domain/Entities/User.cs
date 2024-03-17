using Domain.Entities.Base;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    [BsonCollection("User")]
    public class User : Document
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Profile { get; set; }
        public bool Active { get; set; }

        public void AddUser()
        {
            Validate();
            Password = BCrypt.Net.BCrypt.HashPassword(Password);

        }
        private void Validate()
        {
            if (this.Username is null || (this.Username.Length < 3))
            {
                throw new InvalidNameException("Invalid Name");
            }

            if (Email is null || !Email.Contains("@"))
            {
                throw new InvalidEmailException("Invalid Email");
            }
        }
    }
}