using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoeMais.Controller.Objetos;
using DoeMais.Views;

namespace DoeMais.BD
{
    class InstituicaoBD : ConectaBD
    {//Classe de conexão ao banco com métodos relacionados à instituição
        private Instituicao retorno;

        public Instituicao getDadosInstituicao(String cnpj)
        {
            retorno = new Instituicao();
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " RazaoSocial, " +
                " NomeFantasia, " +
                " Email, " +
                " TelefoneA, " +
                " TelefoneB, " +
                " CEP, " +
                " Logradouro, " +
                " Bairro, " +
                " Cidade, " +
                " Uf, " +
                " Numero, " +
                " Complemento, " +
                " ResumoEmpresa, " +
                " RetiraDoacao, " +
                " HoraAbre, " +
                " HoraFecha, " +
                " DiasAbertos " +
                " FROM " +
                " tblInstituicao " +
                " WHERE CNPJ LIKE @Cnpj " +
                "";

                cmd.Parameters.AddWithValue("@cnpj", cnpj);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        retorno.RazaoSocial = dr[0].ToString();
                        retorno.NomeFantasia = dr[1].ToString();
                        retorno.Email = (dr[2].ToString());
                        retorno.TelefoneA = (dr[3].ToString());
                        retorno.TelefoneB = (dr[4].ToString());
                        retorno.Cep = dr[5].ToString();
                        retorno.Logradouro = dr[6].ToString();
                        retorno.Bairro = dr[7].ToString();
                        retorno.Cidade = dr[8].ToString();
                        retorno.Uf = dr[9].ToString();
                        retorno.Numero = dr[10].ToString();
                        retorno.Complemento = dr[11].ToString();
                        retorno.ResumoEmpresa = dr[12].ToString();
                        retorno.RetiraDoacao = Convert.ToBoolean(dr[13]);
                        retorno.HoraAbre = Convert.ToDateTime(dr[14].ToString());
                        retorno.HoraFecha = Convert.ToDateTime(dr[15].ToString());
                        retorno.DiasAbertos = dr[16].ToString();
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

        public Boolean setDadosInstituicao(Instituicao instituicao)
        {
            try
            {
                open();
                #region CommandText & parameters
                cmd.CommandText =
                " UPDATE tblInstituicao " +
                " SET  " +
                " RazaoSocial = @razaoSocial, " +
                " NomeFantasia = @nomeFantasia, " +
                " Email = @email, " +
                " TelefoneA = @telefoneA, " +
                " TelefoneB = @telefoneB, " +
                " CEP = @cep, " +
                " Logradouro = @logradouro, " +
                " Bairro = @bairro, " +
                " Cidade = @cidade, " +
                " Uf = @uf, " +
                " Numero = @numero, " +
                " Complemento = @complemento, " +
                " ResumoEmpresa = @resumoEmpresa, " +
                " RetiraDoacao = @retiraDoacao, " +
                " HoraAbre = @horaAbre, " +
                " HoraFecha = @horaFecha, " +
                " DiasAbertos = @diasAbertos " +
                " WHERE CNPJ LIKE @cnpj " +
                "";
                cmd.Parameters.AddWithValue("@cnpj", ControlViews.cnpj);
                cmd.Parameters.AddWithValue("@razaoSocial", instituicao.RazaoSocial);
                cmd.Parameters.AddWithValue("@nomeFantasia", instituicao.NomeFantasia);
                cmd.Parameters.AddWithValue("@email", instituicao.Email);
                cmd.Parameters.AddWithValue("@telefoneA", instituicao.TelefoneA);
                if (instituicao.TelefoneB == null || instituicao.TelefoneB.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@telefoneB", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@telefoneB", instituicao.TelefoneB);
                }
                cmd.Parameters.AddWithValue("@cep", instituicao.Cep);
                cmd.Parameters.AddWithValue("@logradouro", instituicao.Logradouro);
                cmd.Parameters.AddWithValue("@bairro", instituicao.Bairro);
                cmd.Parameters.AddWithValue("@cidade", instituicao.Cidade);
                cmd.Parameters.AddWithValue("@uf", instituicao.Uf);
                cmd.Parameters.AddWithValue("@numero", instituicao.Numero);
                if (instituicao.Complemento == null || instituicao.Complemento.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@complemento", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@complemento", instituicao.Complemento);
                }
                cmd.Parameters.AddWithValue("@resumoEmpresa", instituicao.ResumoEmpresa);
                cmd.Parameters.AddWithValue("@retiraDoacao", instituicao.RetiraDoacao);
                cmd.Parameters.AddWithValue("@horaAbre", instituicao.HoraAbre);
                cmd.Parameters.AddWithValue("@horaFecha", instituicao.HoraFecha);
                cmd.Parameters.AddWithValue("@diasAbertos", instituicao.DiasAbertos);
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

