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
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException();

                username = value;
            }
        }

        private string password = "";
        [BsonElement("Password")]
        public string Password {
            get => password;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException();

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
                if (value != RoleConstant.ADMIN && value != RoleConstant.EMPLOYEE)
                    throw new ArgumentException();

                role = value;
            }
        }
    }
}
