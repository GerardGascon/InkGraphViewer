#nullable enable
using Newtonsoft.Json.Linq;

namespace Domain;

public partial class Node {
	private void ReadOption(JArray array) {
		GetChoiceProperties(array, out string jumpIndication, out string choiceName);
		Console.WriteLine(jumpIndication);
		Console.WriteLine(choiceName);
	}

	private static void GetChoiceProperties(JArray array, out string jumpIndication, out string choiceName) {
		bool inEvaluate = false;
		jumpIndication = string.Empty;
		choiceName = string.Empty;

		foreach (JToken token in array) {
			inEvaluate = InEvaluate(inEvaluate, token);
			if (inEvaluate)
				continue;

			if (token is not JObject obj)
				continue;

			string jump = GetJumpIndication(obj);
			jumpIndication = string.IsNullOrEmpty(jump) ? jumpIndication : jump;
			string name = GetChoiceName(obj);
			choiceName = string.IsNullOrEmpty(name) ? choiceName : name;
		}
	}

	private static string GetChoiceName(JObject obj) {
		JArray? choiceName = (JArray)obj["s"];
		if (choiceName == null)
			return string.Empty;
		return GetKnotText((JValue)choiceName[0]);
	}

	private static string GetJumpIndication(JObject obj) {
		JValue? positionToken = (JValue)obj["*"];
		if (positionToken == null)
			return string.Empty;
		return positionToken.Value?.ToString() ?? string.Empty;
	}

	private static bool InEvaluate(bool inEvaluate, JToken token) {
		if (!inEvaluate && token is JValue { Value: "ev" })
			inEvaluate = true;
		if (inEvaluate && token is JValue { Value: "/ev" })
			inEvaluate = false;

		return inEvaluate;
	}
}