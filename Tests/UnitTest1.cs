using Domain;
using Ink;

namespace Tests;

public class Tests {
	private string CompileInk(string code) {
		Compiler compiler = new(code);
		return compiler.Compile().ToJson();
	}

	[Test]
	public void LinearStory_GeneratesOnlyOneNode() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Assert.That(sut.Nodes, Has.Length.EqualTo(1));
	}

	[Test]
	public void Node_ContainsFirstLineOfText() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Node node = sut.Nodes[0];

		Assert.That(node.Lines[0], Is.EqualTo("Here goes a line."));
	}
}