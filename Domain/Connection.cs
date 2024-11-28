namespace Domain;

public struct Connection {
	public enum ConnectionType {
		Init,
		Choice,
		InDialogueVariable,
		InGameVariable,
	}

	public readonly ConnectionType Type;
	public readonly string Name;

	public readonly Node Destination;

	public Connection(string name, ConnectionType type, Node destination) {
		Type = type;
		Name = name;

		Destination = destination;
		Destination.Explore();
	}
}