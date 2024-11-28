namespace Domain;

public struct Connection(Connection.ConnectionType type, string name, Node node) {
	public enum ConnectionType {
		Choice,
		InDialogueVariable,
		InGameVariable,
	}

	public readonly ConnectionType Type = type;
	public readonly string Name = name;
	public readonly Node Destination = node;
}