namespace Connectors.Events;

public delegate void ConnectorEventHandler(IConnector connector);
public delegate void ConnectorEventHandler<EventArgs>(IConnector connector, EventArgs args);