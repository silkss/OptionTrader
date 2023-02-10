namespace Connectors.Types;

using System;
using System.Collections.Generic;

public record OptionTradingClass(
    int ParentId, 
    string TradeClass, 
    DateTime Expiration, 
    HashSet<double> strikes);
