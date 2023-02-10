namespace Connectors;

using Events;
public interface IConnector
{
	event ConnectorEventHandler OnConnected;
	event ConnectorEventHandler OnDisconnected;
	event ConnectorEventHandler<InstrumentEventArgs>  OnInstrumentAdded;

	void Connect();
	void Connect(string host, int port, int id);
}
