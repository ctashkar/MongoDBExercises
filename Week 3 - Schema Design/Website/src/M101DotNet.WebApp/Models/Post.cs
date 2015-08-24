using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class Post
    {
        // XXX WORK HERE
        // add in the appropriate properties for a post
        // The homework instructions contain the schema.

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [BsonRepresentation(BsonType.Array)]
        public List<string> Tags { get; set; }

        [Required]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAtUtc { get; set; }

        [BsonRepresentation(BsonType.Array)]
        public List<Comment> Comments { get; set; }

    }
}