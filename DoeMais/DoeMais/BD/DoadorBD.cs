using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.BD
{
    class DoadorBD : ConectaBD
    {//Classe de conexão ao banco com métodos relacionados ao doador
        public String[] getDoadorParaCadastroDeDoacao(String Cpf)
        {
            String[] dados = new String[3]; ;
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " (Nome + ' ' + Sobrenome), " +
                " CPF_CNPJ, " +
                " IdDoador " +
                " FROM tblDoador " +
                " WHERE CPF_CNPJ LIKE @cpf " +
                "";

                cmd.Parameters.AddWithValue("@cpf", Cpf);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        dados[0] = dr[0].ToString();
                        dados[1] = dr[1].ToString();
                        dados[2] = dr[2].ToString();

                        close();
                        return dados;
                    }
                }

                close();
                return new String[1] { "Não encontrado" };
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return new String[1] { "ERRO" };
            }
        }
    }
}
