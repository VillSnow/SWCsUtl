using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SWCsUtl.Tests {
	[TestClass]
	public class MathToolsTests {

		[TestMethod]
		[DataRow(15, 10, 20, 15)]
		[DataRow(5, 10, 20, 10)]
		[DataRow(25, 10, 20, 20)]
		[DataRow(10, 10, 20, 10)]
		[DataRow(20, 10, 20, 20)]
		public void TestClampInt(int x, int min, int max, int expected) {
			Assert.AreEqual(expected, MathTools.Clamp(x, min, max));
		}
	}
}
