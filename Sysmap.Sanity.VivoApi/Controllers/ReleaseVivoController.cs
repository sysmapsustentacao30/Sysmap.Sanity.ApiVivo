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

        [HttpGet("GetToken")]
        public string GetToken(string email,string senha,[FromServices]VivoDAO releaseDAO)
        {
            var token = releaseDAO.GerarToken(email, senha);

            return token;
        }

        [HttpGet("ListCenarios")]
        public IEnumerable<VivoRelease> ListCenarios(string token, [FromServices]VivoDAO releaseDAO)
        {
            List<VivoRelease> releases = new List<VivoRelease>();

            try
            {
                bool verifyToken = releaseDAO.VerificaToken(token);

                if (verifyToken) {
                     releases = releaseDAO.ListRelease();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return releases;
        }

        [HttpGet("AtualizaCenario")]
        public string AtualizaCenario(string token, string cenario, string executado, string status, string codRelease, [FromServices]VivoDAO releaseDAO)
        {
            try
            {
                bool verifToken = releaseDAO.VerificaToken(token);

                if (verifToken)
                {
                    string update = releaseDAO.UpdateRelease(cenario, executado, status, codRelease);

                    return update;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return "Erro no token";
        }

    }
}
