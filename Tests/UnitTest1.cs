using Ink;

namespace Tests;

public class Tests {
	private string CompileInk(string code) {
		Compiler compiler = new(code);
		return compiler.Compile().ToJson();
	}

	[SetUp]
	public void Setup() { }

	[Test]
	public void Test1() {
		string json = CompileInk("""
		                         Here goes a line.
		                         And here goes another.
		                         """);
	}
}