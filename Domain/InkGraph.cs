namespace Domain;

public class InkGraph {
	public List<Node> Nodes { get; } = [];

	public static InkGraph Generate(string json) {
		InkGraph generatedGraph = new();

		generatedGraph.Nodes.Add(new Node());

		return generatedGraph;
	}
}