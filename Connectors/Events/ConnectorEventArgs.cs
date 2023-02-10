namespace Connectors.Events;

public class InstrumentEventArgs : System.EventArgs
{
    public InstrumentEventArgs(Types.Instrument instrument)
    {
       Instrument = instrument; 
    }

    public Types.Instrument Instrument { get; }
}