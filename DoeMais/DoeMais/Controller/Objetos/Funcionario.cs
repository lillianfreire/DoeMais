using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.Controller.Objetos
{
    class Funcionario
    {
        #region Propriedades
        public String IdFuncionario { get; set; }

        public String Nome { get; set; }

        public String Sobrenome { get; set; }

        public String Cpf { get; set; }

        public String Rg { get; set; }

        public DateTime DataDeNascimento { get; set; }

        public String Cep { get; set; }

        public String Logradouro { get; set; }

        public String Bairro { get; set; }

        public String Cidade { get; set; }

        public String Uf { get; set; }

        public String Numero { get; set; }

        public String Complemento { get; set; }

        public string Email { get; set; }

        public string TelefoneB { get; set; }

        public string TelefoneA { get; set; }

        public Boolean Adm { get; set; }

        public Boolean Ativo { get; set; }
        #endregion
    }
}
