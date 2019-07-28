using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Sysmap.Sanity.VivoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sysmap.Sanity.VivoApi.DAOs
{
    public class VivoDAO
    {
        private IConfiguration _configuracoes;
        private readonly ILogger _logger;

        public VivoDAO(IConfiguration config, ILogger<VivoDAO> logger)
        {
            _configuracoes = config;
            _logger = logger;
        }

        #region Lista de Cenarios Assinados para o analista UiPath
        internal List<TestesVivo> ListaCenarios(string codRelease)
        {
            List<TestesVivo> listCenarios = new List<TestesVivo>();
            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                var query = $"SELECT * FROM Sanity.TestesVivo WHERE CodRelease = '{codRelease}';";

                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                    listCenarios = mysqlCon.Query<TestesVivo>(query).ToList();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return listCenarios;
        }
        #endregion

        //Upadate no Status do Cenario da Release
        internal string UpdateTeste(string codRelease,int numTeste,int status, string observacao)
        {
            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                    mysqlCon.Execute($"UPDATE Sanity.TestesVivo SET Status = {status}, Observacao = '{observacao}' WHERE CodRelease = '{codRelease}' AND Cenario = {numTeste};");
                }
                string result = "Update com sucesso";

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }
        }
    }
}
