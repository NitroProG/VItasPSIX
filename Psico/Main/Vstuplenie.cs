using System;
using System.Drawing;
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
            // Открытие формы Список задач
            SpisokZadach spisokZadach = new SpisokZadach();
            spisokZadach.Show();

            // Закрытие этой формы
            Close();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            // Адаптация по разрешению экрана
            FormAlignment();
        }

        private void FormAlignment()
        {
            // Адаптация по разрешению экрана
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (screen.Width < 1360 && screen.Width > 1000)
            {
                panel2.Width = 1024;
                panel1.Width = 1000;
                richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, 12);
                label1.Left = Width / 2 - label1.Width / 2;
                label2.Left = Width / 2 - label2.Width / 2;
                button1.Left = Width / 2 - button1.Width / 2;
            }

            // Позиционирование элементов формы пользователя
            WindowState = FormWindowState.Maximized;
            BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);
        }
    }
}
