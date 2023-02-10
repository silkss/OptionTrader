namespace Connectors.Types;

public class Option : Base.Instrument
{
    public double Strike { get; init; }
    public string? TradingClass { get; init; }
}