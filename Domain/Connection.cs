namespace Domain;

public struct Connection {
	public enum ConnectionType {
		Choice,
		InDialogueVariable,
		InGameVariable,
	}

	public readonly ConnectionType Type;
	public readonly string Name;
	public readonly Node Destination;

	public Connection(Connection.ConnectionType type, string name, Node node) {
		Type = type;
		Name = name;
		Destination = node;

		node.ExploreRecursively();
	}
}