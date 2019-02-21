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

        //Gerando Token
        internal string GerarToken (string email, string senha)
        {
            string token = "";
            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                using (MySqlConnection mysqlCon = new MySqlConnection(ConnectionString))
                {
                    token = mysqlCon.Query<string>(@"CALL `Sanity`.`procGerarToken`(@Email, @Senha);", new { Email = email, Senha = senha }).SingleOrDefault();
                   
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return token;
        }

        //Verifica se exite o token 
        internal bool VerificaToken(string token)
        {
            bool resultado = false;
            try
            {
                string ConnectionString = _configuracoes.GetConnectionString("Sanity");

                using (var mysqlCon = new MySqlConnection(ConnectionString))
                {
                    var qtd = mysqlCon.Query<int>("SELECT Count(*) as Qtd FROM Sanity.Tokens where Token = @Token", new { Token = token });

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
        internal List<VivoRelease> ListRelease()
        {
            List<VivoRelease> releases = new List<VivoRelease>();
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
                    var result = mysqlCon.Query<VivoRelease>(query);

                    foreach (VivoRelease cenario in result)
                    {
                        releases.Add(cenario);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return releases;
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
