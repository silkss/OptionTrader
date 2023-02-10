namespace Connectors.Ib;

using IBApi;

public class Connector : DefaultEWrapper, IConnector
{
	public void Connect() => Connect("127.0.0.1", 7497, 12);

	public void Connect(string host, int port, int id)
	{

	}
}