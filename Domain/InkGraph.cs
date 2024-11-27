namespace Domain;

public class InkGraph {
	public static InkGraph Generate(string json) {
		InkGraph generatedGraph = new() {
			Nodes = new Node[1]
		};

		return generatedGraph;
	}

	public Node[] Nodes { get; private set; }
}