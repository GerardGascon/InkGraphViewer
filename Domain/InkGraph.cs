using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain;

public class InkGraph {
	public List<Node> Nodes { get; } = [];

	public static InkGraph Generate(string json) {
		InkGraph generatedGraph = new();

		JObject? storyData = JsonConvert.DeserializeObject<JObject>(json);

		if (storyData?["root"] is JArray root)
			ParseKnot(root, generatedGraph);

		return generatedGraph;
	}

	private static void ParseKnot(JArray root, InkGraph generatedGraph) {
		foreach (JToken element in root) {
			if (element is not JArray knot)
				continue;

			ProcessKnot(knot, generatedGraph);
		}
	}

	private static string GetKnotText(JValue text) => ((string)text!)[1..];
	private static void ProcessKnot(JArray knot, InkGraph generatedGraph) {
		Node newNode = new();
		foreach (JToken token in knot) {
			if (token is JValue { Value:  not null and not "\n" } text) {
				newNode.Lines.Add(GetKnotText(text));
			}
		}
		generatedGraph.Nodes.Add(newNode);
	}
}