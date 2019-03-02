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

namespace Psico
{
    class ExitProgram
    {
        WordInsert wordinsert = new WordInsert();

        public void ExProgr()
        {
            WaitingForm wf = new WaitingForm();
            wf.Show();

            try
            {
                Program.AllT = Program.AllT + Program.MainT;

                //Добавление данных в протокол
                Program.Insert = "Окно - Итоговое время:";
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
                Program.Insert = "Окончание протокола";
                wordinsert.Ins();
                wf.Close();
            }

            catch
            {
                MessageBox.Show("Не удалось записать данные в протокол!","Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                wf.Close();
            }
        }

        public void ProtokolSent()
        {
            try
            {
                MailMessage mail = new MailMessage("ProgrammPsicotest", "vit.sax@yandex.ru", "Протокол программы psico", "");
                SmtpClient client = new SmtpClient("smtp.yandex.ru");
                mail.Attachments.Add(new Attachment(Program.doc));
                client.Port = 587;
                client.Credentials = new NetworkCredential("ProgrammPsicotest@yandex.ru", "DogCatPigMonkeyLionTiger");
                client.EnableSsl = true;
                client.Send(mail);
            }

            catch
            {
                //MessageBox.Show("Ошибка отправки протокола на почту, пожалуйста подключитесь к интернету.","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
