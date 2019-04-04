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
        //TODO: сменить на относительный путь
        static readonly string textFile = @"C:\Users\Egor\source\repos\CountForCountry\CountForCountry\Users.txt";

        struct user
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
            List<user> users = new List<user>();

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

                        users.Add(new user()
                        {
                            id = Convert.ToInt32(words[0]),
                            count = Convert.ToInt32(words[1]),
                            country = words[2],
                        });

                    }
                }
            }

            //Юзать LINQ это явно не "без дополнительных алгоритмов", но это самый быстрый метод в плане написания
            //Над вариантом на одних массивах и циклах подумаю попозже
            List<ResultCountry> results = users.GroupBy(t => t.country)
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
