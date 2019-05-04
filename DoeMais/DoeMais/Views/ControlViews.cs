using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoeMais.Views
{
    class ControlViews
    {
        public static String tipoDeAcesso = "";
        public static String idFunc = "";
        public static String cpf = "";
        public static Boolean adm = false;
        public static String cnpj = "";
        public ControlViews()
        {
            new MenuWindow().Show();
        }
    }
}
