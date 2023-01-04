using NUnit.Framework;
using Moq;

namespace RouletteKata;

public class RouletteTableUnitTests
{
    private RouletteTable _table = null!;
    private Mock<IRouletteWheel> _wheel = null!;

    [SetUp]
    public void Setup()
    {
        _wheel = new Mock<IRouletteWheel>();
        _table = new RouletteTable(_wheel.Object);
    }
    
    [Test]
    [TestCase("red", 100, "red", 200)]
    [TestCase("red", 10, "red", 110)]
    [TestCase("red", 10, "black", 90)]
    [TestCase("red", 100, "black", 0)]
    [TestCase("black", 100, "black", 200)]
    [TestCase("black", 10, "black", 110)]
    [TestCase("black", 10, "red", 90)]
    [TestCase("black", 100, "red", 0)]
    public void PlacingABetOnColourChangesThePotByTheBetAmount(
        string betColour, int betAmount, string resultColour, int expectedPot)
    {
        _wheel.Setup(x => x.Spin()).Returns(new SpinResult { Colour = resultColour });
        _table.CashIn(100);

        _table.Bet(BetType.Colour, betColour, betAmount);
        _table.Spin();
        
        Assert.AreEqual(expectedPot, _table.Pot);
    }

    [Test]
    [TestCase(27, 100, 27, 3600)]
    [TestCase(7, 100, 7, 3600)]
    [TestCase(7, 10, 7, 450)]
    [TestCase(7, 100, 8, 0)]
    [TestCase(33, 100, 8, 0)]
    public void PlacingABetOnANumberChangesThePotBy36MultipliedByBetAmount(int betNumber, int betAmount, int spinNumber, int expectedPot)
    {
        _wheel.Setup(x => x.Spin()).Returns(new SpinResult { Number = spinNumber });
        _table.CashIn(100);

        _table.Bet(BetType.Number, betNumber.ToString(), betAmount);
        _table.Spin();
        Assert.AreEqual(expectedPot, _table.Pot);
    }

    [Test]
    [TestCase(1, 100, 1, 300)]
    [TestCase(2, 100, 2, 300)]
    [TestCase(3, 100, 3, 300)]
    [TestCase(1, 100, 2, 0)]
    [TestCase(2, 100, 3, 0)]
    [TestCase(3, 100, 1, 0)]
    public void PlacingAWinningBetOn1stColumn(int column, int betAmount, int spinColumn, int expectedPot)
    {
        _wheel.Setup(x => x.Spin()).Returns(new SpinResult { Column = spinColumn });
        _table.CashIn(100);

        _table.Bet(BetType.Column, column.ToString(), betAmount);
        _table.Spin();
        Assert.AreEqual(expectedPot, _table.Pot);
    }

    [Test]
    public void PlacingTwoConsecutiveWinningBetsOf100On1MakesThePot129600()
    {
        _wheel.Setup(x => x.Spin()).Returns(new SpinResult { Number = 1});
        _table.CashIn(100);

        _table.Bet(BetType.Number, 1.ToString(), 100);
        _table.Spin();
        
        _table.Bet(BetType.Number, 1.ToString(), (int)_table.Pot);
        _table.Spin();
        
        Assert.AreEqual(129600, _table.Pot);
    }


    [Test]
    public void PlacingTwoWinningBetsOnRedAnd1()
    {
        _wheel.Setup(x => x.Spin()).Returns(new SpinResult { Colour = "red", Number = 1});
        _table.CashIn(100);

        _table.Bet(BetType.Number, 1.ToString(), 50);
        _table.Bet(BetType.Colour, "red", 50);
        _table.Spin();
        
        Assert.AreEqual(1900, _table.Pot);
    }
}
