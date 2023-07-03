using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace MyGenerator
{
    [Generator]
    public sealed class MyIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif
            var compilerOptions = context.CompilationProvider.Select((comp, _) =>
            {
                Console.WriteLine(comp.Options);
                return comp.Options;
            });

            context.RegisterSourceOutput(compilerOptions, static (productionContext, options) =>
            {
                productionContext.AddSource("Generator.g.cs", $@"
using System;

namespace Generator
{{
    public static class Test 
    {{
        public static string Text = ""Hello world 5!"";
    }}
}}
");
            });
        }
    }
}