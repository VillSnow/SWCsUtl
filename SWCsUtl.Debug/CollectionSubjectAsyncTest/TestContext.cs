using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SWCsUtl.Debug.CollectionSubjectAsyncTest
{
	public class TestContext
	{
		public int MinAge { get; set; } = 0;
		public int MaxAge { get; set; } = 100;

		public bool ShowMale { get; set; } = true;

		public bool ShowFemale { get; set; } = true;

		public bool IsAsyncMode { get; set; } = false;

		public ICommand QueryCommand { get; }

		public CollectionSubject<Person> Items { get; }

		public TestContext()
		{
			QueryCommand = new QueryCommand(this);

			Items = new CollectionSubject<Person>() {
				Dispatch = Dispatcher.CurrentDispatcher.Invoke
			};

			Query();
		}

		public void Query()
		{
			if (IsAsyncMode)
			{
				Task.Run(QueryImpl);
			}
			else
			{
				QueryImpl();
			}
		}

		public void QueryImpl()
		{
			var q = GetSrouce().Where(
				x => MinAge <= x.Age && x.Age <= MaxAge
			).Where(
				x => ShowMale && x.Sex == Sex.Male || ShowFemale && x.Sex == Sex.Female
			);
			Items.Assign(q.ToList());
		}


		IReadOnlyCollection<Person> GetSrouce() => new[]
		{
			new Person { Id = 1, Name = "吉田 陽鞠", Sex = Sex.Female, Age = 10 },
			new Person { Id = 2, Name = "富永 葵依", Sex = Sex.Female, Age = 15 },
			new Person { Id = 3, Name = "織田 希美", Sex = Sex.Female, Age = 8 },
			new Person { Id = 4, Name = "滋賀 良一", Sex = Sex.Male, Age = 27 },
			new Person { Id = 5, Name = "浜野 敏弘", Sex = Sex.Male, Age = 41 },
			new Person { Id = 6, Name = "吉野 沙菜", Sex = Sex.Female, Age = 5 },
			new Person { Id = 7, Name = "椎名 昭彦", Sex = Sex.Male, Age = 53 },
			new Person { Id = 8, Name = "三枝 沙月", Sex = Sex.Female, Age = 5 },
			new Person { Id = 9, Name = "平原 凛久", Sex = Sex.Male, Age = 7 },
			new Person { Id = 10, Name = "村松 麻央", Sex = Sex.Female, Age = 13 }
		};



	}

	public class Person : IEquatable<Person>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Sex Sex { get; set; }
		public int Age { get; set; }

		public int Hash => GetHashCode();

		public bool Equals(Person other)
		{
			return Id == other.Id;
		}
	}

	public enum Sex
	{
		Male,
		Female
	}

	public class QueryCommand : ICommand
	{
		TestContext Owner { get; }

		public QueryCommand(TestContext owner)
		{
			Owner = owner;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter)
		{
			Owner.Query();
		}
	}
}
