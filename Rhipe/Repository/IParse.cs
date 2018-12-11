﻿using Rhipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rhipe.Repository
{
    public interface IParse
    {
        Token ParseData(string inputText);        
    }
}
