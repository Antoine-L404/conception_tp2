using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using Automate.Utils.Constants;

namespace Automate.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        private string username = "";
        [BsonElement("Username")]
        public string Username {
            get => username;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Trim().Length == 0)
                    throw new ArgumentException("Username length must not be empty");

                username = value;
            }
        }

        private string password = "";
        [BsonElement("Password")]
        public string Password {
            get => password;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Trim().Length == 0)
                    throw new ArgumentException("Password length must not be empty.");

                password = value;
            }
        }

        private string role = "";
        [BsonElement("Role")]
        public string Role {
            get => role;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value != RoleConstants.ADMIN && value != RoleConstants.EMPLOYEE)
                    throw new ArgumentException("Role value must be 'admin' or 'employee'.");

                role = value;
            }
        }
    }
}
