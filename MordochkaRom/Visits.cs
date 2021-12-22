using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MordochkaRom
{
    public partial class Visits : Form
    {
        public Visits()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=10.10.1.24;Initial Catalog=UP_Romash;User ID=pro-41;Password=Pro_41student";
        void View(string sql) // Метод вывода запроса в таблицу клиента
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
               dataGridViewVIsits.DataSource = ds.Tables[0];
                connection.Close();
            }
        }
            private void Visits_Load(object sender, EventArgs e)
        {
            View("SELECT Title, StartTime, DurationInSeconds / 60 AS Minuts FROM Client,ClientService,Service WHERE Client.ID =ClientService.ClientID AND ServiceID = Service.ID AND Client.ID = "+labelID.Text+" ");
        }
    }
}
