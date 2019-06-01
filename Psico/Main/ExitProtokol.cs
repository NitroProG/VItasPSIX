using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InsertWord;
using System.Data.SqlClient;
using SqlConn;
using Xceed.Words.NET;

namespace Psico
{
    public partial class ExitProtokol : Form
    {
        WordInsert wordinsert = new WordInsert();
        SqlConnection con = SQLConnectionString.GetDBConnection();
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        int KolvoSelectedCB = 0;

        public ExitProtokol()
        {
            InitializeComponent();
        }

        private void ExitProtokol_Load(object sender, EventArgs e)
        {
            // Подключение к БД
            con.Open();            

            // Выбор количества выбранных вариантов ответа на этапе Феноменология и анамнез
            KolvoSelectedCB = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'"));

            // Если количество выбранных вариантов ответа больше 0
            if (KolvoSelectedCB > 0)
            {
                // Запись данных в Протокол
                Program.Insert = "Окно - Выбранные 'Галочки' на этапе Феноменологии:";
                wordinsert.Ins();

                // Заполнение таблицы datagr
                new SQL_Query().CreateDatagr("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'", "OtvSelected",panel1,datagr);

                // Запись данных в Протокол
                for (int i = 0; i < KolvoSelectedCB; i++)
                {
                    Program.Insert = datagr.Rows[i].Cells[0].Value.ToString();
                    wordinsert.Ins();
                }

                // Выбор количества всех вариантов ответа на этапе Феноменология и анамнез
                int KolvoALLCB = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) as 'kolvo' from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Fenom'"));

                // Запись данных в Протокол
                Program.Insert = "Выбрано "+KolvoSelectedCB+" из "+KolvoALLCB+"  'Галочек'";
                wordinsert.Ins();
            }

            // Выбор количества выбранных вариантов ответа на этапе Гипотезы
            KolvoSelectedCB = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Teor'"));

            // Если количество выбранных вариантов ответа больше 0
            if (KolvoSelectedCB > 0)
            {
                // Запись данных в Протокол
                Program.Insert = "Окно - Выбранные 'Галочки' на этапе Гипотезы:";
                wordinsert.Ins();

                // Заполнение таблицы datagr1
                new SQL_Query().CreateDatagr("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Teor'", "OtvSelected", panel1, datagr1);

                // Запись данных в Протокол
                for (int i = 0; i < KolvoSelectedCB; i++)
                {
                    Program.Insert = datagr1.Rows[i].Cells[0].Value.ToString();
                    wordinsert.Ins();
                }

                // Выбор количества всех вариантов ответа на этапе Гипотезы
                int KolvoALLCB = Convert.ToInt32(new SQL_Query().GetInfoFromBD("select count(*) as 'kolvo' from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Teor'"));

                // Запись данных в протокол
                Program.Insert = "Выбрано " + KolvoSelectedCB + " из " + KolvoALLCB + "  'Галочек'";
                wordinsert.Ins();
            }

            // Закрываем документ
            Program.Insert = "Окончание протокола";
            wordinsert.Ins();

            // Если студент заходил на этапы
            if (Program.StageName.Count !=0)
            {
                // Открываем документ
                DocX document = DocX.Load(Program.doc);


                // создаём столбцовую диаграмму
                BarChart barChart = new BarChart();
                // создаём набор данных и добавляем в диаграмму
                barChart.AddSeries(TestData.GetSeriesFirst());
                // добавляем столбцовую диаграмму
                document.InsertChart(barChart);


                // создаём линейную диаграмму
                LineChart lineChart = new LineChart();
                // создаём набор данных и добавляем на диаграмму
                lineChart.AddSeries(TestData.GetSeriesSecond());
                // добавляем линейную диаграмму
                document.InsertChart(lineChart);


                // сохраняем документ
                document.Save();
            }
        }

        class TestData
        {
            public string name { get; set; }
            public int value { get; set; }

            private static List<TestData> GetTestDataFirst()
            {
                List<TestData> testDataFirst = new List<TestData>();

                for (int i = 0; i < Program.StageSec.Count; i++)
                {
                    testDataFirst.Add(new TestData() { name = Program.StageName[i].ToString(), value = Program.StageSec[i] });
                }

                return testDataFirst;
            }

            public static Series GetSeriesFirst()
            {
                // создаём набор данных
                Series seriesFirst = new Series("График по "+Program.NomerZadachi+" задаче. Последовательность этапов диагностического процесса (С учётом времени).");

                // заполняем данными
                seriesFirst.Bind(GetTestDataFirst(), "name", "value");

                // Возвращаем данные
                return seriesFirst;
            }

            private static List<TestData> GetTestDataSecond()
            {
                List<TestData> testDataSecond = new List<TestData>();

                for (int i = 0; i < Program.NumberStage.Count; i++)
                {
                    testDataSecond.Add(new TestData() { name = Program.StageName[i].ToString(), value = Program.NumberStage[i] });
                }

                return testDataSecond;
            }

            public static Series GetSeriesSecond()
            {
                // создаём набор данных
                Series seriesSecond = new Series("График по " + Program.NomerZadachi + " задаче. Последовательность этапов диагностического процесса.");

                // заполняем данными
                seriesSecond.Bind(GetTestDataSecond(), "name", "value");

                // Возвращаем данные
                return seriesSecond;
            }
        }
    }
}
