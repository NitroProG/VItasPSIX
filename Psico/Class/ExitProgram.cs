using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;
using Psico;
using System.Threading;
using System.Windows.Forms;
using InsertWord;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Data.SqlClient;
using SqlConn;
using System.Data;

namespace Psico
{
    class ExitProgram
    {
        WordInsert wordinsert = new WordInsert();
        SqlConnection con = DBUtils.GetDBConnection();

        public void ExProgr()
        {
            WaitingForm wf = new WaitingForm();
            wf.UseWaitCursor = true;
            wf.Show();

            try
            {

                //Добавление данных в протокол
                Program.Insert = "Окно - Итоги:";
                wordinsert.Ins();
                Program.Insert = "Время работы с задачей (Без катамнеза): " + Program.AllTBezK + " сек";
                wordinsert.Ins();
                Program.Insert = "Время на этапе феноменология:" + Program.FullAllFenom + " сек";
                wordinsert.Ins();
                Program.Insert = "Время на этапе гипотезы:" + Program.FullAllGip + " сек";
                wordinsert.Ins();
                Program.Insert = "Время на этапе обследования:" + Program.FullAllDpo + " сек";
                wordinsert.Ins();
                Program.Insert = "Время на этапе заключения:" + Program.FullAllZakl + " сек";
                wordinsert.Ins();
                Program.Insert = "Время на этапе мероприятия:" + Program.FullAllMeropr + " сек";
                wordinsert.Ins();
                Program.Insert = "Время на этапе катамнез:" + Program.FullAllKatam + " сек";
                wordinsert.Ins();
                Program.Insert = "Время работы с задачей: " + Program.AllT + " сек";
                wordinsert.Ins();

                Program.Insert = "Окно - Количественные показатели:";
                wordinsert.Ins();
                Program.Insert = "Количество попыток поставить диагноз:" + Program.KolvoOpenZakl + "";
                wordinsert.Ins();

                // подключение к БД
                con.Open();

                // Выбор количества данных в таблице БД
                SqlCommand kolvoProsmotr = new SqlCommand("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Dpo'", con);
                SqlDataReader dr1 = kolvoProsmotr.ExecuteReader();
                dr1.Read();
                Program.Insert = "Количество просмотренных методик:"+ dr1["kolvo"].ToString() + "";
                dr1.Close();
                wordinsert.Ins();                

                ExitProtokol ExPr = new ExitProtokol();
                ExPr.Show();

                con.Close();

                wf.Close();
                ExPr.Close();
            }

            catch
            {
                con.Close();
                MessageBox.Show("Не удалось записать данные в протокол!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                wf.Close();
            }
        }

        public void ExProtokolSent()
        {
            con.Open();

            // Выбор количества данных в таблице БД
            SqlCommand GetUserMail = new SqlCommand("select User_Mail as 'mail' from users where id_user = " + Program.user + "", con);
            SqlDataReader dr1 = GetUserMail.ExecuteReader();
            dr1.Read();
            string UserMail = dr1["mail"].ToString();
            dr1.Close();

            try
            {
                MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", UserMail, "Протокол программы psico.", "Ваш отчёт.");
                SmtpClient client = new SmtpClient("smtp.yandex.ru");
                mail.Attachments.Add(new Attachment(Program.doc));
                client.Port = 587;
                client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                client.EnableSsl = true;
                client.Send(mail);
            }

            catch
            {
                
            }
            con.Close();
        }
    }
}
