using System;
using System.Collections.Generic;

namespace Kendo.NetCore.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }


        private static List<Person> _list;

        public static List<Person> GetList()
        {
            if(_list == null)
            {
                _list = new List<Person>();
                for (int i = 0; i < 100; i++)
                {
                    _list.Add(new Person { Id = i, Name = $"Name {i}", Age = new Random().Next(18, 99) });
                }
            }
            
            return _list;
        }
    }
}
