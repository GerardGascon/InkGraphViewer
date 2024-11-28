using Ink.Runtime;

namespace Domain;

public class Node {
	public readonly List<string> Lines = [];

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
	}
}