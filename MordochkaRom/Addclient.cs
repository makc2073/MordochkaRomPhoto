using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MordochkaRom
{
    public partial class Addclient : Form
    {
        public Addclient()
        {
            InitializeComponent();
          
        }
        /// <summary>
        /// Метод добавления/Обналвения данных
        /// </summary>
        /// <param name="sqlExpr">Запрос</param>
        void add(string sqlExpr)
        {
            string connectionString ="Data Source = 10.10.1.24; Initial Catalog =UP_Romash; User ID = pro-41; Password = Pro_41student";
            string sqlExpression = sqlExpr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        /// <summary>
        /// Возвращает подходящий формат
        /// </summary>
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy";            
        }
        
        /// <summary>
        /// Загрузка фомры добавления/редактирования
        /// </summary>  
        private void Addclient_Load(object sender, EventArgs e)
        {            
            //По умолчанию выбран мужской пол, при редактировании в зависимости от пола параметр будет выбран автоматически
            if (gender == null)
            { radioButtonMan.Checked = true; }
            else
            { if (gender == "м") radioButtonMan.Checked = true; else radioButtonWoman.Checked = true;     }                
        }
        /// <summary>
        /// Принимает текст с текстбоксов, проверяет их на длинну и запрещает ввод некоторых символов
        /// </summary>
        /// <param name="textbox">Текст с текстбоксов ФИО</param>
        /// <returns></returns>
        string checkFIO(string textbox)
        {
            if (Regex.IsMatch(textbox, @"[1\\2\\3\\4\\5\\6\\7\\8\\9\\0\\!\#\$\%\^\&\*\(\)\}\{\,\.\,\/\\?\'\+\=\:\;\№@]"))
            {
                textbox = Regex.Replace(textbox, @"[1\\2\\3\\4\\5\\6\\7\\8\\9\\0\\!\#\$\%\^\&\*\(\)\}\{\,\.\,\/\\?\'\+\=\:\;\№@]", "");
            }
            if (textbox.Length > 50)
            {
                MessageBox.Show("Длина данного поля не может быть больше 50");
                textbox = textbox.Substring(0, 50);
            }
            return textbox;
        }       
         bool rightEmail; // параметр принимающий значение true/false в зависимости от правильности ввода почты
        /// <summary>
        /// Проверка ввода почты
        /// </summary>       
        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBoxEmail.Text, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
            {
                rightEmail = true;
            }
            else
            {
                rightEmail = false;
            }
        }

        private void textBoxFirstName_TextChanged(object sender, EventArgs e) // Проверка ввода фамилии
        {
            textBoxFirstName.Text = checkFIO(textBoxFirstName.Text);
        }

        private void textBoxLastName_TextChanged(object sender, EventArgs e) // провперка ввода имени
        {
            textBoxLastName.Text = checkFIO(textBoxLastName.Text);
        }
        private void textBoxPatronymic_TextChanged(object sender, EventArgs e) // проверка ввода отчества
        {
            textBoxPatronymic.Text = checkFIO(textBoxPatronymic.Text);
        }           
       
        private void textBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '-')
                e.Handled = true;
        }
        public string gender; // принимает  значения в зависимости от выбранного пола
        private void radioButtonMan_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMan.Checked == true) gender = "м"; 
        }

        private void radioButtonWoman_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonWoman.Checked == true) gender = "ж"; 
        }
       public string stat; // принимает занчание  add/upd в зависимости от того какие действия производятся над пользователем добавление/редктирование
      
        /// <summary>
      /// Сохраняет изменения (давабение нового клиента/ редактирование старого)
      /// </summary>
      
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (rightEmail == true)
            {
                try
                {
                    if (stat == "add")
                    {
                        add("INSERT INTO Client (FirstName,LastName,Patronymic,Birthday,RegistrationDate,Email,Phone,GenderCode, PhotoPath) Values ('" + textBoxFirstName.Text + "','" + textBoxLastName.Text + "','" + textBoxPatronymic.Text + "','" + dateTimePicker1.Text + "' ,GETDATE(), '" + textBoxEmail.Text + "', '" + textBoxPhone.Text + "', '" + gender + "', '"+Photoname+"')");
                        MessageBox.Show("Регистрация прошла успешно"); // добавление пользователя
                    }
                    else
                    {
                        add("UPDATE Client SET FirstName= '"+textBoxFirstName.Text+"', LastName = '"+ textBoxLastName.Text+"', Patronymic ='"+textBoxPatronymic.Text+"', Birthday ='"+dateTimePicker1.Text+"', Email = '"+textBoxEmail.Text+"', Phone ='"+textBoxPhone.Text+"', GenderCode = '"+gender+ "', PhotoPath ='"+Photoname+"'  WHERE ID= " + textBoxID.Text+"");
                        MessageBox.Show("Изменения сохранены"); // Обновление данных о пользователе
                    }
                }
                catch
                {
                    MessageBox.Show("Возникла ошибка при сохранении, проверьте заполнение всех полей"); 
                }
            }
            else { MessageBox.Show("Неправильный формат почты!"); }
               

        }
        OpenFileDialog open = new OpenFileDialog();
        string Photoname; // принимает название картинки
        /// <summary>
        /// Выбор и отображение новой  картинки
        /// </summary>    
        private void btnPhoto_Click(object sender, EventArgs e)
        {
            if (open.ShowDialog() == DialogResult.Cancel)// Выбор картинки
                return;
            // получаем выбранный файл
            string filename = open.FileName;           
            try { File.Copy(open.FileName, Path.Combine(@"d:\users\is12323\Desktop\УП\Клиенты", Path.GetFileName(open.FileName))); }
            catch {  }            
            string part = Path.GetFileName(open.FileName);
            Photoname = @"Клиенты\" + part; // сохраняет путь к картинке
            pictureBoxPhoto.Image = Image.FromFile("d:\\users\\is12323\\Desktop\\УП\\" + Photoname + ""); //отображает выбранную картинку
        }

      
    }
}
