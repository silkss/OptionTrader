namespace Connectors;

public interface IConnector
{
	void Connect();
	void Connect(string host, int port, int id);
}
