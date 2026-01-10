using DragonFruit2.GeneratorSupport;
using System.Numerics;
using System.Text;

namespace DragonFruit2.Generators;

internal class StringBuilderWrapper
{
    private readonly int indentSize = 4;
    private string indent = "";

    private readonly StringBuilder _sb = new();
    public void AppendLine(string line)
    {
        _sb.AppendLine($"{indent}{line}");
    }
    public void AppendLine()
    {
        _sb.AppendLine();
    }
    public void Append(string line)
    {
        _sb.Append($"{indent}{line}");
    }

    public void AppendLines(IEnumerable<string> lines)
    {
        foreach (string line in lines)
        {
            AppendLine(line);
        }
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    public void OpenCurly()
    {
        AppendLine($$"""{""");
        indent += new string(' ', indentSize);
    }

    public void CloseCurly(bool closeParens = false, bool endStatement = false)
    {
        indent = indent.Substring(indentSize);
        AppendLine($$"""}{{(closeParens ? ")" : "")}}{{(endStatement ? ";" : "")}}""");
    }

    public void OpenNamespace(string? namespaceName)
    {
        AppendLine();
        if (!string.IsNullOrEmpty(namespaceName))
        {
            AppendLine($"namespace {namespaceName}");
            OpenCurly();
        }
    }

    public void CloseNamespace( string? namespaceName)
    {
        if (!string.IsNullOrEmpty(namespaceName))
        {
            CloseCurly();
        }
    }

    public void CloseClass()
       => CloseCurly();

    public void OpenMethod(string line, string? baseCtor = null)
    {
        AppendLine();
        AppendLine(line);
        if (baseCtor is not null)
        {
            AppendLine($"    : {baseCtor}");
        }
        OpenCurly();
    }

    public void CloseMethod()
       => CloseCurly();

    internal void OpenIf(string condition)
    {
        AppendLine($"if ({condition})");
        OpenCurly();
    }
    internal void CloseIf() => CloseCurly();

    internal void OpenForEach(string loopString)
    {
        AppendLine($"foreach ({loopString})");
        OpenCurly();
    }
    internal void CloseForEach() => CloseCurly();
}
