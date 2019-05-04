using DoeMais.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.BD
{
    class PropagandaBD : ConectaBD
    {//Classe de conexão ao banco com métodos relacionados às propagandas
        public List<String[]> getItensDoTipo(String tipo)
        {//pega itens de um determinado tipo (higiene, alimento, etc) obs: usar apenas a 1ª letra
            List<String[]> itens = new List<String[]>();

            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT  " +
                " tblItemPreCadastro.ItemNome, " +
                " tblItemPreCadastro.ItemTipoMedida " +
                " FROM tblItemInstituicao " +
                " INNER JOIN tblItemPreCadastro " +
                " ON tblItemInstituicao.fk_IdItemPreCadastro = tblItemPreCadastro.IdItemPreCadastro " +
                " WHERE tblItemInstituicao.fk_CNPJ LIKE @cnpj AND " +
                " tblItemPreCadastro.ItemTipo LIKE @tipo + '%' AND " +
                " tblItemInstituicao.Ativo = 1 " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        itens.Add(
                            new String[]
                            {
                                dr[0].ToString(),
                                dr[1].ToString()
                            }
                            );
                    }
                }

                close();
                return itens;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

        public int publicarPropaganda()
        {//publica a propaganda e retorna o seu cód., porém ainda sem itens
            int idPropaganda = 0;

            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " INSERT INTO tblPropaganda " +
                " (DataInicio, DataFim, fk_CNPJ) " +
                " VALUES " +
                " (GETDATE(), @dataFim, @cnpj) " +
                "  " +
                " SELECT MAX(IdPropaganda) FROM tblPropaganda " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@dataFim", DateTime.Now.AddDays(30));
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        idPropaganda = Convert.ToInt32(dr[0].ToString());
                    }
                }

                close();
                return idPropaganda;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return 0;
            }
        }

        public Boolean addItens(int idPropaganda, String nomeItem, int qtd)
        {
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " INSERT INTO tblDetalhePropaganda " +
                " (fk_IdPropaganda, fk_CNPJ, Qtd, fk_IdItemPreCadastro) " +
                " VALUES " +
                " (@IdPropaganda, @cnpj, @qtd, (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem)) " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@idPropaganda", idPropaganda);
                cmd.Parameters.AddWithValue("@nomeItem", nomeItem);
                cmd.Parameters.AddWithValue("@qtd", qtd);
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
