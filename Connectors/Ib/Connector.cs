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

	public Connector()
	{
		_signal = new EReaderMonitorSignal();
		_client = new EClientSocket(this, _signal);
	}

	public event ConnectorEventHandler OnConnected = delegate { };
	public event ConnectorEventHandler OnDisconnected  = delegate { };
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
		foreach ( var symbol in Symbols)
		{
			_client.reqContractDetails(0, new Contract{
				Symbol = symbol.symbol,
				Exchange = symbol.exchange,
				Currency = "USD",
				SecType = "FUT"
			});
		}
	}
	public void RequestMarketData(Future future)
	{

	}
	public void RequestMarketData(Option option)
	{

	}

    public override void connectAck() => OnConnected(this);
	public override void nextValidId(int orderId) => _orderId = orderId;
	public override void contractDetails(int reqId, ContractDetails contractDetails)
	{
		if (contractDetails.Contract.SecType == "FUT")
		{
			var fut = contractDetails.ToFuture();
			_cachedFutures.Add(fut);
			return;
		}
		if (contractDetails.Contract.SecType == "FOP")
		{
			throw new NotImplementedException("Option instument received");
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