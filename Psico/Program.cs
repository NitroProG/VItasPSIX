using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Psico
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Autorization());
        }

        // Номер решаемой задачи
        public static int NomerZadachi;

        // Введённые данные пользователем
        public static string fenomenologiya; // Резюме по феноменологии
        public static string glavsved; // Главные сведения по феноменологии
        public static string gipotezi; // Гипотезы
        public static string obsledovaniya; // Данные по обследованиям
        public static string zakluch; // Заключение
        public static string rekMa; // Рекомендации матери
        public static string rekPodr; // Рекомендации подростку
        public static string rekRukovod; // Рекомендации классному руководителю

        // Перевод на машинный язык
        public static int user; // Код пользователя в программе
        public static int zaklOTV; // Правильные ответы
        public static int NeVernOtv; // Неправильные ответы
        public static int diagnoz; // Итоговый диагноз

        // Общие сведения тестируемого
        public static string doc; // Путь к документу
        public static string FIO; // ФИО пользователя
        public static string Study; // Образование
        public static string Work; // Стаж и место работы
        public static string Year; // Год обучения
        public static string Old; // Возраст

        // Данные необходимые для записи в ворд
        public static string Insert;

        //Время по программе
        public static int AllT; // Общее время на задаче
        public static int AllTBezK; // Общее время на задаче без катамнеза

        public static int Fenom1T; // Время на первой форме феноменологии
        public static int Fenom2T; // Время на второй форме феноменологии
        public static int AllFenom; // Общее время на этапе феноменологии
        public static int FullAllFenom; // Всё время на этапе феноменологии

        public static int gip1T; // Время на первой форме гипотез
        public static int gip2T; // Время на второй форме гипотез
        public static int AllGip; // Общее время на этапе феноменологии
        public static int FullAllGip; // Всё время на этапе гипотез

        public static int infochekT; // Время просмотра данных по обследованию
        public static int dpoT; // Время на форме обследований
        public static int FullAllDpo; // Всё время на этапе обследований

        public static int zakl1T; // Время на первой форме заключения
        public static int zakl2T; // Время на второй форме заключения
        public static int AllZakl; // Общее время на этапе феноменологии
        public static int FullAllZakl; // Всё время на этапе заключения
        public static int KolvoOpenZakl; // Количество попыток поставить диагноз

        public static int meropr1T; // Время на первой форме мероприятий
        public static int meropr2T; // Время на второй форме мероприятий
        public static int AllMeropr; // Общее время на этапе феноменологии
        public static int FullAllMeropr; // Всё время на этапе заключения

        public static int katamT; // Время на форме катамнеза
        public static int FullAllKatam; // Всё время на этапе катамнез
    }
}
