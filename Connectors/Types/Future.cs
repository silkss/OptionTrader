namespace Connectors.Types;

using System.Collections.Generic;

public class Future : Base.Instrument
{
    public List<OptionTradingClass> OptionTradingClasses { get; private set; } = new();

    public void AddOptionTradingClass(OptionTradingClass optionTradingClass)
    {
        if (OptionTradingClasses.Contains(optionTradingClass)) return;
        OptionTradingClasses.Add(optionTradingClass);
    }
}