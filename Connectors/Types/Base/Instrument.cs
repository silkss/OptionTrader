namespace Connectors.Types.Base;

using System;

public class Instrument
{
	public int Id {get; init; }
	public string? Name { get; init; }
	public string? Symbol { get; init; }
	public string? Exchange { get; init; }
	public int Multiplier { get; init; }
	public DateTime LastTradeDate { get; init; }
}