namespace Connectors.Events;

using Types;

public class PriceEventArgs : System.EventArgs {
    public PriceEventArgs(int tickerId, Tick tick, decimal price) {
        this.Tick = tick;
        this.Price = price;
    }
    public int TickerId { get; }
    public Tick Tick { get; }
    public decimal Price { get; }
}