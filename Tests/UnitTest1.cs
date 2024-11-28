using Domain;
using Ink;

namespace Tests;

public class Tests {
	private static string CompileInk(string code) {
		Compiler compiler = new(code);
		return compiler.Compile().ToJson();
	}

	[Test]
	public void Story_GeneratesAnEntryNodeWithRootConnectionType() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);

		Console.WriteLine(json);

		InkGraph sut = InkGraph.Generate(json);

		Assert.That(sut.OutgoingConnections, Has.Count.EqualTo(1));
		Assert.That(sut.OutgoingConnections[0].Type, Is.EqualTo(Connection.ConnectionType.Init));
	}

	[Test]
	public void LinearStory_GeneratesOnlyOneNode() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);

		Console.WriteLine(json);

		InkGraph sut = InkGraph.Generate(json);

		Assert.That(sut.OutgoingConnections, Has.Count.EqualTo(1));
	}

	[Test]
	public void Node_ContainsFirstLineOfText() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Node node = sut.OutgoingConnections[0].Destination;

		Assert.That(node.Lines[0], Is.EqualTo("Here goes a line."));
	}

	[Test]
	public void Node_ContainsAllLinesOfText() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Node node = sut.OutgoingConnections[0].Destination;
		Assert.Multiple(() => {
			Assert.That(node.Lines, Has.Count.EqualTo(2));
			Assert.That(node.Lines[0], Is.EqualTo("Here goes a line."));
			Assert.That(node.Lines[1], Is.EqualTo("And here goes another."));
		});
	}

	[Test]
	public void BranchingDialogue_GeneratesMultipleNodes() {
		string json = CompileInk("""
		                         Here goes a line.
		                         * Select an option
		                             You've selected an option.
		                         * Select another option
		                             You've selected another option.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Assert.That(sut.OutgoingConnections[0].Destination.OutgoingConnections, Has.Count.EqualTo(2));
	}

	[Test]
	public void BranchingDialogue_KeepsBaseNode() {
		string json = CompileInk("""
		                         Here goes a line.
		                         * Select an option
		                             You've selected an option.
		                         * Select another option
		                             You've selected another option.
		                         """);
		Console.WriteLine(json);

		InkGraph sut = InkGraph.Generate(json);

		Assert.Multiple(() => {
			Assert.That(sut.OutgoingConnections[0].Destination.OutgoingConnections[0].Destination.Lines, Has.Count.EqualTo(1));
			Assert.That(sut.OutgoingConnections[0].Destination.OutgoingConnections[0].Destination.Lines[0], Is.EqualTo("Here goes a line."));
		});
	}
}