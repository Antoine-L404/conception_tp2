using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Automate.Models
{
    /// <summary>
    /// Nous ne pouvons pas nommer la classe Task, car elle entre en conflit avec System.Threading.Tasks.Task
    /// </summary>
    class UpcomingTask
    {
        [BsonId]
        public ObjectId Id { get; set; }

        private string _title = "";
        [BsonElement("Title")]
        public string Title
        {
            get => _title;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Trim().Length == 0)
                {
                    throw new ArgumentException();
                }
                _title = value;
            }
        }

        private DateTime _eventDate;
        [BsonElement("DateEvenement")]
        public DateTime EventDate
        {
            get => _eventDate;
            set
            {
                _eventDate = value; 
            }
        }

    }
}
