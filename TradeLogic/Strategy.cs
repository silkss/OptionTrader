namespace TradeLogic;

using System.Collections.Generic;
using Connectors.Types;
using TradeUnits;
using TradeLogic.CommonTypes;

public class Strategy
{
    public Future? ParentInstrument { get; set; } 
    public List<OptionStrategy> Options { get; } = new();
    public Logic Logic { get; set; }
}