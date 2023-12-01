using Microsoft.CodeAnalysis;


namespace ScribanTemplatesGen;


[Generator]
public class ScribanTemplatesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.AdditionalTextsProvider.Where(x => x.Path.EndsWith(".scriban"))
            .Select((text, token) =>
            {
                var name = Path.GetFileNameWithoutExtension(text.Path);
                var source = text.GetText(token);
                return (name, text: source?.ToString());
            })
            .Where(x => x.text is not null);

        context.RegisterSourceOutput(provider,
            static (context, arg) =>
            {
                var (name, text) = arg;
                var source = $$""""
namespace ScribanTemplates {
    internal class {{ name }}
    {
        public const string FileName = "{{ name }}.scriban";
        public const string Text = """
{{ text }}
""";

        public static Scriban.Template Parse(
            Scriban.Parsing.ParserOptions? parserOptions = null,
            Scriban.Parsing.LexerOptions? lexerOptions = null)
        {
            return Scriban.Template.Parse(Text, FileName, parserOptions, lexerOptions);
        }
    }
}
"""";
                context.AddSource(name + ".scriban.cs", source);
            });
    }
}