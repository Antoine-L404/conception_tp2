using Automate.Abstract.Models;
using Automate.Utils.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Automate.Models
{
    public class UpcomingTask : IObjectWithId
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Title")]
        public EventType Title { get; set; }

        [BsonElement("EventDate")]
        public DateTime EventDate { get; set; }
    }
}
