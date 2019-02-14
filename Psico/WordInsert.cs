using System;
using word = Microsoft.Office.Interop.Word; // Подключение ворд
using Psico; // Подключение основной программы
using System.Threading;

namespace InsertWord
{
    class WordInsert
    {

        public void Ins()
        {
            // Объявление и запуск потока
            Thread thread = new Thread(Insert);
            thread.Start();
            thread.Join();
        }

        public void Insert()
        {

            var wordApp = new word.Application(); // Создание процесса ворд

            var wordDocument = wordApp.Documents.Open(Program.doc); // Открытие документа

            Object missing = System.Reflection.Missing.Value;

            word.Paragraph para1 = wordDocument.Content.Paragraphs.Add(ref missing); // Добавление параграфа
            string proverka1 = "Диагностическая"; // Проверка на входные данные
            string proverka2 = "Время"; // Проверка на входные данные

            if (Program.Insert.IndexOf(proverka1) == 0) // Если входные данные включают в себя слово Диагностическая после 0 символа
            {
                para1.Range.Text = Program.Insert; // Текст который будет записан
                para1.Range.Font.Size = 18; // Размер текста
                para1.Range.Font.Color = word.WdColor.wdColorBrown; // Цвет текста
                para1.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter; // Выравнивание
            }

            else if (Program.Insert.IndexOf(proverka2) == 0) // Если входные данные включают в себя слово Время после 0 символа
            {
                para1.Range.Text = "     • " + Program.Insert; // Текст который будет записан
                para1.Range.Font.Size = 14;
                para1.Range.Font.Color = word.WdColor.wdColorDarkBlue;
                para1.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphJustify; // Выравнивание
            }

            else
            {
                para1.Range.Text = "          → " + Program.Insert; // Текст который будет записан
                para1.Range.Font.Size = 12;
                para1.Range.Font.Color = word.WdColor.wdColorDarkGreen;
            }

            para1.Range.Font.Name = "Times New Roman"; // Наименование шрифта
            para1.Range.InsertParagraphAfter(); // Добавление нового параграфа

            wordDocument.SaveAs2(Program.doc); // Сохранение документа
            wordApp.Quit(); // освобождение процесса ворд
        }

        public void CBIns()
        {
            // Объявление и запуск потока
            Thread thread = new Thread(CheckboxInsert);
            thread.Start();
            thread.Join();
        }

        public void CheckboxInsert()
        {

            var wordApp = new word.Application(); // Создание процесса ворд
            var wordDocument = wordApp.Documents.Open(Program.doc); // Открытие документа

            Object missing = System.Reflection.Missing.Value;

            word.Paragraph para1 = wordDocument.Content.Paragraphs.Add(ref missing); // Добавление параграфа

            para1.Range.Text = "          → " + Program.Insert; // Текст который будет записан
            para1.Range.Font.Size = 12;
            para1.Range.Font.Color = word.WdColor.wdColorViolet;

            para1.Range.Font.Name = "Times New Roman"; // Наименование шрифта
            para1.Range.InsertParagraphAfter(); // Добавление нового параграфа

            wordDocument.SaveAs2(Program.doc); // Сохранение документа
            wordApp.Quit(); // освобождение процесса ворд
        }

    }
}