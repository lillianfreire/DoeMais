using System;

namespace DoeMais.Controller.Util
{
    class VerificarSeCpfOuCnpj
    {
        public static Boolean verificar(String texto)
        {
            try
            {
                String teste = FormatCnpjCpf.SemFormatacao(texto);
                ulong teste2 = Convert.ToUInt64(teste);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
