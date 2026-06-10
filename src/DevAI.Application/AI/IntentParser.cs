
using DevAI.Application.AI;
using System.Text.RegularExpressions;

namespace DevAI.Domain.Agents;

public class IntentResult
{
    public IntentType Intent { get; set; }
    public string RawQuery { get; set; } = string.Empty;
}


//public class IntentParser
//{
//    public IntentResult Parse(string input)
//    {
//        input = input.ToLower();

//        var numbers = Regex.Matches(input, @"-?\d+")
//            .Select(m => int.Parse(m.Value))
//            .ToArray();

//        var intent = AIIntent.Unknown;

//        // STRICT MULTIPLICATION SIGNALS
//        if (input.Contains("multiply") ||
//            input.Contains("x") ||
//            input.Contains("*") ||
//            input.Contains("into"))
//        {
//            intent = AIIntent.Multiply;
//        }

//        // STRICT ADDITION SIGNALS
//        if (input.Contains("add") ||
//            input.Contains("plus") ||
//            input.Contains("+"))
//        {
//            intent = AIIntent.Add;
//        }

//        return new IntentResult
//        {
//            Intent = intent,
//            Numbers = numbers
//        };
//    }
//}