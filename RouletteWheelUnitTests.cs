using Moq;
using NUnit.Framework;

namespace RouletteKata;

public class RouletteWheelUnitTests
{
    private RouletteWheel _wheel;
    private Mock<INumberGenerator> _numberGenerator;

    [SetUp]
    public void Setup()
    {
        _numberGenerator = new Mock<INumberGenerator>();
        _wheel = new RouletteWheel(_numberGenerator.Object);
    }

    [Test]
    [TestCase(1, "red")]
    [TestCase(2, "black")]
    [TestCase(3, "red")]
    [TestCase(4, "black")]
    [TestCase(5, "red")]
    [TestCase(6, "black")]
    [TestCase(7, "red")]
    [TestCase(8, "black")]
    [TestCase(9, "red")]
    [TestCase(10, "black")]
    [TestCase(11, "red")]
    [TestCase(12, "black")]
    [TestCase(13, "red")]
    [TestCase(14, "black")]
    [TestCase(15, "red")]
    [TestCase(16, "black")]
    [TestCase(17, "red")]
    [TestCase(18, "black")]
    [TestCase(19, "red")]
    [TestCase(20, "black")]
    [TestCase(21, "red")]
    [TestCase(22, "black")]
    [TestCase(23, "red")]
    [TestCase(24, "black")]
    [TestCase(25, "red")]
    [TestCase(26, "black")]
    [TestCase(27, "red")]
    [TestCase(28, "black")]
    [TestCase(29, "red")]
    [TestCase(30, "black")]
    [TestCase(31, "red")]
    [TestCase(32, "black")]
    [TestCase(33, "red")]
    [TestCase(34, "black")]
    [TestCase(35, "red")]
    [TestCase(36, "black")]
    public void NumberIsCorrectColour(int number, string colour)
    {
        _numberGenerator.Setup(x => x.Next()).Returns(number);
        
        SpinResult result = _wheel.Spin();
        
        Assert.AreEqual(colour, result.Colour);
    }
    
    
    [Test]
    [TestCase(1, 1)]
    [TestCase(12, 1)]
    [TestCase(13, 2)]
    [TestCase(24, 2)]
    [TestCase(25, 3)]
    public void NumberIsCorrectColumn(int number, int column)
    {
        _numberGenerator.Setup(x => x.Next()).Returns(number);
        
        SpinResult result = _wheel.Spin();
        
        Assert.AreEqual(column, result.Column);
    }
}

public class RouletteWheel : IRouletteWheel
{
    private readonly INumberGenerator _numberGenerator;
    private const string BlackString = "black";
    private const string RedString = "red";

    public RouletteWheel(INumberGenerator numberGenerator)
    {
        _numberGenerator = numberGenerator;
    }
    
    public SpinResult Spin()
    {
        int number = _numberGenerator.Next();
        return new SpinResult
        {
            Number = number,
            Colour = CalculateColour(number),
            Column = CalculateColumn(number)
        };
    }

    private int CalculateColumn(int number)
    {
        if (number <= 12)
            return 1;
        
        if (number <= 24)
            return 2;
        
        return 3;
        
    }

    private string CalculateColour(int number)
    {
        if (NumberDividesByTwo(number))
            return BlackString;
        return RedString;
    }

    private static bool NumberDividesByTwo(int number)
    {
        return number % 2 == 0;
    }
}

public interface INumberGenerator
{
    int Next();
}
