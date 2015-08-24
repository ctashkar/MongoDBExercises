using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class Comment
    {
        // XXX WORK HERE
        // Add in the appropriate properties.
        // The homework instructions have the
        // necessary schema.

        [Required]
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAtUtc { get; set; }
    }
}