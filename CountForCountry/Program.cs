using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CountForCountry
{
    class Program
    {

        static readonly string textFile = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, "Users.txt");

        struct User
        {
            public int id;
            public int count;
            public string country;
        }

        struct ResultCountry
        {
            public int DistinctIdCount;
            public int countSum;
            public string country;
        }
        
        static void Main(string[] args)
         
        {   
            List<User> Users = new List<User>();

            using (StreamReader file = new StreamReader(textFile))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    //Регэксп будет чекать только строчки формата цифры;цифры(в тесткейсе count был одной цифрой ну да ладно);Страна (с большой буквы латинницей)
                    if (Regex.Match(line, @"^[0-9]*;[0-9]*;[A-Z][a-z]*$").Success)
                    {
                        string[] words;
                        words = line.Split(';');

                        Users.Add(new User()
                        {
                            id = Convert.ToInt32(words[0]),
                            count = Convert.ToInt32(words[1]),
                            country = words[2],
                        });

                    }
                }
            }

            //Юзать LINQ это явно не "без дополнительных алгоритмов", но это самый быстрый метод в плане написания
            //Над вариантом на одних массивах подумаю попозже
            List<ResultCountry> results = Users.GroupBy(t => t.country)
                .Select(c => new ResultCountry
                {
                    country = c.First().country,
                    DistinctIdCount = c.GroupBy(cl => cl.id).Count(),
                    countSum = c.Sum(cl => cl.count),
                }).ToList();



             foreach (var result in results)
                 Console.WriteLine(result.country + ' ' + result.countSum + ' ' + result.DistinctIdCount);

            Console.ReadKey();

        }
    }
}
