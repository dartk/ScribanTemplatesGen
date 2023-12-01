# ScribanTemplatesGen

C# Source generator that generates static template classes from `*.scriban` files.

## Example

- Create a file `Hello.scriban` with the following content

    ```scriban
    Hello, {{ arg }}!
    ```

- Add it to the project as `AdditionalFile`

    ```xml
    <ItemGroup>
        <AdditionalFiles Include="Hello.scriban" />
    </ItemGroup>
    ```

- The generator will create an internal class `ScribanTemplates.Hello`

    ```c#
    namespace ScribanTemplates {
        internal class Hello
        {
            public const string FileName = "Hello.scriban";
            public const string Text = """
    Hello, {{ arg }}!
    """;

            public static Scriban.Template Parse(
                Scriban.Parsing.ParserOptions? parserOptions = null,
                Scriban.Parsing.LexerOptions? lexerOptions = null)
            {
                return Scriban.Template.Parse(Text, FileName, parserOptions, lexerOptions);
            }
        }
    }
    ```
  
- Now you can use the generated class `ScribanTemplates.Hello`

    ```c#
    Scriban.Template helloTemplate = ScribanTemplates.Hello.Parse();
    Console.WriteLine(helloTemplate.Render(new { arg = "World" });    // Hello, World!
    ```