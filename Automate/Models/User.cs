using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Text.RegularExpressions;
using Automate.Utils.Constants;

namespace Automate.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        //TODO validate the set is not useless

        [BsonElement("Username")]
        public string Username { 
            get => Username;
            set 
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Trim().Length == 0) 
                {
                    throw new ArgumentException();
                }

                Username = value;
            }
        }

        [BsonElement("Password")]
        public string Password {
            get => Password;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (!Regex.IsMatch(Password, "/^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?.&])[A-Za-z\\d@$!%*?.&]{8,}$/") 
                    || value.Trim().Length <= 0)
                {
                    throw new ArgumentException();
                }

                Password = value;
            }
        }

        [BsonElement("Role")]
        public string Role {
            get => Role;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value != RoleConstant.ADMIN && value != RoleConstant.EMPLOYEE)
                {
                    throw new ArgumentException();
                }

                Role = value;
            } 
        }
    }
}
