﻿using CostsAnalyse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Abstracts
{
    public interface IParse
    {
        void GetProduct(string url);
    }
}
