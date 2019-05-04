using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoeMais.Controller.Objetos;

namespace DoeMais.Controller.Util
{
    public class EnderecoDados
    {
        public Endereco GET(String cep)
        {
            String CEP = cep.Replace("-", "");

            try
            {
                var ws = new WSCorreios.AtendeClienteClient();
                var resultado = ws.consultaCEP(CEP);

                Endereco endereco = new Endereco();

                endereco.CEP = cep;
                endereco.Logradouro = resultado.end;
                endereco.Bairro = resultado.bairro;
                endereco.Cidade = resultado.cidade;
                endereco.UF = resultado.uf;

                return endereco;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
