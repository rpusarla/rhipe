using System;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using NUnit.Framework;
using Rhipe.Models;
using Rhipe.Repository;

namespace Tests
{
    public class Tests
    {
        private readonly IParse _parse;
        private readonly string inputText = "Draw an Isosceles Triangle with a base of 100 and height of 200";
        public Token Tokens { get; private set; }

        public Tests(IParse parse)
        {
            _parse = parse;
        }
        [SetUp]
        public void Setup()
        {
            Tokens = new Token();
            Tokens = _parse.BindTokens(inputText);
        }

        [Test]
        public void Test1()
        {
            var test = _parse.ValidateTokens(Tokens);
            Assert.AreEqual(test.TriangleName, "ISOSCELES TRIANGLE");                        
        }
    }
}