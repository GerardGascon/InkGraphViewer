using Ink.Runtime;

namespace Domain;

public class Node {
	public readonly List<string> Lines = [];
	public List<Node> Nodes { get; } = [];
	public List<string> NodeConnections { get; } = [];

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
			AddNode(isChoice: true);
			_story.state.LoadJson(stateBeforeChange);
		}
	}

	private void AddNode(bool isChoice = false) {
		NodeConnections.Add(isChoice ? _story.Continue() : string.Empty);
		Node node = new(_story, _story.state.ToJson());
		Nodes.Add(node);
		node.ExploreRecursively();
	}
}