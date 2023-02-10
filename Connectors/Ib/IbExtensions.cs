namespace Connectors.Ib;

using IBApi;
using System;
using System.Globalization;
using Connectors.Types;

public static class IbExtensions
{
    public static DateTime ToDateTime(this string ibDate) => DateTime.ParseExact(ibDate, "yyyyMMdd", CultureInfo.CurrentCulture);
    public static Future ToFuture(this ContractDetails contract) => new Future{
        Id = contract.Contract.ConId,
        Name = contract.Contract.LocalSymbol,
        Exchange = contract.Contract.Exchange,
        LastTradeDate = contract.Contract.LastTradeDateOrContractMonth.ToDateTime(),
        Multiplier = int.Parse(contract.Contract.Multiplier),
        Symbol = contract.Contract.Symbol
    };

    public static Option ToOption(this ContractDetails contract) => new Option{
        Id = contract.Contract.ConId,
        Name = contract.Contract.LocalSymbol,
        Exchange = contract.Contract.Exchange,
        LastTradeDate = contract.Contract.LastTradeDateOrContractMonth.ToDateTime(),
        Multiplier = int.Parse(contract.Contract.Multiplier),
        Symbol = contract.Contract.Symbol,
        TradingClass = contract.Contract.TradingClass,
        Strike = contract.Contract.Strike
    };
}