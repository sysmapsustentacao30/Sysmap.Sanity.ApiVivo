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
        public ObjectResult ListCenarios(string email,string password, [FromServices]VivoDAO vivoDAO)
        {
            List<VivoCenarios> cenarios = new List<VivoCenarios>();

            try
            {
                bool userExist = vivoDAO.VerificaUser(email, password);

                if (userExist) {
                     cenarios = vivoDAO.ListaCenarios();
                }
                else
                {
                    return new ObjectResult("Email/Password invalido");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return new ObjectResult(cenarios);
        }
       
        [HttpPost("AtualizaCenario")]
        public string AtualizaCenario(string email, string password, string cenario, string executado, string status, string codRelease, [FromServices]VivoDAO vivoDAO)
        {
            try
            {
                bool verifUser = vivoDAO.VerificaUser(email, password);

                if (verifUser)
                {
                    string update = vivoDAO.UpdateRelease(cenario, executado, status, codRelease);

                    return update;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return "Email/Password invalido";
        }

    }
}
