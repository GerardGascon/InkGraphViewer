#nullable enable
using Newtonsoft.Json.Linq;

namespace Domain;

public partial class Node(JArray rootToken, Node parent) {
	public readonly List<string> Lines = [];

	public readonly List<Connection> IncomingConnections = [];
	public readonly List<Connection> OutgoingConnections = [];

	private readonly Node _parent = parent;

	public void Explore() {
		if (rootToken.Count == 1) // #n
			return;

		foreach (JToken token in rootToken) {
			switch (token) {
				case JValue { Value: not null and not "\n" } text:
					Lines.Add(GetKnotText(text));
					break;
				case JArray array:
					ReadOption(array);
					break;
			}
		}
	}

	private void Search(Node origin, string movements) {

	}

	private void GenerateChildNode(JArray token) {
		Node node = new(token, this);
		Connection connection = new("paiosduhyf", Connection.ConnectionType.Choice, node);
		OutgoingConnections.Add(connection);
	}

	private static string GetKnotText(JValue text) => ((string)text)?[1..];
}