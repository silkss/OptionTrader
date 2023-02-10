namespace Connectors.Types;

using System;

public class Instrument
{
	public int Id {get; }
	public string Name { get; }
	public string Symbol { get; }
	public string Exchange { get; }
	public int Multiplier { get; }
	public DateTime LastTradeDate { get; }
}