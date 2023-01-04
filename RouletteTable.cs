using System.Collections.Generic;

namespace RouletteKata;


public interface IRouletteWheel
{
    public SpinResult Spin();
}

public struct SpinResult
{
    public string Colour { get; init; }
    public int Number { get; init; }
    public int Column { get; set; }
}

public class RouletteTable
{
    private readonly IRouletteWheel _wheel;
    private Bet _bet;
    private List<Bet> _bets;

    public RouletteTable(IRouletteWheel wheel)
    {
        _bets = new List<Bet>();
        _wheel = wheel;
    }
    public void CashIn(int amount)
    {
        Pot = amount;
    }

    public double Pot { get; private set; }

    public void Bet(BetType type, string bet, int amount)
    {
        Pot -= amount;
        switch (type)
        {
            case BetType.Colour:
                _bet = new ColourBet(bet, amount);
                _bets.Add(new ColourBet(bet, amount));
                break;
            case BetType.Number:
                _bet = new NumberBet(bet, amount);
                _bets.Add(new NumberBet(bet, amount));
                break;
            case BetType.Column:
                _bet = new ColumnBet(bet, amount);
                _bets.Add(new ColumnBet(bet, amount));
                break;
            default:
                _bet = new NullBet();
                break;
        }
    }

    public void Spin()
    {
        SpinResult result = _wheel.Spin();
        foreach (var bet in _bets)
            Pot += bet.Result(result);
        _bets = new List<Bet>();
    }
}
