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

		Assert.That(sut.Nodes, Has.Count.EqualTo(1));
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

	[Test]
	public void Node_ContainsAllLinesOfText() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Node node = sut.Nodes[0];
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

		Assert.Multiple(() => {
			Assert.That(sut.Nodes, Has.Count.EqualTo(1));
			Assert.That(sut.Nodes[0].Nodes, Has.Count.EqualTo(2));
		});
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

		InkGraph sut = InkGraph.Generate(json);

		Assert.Multiple(() => {
			Assert.That(sut.Nodes[0].Lines, Has.Count.EqualTo(1));
			Assert.That(sut.Nodes[0].Lines[0], Is.EqualTo("Here goes a line."));
		});
	}

	[Test]
	public void BranchingDialogueNodes_HaveAllLinesOfText() {
		string json = CompileInk("""
		                         Here goes a line.
		                         * Select an option
		                             You've selected an option.
		                         * Select another option
		                             You've selected another option.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Assert.Multiple(() => {
			Assert.That(sut.Nodes[0].Nodes, Has.Count.EqualTo(2));
			Assert.That(sut.Nodes[0].Nodes[0].Lines[0], Is.EqualTo("You've selected an option."));
			Assert.That(sut.Nodes[0].Nodes[1].Lines[0], Is.EqualTo("You've selected another option."));
		});
	}

	[Test]
	public void BranchingDialogueConnections_HaveQuestionBlock() {
		string json = CompileInk("""
		                         Here goes a line.
		                         * Select an option
		                             You've selected an option.
		                         * Select another option
		                             You've selected another option.
		                         """);

		InkGraph sut = InkGraph.Generate(json);

		Assert.Multiple(() => {
			Assert.That(sut.Nodes[0].NodeConnections, Has.Count.EqualTo(2));
			Assert.That(sut.Nodes[0].NodeConnections[0], Is.EqualTo("Select an option"));
			Assert.That(sut.Nodes[0].NodeConnections[1], Is.EqualTo("Select another option"));
		});
	}
}