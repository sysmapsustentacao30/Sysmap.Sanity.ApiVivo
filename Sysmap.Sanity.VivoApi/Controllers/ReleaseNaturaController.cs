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
    public class ReleaseNaturaController : ControllerBase
    {
        private readonly ILogger _logger;

        public ReleaseNaturaController(ILogger<ReleaseNaturaController> logger)
        {
            _logger = logger;
        }

        // GET: api/ReleaseNatura/5
        [HttpGet("ListaCenarios")]
        public IActionResult ListCenarios(string codRelease,string executor,[FromServices]NaturaDAO naturaDAO)
        {
            List<NaturaCenarios> cenarios = new List<NaturaCenarios>();

            try
            {
                if(codRelease is null)
                {
                    return NotFound("Error: codRelease is null");
                }
                else
                {
                    if(executor is null)
                    {
                        return NotFound("Error: executor is null");
                    }
                    else
                    {
                        cenarios = naturaDAO.ListaCenarios(codRelease, executor);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return Ok(new { Cenarios = cenarios });
        }

        // POST: api/ReleaseNatura
        [HttpPost("AtualizaCenario")]
        public IActionResult AtualizaCenario(string codRelease, int nCenario, int execStatus, string observacao,[FromServices]NaturaDAO naturaDAO)
        {
            try
            {
                if (codRelease is null)
                {
                    return NotFound("Error: codRelease is null");
                }
                else
                {
                    var chamadoStatus = 0;

                    if (nCenario == 0)
                    {
                        return NotFound("Error: nCenario = 0");
                    }
                    else
                    {
                        if (execStatus == 3)
                        {
                            chamadoStatus = 1;
                        }

                        naturaDAO.UpdateCenario(codRelease, nCenario, execStatus, chamadoStatus,observacao);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound($"ERROR: {ex.ToString()}");
                
            }

            return Ok("Atualização feita com sucesso.");
        }

    }
}
