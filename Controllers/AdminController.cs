using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    [HttpPost()]
    public async Task<ActionResult>AddAddressType([FromQuery] string type)
    {
        try
        {
            if(await _unitOfWork.AddressRepository.Add(type))
            {
                if(await _unitOfWork.Complete())
                {
                    return StatusCode(201);
                }else
                {
                    return StatusCode(500);
                }
            }else
            {
                return BadRequest();
            } // ur soo fiiiiine <3
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
