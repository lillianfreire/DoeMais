using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoeMais.Controller.Objetos;
using DoeMais.Views;

namespace DoeMais.BD
{
    class FuncionarioBD : ConectaBD
    {//Classe de conexão ao banco com métodos relacionados ao funcionário
        Funcionario retorno;

        public List<Funcionario> getFuncionarios(String NomeOuCpf, int inativo, int ativo)
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT " +
                " Nome, " +
                " Sobrenome, " +
                " CPF, " +
                " Ativo " +
                " FROM tblFuncionario " +
                " WHERE ((Nome + ' ' + Sobrenome) LIKE ('%' + @textoDeBusca + '%') OR " +
                " CPF LIKE @textoDeBusca) AND Ativo BETWEEN @b1 AND @b2 " +
                "";

                cmd.Parameters.AddWithValue("@textoDeBusca", NomeOuCpf);
                cmd.Parameters.AddWithValue("@b1", inativo);//se inativo = 0 irá buscar também por inativos
                cmd.Parameters.AddWithValue("@b2", ativo);//se ativo = 1 irá buscar também por ativos
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        retorno = new Funcionario();//precisa limpar sempre
                        retorno.Nome = dr[0].ToString();
                        if (dr.FieldCount > 1)
                        {
                            retorno.Sobrenome = dr[1].ToString();
                            retorno.Cpf = dr[2].ToString();
                            retorno.Ativo = Convert.ToBoolean(dr[3].ToString());
                        }
                        funcionarios.Add(retorno);
                    }
                }
                
                close();
                return funcionarios;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return null;
            }
        }

        public Funcionario getDadosFuncionario(String Cpf)
        {
            retorno = new Funcionario();
            try
            {
                open();
                #region CommandText
                cmd.CommandText =
                " SELECT Nome, " +
                " Sobrenome, " +
                " CPF, " +
                " RG, " +
                " DataNascimento, " +
                " CEP, " +
                " Logradouro, " +
                " Bairro, " +
                " Cidade, " +
                " Uf, " +
                " Numero, " +
                " Complemento, " +
                " TelefoneA, " +
                " TelefoneB, " +
                " Email, " +
                " Adm, " +
                " Ativo " +
                " FROM tblFuncionario " +
                " WHERE CPF LIKE @cpf " +
                "";

                cmd.Parameters.AddWithValue("@cpf", Cpf);
                #endregion

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        retorno.Nome = dr[0].ToString();
                        retorno.Sobrenome = dr[1].ToString();
                        retorno.Cpf = dr[2].ToString();
                        retorno.Rg = dr[3].ToString();
                        retorno.DataDeNascimento = Convert.ToDateTime(dr[4].ToString());
                        retorno.Cep = dr[5].ToString();
                        retorno.Logradouro = dr[6].ToString();
                        retorno.Bairro = dr[7].ToString();
                        retorno.Cidade = dr[8].ToString();
                        retorno.Uf = dr[9].ToString();
                        retorno.Numero = dr[10].ToString();
                        retorno.Complemento = dr[11].ToString();
                        retorno.TelefoneA = dr[12].ToString();
                        retorno.TelefoneB = dr[13].ToString();
                        retorno.Email = dr[14].ToString();
                        retorno.Adm = Convert.ToBoolean(dr[15].ToString());
                        retorno.Ativo = Convert.ToBoolean(dr[16].ToString());
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

        public Boolean setDadosFuncionario(Funcionario funcionario)
        {
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " UPDATE tblFuncionario " +
                " SET CEP = @cep,  " +
                " Logradouro = @logradouro,  " +
                " Bairro = @bairro,  " +
                " Cidade = @cidade,  " +
                " Uf = @uf,  " +
                " Numero = @numero,  " +
                " Complemento = @complemento,  " +
                " TelefoneA = @telefonea,  " +
                " TelefoneB = @telefoneb,  " +
                " Email = @email,  " +
                " Adm = @adm,  " +
                " Ativo = @ativo " +
                " WHERE CPF LIKE @cpf " +
                "";

                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                cmd.Parameters.AddWithValue("@cep", funcionario.Cep);
                cmd.Parameters.AddWithValue("@logradouro", funcionario.Logradouro);
                cmd.Parameters.AddWithValue("@bairro", funcionario.Bairro);
                cmd.Parameters.AddWithValue("@cidade", funcionario.Cidade);
                cmd.Parameters.AddWithValue("@uf", funcionario.Uf);
                cmd.Parameters.AddWithValue("@numero", funcionario.Numero);
                if (funcionario.Complemento == null || funcionario.Complemento.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@complemento", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@complemento", funcionario.Complemento);
                }
                cmd.Parameters.AddWithValue("@telefoneA", funcionario.TelefoneA);
                if (funcionario.TelefoneB == null || funcionario.TelefoneB.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@telefoneB", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@telefoneB", funcionario.TelefoneB);
                }
                cmd.Parameters.AddWithValue("@email", funcionario.Email);
                cmd.Parameters.AddWithValue("@adm", funcionario.Adm);
                cmd.Parameters.AddWithValue("@ativo", funcionario.Ativo);
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

        public String cadastrarFuncionario(Funcionario funcionario, String senha)
        {
            try
            {
                open();

                #region CommandText & Parameters
                cmd.CommandText =
                " IF NOT EXISTS (SELECT CPF FROM tblFuncionario WHERE CPF LIKE @cpf) " +
                " BEGIN " +
                " INSERT INTO dbo.tblFuncionario " +
                " (idFuncionario, " +
                " senha, " +
                " nome, " +
                " sobrenome, " +
                " cpf, " +
                " rg, " +
                " dataNascimento, " +
                " cep, " +
                " logradouro, " +
                " bairro, " +
                " cidade, " +
                " uf, " +
                " numero, " +
                " complemento, " +
                " telefoneA, " +
                " telefoneB, " +
                " email, " +
                " adm, " +
                " fk_CNPJ, " +
                " Ativo) " +
                " VALUES " +
                " (@idFuncionario, " +
                " @senha, " +
                " @nome, " +
                " @sobrenome, " +
                " @cpf, " +
                " @rg, " +
                " @dataNascimento, " +
                " @cep, " +
                " @logradouro, " +
                " @bairro, " +
                " @cidade, " +
                " @uf, " +
                " @numero, " +
                " @complemento, " +
                " @telefoneA, " +
                " @telefoneB, " +
                " @email, " +
                " @adm, " +
                " @fkCnpj, " +
                " 1) " +
                " SELECT 'Cadastrado com sucesso' " +
                " END " +
                " ELSE IF EXISTS (SELECT CPF FROM tblFuncionario WHERE CPF LIKE @cpf AND fk_CNPJ LIKE @fkCnpj AND Ativo = 1) " +
                " BEGIN " +
                " SELECT 'FUNCIONÁRIO JÁ CADASTRADO, QUALQUER ALTERAÇÃO DEVE SER REALIZADA EM CONSULTA DE FUNCIONÁRIOS' " +
                " END " +
                " ELSE IF EXISTS (SELECT CPF FROM tblFuncionario WHERE CPF LIKE @cpf AND fk_CNPJ LIKE @fkCnpj AND Ativo = 0) " +
                " BEGIN " +
                " SELECT 'FUNCIONÁRIO JÁ CADASTRADO, PORÉM INATIVO, FAVOR ATIVAR NA TELA CONSULTA DE FUNCIONÁRIOS!' " +
                " END " +
                " ELSE IF EXISTS (SELECT CPF FROM tblFuncionario WHERE CPF LIKE @cpf AND fk_CNPJ NOT LIKE @fkCnpj) " +
                " BEGIN " +
                " SELECT 'FUNCIONÁRIO PERTENCENTE A OUTRA INSTITUICAO' " +
                " END " +
                "";

                cmd.Parameters.AddWithValue("@idFuncionario", funcionario.IdFuncionario);
                cmd.Parameters.AddWithValue("@senha", senha);
                cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
                cmd.Parameters.AddWithValue("@sobrenome", funcionario.Sobrenome);
                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                cmd.Parameters.AddWithValue("@rg", funcionario.Rg);
                cmd.Parameters.AddWithValue("@dataNascimento", funcionario.DataDeNascimento);
                cmd.Parameters.AddWithValue("@cep", funcionario.Cep);
                cmd.Parameters.AddWithValue("@logradouro", funcionario.Logradouro);
                cmd.Parameters.AddWithValue("@bairro", funcionario.Bairro);
                cmd.Parameters.AddWithValue("@cidade", funcionario.Cidade);
                cmd.Parameters.AddWithValue("@uf", funcionario.Uf);
                cmd.Parameters.AddWithValue("@numero", funcionario.Numero);
                if (funcionario.Complemento == null || funcionario.Complemento.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@complemento", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@complemento", funcionario.Complemento);
                }
                cmd.Parameters.AddWithValue("@telefoneA", funcionario.TelefoneA);
                if (funcionario.TelefoneB == null || funcionario.TelefoneB.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@telefoneB", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@telefoneB", funcionario.TelefoneB);
                }
                cmd.Parameters.AddWithValue("@email", funcionario.Email);
                cmd.Parameters.AddWithValue("@adm", funcionario.Adm);
                cmd.Parameters.AddWithValue("@fkCnpj", ControlViews.cnpj);
                #endregion

                dr = cmd.ExecuteReader();
                String resultado = "ERRO";
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        resultado = dr[0].ToString();
                    }
                }
                close();
                return resultado;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                close();
                return "ERRO";
            }
        }

        public Boolean setSenhaFuncionario(String Cpf, String Senha)
        {
            try
            {
                open();

                #region CommandText
                cmd.CommandText =
                " UPDATE tblFuncionario " +
                " SET Senha = @Senha " +
                " WHERE CPF LIKE @Cpf " +
                "";

                cmd.Parameters.AddWithValue("@Senha", Senha);
                cmd.Parameters.AddWithValue("@Cpf", Cpf);
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
