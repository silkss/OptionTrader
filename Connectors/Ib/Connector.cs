namespace Connectors.Ib;

using IBApi;
using Types;
using Events;
using System;
using System.Threading;
using System.Collections.Generic;

public class Connector : DefaultEWrapper, IConnector
{
	private readonly EClientSocket _client;
	private readonly EReaderSignal _signal;
	private readonly List<Future> _cachedFutures = new();
	private int _orderId; 
	private Future? getFuturesById(int id)
	{
		foreach (var fut in _cachedFutures)
		{
			if (fut.Id == id)
			{
				return fut;
			}
		}
		return null;
	}
	public Connector()
	{
		_signal = new EReaderMonitorSignal();
		_client = new EClientSocket(this, _signal);
	}

	public event ConnectorEventHandler OnConnected = delegate { };
	public event ConnectorEventHandler OnDisconnected  = delegate { };
	public event ConnectorEventHandler<OptionAddedEventArgs> OnOptionAdded = delegate { };
	public event ConnectorEventHandler<PriceEventArgs> OnPriceChanged = delegate { };

	public (string symbol, string exchange)[] Symbols { get; } = new[] {
		("EUR", "CME")};
	public IEnumerable<Future> GetCachedFutures() => _cachedFutures;
	public void Connect() => Connect("127.0.0.1", 7497, 12);
	public void Connect(string host, int port, int id)
	{
		_client.eConnect(host, port, id);
		var reader = new EReader(_client, _signal);
		reader.Start();

		new Thread(() => {
			while (_client.IsConnected()){
				_signal.waitForSignal();
				reader.processMsgs();
			}
		}) { IsBackground = true }
		.Start();

		_client.reqMarketRule(3);
	}
	public void UpdateCachedFutures()
	{
		_cachedFutures.Clear();
		foreach ( var (symbol, exchange) in Symbols)
		{
			var contract = new Contract() {
				Symbol = symbol,
				Exchange = exchange,
				Currency = "USD",
				SecType = "FUT"
			};
			_client.reqContractDetails(0, contract);
		}
	}
	public void RequestMarketData(Future future) { }
	public void RequestMarketData(Option option) { }
	public void RequestOption(Future parent, double strike, OptionType type, DateTime expiration)
	{
		var contract = new Contract() {
			Symbol = parent.Symbol,
			Strike = strike,
			Right = type == OptionType.Call ? "C" : "P",
			LastTradeDateOrContractMonth = expiration.ToString("yyyyMMdd"),
			Exchange = parent.Exchange,
			SecType = "FOP",
		};
		_client.reqContractDetails(0, contract);
	}
    public override void connectAck() => OnConnected(this);
	public override void nextValidId(int orderId) => _orderId = orderId;
	public override void contractDetails(int reqId, ContractDetails contractDetails)
	{
		if (contractDetails.Contract.SecType == "FUT")
		{
			var fut = contractDetails.ToFuture();
			_cachedFutures.Add(fut);
			_client.reqSecDefOptParams(0, fut.Symbol, fut.Exchange, "FUT", fut.Id);
			return;
		}
		if (contractDetails.Contract.SecType == "FOP")
		{
			throw new NotImplementedException("Option instument received");
		}
	}
	public override void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
    {
		if (getFuturesById(underlyingConId) is Future fut)
		{	
			foreach (var expiration in expirations)
			{
				fut.AddOptionTradingClass(
					new OptionTradingClass(underlyingConId, tradingClass, expiration.ToDateTime(), strikes)
				);
			}
		}
    } 
	private void log(string msg) => Console.WriteLine(msg);
    public override void error(Exception e) => log(e.Message);
    public override void error(int id, int errorCode, string errorMsg, string advancedOrderRejectJson)
    {
		var msg = $"{errorCode}::{errorMsg}";
		log(msg);
    }
    public override void error(string str) => log(str);
}