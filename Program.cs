using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyHomeLib.Net;

namespace ConvertToSqlServer
{
	class Program
	{
		static ManualResetEvent[] manualEvents =
			new ManualResetEvent[threadCount];

		struct MyStruct
		{
			public IEnumerable<libavtorname> Avtor;
			public ManualResetEvent Event;
		}

		private const int threadCount = 64;
		private const int entiresCount = 250;

		static void Main(string[] args)
		{
			Database.SetInitializer(new DropCreateDatabaseAlways<BdContext>());
			//Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BdContext>());
			for (int i = 0; i < threadCount; i++)
			{
				manualEvents[i] = new ManualResetEvent(false);
			}
			//using (BdContext cont = new BdContext())
			//{
			using (libraryEntities context = new libraryEntities())
			{
				var tmp = context.libavtornames.OrderBy(x => x.AvtorId).ToList();
				int counter = 0;
				long allCount = context.libavtornames.Count();

				while (counter < allCount)
				{
					//foreach (var manualResetEvent in manualEvents)
					//{
					//	//manualResetEvent.Reset();
					//	manualResetEvent.Set();
					//}
					//manualEvents[0].Reset();
					for (int i = 0; i < threadCount; i++)
					{
						manualEvents[i].Reset();
						new Thread(AddAvtors).Start(new MyStruct()
							{
								Avtor = tmp.Skip(counter).Take(entiresCount).ToList(),
								Event = manualEvents[i]
							});
						counter += entiresCount;
						//Thread.Sleep(1000);
					}
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 250).Take(250).ToList(), Event = manualEvents[1] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 500).Take(250).ToList(), Event = manualEvents[2] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 750).Take(250).ToList(), Event = manualEvents[3] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 1000).Take(250).ToList(), Event = manualEvents[4] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 1250).Take(250).ToList(), Event = manualEvents[5] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 1500).Take(250).ToList(), Event = manualEvents[6] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 1750).Take(250).ToList(), Event = manualEvents[7] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 2000).Take(250).ToList(), Event = manualEvents[8] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 2250).Take(250).ToList(), Event = manualEvents[9] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 2500).Take(250).ToList(), Event = manualEvents[10] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 2750).Take(250).ToList(), Event = manualEvents[11] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 3000).Take(250).ToList(), Event = manualEvents[12] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 3250).Take(250).ToList(), Event = manualEvents[13] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 3500).Take(250).ToList(), Event = manualEvents[14] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 3750).Take(250).ToList(), Event = manualEvents[15] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 4000).Take(250).ToList(), Event = manualEvents[16] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 4250).Take(250).ToList(), Event = manualEvents[17] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 4500).Take(250).ToList(), Event = manualEvents[18] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 4750).Take(250).ToList(), Event = manualEvents[19] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 5000).Take(250).ToList(), Event = manualEvents[20] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 5250).Take(250).ToList(), Event = manualEvents[21] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 5500).Take(250).ToList(), Event = manualEvents[22] });
					//Thread.Sleep(100);
					//new Thread(AddAvtors).Start(new MyStruct() { Avtor = tmp.Skip(counter + 5750).Take(250).ToList(), Event = manualEvents[23] });
					//Thread.Sleep(100);

					WaitHandle.WaitAll(manualEvents);
					//counter += 5750;
					//counter += 250;
					Console.WriteLine(counter);
				}
				using (BdContext cont = new BdContext("ConnectionStringName"))
				{
					foreach (var autor in listAutor)
					{

						cont.Autors.Add(autor);
					}
					cont.SaveChanges();
				}
			}
			//}
		}

		private static List<Autor> listAutor = new List<Autor>();

		static void AddAvtors(object obj)
		{
			try
			{
				if (obj == null) throw new ArgumentNullException("obj");
				var list = (MyStruct)obj;
				using (BdContext cont = new BdContext("ConnectionStringName"))
				{
					using (libraryEntities context = new libraryEntities())
					{
						var tempList = new List<Autor>();
						foreach (var libavtorname in list.Avtor)
						{
							if (cont.Autors.Any(x => x.IdAutor == libavtorname.AvtorId))
								continue;
							var autor = new Autor()
								{
									Email = libavtorname.Email,
									FirstName = libavtorname.FirstName,
									LastName = libavtorname.LastName,
									IdAutor = (int)libavtorname.AvtorId,
									MiddleName = libavtorname.MiddleName,
									NickName = libavtorname.NickName,
									Books = new List<Book>()
								};
							var temp1 =
								from avtors in context.libavtors
								join books in context.libbooks on avtors.BookId equals books.BookId
								join avtornames in context.libavtornames on avtors.AvtorId equals avtornames.AvtorId
								where (avtors.AvtorId == libavtorname.AvtorId)
								select books;

							foreach (var l in temp1.ToList())
							{
								var book = new Book()
									{
										FileSize = l.FileSize,
										FileType = l.FileType,
										IdBook = (int)l.BookId,
										Keywords = l.keywords,
										Lang = l.Lang,
										Modified = l.Modified,
										Time = l.Time,
										Title = l.Title,
										Title1 = l.Title1,
										Year = l.Year
									};
								autor.Books.Add(book);
							}
							tempList.Add(autor);
						}
						lock (listAutor)
						{
							listAutor.AddRange(tempList);
						}
					}
					//cont.SaveChanges();
				}
			}
			finally
			{
				((MyStruct)obj).Event.Set();
			}
		}
	}
}
