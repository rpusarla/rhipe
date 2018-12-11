using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhipe.Shared
{
    public static class Exceptions
    {
        public static string InvalidInputError =
            "Invalid input provided. Provide input as per 'Draw a(n) <shape> with a(n) <measurement> of <amount> (and a(n) <measurement> of <amount>)' format."
            + Environment.NewLine + "Allowed shape is 'triangle' and allowed measurements are 'base/side/height'."
            + Environment.NewLine +
            "Also, lengths of all sides must be positive numbers. Please correct the input and try again.";

        public static string ScaleneTriangleError =
            "Invalid Measurement for Scalene Triangle. All three sides must be specified for Scalene Triangle and all of them must be positive numbers.";

        public static string IsoscelesTriangleError =
            "Invalid Measurement for Isosceles Triangle. One height and One side must be specified for Isosceles Triangle and both of them must be positive numbers.";

        public static string EquilateralTriangleError =
            "Invalid Measurement for Equilateral Triangle. Only one side must be specified for Equilateral Triangle and it must a positive number.";

        public static string TriangleInequalityError =
            "Triangle Inequalities issue. The sum of the lengths of any two sides must be greater than the length of the third side.";
    }
}
