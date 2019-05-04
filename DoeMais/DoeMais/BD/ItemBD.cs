using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoeMais.Controller.Objetos;
using DoeMais.Views;

namespace DoeMais.BD
{
    class ItemBD : ConectaBD
    {//Classe de conexão ao banco com métodos relacionados aos itens
        Item item;
        List<Item> itens;

        public List<Item> getItems()
        {//pega todos os items existentes e ativos no banco
            itens = new List<Item>();//Instanciando
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " ItemNome, " +
                " ItemTipo, " +
                " ItemTipoMedida  " +
                " FROM tblItemPreCadastro " +
                "";
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();//Instanciando
                        item.Nome = dr[0].ToString();
                        item.Tipo = dr[1].ToString();
                        item.TipoDeMedida = dr[2].ToString();
                        itens.Add(item);
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

        public List<Item> getItensInstituicao()
        {//pega todos os itens ativos da instituição somente nome
            itens = new List<Item>();//Instanciando 
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT  " +
                " (SELECT ItemNome FROM tblItemPreCadastro  " +
                " WHERE IdItemPreCadastro LIKE fk_IdItemPreCadastro)  " +
                " FROM tblItemInstituicao " +
                " WHERE fk_CNPJ LIKE @cnpj AND Ativo = 1 " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();//Instanciando
                        item.Nome = dr[0].ToString();
                        itens.Add(item);
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

        public Boolean addItemInstituicao(String nomeItem)
        {//Adiciona o item na/para a instituição
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " IF EXISTS " +
                " ( " +
                " SELECT  " +
                " (SELECT ItemNome FROM tblItemPreCadastro  " +
                " WHERE IdItemPreCadastro LIKE fk_IdItemPreCadastro)  " +
                " FROM tblItemInstituicao " +
                " WHERE fk_CNPJ LIKE @cnpj  " +
                " AND fk_IdItemPreCadastro = (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem) " +
                " ) " +
                " BEGIN " +
                " UPDATE tblItemInstituicao " +
                " SET Ativo = 1 " +
                " WHERE fk_CNPJ LIKE @cnpj AND  " +
                " fk_IdItemPreCadastro = (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem) " +
                " END " +
                " ELSE " +
                " BEGIN " +
                " INSERT INTO tblItemInstituicao (Ativo, fk_CNPJ, fk_IdItemPreCadastro) " +
                " Values (1, @cnpj, (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem)) " +
                " END " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
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

        public Boolean removeItemInstituicao(String nomeItem)
        {//desativa o item da instituição
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " IF EXISTS " +
                " ( " +
                " SELECT  " +
                " (SELECT ItemNome FROM tblItemPreCadastro  " +
                " WHERE IdItemPreCadastro LIKE fk_IdItemPreCadastro)  " +
                " FROM tblItemInstituicao " +
                " WHERE fk_CNPJ LIKE @cnpj  " +
                " AND fk_IdItemPreCadastro = (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem) " +
                " ) " +
                " BEGIN " +
                " UPDATE tblItemInstituicao " +
                " SET Ativo = 0 " +
                " WHERE fk_CNPJ LIKE @cnpj AND  " +
                " fk_IdItemPreCadastro = (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nomeItem) " +
                " END " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
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

        public List<Item> getItensArmazenados(String Filtro)
        {//pega os itens armazenados na instituição de acordo com o filtro se não nulo e se contem as iniciais do tipo
            itens = new List<Item>();//Instanciando

            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " IPC.ItemTipo, " +
                " IPC.ItemNome, " +
                " COUNT(*) AS Qtd " +
                " FROM tblItemDetalhe AS ID " +
                " INNER JOIN tblItemPreCadastro AS IPC " +
                " ON ID.fk_IdItemPreCadastro = IPC.IdItemPreCadastro " +
                " WHERE ID.fk_CNPJ = @cnpj AND ID.NoEstoque = 1 " +
                "";
                if (Filtro == null || Filtro.Trim().Equals(""))
                {
                    //para o filtro deve se utilizar as iniciais do tipo de item
                }
                else
                {
                    cmd.CommandText += " AND (";
                    if (Filtro.Contains("A"))
                        cmd.CommandText += " (ItemTipo LIKE 'A' + '%') OR ";
                    if (Filtro.Contains("C"))
                        cmd.CommandText += " (ItemTipo LIKE 'C' + '%') OR ";
                    if (Filtro.Contains("H"))
                        cmd.CommandText += " (ItemTipo LIKE 'H' + '%') OR ";
                    if (Filtro.Contains("R"))
                        cmd.CommandText += " (ItemTipo LIKE 'R' + '%') OR ";
                    cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 3);//remove o último OR para não dar erro
                    cmd.CommandText += " ) ";
                }
                cmd.CommandText +=
                " GROUP BY " +
                " IPC.ItemTipo, " +
                " IPC.ItemNome ";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();//Instanciando
                        item.Tipo = dr[0].ToString();
                        item.Nome = dr[1].ToString();
                        item.QTD = Convert.ToInt32(dr[2].ToString());
                        itens.Add(item);
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

        private List<Item> getItemComEsseNome(String nome)
        {//método para pegar ites(Alimento e Higiene) específicos agrupados por mais características
            itens = new List<Item>();//Instanciando

            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " IPC.ItemTipo, " +
                " IPC.ItemNome, " +
                " ID.Medida,  " +
                " IPC.ItemTipoMedida, " +
                " COUNT(*) AS QTD, " +
                " ID.Validade " +
                " FROM tblItemDetalhe AS ID " +
                " INNER JOIN tblItemPreCadastro AS IPC " +
                " ON ID.fk_IdItemPreCadastro = IPC.IdItemPreCadastro " +
                " WHERE ID.fk_CNPJ LIKE @cnpj AND ID.NoEstoque = 1 AND " +
                " IPC.ItemNome LIKE @nome " +
                " GROUP BY  " +
                " IPC.ItemTipo, " +
                " IPC.ItemNome, " +
                " ID.Medida, " +
                " IPC.ItemTipoMedida, " +
                " ID.Validade " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@nome", nome);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();//Instanciando
                        item.Tipo = dr[0].ToString();
                        item.Nome = dr[1].ToString();
                        item.Medida = dr[2].ToString();
                        item.TipoDeMedida = dr[3].ToString();
                        item.QTD = Convert.ToInt32(dr[4].ToString());
                        item.Validade = dr[5].ToString();
                        itens.Add(item);
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

        public List<Item> getAlimentoComNome(String Alimento)
        {
            return getItemComEsseNome(Alimento);
        }

        public List<Item> getItemDeHigieneComNome(String Higiene)
        {
            return getItemComEsseNome(Higiene);
        }

        public List<Item> getRoupaComNome(String Roupa)
        {//método para pegar itens(Roupa) específicos agrupados por mais características
            itens = new List<Item>();//Instanciando

            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " IPC.ItemTipo, " +
                " IPC.ItemNome, " +
                " ID.Medida,  " +
                " IPC.ItemTipoMedida, " +
                " COUNT(*) AS QTD, " +
                " ID.Genero, " +
                " ID.FaixaEtaria, " +
                " ID.Condicao, " +
                " ID.TamanhoRoupa " +
                " FROM tblItemDetalhe AS ID " +
                " INNER JOIN tblItemPreCadastro AS IPC " +
                " ON ID.fk_IdItemPreCadastro = IPC.IdItemPreCadastro " +
                " WHERE ID.fk_CNPJ LIKE @cnpj AND ID.NoEstoque = 1 AND " +
                " IPC.ItemNome LIKE @nome " +
                " GROUP BY  " +
                " IPC.ItemTipo, " +
                " IPC.ItemNome, " +
                " ID.Medida, " +
                " IPC.ItemTipoMedida, " +
                " ID.Genero, " +
                " ID.FaixaEtaria, " +
                " ID.Condicao, " +
                " ID.TamanhoRoupa " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@nome", Roupa);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();
                        item.Tipo = dr[0].ToString();
                        item.Nome = dr[1].ToString();
                        item.Medida = dr[2].ToString();
                        item.TipoDeMedida = dr[3].ToString();
                        item.QTD = Convert.ToInt32(dr[4].ToString());
                        item.Genero = dr[5].ToString();
                        item.FaixaEtaria = dr[6].ToString();
                        item.Condicao = dr[7].ToString();
                        item.Tamanho = dr[8].ToString();
                        itens.Add(item);
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

        public List<Item> getItensSemTriagem()
        {//Método para pegar itens sem triagem
            itens = new List<Item>();//Instanciando

            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " ItemTipo, " +
                " tblItemPreCadastro.ItemNome, " +
                " COUNT (*) " +
                " FROM tblDetalheDoacao " +
                " LEFT JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " LEFT JOIN tblItemPreCadastro " +
                " ON tblItemDetalhe.fk_IdItemPreCadastro = tblItemPreCadastro.IdItemPreCadastro " +
                " WHERE tblItemDetalhe.fk_CNPJ LIKE @cnpj AND " +
                " tblItemDetalhe.NoEstoque = 1 AND " +
                " (Medida IS NULL AND Validade IS NULL AND Genero IS NULL AND  " +
                " FaixaEtaria IS NULL AND Condicao IS NULL AND TamanhoRoupa IS NULL) " +
                " GROUP BY ItemTipo, ItemNome " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();
                        item.Tipo = dr[0].ToString();
                        item.Nome = dr[1].ToString();
                        item.QTD = Convert.ToInt32(dr[2].ToString());
                        itens.Add(item);
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

        private List<Item> getItensSemTriagemComEsseNome(String nome)
        {//Método para pegar itens(exceto roupa) sem triagem de um nome específico
            itens = new List<Item>();

            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " tblItemPreCadastro.ItemNome, " +
                " COUNT (*), " +
                " tblItemPreCadastro.ItemTipoMedida " +
                " FROM tblDetalheDoacao " +
                " LEFT JOIN tblItemDetalhe " +
                " ON tblDetalheDoacao.fk_IdItemDetalhe = tblItemDetalhe.IdItemDetalhe " +
                " LEFT JOIN tblItemPreCadastro " +
                " ON tblItemDetalhe.fk_IdItemPreCadastro = tblItemPreCadastro.IdItemPreCadastro " +
                " WHERE tblItemDetalhe.fk_CNPJ LIKE @cnpj AND  " +
                " ItemNome LIKE @nome AND " +
                " tblItemDetalhe.NoEstoque = 1 AND " +
                " ((Medida IS NULL) AND (Validade IS NULL) AND (Genero IS NULL) AND  " +
                " (FaixaEtaria IS NULL) AND (Condicao IS NULL) AND (TamanhoRoupa IS NULL)) " +
                " GROUP BY ItemNome, ItemTipoMedida " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@nome", nome);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();//instanciando
                        item.Nome = dr[0].ToString();
                        item.QTD = Convert.ToInt32(dr[1].ToString());//Lembrando que essa é a quantidade genérica do item, contando as unidades, não KG ou LItros ou o que for
                        item.TipoDeMedida = dr[2].ToString();
                        itens.Add(item);
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

        public List<Item> getAlimentoSemTriagemComNome(String Alimento)
        {//Retorna um alimento - o tipo de medida - o nome - e a quantidade genérica (por unidade)
            return getItensSemTriagemComEsseNome(Alimento);
        }

        public List<Item> getItemDeHigieneSemTriagemComNome(String Higiene)
        {//Retorna um item de higiene - o tipo de medida - o nome - e a quantidade genérica (por unidade)
            return getItensSemTriagemComEsseNome(Higiene);
        }

        public List<Item> getRoupaSemTriagemComNome(String Roupa)
        {//Retorna um Roupa - o tipo de medida - o nome - e a quantidade genérica (por unidade)
            itens = new List<Item>();

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
                " WHERE tblItemDetalhe.fk_CNPJ LIKE @cnpj AND  " +
                " ItemNome LIKE @nome AND " +
                " tblItemDetalhe.NoEstoque = 1 AND " +
                " ((Medida IS NULL) AND (Validade IS NULL) AND (Genero IS NULL) AND  " +
                " (FaixaEtaria IS NULL) AND (Condicao IS NULL) AND (TamanhoRoupa IS NULL)) " +
                " GROUP BY ItemNome " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@nome", Roupa);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        item = new Item();
                        item.Nome = dr[0].ToString();
                        item.QTD = Convert.ToInt32(dr[1].ToString());
                        itens.Add(item);
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

        public Boolean triarAlimento(Item itemParaTriar)
        {//atualiza um alimento com medida(X,XX) e validade (dd/mm/aaaa)
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " UPDATE tblItemDetalhe " +
                " SET  " +
                " Medida = @medida, " +
                " Validade = @validade " +
                " WHERE fk_CNPJ LIKE @cnpj AND " +
                " NoEstoque = 1 AND " +
                " IdItemDetalhe = " +
                " (SELECT MAX(IdItemDetalhe) FROM tblItemDetalhe " +
                " WHERE  " +
                " Medida IS NULL AND " +
                " fk_IdItemPreCadastro = (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nome)) " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@nome", itemParaTriar.Nome);
                cmd.Parameters.AddWithValue("@medida", Convert.ToDecimal(itemParaTriar.Medida));
                cmd.Parameters.AddWithValue("@validade", Convert.ToDateTime(itemParaTriar.Validade));
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

        public Boolean triarItemDeHigiene(Item itemParaTriar)
        {//atualiza um item de higiene com validade (dd/mm/aaaa)
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " UPDATE tblItemDetalhe " +
                " SET  " +
                " Medida = 1, " +
                " Validade = @validade " +
                " WHERE fk_CNPJ LIKE @cnpj AND " +
                " NoEstoque = 1 AND " +
                " IdItemDetalhe = " +
                " (SELECT MAX(IdItemDetalhe) FROM tblItemDetalhe " +
                " WHERE  " +
                " Medida IS NULL AND " +
                " fk_IdItemPreCadastro = (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nome)) " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@nome", itemParaTriar.Nome);
                cmd.Parameters.AddWithValue("@validade", Convert.ToDateTime(itemParaTriar.Validade));
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

        public Boolean triarRoupa(Item itemParaTriar)
        {//atualiza uma roupa com Genero (M)-FaixaEtaria(Infantil)-Condicao(novo)-tamanho(GG/M/80) tamanho pode ser até dois caracteres
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " UPDATE tblItemDetalhe " +
                " SET  " +
                " Medida = 1, " +
                " Genero = @genero, " +
                " FaixaEtaria = @faixaEtaria, " +
                " Condicao = @condicao, " +
                " TamanhoRoupa = @tamanho " +
                " WHERE fk_CNPJ LIKE @cnpj AND " +
                " NoEstoque = 1 AND " +
                " IdItemDetalhe = " +
                " (SELECT MAX(IdItemDetalhe) FROM tblItemDetalhe " +
                " WHERE  " +
                " Medida IS NULL AND " +
                " fk_IdItemPreCadastro = (SELECT IdItemPreCadastro FROM tblItemPreCadastro WHERE ItemNome LIKE @nome)) " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@nome", itemParaTriar.Nome);
                cmd.Parameters.AddWithValue("@genero", itemParaTriar.Genero);
                cmd.Parameters.AddWithValue("@faixaEtaria", itemParaTriar.FaixaEtaria);
                cmd.Parameters.AddWithValue("@condicao", itemParaTriar.Condicao);
                cmd.Parameters.AddWithValue("@tamanho", itemParaTriar.Tamanho);
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
