using Ink.Runtime;

namespace Domain;

public class InkGraph {
	public List<Node> Nodes { get; } = [];

	private InkGraph(Story story) {
		Nodes.Add(new Node(story, story.state.ToJson()));
		Nodes[^1].ExploreRecursively();
	}

	public static InkGraph Generate(string json) {
		InkGraph generatedGraph = new(new Story(json));
		return generatedGraph;
	}
}