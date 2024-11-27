namespace Domain;

public class InkGraph {
	public static InkGraph Generate(string json) {
		InkGraph generatedGraph = new() {
			Nodes = new Node[1]
		};

		for (int i = 0; i < generatedGraph.Nodes.Length; i++)
			generatedGraph.Nodes[i] = new Node();

		return generatedGraph;
	}

	public Node[] Nodes { get; private set; }
}