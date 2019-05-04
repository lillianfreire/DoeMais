using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoeMais.Controller.Objetos;
using DoeMais.Views;

namespace DoeMais.BD
{
    class DoacaoBD : ConectaBD
    {//Classe de conexão ao banco com métodos relacionados à  doação
        public String addDoacao(int idDoador, String idFuncionario)
        {//Adiciona uma nova doação e retorna o código dela para uso posterior
            String codDoacao = "Erro";
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " INSERT INTO tblDoacao " +
                " (DataRegistro, " +
                " DataEntregue, " +
                " Pendente, " +
                " fk_IdDoador, " +
                " fk_IdFuncionario) " +
                " VALUES " +
                " (GETDATE(), " +
                " GETDATE(), " +
                " 0, " +
                " @idDoador, " +
                " @idFuncionario) " +
                "  " +
                " SELECT MAX(IdDoacao) FROM tblDoacao " +
                "";
                if (idDoador == 0)//se for nulo deve ser passado zero nos parâmetros
                {
                    cmd.Parameters.AddWithValue("@idDoador", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@idDoador", idDoador);
                }
                if (idFuncionario == null || idFuncionario.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@idFuncionario", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                }
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        codDoacao = dr[0].ToString();
                    }
                }

                close();
                return codDoacao;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

        public Boolean addItemNaDoacao(int idDoacao, String item)
        {//adiciona um item na doação
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " INSERT INTO tblItemDetalhe " +
                " (fk_CNPJ, " +
                " fk_IdItemPreCadastro, " +
                " NoEstoque) " +
                " VALUES " +
                " (@cnpj, " +
                " (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @item), " +
                " 1) " +
                "  " +
                " INSERT INTO tblDetalheDoacao " +
                " (fk_IdDoacao, " +
                " fk_IdItemDetalhe, " +
                " Pendente) " +
                " VALUES " +
                " (@idDoacao, " +
                " (SELECT MAX(IdItemDetalhe) FROM tblItemDetalhe), " +
                " 0) " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@idDoacao", idDoacao);
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

        public List<String[]> getDoacoes(String CpfCnpjOuNome)
        {//retorna as doações pendentes/ainda não entregues numa list de vetor 5
            List<String[]> doacoes = new List<String[]>();
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " DISTINCT IdDoacao, " +
                " DataEntrega, " +
                " CONCAT((tblDoador.Nome + ' ' + tblDoador.Sobrenome),tblDoador.RazaoSocial), " +
                " tblDoador.CPF_CNPJ, " +
                " IIF(tblDoacao.DataRetirada IS NULL, 0, 1) " +
                " FROM tblDoacao " +
                " LEFT JOIN tblDoador " +
                " ON tblDoacao.fk_IdDoador = tblDoador.IdDoador " +
                " LEFT JOIN tblDetalheDoacao " +
                " ON tblDoacao.IdDoacao = tblDetalheDoacao.fk_IdDoacao " +
                " LEFT JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " WHERE tblDoacao.Pendente = 1 AND tblItemDetalhe.fk_CNPJ LIKE @cnpj " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                if (CpfCnpjOuNome == null || CpfCnpjOuNome.Trim().Equals(""))
                {
                }
                else
                {
                    cmd.CommandText +=
                    " AND ((tblDoador.Nome + ' ' + tblDoador.Sobrenome) LIKE ('%' + @textoDeBusca + '%') " +
                    " OR tblDoador.CPF_CNPJ LIKE @textoDeBusca) ";
                    cmd.Parameters.AddWithValue("@textoDeBusca", CpfCnpjOuNome);
                }
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        String[] doacao = new String[5];
                        doacao[0] = dr[0].ToString();
                        doacao[1] = dr[1].ToString();
                        doacao[2] = dr[2].ToString();
                        doacao[3] = dr[3].ToString();
                        doacao[4] = dr[4].ToString();
                        doacoes.Add(doacao);
                    }
                }

                close();
                return doacoes;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

        public List<String> getItensDaDoacao(int idDoacao)
        {//retorna os itens da doação e a quantidade na linha abaixo do item
            List<String> retorno = new List<String>();
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " tblItemPreCadastro.ItemNome, " +
                " COUNT (*) " +
                " FROM tblDetalheDoacao " +
                " LEFT JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " LEFT JOIN tblItemPreCadastro " +
                " ON tblItemDetalhe.fk_IdItemPreCadastro = tblItemPreCadastro.IdItemPreCadastro " +
                " WHERE tblDetalheDoacao.fk_IdDoacao = @codDoacao " +
                " GROUP BY ItemNome " +
                "";
                cmd.Parameters.AddWithValue("@codDoacao", idDoacao);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        retorno.Add(dr[0].ToString());
                        retorno.Add(dr[1].ToString());
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

        public Boolean retiraPendenciaDaDoacao(int idDoacao)
        {//Conclui a doação
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " UPDATE tblDoacao " +
                " SET Pendente = 0 " +
                " WHERE IdDoacao = @codDoacao " +
                "";

                cmd.Parameters.AddWithValue("@codDoacao", idDoacao);
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

        public Boolean retiraPendenciaDoItemDoado(int idDoacao, String nomeItem)
        {//hm nome sugestivo, e também add no estoque
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " UPDATE tblItemDetalhe " +
                " SET tblItemDetalhe.NoEstoque = 1 " +
                " FROM tblItemDetalhe " +
                " INNER JOIN tblDetalheDoacao " +
                " ON tblItemDetalhe.IdItemDetalhe= tblDetalheDoacao.fk_IdItemDetalhe " +
                " INNER JOIN tblItemPreCadastro " +
                " ON tblItemDetalhe.fk_IdItemPreCadastro = tblItemPreCadastro.IdItemPreCadastro " +
                " WHERE tblItemPreCadastro.ItemNome = @nomeItem AND " +
                " tblItemDetalhe.IdItemDetalhe  =  " +
                " (SELECT MAX(fk_IdItemDetalhe)  " +
                " FROM tblDetalheDoacao WHERE fk_IdDoacao = @codDoacao AND Pendente = 1 AND  " +
                " fk_IdItemDetalhe IN " +
                " (SELECT idItemDetalhe FROM tblItemDetalhe WHERE fk_IdItemPreCadastro =  " +
                " (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem))) " +
                "" +
                " UPDATE tblDetalheDoacao " +
                " SET tblDetalheDoacao.Pendente = 0 " +
                " FROM tblDetalheDoacao " +
                " INNER JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " INNER JOIN tblItemPreCadastro " +
                " ON tblItemDetalhe.fk_IdItemPreCadastro = tblItemPreCadastro.IdItemPreCadastro " +
                " WHERE tblItemPreCadastro.ItemNome = @nomeItem AND " +
                " tblDetalheDoacao.fk_IdItemDetalhe =  " +
                " (SELECT MAX(fk_IdItemDetalhe) FROM tblDetalheDoacao WHERE fk_IdDoacao = @codDoacao AND Pendente = 1 AND  " +
                " fk_IdItemDetalhe IN " +
                " (SELECT idItemDetalhe FROM tblItemDetalhe WHERE fk_IdItemPreCadastro =  " +
                " (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem))) " +
                "";

                cmd.Parameters.AddWithValue("@codDoacao", idDoacao);
                cmd.Parameters.AddWithValue("@nomeItem", nomeItem);
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

        public DateTime getMenorDataDoacaoDeDomicilioPendente()
        {
            DateTime menorData = new DateTime();
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " MIN(DataRetirada) " +
                " FROM tblDoacao " +
                " LEFT JOIN tblDetalheDoacao " +
                " ON tblDoacao.IdDoacao = tblDetalheDoacao.fk_IdDoacao  " +
                " LEFT JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " WHERE tblDoacao.Pendente = 1 AND " +
                " DataRetirada IS NOT NULL AND " +
                " tblItemDetalhe.fk_CNPJ LIKE @cnpj " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr[0].ToString().Trim().Equals(""))
                        {
                            menorData = DateTime.Now;
                        }
                        else
                        {
                            menorData = Convert.ToDateTime(dr[0].ToString());
                        }
                    }
                }

                close();
                return menorData;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return new DateTime();
            }
        }

