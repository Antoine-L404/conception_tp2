using Automate.Utils.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Automate.Models
{
    public class UpcomingTask
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Title")]
        public EventType Title { get; set; }

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
