using System;
using System.Collections.Generic;

using DirectDBconnector;
using Schemas;

namespace SimpleTest
{
    // Создать перечисление
    enum UI : long { Name, Family, ShortName = 5, Age, Sex }

	public class SProps
	{
		public int order;
		public string sentence;

		public SProps(int p, string p_2)
		{
			// TODO: Complete member initialization
			this.order = p;
			this.sentence = p_2;
		}
	}
	public class AttrsCollection
	{
		private List<int> attrs = new List<int> {1, 2};


		public List<int> Attrs
		{
			get
			{
				return attrs;
			}
		}

		public void Print()
		{
			Console.WriteLine("List: {0}", string.Join(",", attrs));
			Console.ReadKey();
		}
	}

	class Program
    {
        static void Main()
        {
			TestList();

			return;

			RunTest();

            UI user1;
            for (user1 = UI.Name; user1 <= UI.Sex; user1++)
                Console.WriteLine("Элемент: \"{0}\", значение {1}",user1,(int)user1);
			string s = Enum.GetName(typeof(UI), 0);


			Console.WriteLine(s);
			var dt = DateTime.Now;
			Console.WriteLine(dt.ToShortDateString());

			var mainlist = new List<SProps>();
			var n = new SProps(1, "n1");
			mainlist.Add(n);
			n = new SProps(2, "n2");
			mainlist.Add(n);

			Console.WriteLine("mainlist before");
			foreach(var item in mainlist)
				Console.WriteLine(item.order.ToString() + " - " + item.sentence);

			var reslist = new List<SProps>();
			reslist.AddRange(mainlist);

			reslist[0].sentence = "nn1";
			Console.WriteLine("mainlist after");
			foreach (var item in mainlist)
				Console.WriteLine(item.order.ToString() + " - " + item.sentence);


            Console.ReadLine();
        }

        static void RunTest()
        {
            SQLiteConnector dbConnector = SQLiteConnector.Instance;
            // act
            //dbConnector.EmptyDB();
            //dbConnector.selectAll(dbTables.tblTermContent);

        }

		static void TestList()
		{
			var x = new AttrsCollection();
			x.Print();
			var attrs = x.Attrs;

			attrs[1] = 3; // присваивается
			x.Print();

			attrs = new List<int> { 7, 8 }; // не присваивается
			x.Print();
		}

	}
}
