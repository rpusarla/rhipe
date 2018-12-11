using System;
using NUnit.Framework;
using Rhipe.Models;
using Rhipe.Repository;
using Rhipe.Shared;
using FluentAssertions;

namespace Rhipe.Tests
{
    [TestFixture]
    public class Tests
    {
        private IParse _parse;
        private string _validIsoscelesTriangleInput;
        private string _validEquilateralTriangleInput;
        private string _validScaleneTriangleInput;        
        private string _missingTriangleNameInput;
        private string _invalidScaleneTriangleInput;
        private string _invalidEquilateralTriangleInput;
        private string _triangleInEqualityInput;

        [OneTimeSetUp]
        public void Setup()
        {
            _parse = new Parse();            
            _validIsoscelesTriangleInput = "Draw an Isosceles Triangle with a base of 200 and height of 100";
            _validEquilateralTriangleInput = "Draw an equilateral triangle with a side of 200";
            _validScaleneTriangleInput = "Draw a Scalene Triangle with a base of 200 and a side of 100 and side of 150";
            _missingTriangleNameInput = "Draw a Circle with a base of 100 and a side of 200";
            _invalidScaleneTriangleInput = "Draw a Scalene Triangle with a side of 200 and a side of 300";
            _invalidEquilateralTriangleInput = "Draw an Equilateral Triangle with a side of 200 and side of 300";
            _triangleInEqualityInput =
                "Draw a Scalene Triangle with a side of 200 and a side of 100 and a side of 250";
        }

        [Test]
        public void ValidateIsoscelesTriangleDimensionsForValidInput()
        {
            var expectedValue = _parse.ParseData(_validIsoscelesTriangleInput);            
            var actualValue = new Token()
            {
                TriangleName = Constants.IsoscelesTriangle,
                Base = 200,                
                Side1 = 282.842712474619,
                Side2 = 282.842712474619
            };
            actualValue.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void ValidateEquilateralTriangleDimensionsForValidInput()
        {
            var expectedValue = _parse.ParseData(_validEquilateralTriangleInput);            
            var actualValue = new Token()
            {
                TriangleName = Constants.EquilateralTriangle,
                Base = 200,                
                Side1 = 200,
                Side2 = 200
            };
            actualValue.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void ValidateScaleneTriangleDimensionsForValidInput()
        {
            var expectedValue = _parse.ParseData(_validScaleneTriangleInput);            
            var actualValue = new Token()
            {
                TriangleName = Constants.ScaleneTriangle,
                Base = 200,                
                Side1 = 100,
                Side2 = 150
            };
            actualValue.Should().BeEquivalentTo(expectedValue);
        }

        [Test]
        public void MissingTriangleNameInputShouldThrowException()
        {
            var exception = Exceptions.InvalidInputError;
            try
            {
                var expectedValue = _parse.ParseData(_missingTriangleNameInput);                
                
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exception);
            }            
        }
        [Test]
        public void InvalidScaleneTriangleDimensionsShouldThrowException()
        {
            var exception = Exceptions.ScaleneTriangleError;
            try
            {
                var expectedValue = _parse.ParseData(_invalidScaleneTriangleInput);                

            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exception);
            }
        }
        [Test]
        public void InvalidEquilateralTriangleDimensionsShouldThrowException()
        {
            var exception = Exceptions.EquilateralTriangleError;
            try
            {
                var expectedValue = _parse.ParseData(_invalidEquilateralTriangleInput);                

            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exception);
            }
        }

        [Test]
        public void InvalidDimensionsShouldThrowTriangleInEqualityException()
        {
            var exception = Exceptions.TriangleInequalityError;
            try
            {
                var expectedValue = _parse.ParseData(_triangleInEqualityInput);                

            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, exception);
            }
        }
    }
}