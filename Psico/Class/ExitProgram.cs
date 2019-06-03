using System.Windows.Forms;
using InsertWord;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using SqlConn;

namespace Psico
{
    class ExitProgram
    {
        WordInsert wordinsert = new WordInsert();
        SqlConnection con = SQLConnectionString.GetDBConnection();

        public void ExProgr()
        {

            // Открытие формы ожидания формирования протокола
            WaitingForm wf = new WaitingForm();
            wf.Show();
            wf.UseWaitCursor = true;

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
                Program.Insert = "Количество просмотренных методик:" + new SQL_Query().GetInfoFromBD("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Dpo'") + "";

                // Открытие формы выхода из программы
                ExitProtokol ExPr = new ExitProtokol();
                ExPr.Show();

                // Закрытие формы
                wf.Close();
                ExPr.Close();
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные в протокол!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                wf.Close();
            }
        }

        public void ExProtokolSent()
        {
            // Выбор почты главного администратора
            string UserMail = new Shifr().DeShifrovka(new SQL_Query().GetInfoFromBD("select User_Mail as 'mail' from users where id_user = 1"), "Mail");

            // Отправка сообщения с протоколом на почту главному администратору
            try
            {
                MailMessage mail = new MailMessage("ProgrammPsicotest@yandex.ru", UserMail, "Протокол от программы psico.", "");
                SmtpClient client = new SmtpClient("smtp.yandex.ru");
                mail.Attachments.Add(new Attachment(Program.doc));
                client.Port = 587;
                client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                client.EnableSsl = true;
                client.Send(mail);
            }catch { }

            // Изменение статуса пользователя на "Не в сети"
            new SQL_Query().UpdateOneCell("UPDATE users SET UserStatus=0 WHERE id_user = " + Program.user + "");
        }
    }
}
