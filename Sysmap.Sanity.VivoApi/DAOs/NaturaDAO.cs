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
    public class NaturaDAO
    {
        private IConfiguration _configuracoes;
        private readonly ILogger _logger;

        public NaturaDAO(IConfiguration config, ILogger<NaturaDAO> logger)
        {
            _logger = logger;
            _configuracoes = config;
        }

        internal List<NaturaCenarios> ListaCenarios(string cod_release, string executor)
        {
            List<NaturaCenarios> natura = new List<NaturaCenarios>();

            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                string query = @"SELECT * FROM vCenariosAPI_Natura
                                 WHERE cod_release = @cod_release AND executor = @executor AND execucao_status <> 4;";

                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                    var result = mysqlCon.Query<NaturaCenarios>(query, new { cod_release, executor });

                    foreach (NaturaCenarios cenario in result)
                    {
                        string[] cenarioCodigo = cenario.Cenario.Split(" ");
                        cenario.Cenario = cenarioCodigo[0];
                        natura.Add(cenario);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro: {0}", ex);
            }

            return natura;
        }

        internal void UpdateCenario(string cod_release,int nCenario, int execStatus, int chamadoStatus, string observacao)
        {
            try
            {
                string date = null;

                if(execStatus == 4)
                {
                    date = DateTime.Now.ToString("yyyy/MM/dd");
                }
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                string query = @"UPDATE testes_natura
                                SET
	                                execucao_status = @execStatus,
                                    chamado_status = @chamadoStatus,
	                                observacao = @observacao,
                                    data_executado = @date
                                WHERE cod_release = @cod_release and numero_teste = @nCenario;";

                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                   mysqlCon.Execute(query,new { execStatus, chamadoStatus,observacao, date, cod_release, nCenario });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro: {0}", ex);
            }
        }
    }
}
