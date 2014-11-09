using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDBExercises
{
	class Program
	{
		static void Main(string[] args)
		{
			var connectionString = "mongodb://localhost";
			MongoClient client = new MongoClient(connectionString); 
			MongoServer server = client.GetServer();
			MongoDatabase studentsDB = server.GetDatabase("students");
			double iDocusCount = studentsDB.GetCollection("grades").Count();
			Console.WriteLine("The total number of documents in the database is: " + iDocusCount.ToString());
			

		}
	}
}