        public DateTime getmaiorDataDoacaoDeDomicilioPendente()
        {
            DateTime maiorData = new DateTime();
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " MAX(DataRetirada) " +
                " FROM tblDoacao " +
                " LEFT JOIN tblDetalheDoacao " +
                " ON tblDoacao.IdDoacao = tblDetalheDoacao.fk_IdDoacao  " +
                " LEFT JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " WHERE tblDoacao.Pendente = 1 AND " +
                " DataRetirada IS NOT NULL AND " +
                " tblItemDetalhe.fk_CNPJ LIKE @cnpj " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr[0].ToString().Trim().Equals(""))
                        {
                            maiorData = DateTime.Now;
                        }
                        else
                        {
                            maiorData = Convert.ToDateTime(dr[0].ToString());
                        }
                    }
                }

                close();
                return maiorData;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return new DateTime();
            }
        }

        public List<String[]> getDoacoesDeDomicilio(DateTime dessaData, DateTime ateData)
        {
            List<String[]> doacoesEmDomicilio = new List<String[]>();

            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " DISTINCT IdDoacao, " +
                " DataRetirada, " +
                " (tblDoador.Nome + ' ' + tblDoador.Sobrenome) " +
                " FROM tblDoacao " +
                " LEFT JOIN tblDoador " +
                " ON tblDoacao.fk_IdDoador = tblDoador.IdDoador " +
                " LEFT JOIN tblDetalheDoacao " +
                " ON tblDoacao.IdDoacao = tblDetalheDoacao.fk_IdDoacao  " +
                " LEFT JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " WHERE tblDoacao.DataRetirada IS NOT NULL AND " +
                " tblDoacao.Pendente = 1 AND " +
                " fk_CNPJ LIKE @cnpj AND " +
                " DataRetirada BETWEEN @de AND @ate " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@de", dessaData);
                cmd.Parameters.AddWithValue("@ate", ateData);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        doacoesEmDomicilio.Add(
                            new String[]
                            {
                                dr[0].ToString(),
                                dr[1].ToString(),
                                dr[2].ToString()
                            }
                            );
                    }
                }

                close();
                return doacoesEmDomicilio;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

        public String[] getDoadorDaDoacaoDeID(int idDoacao)
        {
            String[] doador = new String[] { null };
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " IIF(RazaoSocial IS NULL, (Nome + ' ' + Sobrenome), (RazaoSocial)), " +
                " CPF_CNPJ, " +
                " DataRetirada, " +
                " CEP, " +
                " Logradouro, " +
                " Bairro, " +
                " Cidade, " +
                " Uf, " +
                " Numero, " +
                " Complemento " +
                " FROM tblDoador " +
                " LEFT JOIN tblDoacao " +
                " ON tblDoador.IdDoador = tblDoacao.fk_IdDoador " +
                " WHERE tblDoacao.IdDoacao = @idDoacao " +
                "";
                cmd.Parameters.AddWithValue("@idDoacao", idDoacao);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        doador = new String[]
                        {
                            dr[0].ToString(),
                            dr[1].ToString(),
                            dr[2].ToString(),
                            dr[3].ToString(),
                            dr[4].ToString(),
                            dr[5].ToString(),
                            dr[6].ToString(),
                            dr[7].ToString(),
                            dr[8].ToString(),
                            dr[9].ToString()
                        };
                    }
                }

                close();
                return doador;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

    }
}
