using MongoDB.Bson;

namespace Automate.Abstract.Models
{
    public interface IObjectWithId
    {
        ObjectId Id { get; set; }
    }
}
