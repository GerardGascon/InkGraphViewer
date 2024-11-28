using Newtonsoft.Json.Linq;

namespace Domain;

public class Utils {
	public static bool IsDoneCommand(JArray knot) {
		if (knot.Count != 2)
			return false;
		if (knot[0] is not JValue element)
			return false;
		return (string)element.Value! == "done";
	}
}