namespace RouletteKata;

public enum BetType
{
    Number,
    Colour,
    Column
}

public class NullBet : Bet
{
    protected override int WinMultiplier => 0;
    public NullBet(): base(string.Empty, 0) {}
    
    protected override bool IsWin(SpinResult spinResult) => false;

}

public class ColumnBet : Bet
{
    protected override int WinMultiplier => 3;
    public ColumnBet(string value, int amount): base(value, amount)
    {
    }
    
    protected override bool IsWin(SpinResult spinResult)
    {
        if (int.TryParse(Value, out int number))
            return number == spinResult.Column;
        return false;
    }
}

public class NumberBet : Bet
{
    protected override int WinMultiplier => 36;
    public NumberBet(string value, int amount): base(value, amount)
    {}
    
    protected override bool IsWin(SpinResult spinResult)
    {
        if (int.TryParse(Value, out int number))
            return number == spinResult.Number;
        return false;
    }
}

public class ColourBet : Bet
{
    protected override int WinMultiplier => 2;
    public ColourBet(string value, int amount): base(value, amount){}

    protected override bool IsWin(SpinResult spinResult) => spinResult.Colour == Value;
}

public abstract class Bet
{
    protected string Value { get; }
    private int Amount { get; }
    protected abstract int WinMultiplier { get; }

    protected Bet(string value, int amount)
    {
        Value = value;
        Amount = amount;
    }
    
    public int Result(SpinResult spinResult)
    {
        if (IsWin(spinResult))
            return WinMultiplier * Amount;
        return 0;
    }

    protected abstract bool IsWin(SpinResult spinResult);
}