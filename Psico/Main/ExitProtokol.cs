﻿using System;
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
using Xceed.Words.NET;

namespace Psico
{
    public partial class ExitProtokol : Form
    {
        WordInsert wordinsert = new WordInsert();
        SqlConnection con = DBUtils.GetDBConnection();
        DataGridView datagr = new DataGridView();
        DataGridView datagr1 = new DataGridView();
        int KolvoSelectedCB = 0;

        public ExitProtokol()
        {
            InitializeComponent();
        }

        private void ExitProtokol_Load(object sender, EventArgs e)
        {
            con.Open();            

            // Выбор количества данных в таблице БД
            SqlCommand kolvoSelectedCBFenom = new SqlCommand("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'", con);
            SqlDataReader dr2 = kolvoSelectedCBFenom.ExecuteReader();
            dr2.Read();
            KolvoSelectedCB = Convert.ToInt32(dr2["kolvo"].ToString());
            dr2.Close();

            if (KolvoSelectedCB > 0)
            {
                Program.Insert = "Окно - Выбранные 'Галочки' на этапе Феноменологии:";
                wordinsert.Ins();

                // Динамическое создание таблицы
                datagr.Name = "datagrview";
                datagr.Size = new Size(300, 300);
                datagr.Location = new Point(0, 0);
                SqlDataAdapter da1 = new SqlDataAdapter("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Fenom'", con);
                SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "OtvSelected");
                datagr.DataSource = ds1.Tables[0];
                Controls.Add(datagr);
                datagr.Visible = false;

                for (int i = 0; i < KolvoSelectedCB; i++)
                {
                    Program.Insert = datagr.Rows[i].Cells[0].Value.ToString();
                    wordinsert.Ins();
                }

                // Выбор количества данных в таблице БД
                SqlCommand kolvoALLCBFenom = new SqlCommand("select count(*) as 'kolvo' from CBFormFill where zadacha_id = "+Program.NomerZadachi+" and FormCB = 'Fenom'", con);
                SqlDataReader dr4 = kolvoALLCBFenom.ExecuteReader();
                dr4.Read();
                int KolvoALLCB = Convert.ToInt32(dr4["kolvo"].ToString());
                dr4.Close();

                Program.Insert = "Выбрано "+KolvoSelectedCB+" из "+KolvoALLCB+"  'Галочек'";
                wordinsert.Ins();
            }

            // Выбор количества данных в таблице БД
            SqlCommand kolvoSelectedCBGip = new SqlCommand("select count(*) as 'kolvo' from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Teor'", con);
            SqlDataReader dr3 = kolvoSelectedCBGip.ExecuteReader();
            dr3.Read();
            KolvoSelectedCB = Convert.ToInt32(dr3["kolvo"].ToString());
            dr3.Close();

            if (KolvoSelectedCB > 0)
            {
                Program.Insert = "Окно - Выбранные 'Галочки' на этапе Гипотезы:";
                wordinsert.Ins();

                // Динамическое создание таблицы
                datagr1.Name = "datagrview1";
                datagr1.Size = new Size(300, 300);
                datagr1.Location = new Point(310, 0);
                SqlDataAdapter da1 = new SqlDataAdapter("select InfoSelected from OtvSelected where users_id = " + Program.user + " and FormOtvSelected = 'Teor'", con);
                SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "OtvSelected");
                datagr1.DataSource = ds1.Tables[0];
                Controls.Add(datagr1);
                datagr1.Visible = false;

                for (int i = 0; i < KolvoSelectedCB; i++)
                {
                    Program.Insert = datagr1.Rows[i].Cells[0].Value.ToString();
                    wordinsert.Ins();
                }

                // Выбор количества данных в таблице БД
                SqlCommand kolvoALLCBTeor = new SqlCommand("select count(*) as 'kolvo' from CBFormFill where zadacha_id = " + Program.NomerZadachi + " and FormCB = 'Teor'", con);
                SqlDataReader dr4 = kolvoALLCBTeor.ExecuteReader();
                dr4.Read();
                int KolvoALLCB = Convert.ToInt32(dr4["kolvo"].ToString());
                dr4.Close();

                Program.Insert = "Выбрано " + KolvoSelectedCB + " из " + KolvoALLCB + "  'Галочек'";
                wordinsert.Ins();
            }

            // Закрываем документ
            Program.Insert = "Окончание протокола";
            wordinsert.Ins();

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
                Series seriesFirst = new Series("График по "+Program.NomerZadachi+" задаче.");

                // заполняем данными
                seriesFirst.Bind(GetTestDataFirst(), "name", "value");

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
                Series seriesSecond = new Series("График по " + Program.NomerZadachi + " задаче.");

                // заполняем данными
                seriesSecond.Bind(GetTestDataSecond(), "name", "value");

                return seriesSecond;
            }
        }
    }
}