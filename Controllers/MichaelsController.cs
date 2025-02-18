using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MichaelsController : ControllerBase
{
    [HttpGet()]
    public ActionResult Get()
    {
        return Ok(":)");
    }
}
