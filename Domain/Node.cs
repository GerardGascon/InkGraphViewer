using Ink.Runtime;

namespace Domain;

public class Node {
	public readonly List<string> Lines = [];
	public List<Node> Nodes { get; } = [];
	public List<Connection> NodeConnections { get; } = [];

	private readonly Story _story;

	public Node(Story story, string stateJson) {
		_story = story;
		_story.state.LoadJson(stateJson);
	}

	private string GetCurrentNodeId() => _story.state.currentPathString ?? "root";

	public void ExploreRecursively() {
		string id = GetCurrentNodeId();
		string text = _story.ContinueMaximally();
		Lines.AddRange(text.Split('\n', StringSplitOptions.RemoveEmptyEntries));

		for (int i = 0; i < _story.currentChoices.Count; i++) {
			string stateBeforeChange = _story.state.ToJson();
			_story.ChooseChoiceIndex(i);
			AddNode(Connection.ConnectionType.Choice);
			_story.state.LoadJson(stateBeforeChange);
		}
	}

	private void AddNode(Connection.ConnectionType connectionType) {
		Node node = new(_story, _story.state.ToJson());
		switch (connectionType) {
			case Connection.ConnectionType.Choice:
				AddChoiceConnection(node);
				break;
		}
		Nodes.Add(node);
		node.ExploreRecursively();
	}

	private void AddChoiceConnection(Node node) {
		const Connection.ConnectionType connectionType = Connection.ConnectionType.Choice;
		string name = _story.Continue().TrimEnd('\r', '\n');
		Connection connection = new(connectionType, name, node);
		NodeConnections.Add(connection);
	}
}