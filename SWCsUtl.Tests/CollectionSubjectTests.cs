using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SWCsUtl.Tests {
	[TestClass]
	public class CollectionSubjectTests {

		[TestMethod]
		public void TestInitialState() {
			var collection = new CollectionSubject<EquatableElement>();
			CollectionAssert.AreEqual(new EquatableElement[0], collection);
		}

		[TestMethod]
		public void TestEquatableElement() {
			Assert.AreNotEqual(new EquatableElement(42, "hoge"), new EquatableElement(42, "hoge"));
			Assert.IsFalse(new EquatableElement(42, "hoge") == new EquatableElement(42, "fuga"));
			Assert.IsFalse(new[] { new EquatableElement(42, "hoge") }.Contains(new EquatableElement(42, "hoge")));
			Assert.IsTrue(EqualityComparer<EquatableElement>.Default.Equals(new EquatableElement(42, "hoge"), new EquatableElement(42, "fuga")));
			Assert.AreEqual(0, Comparer<EquatableElement>.Default.Compare(new EquatableElement(42, "hoge"), new EquatableElement(42, "fuga")));
		}

		[TestMethod]
		public void TestAssignValueObjects() {
			foreach (var seed in Enumerable.Range(0, 1000)) {
				var random = new Random(seed);
				var collection = new CollectionSubject<EquatableElement>();
				for (int i = 0; i < random.Next(1, 10); ++i) {
					var postValues = collection.ToList();
					var nextValues = Enumerable.Range(0, random.Next(0, 10)).Select(_ => new EquatableElement(random.Next(10), random.Next(10).ToString())).ToArray();
					collection.Assign(nextValues);

					var postValuesStr = "[" + string.Join(", ", postValues.Select(x => x.Id)) + "]";
					var nextValuesStr = "[" + string.Join(", ", nextValues.Select(x => x.Id)) + "]";
					var actualStr = "[" + string.Join(", ", collection.Select(x => x.Id)) + "]";

					CollectionAssert.AreEqual(nextValues, collection, Comparer<EquatableElement>.Default, $"{postValuesStr} => {nextValuesStr} = {actualStr}");
				}
			}
		}

		[TestMethod]
		public void TestAssignEntities() {
			foreach (var seed in Enumerable.Range(0, 1000)) {
				var random = new Random(seed);
				var collection = new CollectionSubject<EquatableElement> {
					AssignItem = (src, dst) => {
						Console.WriteLine("assign item");
						src.Value = dst.Value;
					}
				};

				for (int i = 0; i < random.Next(1, 10); ++i) {
					var postValues = collection.ToList();
					var nextValues =
						Enumerable.Range(
							0, random.Next(0, 10)
						).Select(
							_ => new EquatableElement(random.Next(10), random.Next(10).ToString())
						).ToArray();

					collection.Assign(nextValues);

					var postValuesStr = "[" + string.Join(", ", postValues.Select(x => $"{x.Id}-{x.Value}")) + "]";
					var nextValuesStr = "[" + string.Join(", ", nextValues.Select(x => $"{x.Id}-{x.Value}")) + "]";
					var actualStr = "[" + string.Join(", ", collection.Select(x => $"{x.Id}-{x.Value}")) + "]";

					CollectionAssert.AreEqual(nextValues.Select(x => x.Value).ToArray(), collection.Select(x => x.Value).ToArray(), $"{postValuesStr} => {nextValuesStr} = {actualStr}");
				}
			}
		}

		class EquatableElement : IEquatable<EquatableElement>, IComparable<EquatableElement> {

			public int Id { get; }
			public string Value { get; set; }
			public EquatableElement(int id, string value) {
				Id = id;
				Value = value;
			}

			public bool Equals(EquatableElement other) {
				return Id.Equals(other.Id);
			}

			public int CompareTo(EquatableElement other) {
				return Id.CompareTo(other.Id);
			}
		}

	}

}
