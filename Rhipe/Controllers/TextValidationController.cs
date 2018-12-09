using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Rhipe.Shared;
using Rhipe.Models;

namespace Rhipe.Controllers
{
    [Route("api/[controller]")]
    public class TextValidationController : Controller
    {        
        public Token Tokens;
        

        public TextValidationController()
        {            
            Tokens = new Token();
        }

        [HttpGet("[action]")]
        public IActionResult IsInputTextValid(string inputText)
        {
            string pattern = @"(\S* (triangle))|(?:\S* \w+ \d+)";
            var matches = Regex.Matches(inputText, pattern);
            
            BindTokensModel(matches);
            
            return ValidateTokens();
            
        }

        private void BindTokensModel(MatchCollection matches)
        {
            // Scalene Triangle
            if(matches.Count(match => match.Value.ToUpper() == Constants.ScaleneTriangle) == 1 &&
                matches.Count(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                matches.Count(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                matches.Count == 3)
            {
                Tokens.TriangleName = Constants.ScaleneTriangle;
                Tokens.Base = StringToNumber(matches.First(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("base of ", ""));
                Tokens.Height = StringToNumber(matches.Last(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("height of ", ""));
            }

            // Isosceles Triangle
            if (matches.Count(match => match.Value.ToUpper() == Constants.IsoscelesTriangle) == 1 &&
                matches.Count(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                matches.Count(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&
                matches.Count == 3)
            {
                Tokens.TriangleName = Constants.IsoscelesTriangle;
                Tokens.Base = StringToNumber(matches.First(match => match.Value.IndexOf("base of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("base of ", ""));
                Tokens.Height = StringToNumber(matches.Last(match => match.Value.IndexOf("height of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("height of ", ""));                                                
            }

            // Equilateral Triangle
            if (matches.Count(match => match.Value.ToUpper() == Constants.EquilateralTriangle) == 1 &&
                matches.Count(match => match.Value.IndexOf("side of ", StringComparison.OrdinalIgnoreCase) != -1) == 1 &&                
                matches.Count == 2)
            {
                Tokens.TriangleName = Constants.EquilateralTriangle;
                Tokens.Base = Tokens.Side1 = Tokens.Side2 = StringToNumber(matches.First(match => match.Value.IndexOf("side of ", StringComparison.OrdinalIgnoreCase) != -1).Value.Replace("side of ", ""));                
            }            
        }
        
        private double StringToNumber(string inputText) => double.TryParse(inputText, out double result) && result > 0 ? result : -1;

        private IActionResult ValidateTokens()
        {
            if (Tokens.TriangleName == null)
            {
                return BadRequest(
                    "Invalid input provided. Provide input under Draw a(n) <shape> with a(n) <measurement> of <amount> (and a(n) <measurement> of <amount>)");
            }
            else if (Tokens.TriangleName == Constants.ScaleneTriangle && (Tokens.Side1 == -1 || Tokens.Side2 == -1))
            {
                return BadRequest(
                    "Invalid Measurement for Scalene Triangle. Two sides must be specified for Scalene Triangle and both of them must be positive numbers.");
            }
            else if (Tokens.TriangleName == Constants.IsoscelesTriangle && (Tokens.Side1 == -1 || Tokens.Height == -1))
            {
                return BadRequest(
                    "Invalid Measurement for Isosceles Triangle. One height and One side must be specified for Isosceles Triangle and both of them must be positive numbers.");
            }
            else if (Tokens.TriangleName == Constants.EquilateralTriangle && Tokens.Side1 == -1)
            {
                return BadRequest(
                    "Invalid Measurement for Equilateral Triangle. Only one side must be provided for Equilateral Triangle and it must a positive number.");
            }

            if (Tokens.TriangleName == Constants.IsoscelesTriangle)
            {
                Tokens.Side1 = Tokens.Side2 = Math.Sqrt((Tokens.Base * Tokens.Base) + (4 * Tokens.Height * Tokens.Height));
            }

            if (Tokens.TriangleName == Constants.ScaleneTriangle)
            {
                Tokens.Side1 = (Tokens.Height * Tokens.Base) / 2;
                Tokens.Side2 = Math.Sqrt((Tokens.Base * Tokens.Base) + (Tokens.Side1 * Tokens.Side1));
            }

            // Check for Triangle inequalities
            // Side1 + Base > Side2
            // Base + Side2 > Side1
            // Side1 + Side2 > Base
            if ((Tokens.Side1 + Tokens.Base <= Tokens.Side2) ||
                (Tokens.Base + Tokens.Side2 <= Tokens.Side1) ||
                (Tokens.Side1 + Tokens.Side2 <= Tokens.Base))
            {
                return BadRequest(
                    "Triangle Inequalities issue. The sum of the lengths of any two sides must be greater than the length of the third side.");
            }


            return Ok(Tokens);
        }
    }
}
