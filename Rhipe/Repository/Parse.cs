using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rhipe.Models;
using Rhipe.Shared;
using Rhipe.ViewModels;

namespace Rhipe.Repository
{    
    public class Parse : IParse
    {
        private const string BaseOfConst = "AND A BASE OF ";
        private const string SideOfConst = "AND A SIDE OF";
        private const string HeightOfConst = "AND A HEIGHT OF ";        
        //private const string BaseOfConst = "BASE OF ";
        //private const string SideOfConst = "SIDE OF";
        //private const string HeightOfConst = "HEIGHT OF ";
        //private const string RegExPattern = @"(\S* (TRIANGLE))|(?:\S* \w+ \d+)";
        private const string RegExPattern = @"((DRAW A(N)?) (ISOSCELES|EQUILATERAL|SCALENE) (TRIANGLE))|(?:((WITH A)|(AND A)) (SIDE OF|BASE OF|HEIGHT OF) \d+)";

        

        public Token ParseData(string inputText)
        {
            try
            {                
                var convertedInput = inputText.Trim().ToUpper();
                var matches = Regex.Matches(convertedInput, RegExPattern);

                var objTokensViewModel = new TokenViewModel()
                {
                    //TriangleName = matches.FirstOrDefault(match => match.Value.Contains("TRIANGLE"))?.Value,                    
                    //TriangleName = matches.FirstOrDefault(match => match.Value.IndexOf("DRAW A ", StringComparison.Ordinal) != -1)?.Value.Replace("DRAW A ", ""),
                    Base = StringToNumber(matches.FirstOrDefault(match => match.Value.IndexOf(BaseOfConst, StringComparison.Ordinal) != -1)?.Value.Replace(BaseOfConst, "")),
                    Side1 = StringToNumber(matches.FirstOrDefault(match => match.Value.IndexOf(SideOfConst, StringComparison.Ordinal) != -1)?.Value.Replace(SideOfConst, "")),
                    Side2 = StringToNumber(matches.LastOrDefault(match => match.Value.IndexOf(SideOfConst, StringComparison.Ordinal) != -1)?.Value.Replace(SideOfConst, "")),
                    Height = StringToNumber(matches.FirstOrDefault(match => match.Value.IndexOf(HeightOfConst, StringComparison.Ordinal) != -1)?.Value.Replace(HeightOfConst, ""))
                };
                if (matches.Any(match => match.Value.IndexOf("DRAW A ", StringComparison.Ordinal) != -1))
                {
                    objTokensViewModel.TriangleName = matches.FirstOrDefault(match => match.Value.IndexOf("DRAW A ", StringComparison.Ordinal) != -1)?.Value.Replace("DRAW A ", "");
                }
                else if(matches.Any(match => match.Value.IndexOf("DRAW AN ", StringComparison.Ordinal) != -1))
                {
                    objTokensViewModel.TriangleName = matches.FirstOrDefault(match => match.Value.IndexOf("DRAW AN ", StringComparison.Ordinal) != -1)?.Value.Replace("DRAW AN ", "");
                }


                // Scalene Triangle
                if (matches.Count(match => match.Value == Constants.ScaleneTriangle) == 1) {
                    if(!(matches.Count(match => match.Value.IndexOf(BaseOfConst, StringComparison.Ordinal) != -1) == 1 &&
                        matches.Count(match => match.Value.IndexOf(SideOfConst, StringComparison.Ordinal) != -1) == 2 &&
                        matches.Count == 4 && objTokensViewModel.Base > 0 && objTokensViewModel.Side1 > 0 && objTokensViewModel.Side2 > 0))
                    {
                        throw new Exception(Exceptions.ScaleneTriangleError);
                    }
                }

                // Isosceles Triangle
                if (matches.Count(match => match.Value == Constants.IsoscelesTriangle) == 1)
                {
                    if (matches.Count(match => match.Value.IndexOf(BaseOfConst, StringComparison.Ordinal) != -1) == 1 &&
                        matches.Count(match => match.Value.IndexOf(HeightOfConst, StringComparison.Ordinal) != -1) == 1 &&
                        matches.Count == 3 && objTokensViewModel.Base > 0 && objTokensViewModel.Height > 0)
                    {
                        objTokensViewModel.Side1 = objTokensViewModel.Side2 = Math.Sqrt((objTokensViewModel.Base * objTokensViewModel.Base) + (4 * objTokensViewModel.Height * objTokensViewModel.Height));
                    }
                    else throw new Exception(Exceptions.ScaleneTriangleError);
                }

                // Equilateral Triangle
                if (matches.Count(match => match.Value == Constants.EquilateralTriangle) == 1)
                {
                    if (matches.Count(match => match.Value.IndexOf(SideOfConst, StringComparison.Ordinal) != -1) == 1 &&
                        matches.Count == 2 && objTokensViewModel.Side1 > 0)
                    {
                        objTokensViewModel.Base = objTokensViewModel.Side2 = objTokensViewModel.Side1;
                    }
                    else throw new Exception(Exceptions.EquilateralTriangleError);
                }

                // Check if the triangle name is valid.
                if (objTokensViewModel.TriangleName == null ||
                    (objTokensViewModel.TriangleName != Constants.IsoscelesTriangle &&
                     objTokensViewModel.TriangleName != Constants.ScaleneTriangle &&
                     objTokensViewModel.TriangleName != Constants.EquilateralTriangle))
                    throw new Exception(Exceptions.InvalidInputError);


                // Check for Triangle inequalities
                // Side1 + Base > Side2
                // Base + Side2 > Side1
                // Side1 + Side2 > Base

                if ((objTokensViewModel.Side1 + objTokensViewModel.Base <= objTokensViewModel.Side2) ||
                    (objTokensViewModel.Base + objTokensViewModel.Side2 <= objTokensViewModel.Side1) ||
                    (objTokensViewModel.Side1 + objTokensViewModel.Side2 <= objTokensViewModel.Base))
                   throw new Exception(Exceptions.TriangleInequalityError);

                return new Token()
                {
                    TriangleName = objTokensViewModel.TriangleName,
                    Base = objTokensViewModel.Base,
                    Side1 = objTokensViewModel.Side1,
                    Side2 = objTokensViewModel.Side2
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private double StringToNumber(string inputText) => double.TryParse(inputText, out double result) ? result : 0;                   
    }
}
