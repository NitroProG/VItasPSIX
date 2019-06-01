using System;
using word = Microsoft.Office.Interop.Word;
using Psico;
using System.Threading;
using System.Windows.Forms;

namespace InsertWord
{
    class WordInsert
    {
        public void Ins()
        {
            try
            {
                // Создание процесса Word
                var wordApp = new word.Application();

                // Открытие Word документа
                var wordDocument = wordApp.Documents.Open(Program.doc);
                
                Object missing = System.Reflection.Missing.Value;

                // Закрытие документа и процесса Word
                if (Program.Insert == "Окончание протокола")
                {
                    wordApp.Documents.Close();
                    wordApp.Quit();
                }

                // Запись данных в протокол
                else
                {
                    word.Paragraph para1 = wordDocument.Content.Paragraphs.Add(ref missing);

                    string proverka1 = "Диагностическая";
                    string proverka2 = "Время";
                    string proverka3 = "Окно";
                    string proverka4 = "Диагноз";

                    // Проверка записанных данных
                    if (Program.Insert.IndexOf(proverka1) == 0)
                    {
                        para1.Range.Text = Program.Insert;
                        para1.Range.Font.Size = 18;
                        para1.Range.Font.Color = word.WdColor.wdColorBrown;
                        para1.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }

                    // Проверка записанных данных
                    else if (Program.Insert.IndexOf(proverka2) == 0)
                    {
                        para1.Range.Text = "     • " + Program.Insert;
                        para1.Range.Font.Size = 14;
                        para1.Range.Font.Color = word.WdColor.wdColorDarkBlue;
                        para1.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphJustify;
                    }

                    // Проверка записанных данных
                    else if (Program.Insert.IndexOf(proverka3) == 0)
                    {
                        para1.Range.Text = Program.Insert;
                        para1.Range.Font.Size = 16;
                        para1.Range.Font.Color = word.WdColor.wdColorBlack;
                        para1.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphJustify;
                    }

                    // Проверка записанных данных
                    else if (Program.Insert.IndexOf(proverka4) == 0)
                    {
                        para1.Range.Text = "          → " + Program.Insert;
                        para1.Range.Font.Size = 16;
                        para1.Range.Font.Color = word.WdColor.wdColorDarkGreen;
                        para1.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphJustify;
                    }

                    // Проверка записанных данных
                    else
                    {
                        para1.Range.Text = "          → " + Program.Insert;
                        para1.Range.Font.Size = 12;
                        para1.Range.Font.Color = word.WdColor.wdColorDarkGreen;
                        para1.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphJustify;
                    }

                    para1.Range.Font.Name = "Times New Roman";
                    para1.Range.InsertParagraphAfter();

                    // Сохранение документа и выход
                    wordDocument.Save();
                    wordApp.Quit();
                }
            }
            catch
            {
                // Вывод сообщения
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            try
            {
                // Создание Word процесса
                var wordApp = new word.Application();

                // Открытие Word документа
                var wordDocument = wordApp.Documents.Open(Program.doc);

                // Запись данных в протокол
                Object missing = System.Reflection.Missing.Value;

                word.Paragraph para1 = wordDocument.Content.Paragraphs.Add(ref missing);

                para1.Range.Text = "          → " + Program.Insert;
                para1.Range.Font.Size = 12;
                para1.Range.Font.Color = word.WdColor.wdColorViolet;

                para1.Range.Font.Name = "Times New Roman";
                para1.Range.InsertParagraphAfter();

                // Сохранение документа и выход 
                wordDocument.Save();
                wordApp.Quit();
            }
            catch
            {
                // Вывод сообщения
                MessageBox.Show("Отсутствует шаблон протокола! Обратитесь в службу поддержки.", "Внимание!",
                                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void CreateShift()
        {
            // Создание Word процесса
            var wordApp = new word.Application();

            // Открытие WOrd документа
            var wordDocument = wordApp.Documents.Open(Program.doc);

            //Вставка разрыва страницы
            object unit;
            object extend;
            unit = word.WdUnits.wdStory;
            extend = word.WdMovementType.wdMove;
            wordApp.Selection.EndKey(ref unit, ref extend);
            object oType;
            oType = word.WdBreakType.wdSectionBreakNextPage;
            wordApp.Selection.InsertBreak(ref oType);

            // Сохранение документа и выход
            wordApp.Documents.Save();
            wordApp.Quit();
        }
    }
}