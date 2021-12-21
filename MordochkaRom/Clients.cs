﻿using System;
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
    public partial class FormClients : Form
    {
        public FormClients()
        {
            InitializeComponent();
            
        }
     
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        string countlabel2;
        string countlabel1;
        string connectionString = "Data Source=10.10.1.24;Initial Catalog=UP_Romash;User ID=pro-41;Password=Pro_41student";
        void View(string sql) // Метод вывода запроса в таблицу клиента
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridViewClients.DataSource = ds.Tables[0];
                connection.Close();
            }

            string sqlExpression = "SELECT COUNT(*) FROM Client";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int count = (int)command.ExecuteScalar();
                countlabel2 = count.ToString();
                connection.Close();
            }
        }

        private void FormClients_Load(object sender, EventArgs e)
        {
            View("SELECT * FROM Client");
            labelrows();
        }

        int str = 0;
        string list;
        int viewtop;
        string toplist;
        int top = 10;
        int rows;       
        string sort;
        int topstat;

        void sheets(int topstr, int str)
        {
            viewtop = topstr * str;
            toplist = Convert.ToString(viewtop);
            list = Convert.ToString(topstr);
            //View("SELECT * FROM Client ORDER BY " + sort + " ID  OFFSET " + toplist + " ROWS FETCH NEXT " + list + " ROWS ONLY");
            View("SELECT Client.ID, FirstName, LastName, Patronymic, Birthday,RegistrationDate, Email, Phone, GenderCode, PhotoPath " + starttime + "  FROM Client " + where+" ORDER BY " + sort + " Client.ID  OFFSET " + toplist + " ROWS FETCH NEXT " + list + " ROWS ONLY");
            labelcount.Text = countlabel1 + "/" + countlabel2;

        }
        void labelrows()
        {
            rows = dataGridViewClients.Rows.Count - 1;
            countlabel1 = rows.ToString();
            labelcount.Text = countlabel1 + "/" + countlabel2;
            if (Convert.ToInt32(countlabel1) == Convert.ToInt32(countlabel2))
            {
                btnForward.Enabled = false;
            }
            else
            { btnForward.Enabled = true; }
            btnBack.Enabled = true;
        }
        private void top10_Click(object sender, EventArgs e)
        {
            top = 10;
            str = 0;
            sheets(top, str);
            labelrows();
            topstat = 10;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {

            if (str > 0)
            {
                rows = Convert.ToInt32(countlabel1) - dataGridViewClients.Rows.Count + 1;
                str = str - 1;
                sheets(top, str);
                //rows = Convert.ToInt32(countlabel1) - dataGridViewClients.Rows.Count + 1;
                countlabel1 = rows.ToString();
                labelcount.Text = countlabel1 + "/" + countlabel2;

            }
            else
            {
                str = 0;
                btnBack.Enabled = false;

            }
            if (Convert.ToInt32(countlabel1) == Convert.ToInt32(countlabel2))
            {
                btnForward.Enabled = false;
            }
            else
            {
                btnForward.Enabled = true;
            }

        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            str = str + 1;
            sheets(top, str);
            btnBack.Enabled = true;
            rows = rows + dataGridViewClients.Rows.Count - 1;
            countlabel1 = rows.ToString();
            labelcount.Text = countlabel1 + "/" + countlabel2;
            if (Convert.ToInt32(countlabel1) == Convert.ToInt32(countlabel2))
            { btnForward.Enabled = false; }

        }

        private void top50_Click(object sender, EventArgs e)
        {
            btnForward.Enabled = true;
            top = 50;
            str = 0;
            sheets(top, str);
            labelrows();
            topstat = 50;
        }
        private void top100_Click(object sender, EventArgs e)
        {
            btnForward.Enabled = true;
            top = 100;
            str = 0;
            sheets(top, str);
            labelrows();
            topstat = 100;
        }
        private void topAll_Click(object sender, EventArgs e)
        {

            View("SELECT * FROM Client");
            labelrows();
            topstat = 0;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
        void topstats(int topstat)
        {
            if (topstat == 10)
            {
                top = 10;
                str = 0;
                sheets(top, str);
                if (topstat == 50)
                {
                    top = 50;
                    str = 0;
                    sheets(top, str);
                    if (topstat == 100)
                    {
                        top = 100;
                        str = 0;
                        sheets(top, str);
                    }
                }

            }
            else 
            {
                View("SELECT Client.ID,  FirstName, LastName, Patronymic, Birthday,RegistrationDate, Email, Phone, GenderCode, PhotoPath " + starttime + " FROM Client " + where+" ORDER BY " + sort + " ID");
            }

        }
        string genderSearch;
        private void BtnMan_Click(object sender, EventArgs e)
        {
            starttime = null;
            where = null;           
            sort = "GenderCode DESC,";
            genderSearch = "ORDER BY GenderCode DESC";
            topstats(topstat);        
            labelrows();
        }

        private void BtnWoman_Click(object sender, EventArgs e)
        {
            starttime = null;
            where = null;           
            sort = "GenderCode ASC,";
            genderSearch = "ORDER BY GenderCode ASC";           
            topstats(topstat);
            labelrows();
        }

        private void BtnAllGender_Click(object sender, EventArgs e)
        {
            starttime = null;
            where = null;            
            sort = null;
            topstats(topstat);
            labelrows();
        }
        
        private void SearchName_TextChanged(object sender, EventArgs e)
        {
            
            View("SELECT * FROM Client  WHERE (FirstName LIKE '" + SearchName.Text+ "%') OR (LastName LIKE '" + SearchName.Text+ "%') OR (Patronymic LIKE '" + SearchName.Text + "%') " + genderSearch + "");
            labelrows();
        }

        private void SearchMail_TextChanged(object sender, EventArgs e)
        {
            View("SELECT * FROM Client WHERE Email LIKE '" + SearchMail.Text + "%'");
            labelrows();
        }

        private void SearchPhone_TextChanged(object sender, EventArgs e)
        {
            View("SELECT * FROM Client WHERE Phone LIKE '" + SearchPhone.Text + "%'");
            labelrows();
        } 

        private void btnsortFname_Click(object sender, EventArgs e)
        {

            sort = "FirstName ASC,";
            topstats(topstat);
            
            labelrows();            
        }
        string starttime;
        string where;
        private void Lastdate_Click(object sender, EventArgs e)
        {
            
            sort = "StartTime DESC,";
            starttime = ",StartTime ";
            where = ",ClientService WHERE Client.ID = ClientService.ClientID";
            topstats(topstat);            
            labelrows();
            
        }
        private void btnVisits_Click(object sender, EventArgs e)
        {
            View("SELECT Client.ID, FirstName,LastName, Patronymic,  Birthday, RegistrationDate, Email, Phone, GenderCode, PhotoPath, COUNT(StartTime) as Visit FROM Client LEFT JOIN ClientService ON ClientService.ClientID = Client.ID Group by Client.ID, FirstName, LastName, Patronymic, Birthday, RegistrationDate, Email, Phone, GenderCode,PhotoPath ORDER BY visit DESC");
            labelrows();
        }
        private void btnBirthday_Click(object sender, EventArgs e)
        {
            starttime = null;
            where = null;
            sort = "DAY(Birthday) DESC,";
            where = "WHERE ABS(Month(GETDATE()) - MONTH(Birthday)) = 0";
            topstats(topstat);
            labelrows();
        }
        int c;
        int visit;
        private void btndelete_Click(object sender, EventArgs e)
        {

            visit = Convert.ToInt32(label4.Text);
            if (visit == 0)
            {
                DialogResult result = MessageBox.Show(
            "Удалить пользователя " + labelNameDel.Text + "?",
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    string sqlExpression = "DELETE FROM Client WHERE ID = " + textBoxID.Text + "";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    dataGridViewClients.Rows.RemoveAt(c);
                    MessageBox.Show("Успешно");
                }
            }
            else
            {
                MessageBox.Show("У пользователя есть посещения");
            }
                

            this.TopMost = true;
        }
        private void dataGridViewClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            c = dataGridViewClients.CurrentCell.RowIndex;
            textBoxID.Text = dataGridViewClients.Rows[c].Cells[0].Value.ToString();
            labelNameDel.Text = dataGridViewClients.Rows[c].Cells[1].Value.ToString();
            
            string sqlExpression = "SELECT  COUNT(StartTime) as visit, Client.ID, FirstName FROM Client LEFT JOIN ClientService ON ClientService.ClientID = Client.ID Where Client.ID = "+textBoxID.Text+" Group by Client.ID, FirstName ORDER BY visit DESC "; 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                string count = command.ExecuteScalar().ToString();

                label4.Text = count.ToString();
            }
           



        }
        Addclient AddForm = new Addclient();
        public string stat;
        private void btnAddClient_Click(object sender, EventArgs e)
        {
            
           
            AddForm = new Addclient();
            AddForm.stat = "add";
            AddForm.Show();
        }

       
        private void BtnUpd_Click(object sender, EventArgs e)
        {
           
            AddForm = new Addclient();
            AddForm.stat = "upd";
            int rowindex = dataGridViewClients.CurrentCell.RowIndex;
            AddForm.textBoxID.Text= dataGridViewClients.Rows[rowindex].Cells[0].Value.ToString();
            AddForm.textBoxFirstName.Text = dataGridViewClients.Rows[rowindex].Cells[1].Value.ToString();
            AddForm.textBoxLastName.Text = dataGridViewClients.Rows[rowindex].Cells[2].Value.ToString();
            AddForm.textBoxPatronymic.Text = dataGridViewClients.Rows[rowindex].Cells[3].Value.ToString();
            string date = dataGridViewClients.Rows[rowindex].Cells[4].Value.ToString();
            var parcedDate = DateTime.Parse(date);
            AddForm.dateTimePicker1.Value = parcedDate;
            AddForm.textBoxEmail.Text = dataGridViewClients.Rows[rowindex].Cells[6].Value.ToString();
            AddForm.textBoxPhone.Text = dataGridViewClients.Rows[rowindex].Cells[7].Value.ToString();
            AddForm.gender = dataGridViewClients.Rows[rowindex].Cells[8].Value.ToString();
            try
            {
                AddForm.pictureBoxPhoto.Image = Image.FromFile("d:\\users\\is12323\\Desktop\\УП\\" + dataGridViewClients.Rows[rowindex].Cells[9].Value.ToString() + "");
            }
            catch 
            {
                AddForm.pictureBoxPhoto.Image = Image.FromFile("d:\\users\\is12323\\Desktop\\УП\\Клиенты\\nonImage.png");
            }
           


            AddForm.Show();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
    }
}
