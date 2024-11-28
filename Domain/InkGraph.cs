using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain;

public class InkGraph : Node {
	private InkGraph(JArray rootToken, Node parent) : base(rootToken, parent) {
		foreach (JToken element in rootToken) {
			if (element is not JArray knot)
				continue;

			OutgoingConnections.Add(
				new Connection("Init", Connection.ConnectionType.Init, new Node(knot, this)));
		}
	}

	public static InkGraph Generate(string json) {
		JObject storyData = JsonConvert.DeserializeObject<JObject>(json);
		InkGraph generatedGraph = new(storyData["root"] as JArray, null);
		return generatedGraph;
	}
}