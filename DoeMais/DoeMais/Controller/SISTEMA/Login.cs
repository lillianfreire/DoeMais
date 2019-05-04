using DoeMais.BD;
using DoeMais.Controller.Util;
using DoeMais.Views;
using System;

namespace DoeMais.Controller.SISTEMA
{
    class Login
    {
        public Boolean logar(String Login, String Senha)
        {
            if (VerificarSeCpfOuCnpj.verificar(Login))
            {//Se for cpf ou cnpj preparar dado para pesquisar no banco
                Login = FormatCnpjCpf.SemFormatacao(Login);//tirando pontos
                if (Login.Length == 11)//formatando de acordo com o tipo
                    Login = FormatCnpjCpf.FormatCPF(Login);
                else
                    Login = FormatCnpjCpf.FormatCNPJ(Login);
            }

            String[] info = new LoginBD().Logar(Login, Senha).ToArray();//fazendo pesquisa no banco
            if (info.Length < 2)
            {//se retornar apenas um dado
                ControlViews.tipoDeAcesso = info[0];
                return false;
            }
            else
            {//se retornar os dados corretamente
                ControlViews.tipoDeAcesso = info[0];
                if (ControlViews.tipoDeAcesso.Equals("funcionario"))
                {
                    ControlViews.idFunc = info[1];
                    ControlViews.cpf = info[2];
                    ControlViews.adm = Convert.ToBoolean(info[3]);
                    ControlViews.cnpj = info[4];
                }
                else if (ControlViews.tipoDeAcesso.Equals("instituicao"))
                {
                    ControlViews.cnpj = info[1];
                }
                return true;
            }
        }
    }
}
