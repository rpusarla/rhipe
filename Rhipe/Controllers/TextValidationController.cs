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
using Rhipe.Repository;

namespace Rhipe.Controllers
{
    [Route("api/[controller]")]
    public class TextValidationController : Controller
    {
        private readonly IParse _parse;       
        public TextValidationController(IParse parse)
        {
            _parse = parse;
        }

        [HttpGet("[action]")]
        public IActionResult IsInputTextValid(string inputText)
        {
            try
            {                
                var tokens = _parse.ParseData(inputText);

                return Ok(tokens);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }                        
        }
    }
}
