using System;
using System.Drawing;
using System.Windows.Forms;

namespace Psico
{
    class FormAlign
    {
        public void Alignment(Panel panel1, Panel panel2, Label label3, Form form, Button button1, Button button2, Button button3)
        {
            // Адаптация под разрешение экрана
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            if (Convert.ToInt32(screen.Size.Width) < 1300)
            {
                form.Width = 1024;
                form.Height = 768;

                panel2.Width = 1024;
                panel2.Height = 768;

                panel1.Width = 1003;
                panel1.Height = 747;

                foreach (Control ctrl in panel1.Controls)
                {
                    if (ctrl is CheckBox)
                    {
                        ctrl.Left = ctrl.Left - 130;
                        ctrl.Font = new Font(ctrl.Font.FontFamily, 8);
                    }
                }
            }

            // Позиционирование элементов формы пользователя
            form.WindowState = FormWindowState.Maximized;
            form.BackColor = Color.PowderBlue;
            panel2.Location = new Point(screen.Size.Width / 2 - panel2.Width / 2, screen.Size.Height / 2 - panel2.Height / 2);
            panel1.Location = new Point(panel2.Width / 2 - panel1.Width / 2, panel2.Height / 2 - panel1.Height / 2);
            label3.Width = panel1.Width;
            label3.TextAlign = ContentAlignment.TopCenter;
            button1.Location = new Point(panel1.Width - button1.Width - 10, panel1.Height - button1.Height - 10);
            button2.Location = new Point(10, panel1.Height - button2.Height - 10);
            button3.Location = new Point(panel1.Width - button3.Width - 10, 10);
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Label || ctrl is RichTextBox)
                {
                    ctrl.Left = panel1.Width / 2 - ctrl.Width / 2;
                }
            }
        }
    }
}
