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
		if (knot.Count == 1)
			return;
		if (IsDoneCommand(knot))
			return;

		Node newNode = new();
		foreach (JToken token in knot) {
			switch (token) {
				case JValue { Value: not null and not "\n" } text:
					newNode.Lines.Add(GetKnotText(text));
					break;
				case JArray subKnot:
					ProcessKnot(subKnot, generatedGraph);
					break;
			}
		}
		generatedGraph.Nodes.Add(newNode);
	}

	private static bool IsDoneCommand(JArray knot) {
		if (knot.Count != 2)
			return false;
		if (knot[0] is not JValue element)
			return false;
		return (string)element.Value! == "done";
	}
}