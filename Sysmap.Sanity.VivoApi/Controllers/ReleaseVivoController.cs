using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sysmap.Sanity.VivoApi.DAOs;
using Sysmap.Sanity.VivoApi.Models;

namespace Sysmap.Sanity.VivoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReleaseVivoController : ControllerBase
    {
        private readonly ILogger _logger;

        public ReleaseVivoController(ILogger<ReleaseVivoController> logger)
        {
            _logger = logger;
        }


        [HttpGet("ListCenarios")]
        public IActionResult ListCenarios(string codRelease,string analista ,[FromServices]VivoDAO vivoDAO)
        {
            _logger.LogInformation($"Código da release: {codRelease}");
            try
            {
                List<TestesVivo> cenarios = vivoDAO.ListaCenarios(codRelease, analista);
                if(cenarios.Count == 0)
                {
                    return BadRequest("Còdigo da release não encontrado");
                }
                ObjectResult result = new ObjectResult(cenarios);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("AtualizaCenario")]
        public IActionResult AtualizaCenario(string codRelease,int numTeste ,int status, string observacao ,[FromServices]VivoDAO vivoDAO)
        {
            try
            {
                _logger.LogInformation($"Param: codRelease= {codRelease}, numTeste= {numTeste}, status= {status}, observacao= {observacao}");
                string result = vivoDAO.UpdateTeste(codRelease, numTeste, status, observacao);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
