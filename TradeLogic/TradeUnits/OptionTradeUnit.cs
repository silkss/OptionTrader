namespace TradeLogic.TradeUnits;

using Connectors.Types;
using TradeLogic.CommonTypes;

public class OptionTradeUnit
{
    public Logic Logic { get; set; } 
    public Directions Direction { get; set; }
    public Option? Instrument { get; set; }
    public int Volume { get; set; }
    public int Position { get; set; }
}