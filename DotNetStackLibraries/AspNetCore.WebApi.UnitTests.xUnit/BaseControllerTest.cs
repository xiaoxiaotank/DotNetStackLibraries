using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.WebApi.UnitTests.xUnit
{
    public class BaseControllerTest<TController> where TController : ControllerBase
    {
        protected TController _controller;
    }
}
