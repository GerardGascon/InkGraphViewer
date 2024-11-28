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
		generatedGraph.Nodes.Add(newNode);
		bool wasInEvaluation = false;
		foreach (JToken token in knot) {
			switch (token) {
				case JValue { Value: not null and not "\n" } text:
					bool inEvaluation = IsInEvaluation(text, wasInEvaluation);
					if (inEvaluation || wasInEvaluation) {
						wasInEvaluation = inEvaluation;
						continue;
					}
					newNode.Lines.Add(GetKnotText(text));
					break;
				case JObject subKnot:
					ParseKnotObject(subKnot, generatedGraph);
					break;
			}
		}
	}

	private static bool IsDoneCommand(JArray knot) {
		if (knot.Count != 2)
			return false;
		if (knot[0] is not JValue element)
			return false;
		return (string)element.Value! == "done";
	}

	private static bool IsInEvaluation(JValue value, bool wasInEvaluation) =>
		wasInEvaluation switch {
			false when value is { Value: "ev" } => true,
			true when value is { Value: "/ev" } => false,
			_ => wasInEvaluation
		};

	private static void ParseKnotObject(JObject knot, InkGraph generatedGraph) {
		foreach (JProperty property in knot.Properties()) {
			if (property.Value is not JArray subKnot)
				continue;

			ProcessKnot(subKnot, generatedGraph);
		}
	}
}