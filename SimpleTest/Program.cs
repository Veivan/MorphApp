using System;

namespace SimpleTest
{
    // Создать перечисление
    enum UI : long { Name, Family, ShortName = 5, Age, Sex }

    class Program
    {
        static void Main()
        {
            UI user1;
            for (user1 = UI.Name; user1 <= UI.Sex; user1++)
                Console.WriteLine("Элемент: \"{0}\", значение {1}",user1,(int)user1);

            Console.ReadLine();
        }
    }
}
