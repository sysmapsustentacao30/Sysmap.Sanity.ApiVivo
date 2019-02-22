using Dapper;
using Microsoft.Extensions.Configuration;
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

        public VivoDAO(IConfiguration config)
        {
            _configuracoes = config;
        }

        //Verifica user exite o token 
        internal bool VerificaUser (string email, string password)
        {
            bool resultado = false;
            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                    var qtd = mysqlCon.Query<int>("SELECT count(*) as qtd FROM Sanity.User_Test Where Email = @email and Password = @password;", new {email, password });

                    foreach (var i in qtd)
                    {
                        if (i == 1)
                        {
                            resultado = true;
                        }
                        else
                        {
                            resultado = false;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        //Lista de Cenarios Assinados para o analista UiPath
        internal List<VivoCenarios> ListaCenarios()
        {
            List<VivoCenarios> listCenarios = new List<VivoCenarios>();
            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                var query = @"SELECT ce.* 
                               FROM Sanity.Cenarios ce 
                               JOIN Sanity.Release re 
	                                ON re.CodRelease = ce.CodRelease
                                Where ce.Analista = 'UiPath'
	                                  AND ce.Excluido = 0
                                      AND re.Status = 0;";


                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                    var result = mysqlCon.Query<VivoCenarios>(query);

                    foreach (VivoCenarios cenario in result)
                    {
                        listCenarios.Add(cenario);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listCenarios;
        }

        //Upadate no Status do Cenario da Release
        internal string UpdateRelease(string cenario, string executado, string status, string codRelease)
        {
            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                    mysqlCon.Execute(@"UPDATE Sanity.Cenarios SET `Status` = @Status, `Executado` = @Executado WHERE `Cenario` = @Cenario AND CodRelease = @CodRelease;", new { Status = status, Executado = executado, Cenario = cenario, CodRelease = codRelease });
                }
                string retorno = "Update com sucesso";

                return retorno;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
