using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
// BsonClassMap
using MongoDB.Bson.Serialization.Conventions;

namespace Homework3._1
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
            var studentsCollection = RetrieveMainDatabaseCollection();

            var listOfStudents = await studentsCollection.Find(new BsonDocument()).ToListAsync();
            foreach (var student in listOfStudents)
            {
                Console.WriteLine();
                Console.WriteLine("Student ID: " + student.Id);
                Console.WriteLine("Student name: " + student.Name);

                var scoresLine = "\tThe scores of the student are:";

                var lowestHomeworkScore = -1.0;

                student.Scores.ForEach(score => 
                {
                    scoresLine += "\n\t\t Type: " + score.Type + " + Score: " + score.ScoreValue.ToString();

                    if (score.Type == "homework")
                    {
                        if (lowestHomeworkScore < 0)
                        {
                            lowestHomeworkScore = score.ScoreValue; // This saves the first homework score of the array
                        }
                        else if (lowestHomeworkScore > score.ScoreValue)
                        {
                            lowestHomeworkScore = score.ScoreValue; // This saves the second homework score of the array only if it is lower than the first
                        }
                    }
                });

                Console.WriteLine(scoresLine);
                Console.WriteLine("\n\t The lowest Homework score is: " + lowestHomeworkScore + " - Removing it now...");

                // We perform the FindOneAndUpdate action in here now
                var updatedStudent = await RemoveLowerHomeworkFromStudentScores(studentsCollection, student, lowestHomeworkScore);

                // Finally we check the results
                if (updatedStudent.Scores.Count == 3 &&
                    !updatedStudent.Scores.Contains(new Score() {Type = "homework", ScoreValue = lowestHomeworkScore}))
                {
                    Console.WriteLine("\n\n\t Updated student and lowest homework score removed!\n");
                }
                else
                {
                    Console.WriteLine("\n\n\n\t **** Something went wrong here!\n");
                }
            }
        }

        private static async Task<Student> RemoveLowerHomeworkFromStudentScores(IMongoCollection<Student> studentsCollection, Student student, double lowestHomeworkScore)
        {
            var filterDefinition = Builders<Student>.Filter.Eq(s => s.Id, student.Id);

            var updateDefinition = Builders<Student>.Update.PullFilter(p => p.Scores, 
                    Builders<Score>.Filter.Eq(f => f.Type, "homework") &
                    Builders<Score>.Filter.Eq(f => f.ScoreValue, lowestHomeworkScore));

            var updateOptions = new FindOneAndUpdateOptions<Student>() {ReturnDocument = ReturnDocument.After};

            var result = await studentsCollection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, updateOptions);

            return result;
        }        

        private static IMongoCollection<Student> RetrieveMainDatabaseCollection()
        {
            var db = InitialiseDatabase().GetDatabase("school");
            var col = db.GetCollection<Student>("students");
            return col;
        }

        private static MongoClient InitialiseDatabase()
        {
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            return client;
        }

        class Student
        {
            public int Id { get; set; }

            [BsonElement("name")]
            public string Name { get; set; }

            [BsonElement("scores")]
            public List<Score> Scores { get; set; }
        }

        class Score
        {
            [BsonElement("type")]
            public string Type { get; set; }

            [BsonElement("score")]
            public double ScoreValue { get; set; }
        }
    }
}
