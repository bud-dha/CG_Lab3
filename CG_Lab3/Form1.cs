using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int xn, yn, xk, yk; // концы отрезка
        Bitmap myBitmap; // объект Bitmap для вывода отрезка
        Color currentColorBorder, currentFillColor; // текущий цвет отрезка и заливки
        Graphics g;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            xn = e.X;
            yn = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (g)
                {
                    myBitmap = pictureBox1.Image as Bitmap;
                    CDA(xn, yn, e.X, e.Y);
                    pictureBox1.Refresh();
                    pictureBox1.Image = myBitmap;
                }

                pictureBox1.Image = myBitmap;
            }
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        //выполнить
        private void button1_Click(object sender, EventArgs e)
        {
            //отключаем кнопки
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            //создаем новый экземпляр Bitmap размером с элемент

            //на нем выводим попиксельно отрезок
            myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromHwnd(pictureBox1.Handle);
            using (g)
            {
                if (radioButton1.Checked == true)
                {
                    //рисуем прямоугольник
                    CDA(10, 10, 10, 110);
                    CDA(10, 10, 110, 10);
                    CDA(10, 110, 110, 110);
                    CDA(110, 10, 110, 110);

                    //рисуем треугольник
                    CDA(150, 10, 150, 200);
                    CDA(150, 200, 250, 50);
                    CDA(150, 10, 250, 175);

                    //рисуем сложную фигуру
                    /*
                    CDA(150, 10, 150, 210);
                    CDA(150, 210, 300, 150);
                    CDA(300, 150, 225, 110);
                    CDA(225, 110, 300, 75);
                    CDA(300, 75, 150, 10);*/

                }
                else if (radioButton2.Checked == true)
                {

                    //получаем растр созданного рисунка в mybitmap
                    myBitmap = pictureBox1.Image as Bitmap;

                    // вызываем рекурсивную процедуру заливки с затравкой
                    Zaliv(xn, yn);

                }
                //передаем полученный растр mybitmap в элемент pictureBox
                pictureBox1.Image = myBitmap;
                //обновляем pictureBox и активируем кнопки
                pictureBox1.Refresh();
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                xn = 0;
                yn = 0;
                xk = 0;
                yk = 0;
            }
        }

        //цвет
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = colorDialog1.ShowDialog();

            if (dialogResult.Equals(DialogResult.OK) && radioButton1.Checked)
            {
                currentColorBorder = colorDialog1.Color;
            }
            if (dialogResult.Equals(DialogResult.OK) && radioButton2.Checked)
            {
                currentFillColor = colorDialog1.Color;
            }
        }

        //очистить
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void CDA(int x1, int y1, int x2, int y2)
        {
            int i, n;
            double xt, yt, dx, dy;

            xn = x1;
            yn = y1;
            xk = x2;
            yk = y2;
            dx = xk - xn;
            dy = yk - yn;
            n = 300;
            xt = xn;
            yt = yn;
            for (i = 1; i <= n; i++)
            {
                myBitmap.SetPixel((int)xt, (int)yt, currentColorBorder);
                xt += dx / n;
                yt += dy / n;
            }
        }

        private void Zaliv(int x1, int y1)
        {
            Color oldСolor;
            // получаем цвет текущего пикселя с координатами x1, y1
            oldСolor = myBitmap.GetPixel(x1, y1);
            // сравнение цветов происходит в формате RGB
            // для этого используем метод ToArgb объекта Color
            if ((oldСolor.ToArgb() != currentColorBorder.ToArgb()) &&
           (oldСolor.ToArgb() != currentFillColor.ToArgb()))
            {
                //перекрашиваем пиксель
                myBitmap.SetPixel(x1, y1, currentFillColor);

                //вызываем метод для 4-х соседних пикселей
                Zaliv(x1 + 1, y1);
                Zaliv(x1 - 1, y1);
                Zaliv(x1, y1 + 1);
                Zaliv(x1, y1 - 1);
            }
            else
            {
                //выходим из метода
                return;
            }

        }
    }
}
