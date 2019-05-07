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

        private void FormLoad(object sender, EventArgs e)
        {
            FormAlignment();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message n = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref n);
        }

        private void FormAlignment()
        {
            // Адаптация разрешения экрана пользователя
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
