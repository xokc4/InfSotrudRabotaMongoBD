using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using System.Diagnostics;


namespace InfSotrudRabotaMongoBD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Здравствуйте, это приложение показывает информацию о работниках. Также вы можете изменить, добавить или удалить работника.");

            FindDocs();

            Console.WriteLine("чтобы добавить информацию напишите ReplaсeModel, \nчтобы изменить информацию напишите UpdateModel, \nчтобы удалить информацию напишите DeleteModel.");
            string selection = Console.ReadLine();
  // условие какой метод будет работать 
             if(selection == "ReplaceModel")
            {
                SaveDocs();
            }
            if (selection == "UpdateModel")
            {
                UpdatePerson();
            }
            if (selection == "DeleteModel")
            {
                DeletePerson();
            }



            Console.ReadKey();
        }
        private static async Task FindDocs()
        {
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("BDInfRabota");
            var collection = database.GetCollection<BsonDocument>("InfRabota");
            var filter = new BsonDocument();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var people = cursor.Current;
                    foreach (var doc in people)
                    {
                        Console.WriteLine(doc);

                    }
                }
            }
        }// изменение в бд
        private static async Task DeletePerson()
        {
            Console.WriteLine("Введите имя работника которого нужно удалить.");
            string Name = Console.ReadLine();
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("BDInfRabota");
            var collection = database.GetCollection<BsonDocument>("InfRabota");

            var filter = Builders<BsonDocument>.Filter.Eq("Name", Name);
            await collection.DeleteOneAsync(filter);

            var people = await collection.Find(new BsonDocument()).ToListAsync();
            foreach (var p in people)
                Console.WriteLine(p);
                Console.WriteLine("Работник удалён");
        }// удаление в бд
        private static async Task SaveDocs()// добавление в бд
        {
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("BDInfRabota");
            var collection = database.GetCollection<Person>("InfRabota");
            Console.WriteLine("введите количество участников которых хотите добавить в бд");
            int ColU4astnicov = Convert.ToInt32(Console.ReadLine());

           

            for (int i = 0; i<=ColU4astnicov; i++)
            {
                Console.WriteLine("Напишите имя работника ");
                string Name = Console.ReadLine();
                Console.WriteLine("Напишите Возраст работника ");
                int age = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Напишите язык работника ");
                string Languages = Console.ReadLine();
                Console.WriteLine("Напишите должность");
                string post = Console.ReadLine();

                Person person = new Person
                {
                    Name = Name,
                    Age = age,
                    Languages = new List<string> { "english", Languages },
                    Post = post,
                    Company = new Company
                    {
                        Name = "Apple"
                    }
                };
                await collection.InsertOneAsync(person);
                Console.WriteLine("все удачно сохранилось");
            }



        }
        private static async Task UpdatePerson()// изменение бд
        {
            Console.WriteLine("Напишите имя работника которого хотите изменить");
            string NameIsm = Console.ReadLine();
            Console.WriteLine("Изменение имение, если не надо то пропишите заново имя");
            string Name = Console.ReadLine();
            Console.WriteLine("Изменение возраста, если не надо то пропишите заново ");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Изменение 2 языка, если не надо то пропишите заново");
            string Languages = Console.ReadLine();
            Console.WriteLine("Изменение должности, если не надо то пропишите заново ");
            string post = Console.ReadLine();     

            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("BDInfRabota");
            var collection = database.GetCollection<BsonDocument>("InfRabota");
            var result = await collection.ReplaceOneAsync(new BsonDocument("Name", NameIsm),
            new BsonDocument
                {
                    {"Name", Name},
                    {"Age", age},
                    {"Languages", new BsonArray(new []{"english", Languages})},
                    {"Post", post},
                    { "Company",
                        new BsonDocument{
                            {"Name" , "Apple"}
                        }
                    }
                });
        }



    }
}
