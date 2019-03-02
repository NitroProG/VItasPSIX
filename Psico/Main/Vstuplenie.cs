using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Psico
{
    public partial class Vstuplenie : Form
    {
        public Vstuplenie()
        {
            InitializeComponent();
        }

        private void OpenNextForm(object sender, EventArgs e)
        {
            SpisokZadach spisokZadach = new SpisokZadach();
            spisokZadach.Show();
            Close();
        }

        private void ExitFromProgram(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Адаптация разрешения экрана пользователя
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1300)
            {
                Width = 1024;
                Height = 768;
                panel2.Width = 1024;
                panel2.Height = 768;
                int newFontSize = 12;
                richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, newFontSize);
                button3.Left = button3.Left - 350;
            }

            // Позиционирование элементов формы пользователя
            panel1.Left = Width / 2 - panel1.Width / 2;
            Left = Convert.ToInt32(screen.Size.Width) / 2 - Width / 2;  
            label1.Left = Width / 2 - label1.Width / 2;
            label2.Left = Width / 2 - label2.Width / 2;
            button1.Left = Width / 2 - button1.Width / 2;
        }
    }
}
