using DoeMais.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.BD
{
    class MensagemBD : ConectaBD
    {//Classe de conexão ao banco com métodos relacionados às mensagens
        public List<String[]> getMensagens()
        {//Retorna uma lista da última mensagem de cada doador ordenado por data
            List<String[]> retorno = new List<String[]>();
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT  " +
                " tblMensagem.fk_IdDoador, " +
                " (tblDoador.Nome + ' ' + tblDoador.Sobrenome), " +
                " tblMensagem.DataDeEnvio " +
                " FROM tblMensagem " +
                " LEFT JOIN tblDoador " +
                " ON tblMensagem.fk_IdDoador = tblDoador.IdDoador " +
                " WHERE tblMensagem.fk_Cnpj = @cnpj AND (tblMensagem.Lida = 0 or tblMensagem.Lida is null) " +
                " ORDER BY tblMensagem.DataDeEnvio DESC " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        String[] info = new String[]
                        {
                            dr[0].ToString(),//IdDoador
                            dr[1].ToString(),//Nome Doador
                            dr[2].ToString()//data de envio
                        };
                        retorno.Add(info);
                    }
                }

                close();
                return retorno;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

        public List<String[]> getMensagensDoDoador(int idDoador)
        {//Recebe as mensagens do doador de acordo com o seu id
            List<String[]> retorno = new List<String[]>();

            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " DataDeEnvio,  " +
                " tblDoador.Nome, " +
                " tblFuncionario.Nome, " +
                " Texto " +
                " FROM tblMensagem " +
                " LEFT JOIN tblDoador " +
                " ON tblMensagem.fk_IdDoador = tblDoador.IdDoador " +
                " LEFT JOIN tblFuncionario " +
                " ON tblMensagem.fk_IdFuncionario = tblFuncionario.IdFuncionario " +
                " WHERE tblMensagem.fk_Cnpj = @cnpj AND tblMensagem.fk_IdDoador = @IdDoador " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@idDoador", idDoador);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        String[] info = new String[]
                        {
                            dr[0].ToString(),//Data de Envio
                            dr[1].ToString(),//Nome do doador
                            dr[2].ToString(),//Nome do funcionário
                            dr[3].ToString()//texto da Mensagem
                        };
                        retorno.Add(info);
                    }
                }

                close();
                return retorno;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

        public Boolean enviarMensagem(int idDoador, String Texto)
        {//Envia uma Mensagem em resposta ao doador
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " UPDATE tblMensagem " +
                " SET Lida = 1 " +
                " WHERE fk_Cnpj = @cnpj AND fk_IdDoador = @idDoador " +
                "  " +
                " INSERT INTO [dbo].[tblMensagem] " +
                " ([Texto] " +
                " ,[DataDeEnvio] " +
                " ,[fk_IdFuncionario] " +
                " ,[fk_IdDoador] " +
                " ,[fk_Cnpj]) " +
                " VALUES " +
                " (@texto " +
                " ,GETDATE() " +
                " ,@idFuncionario " +
                " ,@idDoador " +
                " ,@cnpj) " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@idDoador", idDoador);
                cmd.Parameters.AddWithValue("@idFuncionario", ControlViews.idFunc);
                cmd.Parameters.AddWithValue("@texto", Texto);
                #endregion

                cmd.ExecuteNonQuery();

                close();
                return true;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return false;
            }
        }
    }
}
