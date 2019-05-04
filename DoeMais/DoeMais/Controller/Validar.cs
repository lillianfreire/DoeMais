using DoeMais.Controller.Util;
using System;

namespace DoeMais.Controller
{
    public static class Validar
    {
        public static Boolean cnpj(String cnpj)
        {
            return ValidaCNPJ.IsCnpj(cnpj);
        }

        public static Boolean cpf(String cpf)
        {
            return ValidaCPF.IsCpf(cpf);
        }

        public static Boolean rg(String rg)
        {
            if (rg.Trim().Replace(".", "").Replace("-", "").Length != 9)
                return false;
            return true;
        }

        public static Boolean cep(String cep)
        {
            if (cep.Replace("-", "").Length != 8)
                return false;
            return true;
        }

        public static Boolean telefone(String telefone)
        {
            if (telefone.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Length < 10)
                return false;
            return true;
        }

        public static Boolean email(String email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }
}
