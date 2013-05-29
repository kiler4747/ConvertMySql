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
			using (var context = new BdContext("flibusta_online_fb2Entities"))
			{
				context.Autors.Load();
			}
			return;
			for (int i = 0; i < threadCount; i++)
			{
				manualEvents[i] = new ManualResetEvent(false);
			}
			using (libraryEntities context = new libraryEntities())
			{
				var tmp = context.libavtornames.OrderBy(x => x.AvtorId).ToList();
				int counter = 0;
				long allCount = context.libavtornames.Count();

				while (counter < allCount)
				{
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
					WaitHandle.WaitAll(manualEvents);
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

		private static List<Author> listAutor = new List<Author>();

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
						var tempList = new List<Author>();
						foreach (var libavtorname in list.Avtor)
						{
							if (cont.Autors.Any(x => x.AuthorID == libavtorname.AvtorId))
								continue;
							var autor = new Author()
								{
									FirstName = libavtorname.FirstName,
									LastName = libavtorname.LastName,
									AuthorID = (int)libavtorname.AvtorId,
									MiddleName = libavtorname.MiddleName,
									//Books = new List<Book>()
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
								//autor.Books.Add(book);
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
