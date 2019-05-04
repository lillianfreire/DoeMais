using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.BD
{
    class LoginBD : ConectaBD
    {//Classe de conexão ao banco com método(s) relacionado(s) ao Login
        public List<String> Logar(String login, String senha)
        {
            List<String> retorno = new List<String>();
            try
            {
                open();//Abre banco

                #region CommandText
                cmd.CommandText =
                    " IF EXISTS  " +
                    " (SELECT IdFuncionario " +
                    " FROM tblFuncionario " +
                    " WHERE ((IdFuncionario LIKE @login OR CPF LIKE @login) AND Senha LIKE @senha) AND Ativo = 0) " +
                    " BEGIN " +
                    " SELECT 'Usuário Inativo!' " +
                    " END " +
                    " ELSE IF EXISTS  " +
                    " (SELECT IdFuncionario " +
                    " FROM tblFuncionario " +
                    " WHERE ((IdFuncionario LIKE @login OR CPF LIKE @login) AND Senha LIKE @senha) AND Ativo = 1) " +
                    " BEGIN " +
                    " SELECT 'funcionario', IdFuncionario, CPF, Adm, fk_CNPJ  " +
                    " FROM tblFuncionario " +
                    " WHERE (IdFuncionario LIKE @login OR CPF LIKE @login) AND Senha LIKE @senha " +
                    " END " +
                    " ELSE IF EXISTS " +
                    " (SELECT CNPJ FROM tblInstituicao " +
                    " WHERE CNPJ LIKE @login AND Senha LIKE @senha AND Ativo = 1) " +
                    " BEGIN " +
                    " SELECT 'instituicao', CNPJ FROM tblInstituicao " +
                    " WHERE CNPJ LIKE @login AND Senha LIKE @senha " +
                    " END " +
                    " ELSE " +
                    " BEGIN " +
                    " SELECT 'Login e/ou Senha não encontrados!' " +
                    " END " +
                    "";
                #endregion

                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@senha", senha);

                dr = cmd.ExecuteReader();//Executa a leitura

                if (dr.HasRows)//Se tiver linhas
                {
                    while (dr.Read())//ler linhas
                    {
                        retorno.Add(dr[0].ToString());//Adicionar 1º item
                        if (dr.FieldCount > 1)//se houver mais de uma coluna
                            retorno.Add(dr[1].ToString());//Add item
                        if (dr.FieldCount > 2)//se houver mais de duas colunas
                        {
                            retorno.Add(dr[2].ToString());
                            retorno.Add(dr[3].ToString());
                            retorno.Add(dr[4].ToString());
                        }
                    }
                }

                close();
                return retorno;
            }
            catch (System.Data.SqlClient.SqlException sqlE)
            {
                close();
                retorno.Clear();
                retorno.Add(sqlE.ToString());
                return retorno;
            }
        }
    }
}
