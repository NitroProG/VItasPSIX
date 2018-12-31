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
            Application.Run(new SpisokZadach());
        }

        public static int NomerZadachi;

        public static int time = 0;

        public static string fio;
        public static string obrazov;
        public static string mestrab;
        public static string godob;
        public static string vozr;

        public static int fenomt;
        public static int teort;
        public static int dpot;
        public static int dzt;
        public static int katamt;
        public static int meroprt;

        public static string fenomenologiya;
        public static string glavsved;
        public static string gipotezi;
        public static string obsledovaniya;
        public static string zakluch;
        public static int zaklOTV;
        public static int NeVernOtv;
        public static int diagnoz;
    }
}
