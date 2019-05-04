using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.Controller.Objetos
{
    class Item
    {
        #region Propriedades
        public String Tipo { get; set; }

        public String Nome { get; set; }

        public String Medida { get; set; }

        public String TipoDeMedida { get; set; }

        public String NoEstoque { get; set; }

        public String Validade { get; set; }

        public String Genero { get; set; }

        public String FaixaEtaria { get; set; }

        public String Condicao { get; set; }

        public String Tamanho { get; set; }
        #endregion
        public int QTD = 0;
    }
}
