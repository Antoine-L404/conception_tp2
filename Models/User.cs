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

        [BsonElement("Username")]
        public string Username { 
            get
            {
                return Username;
            } 
            set 
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Length > 0) 
                {
                    Username = value;   
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        [BsonElement("Password")]
        public string Password {
            get
            {
                return Password;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (!Regex.IsMatch(Password, "/^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?.&])[A-Za-z\\d@$!%*?.&]{8,}$/"))
                {
                    throw new ArgumentException();
                }
                if (value.Length <= 0)
                {
                    throw new ArgumentException();
                }
                Password = value;
            }
        }

        [BsonElement("Role")]
        public string Role {
            get
            {
                return Role;
            }
            set
            {
                
                ArgumentNullException.ThrowIfNull(value);
                if (value != RoleConstant.ADMIN && value != RoleConstant.EMPLOYEES)
                {
                    throw new ArgumentException();
                }
                Role = value;
            } 
        }
    }
}
