using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using SqlConn;
using InsertWord;

namespace Psico
{
    public partial class Zadacha : Form
    {
        SqlConnection con = SQLConnectionString.GetDBConnection();
        WordInsert wordinsert = new WordInsert();
        ExitProgram exitProgram = new ExitProgram();
        string podzadacha;

        public Zadacha()
        {
            InitializeComponent();
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            // Если задача решена верно
            if (Program.diagnoz == 3)
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Формирование протокола
                    ExitFromReshZadacha();

                    // Отправка протокола по почте главному администратору
                    exitProgram.ExProtokolSent();

                    // Выход из программы
                    Application.Exit();
                }
            }

            // Если задача не решена
            else
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Формирование протокола
                    exitProgram.ExProgr();

                    // Отправка протокола на почту главному администратору
                    exitProgram.ExProtokolSent();

                    // Выход из программы
                    Application.Exit();
                }
            }
        }

        private void OpenSpisokZadach(object sender, EventArgs e)
        {
            // Если задача решена верно
            if (Program.diagnoz == 3)
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, у вас не будет возможности вернутся к этой задаче!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Формирование протокола
                    ExitFromReshZadacha();

                    // Открытие формы со списком задач
                    SpisokZadach spisokZadach = new SpisokZadach();
                    spisokZadach.Show();
                    Close();
                }
            }

            // Если задача не решена
            else
            {
                // Вывод сообщения
                DialogResult result = MessageBox.Show("Если вы закроете программу, ваши данные не сохранятся!", "Внимание!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                // Если пользователь нажал ОК
                if (result == DialogResult.OK)
                {
                    // Формирование протокола
                    exitProgram.ExProgr();

                    // Открытие формы со списокм задач
                    SpisokZadach spisokZadach = new SpisokZadach();
                    spisokZadach.Show();
                    Close();
                }
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            //Выбор данных из БД
            con.Open();

            // Запись данных из БД
            label3.Text = new SQL_Query().GetInfoFromBD("select Zapros from zadacha where id_zadacha = " + Program.NomerZadachi + "");
            label1.Text = "Задача №" + Convert.ToString(Program.NomerZadachi) + "   " + new SQL_Query().GetInfoFromBD("select sved from zadacha where id_zadacha = " + Program.NomerZadachi + "") + "";

            // Проверка решения задачи
            switch (Program.diagnoz)
            {
                case 1:
                    label4.Text = "Диагноз неверный";
                    label4.ForeColor = Color.Red;
                    break;
                case 2:
                    label4.Text = "Диагноз частично верный";
                    label4.ForeColor = Color.Green;
                    break;
                case 3:
                    label4.Text = "Диагноз верный";
                    label4.ForeColor = Color.Lime;
                    radioButton3.Enabled = true;
                    radioButton6.Enabled = true;
                    break;
                default:
                    label4.Text = "";
                    break;
            }

            // Адаптация под разрешение экрана
            new FormAlign().Alignment(panel1,panel2,label3,this,button1,button2,button3);
        }

        private void OpenCheckForm(object sender, EventArgs e)
        {
            // Переход на выбранную форму
            switch (podzadacha)
            {
                case "1":
                   Fenom1 fenom1 = new Fenom1();
                   fenom1.Show();
                   Close();
                   break;
                case "2":
                   teor1 teor1 = new teor1();
                   teor1.Show();
                   Close();
                   break;
                case "3":
                   dpo dpo = new dpo();
                   dpo.Show();
                   Close();
                   break;
                case "4":
                   dz1 dz1 = new dz1();
                   dz1.Show();
                   Close();
                   break;
                case "5":
                   meropriyatiya1 meropriyatiya1 = new meropriyatiya1();
                   meropriyatiya1.Show();
                   Close();
                   break;
                case "6":
                   katamnez katamnez = new katamnez();
                   katamnez.Show();
                   Close();
                   break;
                default:
                    MessageBox.Show("Вы ничего не выбрали!","Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "1";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "2";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "3";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "4";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "5";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            podzadacha = "6";
        }

        private void ExitFromReshZadacha()
        {
            // Добавление данных о решении задачи пользователем
            SqlCommand StrPrc1 = new SqlCommand("resh_add", con);
            StrPrc1.CommandType = CommandType.StoredProcedure;
            StrPrc1.Parameters.AddWithValue("@Users_id", Program.user);
            StrPrc1.Parameters.AddWithValue("@Zadacha_id", Program.NomerZadachi);
            StrPrc1.ExecuteNonQuery();

            // Формирование протокола
            exitProgram.ExProgr();
        }
    }
}
