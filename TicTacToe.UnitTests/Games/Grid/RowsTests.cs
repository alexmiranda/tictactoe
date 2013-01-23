using NUnit.Framework;
using System.Linq;
using TicTacToe.Games.Grid;
using TicTacToe.Games.Grid.ThreePerThree;
using TicTacToe.UnitTests.Support.Data;

namespace TicTacToe.UnitTests.Games.Grid
{
    [TestFixture]
    public class RowsTests
    {
        [Test]
        public void GetAllReturnsAll3x3Rows()
        {
            var rows = Rows.GetAll<Rows3X3>();
            Assert.That(rows, Is.Not.Null);
            Assert.That(rows, Is.Not.Empty);
            Assert.That(rows.Count(), Is.EqualTo(8));
        }

        [Test]
        [TestCaseSource(typeof(TestCaseDataProvider), "Positions")]
        public void GetRowsOfPositionReturnsCorrectly(IPosition position)
        {
            var rows = Rows.OfPosition<Rows3X3>(position);
            Assert.That(rows, Is.Not.Null);
            Assert.That(rows, Is.Not.Empty);
            CollectionAssert.AllItemsAreNotNull(rows);
            Assert.IsTrue(rows.All(x => x.Includes(position)));
        }
    }
}
