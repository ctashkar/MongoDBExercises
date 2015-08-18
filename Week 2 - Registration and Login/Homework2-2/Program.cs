using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization; // BsonClassMap
using MongoDB.Bson.Serialization.Conventions;

namespace Homework2_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Program starting...");
            Console.WriteLine();
            MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("students");
            var col = db.GetCollection<Grades>("grades");

            /*
            var filterBuilder = Builders<Grades>.Filter;
            var filter = filterBuilder.Gt("score", 65) & filterBuilder.Lt("score", 69);
            var list = await col.Find(filter).ToListAsync();
            */

            var list = await col.Find(grade => grade.type == "homework")
                .SortBy(grade => grade.student_id)
                .ThenByDescending(grade => grade.score)
                .ToListAsync();

            var studentId = -1;
            var gradeId = new ObjectId();
            var greaterScore = 0.0;

            list.ForEach(grade =>
                {
                    if (studentId != grade.student_id)
                    {
                        Console.WriteLine();
                        Console.WriteLine("New student with ID: " + grade.student_id.ToString());
                        Console.WriteLine("\tGrade => Score: " + grade.score.ToString() + " - Type: " + grade.type);

                        gradeId = grade.Id;
                        studentId = grade.student_id;
                        greaterScore = grade.score;
                    }
                    else
                    {
                        Console.WriteLine("\tGrade => Score: " + grade.score.ToString() + " - Type: " + grade.type);
                        if (greaterScore > grade.score)
                        {
                            var result = col.DeleteOneAsync(x => x.Id == grade.Id);
                            Console.WriteLine("\t\tDeleted grade with score: " + grade.score.ToString() + " and Id: " + grade.Id.ToString());
                        }
                        else
                        {
                            var result = col.DeleteOneAsync(x => x.Id == gradeId);
                            Console.WriteLine("\t\tDeleted grade with score: " + greaterScore.ToString() + " and Id: " + gradeId.ToString());
                        }
                    }
                });            
        }
    }

    class Grades
    {
        public ObjectId Id { get; set; }
        public int student_id { get; set; }
        public string type { get; set; }
        public double score { get; set; }
    }
}


