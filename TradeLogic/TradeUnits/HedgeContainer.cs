namespace TradeLogic.TradeUnits;

using System.Collections.Generic;

public class HedgeContainer
{
    public List<HedgeLevel> Levels { get; init; } = new();
}