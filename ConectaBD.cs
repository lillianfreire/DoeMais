using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace DoeMais.BD
{
    class ConectaBD
    {
        #region Instâncias/Váriavel SQL
	//Edite abaixo
        public SqlConnection conn = new SqlConnection("Data Source=*HOST*;Initial Catalog=DoeMais;Persist Security Info=True;User ID=*usuário*;Password=*senha*");
        public SqlCommand cmd = new SqlCommand();
        public SqlDataReader dr;
        #endregion

        public ConectaBD()
        {
            try
            {
                cmd.Connection = conn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void open()
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void close()
        {
            try
            {
                dr = null;
                cmd.Parameters.Clear();
                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
