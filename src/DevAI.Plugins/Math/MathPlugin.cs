using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace DevAI.Plugins.Math;

public class MathPlugin
{
    [KernelFunction]
    [Description("Adds two numbers")]
    public int Add(int a, int b)
    {
        return a + b;
    }

    [KernelFunction]
    [Description("Multiplies two numbers")]
    public int Multiply(int a, int b)
    {
        return a * b;
    }
}