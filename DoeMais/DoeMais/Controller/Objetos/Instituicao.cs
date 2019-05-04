using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.Controller.Objetos
{
    class Instituicao
    {
        #region Propriedades
        public String RazaoSocial { get; set; }

        public String NomeFantasia { get; set; }

        public string Email { get; set; }

        public string TelefoneB { get; set; }

        public string TelefoneA { get; set; }

        public String Cep { get; set; }

        public String Logradouro { get; set; }

        public String Bairro { get; set; }

        public String Cidade { get; set; }

        public String Uf { get; set; }

        public String Numero { get; set; }

        public String Complemento { get; set; }

        public String ResumoEmpresa { get; set; }

        public Boolean RetiraDoacao { get; set; }

        public DateTime HoraAbre { get; set; }

        public DateTime HoraFecha { get; set; }

        public String DiasAbertos { get; set; }
        #endregion

    }
}
