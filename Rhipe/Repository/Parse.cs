using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rhipe.Models;
using Rhipe.Shared;

namespace Rhipe.Repository
{
    public class Parse : IParse
    {
        public Token BindTokens(string inputText)
        {
            try
            {
                string pattern = @"(\S* (triangle))|(?:\S* \w+ \d+)";
                var matches = Regex.Matches(inputText, pattern);

                Token tokens = new Token();

                // Scalene Triangle
                if (matches.Count(match => match.Value.ToUpper() == Constants.ScaleneTriangle) == 1 &&
                    matches.Count(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                    matches.Count(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                    matches.Count == 3)
                {
                    tokens.TriangleName = Constants.ScaleneTriangle;
                    tokens.Base = StringToNumber(matches.First(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("base of ", ""));
                    tokens.Height = StringToNumber(matches.Last(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("height of ", ""));
                }

                // Isosceles Triangle
                if (matches.Count(match => match.Value.ToUpper() == Constants.IsoscelesTriangle) == 1 &&
                    matches.Count(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                    matches.Count(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                    matches.Count == 3)
                {
                    tokens.TriangleName = Constants.IsoscelesTriangle;
                    tokens.Base = StringToNumber(matches.First(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("base of ", ""));
                    tokens.Height = StringToNumber(matches.Last(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("height of ", ""));
                }

                // Equilateral Triangle
                if (matches.Count(match => match.Value.ToUpper() == Constants.EquilateralTriangle) == 1 &&
                    matches.Count(match => match.Value.IndexOf("side of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                    matches.Count == 2)
                {
                    tokens.TriangleName = Constants.EquilateralTriangle;
                    tokens.Base = tokens.Side1 = tokens.Side2 = StringToNumber(matches.First(match => match.Value.IndexOf("side of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("side of ", ""));
                }

                return tokens;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private double StringToNumber(string inputText) => double.TryParse(inputText, out double result) && result > 0 ? result : -1;        

        public Token ValidateTokens(Token tokens)
        {
            try
            {
                if (tokens.TriangleName == null)
                {
                    throw new Exception("Invalid input provided. Provide input under Draw a(n) <shape> with a(n) <measurement> of <amount> (and a(n) <measurement> of <amount>)");
                }

                if (tokens.TriangleName == Constants.ScaleneTriangle && (tokens.Side1 == -1 || tokens.Side2 == -1))
                {
                    throw new Exception("Invalid Measurement for Scalene Triangle. Two sides must be specified for Scalene Triangle and both of them must be positive numbers.");
                }

                if (tokens.TriangleName == Constants.IsoscelesTriangle && (tokens.Side1 == -1 || tokens.Height == -1))
                {
                    throw new Exception("Invalid Measurement for Isosceles Triangle. One height and One side must be specified for Isosceles Triangle and both of them must be positive numbers.");
                }

                if (tokens.TriangleName == Constants.EquilateralTriangle && tokens.Side1 == -1)
                {
                    throw new Exception("Invalid Measurement for Equilateral Triangle. Only one side must be provided for Equilateral Triangle and it must a positive number.");
                }

                // Check for Triangle inequalities
                // Side1 + Base > Side2
                // Base + Side2 > Side1
                // Side1 + Side2 > Base

                if ((tokens.Side1 + tokens.Base <= tokens.Side2) ||
                    (tokens.Base + tokens.Side2 <= tokens.Side1) ||
                    (tokens.Side1 + tokens.Side2 <= tokens.Base))
                {
                    throw new Exception("Triangle Inequalities issue. The sum of the lengths of any two sides must be greater than the length of the third side.");
                }


                if (tokens.TriangleName == Constants.IsoscelesTriangle)
                {
                    tokens.Side1 = tokens.Side2 = Math.Sqrt((tokens.Base * tokens.Base) + (4 * tokens.Height * tokens.Height));
                }

                if (tokens.TriangleName == Constants.ScaleneTriangle)
                {
                    tokens.Side1 = (tokens.Height * tokens.Base) / 2;
                    tokens.Side2 = Math.Sqrt((tokens.Base * tokens.Base) + (tokens.Side1 * tokens.Side1));
                }


                return tokens;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
