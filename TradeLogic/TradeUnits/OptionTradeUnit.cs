namespace TradeLogic.TradeUnits;

public class OptionTradeUnit
{
    public Connectors.Types.Instrument? Instrument { get; set; }
    public int Volume { get; set; }
    public int Position { get; set; }
}