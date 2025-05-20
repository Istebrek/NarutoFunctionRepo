using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeFunctionCode.Models
{
    public class NarutoCharacters
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string? Picture { get; set; }
        public string Name { get; set; }
        public string? Village { get; set; }
        public string? Jutsu { get; set; }
        public int? Rating { get; set; }        
    }
}
