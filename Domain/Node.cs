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

	private Node Search(Node origin, string movements) {
		string[] tokens = movements.Split('.');
		Queue<string> actions = new();
		foreach (string token in tokens) {
			actions.Enqueue(token);
		}

		JToken startPosition = movements[0] == '.' ? rootToken : rootToken;
		return Search(origin, startPosition, actions);
	}

	private Node Search(Node origin, JToken position, Queue<string> movements) {
		if (!movements.TryDequeue(out string movement)) {

		}

		if (int.TryParse(movement, out int index)) {
			return SearchByIndex(origin, position, index, movements);
		}

		if (movement == "^") {

		}
	}

	private Node SearchByIndex(Node origin, JToken position, int index, Queue<string> movements) {
		int i = 0;
		foreach (JToken token in (JArray)position) {
			if (token is not JObject obj)
				continue;
			if (i == index)
				return Search(origin, obj, movements);
			i++;
		}

		throw new IndexOutOfRangeException();
	}

	private void GenerateChildNode(JArray token) {
		Node node = new(token, this);
		Connection connection = new("paiosduhyf", Connection.ConnectionType.Choice, node);
		OutgoingConnections.Add(connection);
	}

	private static string GetKnotText(JValue text) => ((string)text)?[1..];
}