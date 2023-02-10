namespace Connectors;

using Events;
using System.Collections.Generic;
using Types;

public interface IConnector
{
	event ConnectorEventHandler OnConnected;
	event ConnectorEventHandler OnDisconnected;
	event ConnectorEventHandler<PriceEventArgs> OnPriceChanged;

	IEnumerable<Future> GetCachedFutures();
	void Connect();
	void Connect(string host, int port, int id);
	void UpdateCachedFutures();
	void RequestMarketData(Future future);
	void RequestMarketData(Option option);
}
