using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace game_HOT
{
    public partial class Form1 : Form
    {
        PictureBox[] box;
        int[] cards_number;
        bool flip_card = true;

        public Form1()
        {
            InitializeComponent();
            delent_box();// функция отвечающая за очистку поля от белых боксов
        }

        // перемешивание карточек 
        private void shuffle_cards()
        {
            for (int i = cards_number.Length - 1; i >= 1; i--)
            {
                Random random = new Random();
                int j = random.Next(i + 1);
                var temp = cards_number[j];
                cards_number[j] = cards_number[i];
                cards_number[i] = temp;
            }
        }
        // удаление пустых боксов
        private void delent_box()
        {
            box = new PictureBox[] {pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11,
            pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17};
            foreach (PictureBox na in box)
            {
                na.Parent = pictureBox1;
                na.Visible = false;
            }
            
        }
        // Отображение рубашки
        private void Back_Front()
        {
            foreach (PictureBox na in box)
            {
                this.BeginInvoke(new Action(() => na.Enabled = true));
                this.BeginInvoke(new Action(() => na.Visible = true));
                na.Parent = pictureBox1;
                na.Image = Image.FromFile(Application.StartupPath + @"\card\file2.gif");
                na.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        // массив для подсчета открытх карт
        int check_open_card = 0;
        // пикчербоксы 
        PictureBox card1;
        PictureBox card2;
        // номер карты в массиве ( нужен для поиск сходимости и показа анимации)
        int cards_num1 = 0;
        int cards_num2 = 0;
        // номер для пикчербокса
        int number2 = 0;
        int number1 = 0;
        // поиск одинаковых карт
        private void Analogy(PictureBox open, int number)
        {
            // так как вызывается эта функция, значит карта открыта
            check_open_card += 1;
            // вызываем функцию открытия (время, пикчербокс, номер карты в перемешенном массиве)
            wait_open(2600,open, cards_number[number]);
            // если открыта одна, то записываем данные в переменные 
            if (check_open_card == 1)
            {
                card2 = open;
                number2 = number;
                cards_num2 = cards_number[number];
            }
            // Если открыты две карты, сравниваем != закрытие. == одинаковые карты, убираем с поля 
            if (check_open_card == 2)
            {
                card1 = open;
                number1 = number;
                cards_num1 = cards_number[number];
                // одинковые 
                if (cards_num1 == cards_num2)
                {
                    label1.Text = "find parse";

                    int del = number2;// номер элемента который надо удалить из массива
                    var query = box.Where(n => box.ElementAt(del) != n);

                    int del1 = number1;// номер элемента который надо удалить из массива
                    var query1 = box.Where(n => box.ElementAt(del1) != n);

                    card1.Visible = false;
                    card2.Visible = false;
                }
                // закрытие
                else
                {
                    label1.Text = "don't find parse";
                    Thread t2 = new Thread(delegate () { wait_close(2600,card1, cards_num1); });
                    t2.Start();
                    Thread t = new Thread(delegate () { wait_close(2600,card2, cards_num2); });
                    t.Start();
                }
                check_open_card = 0;

            }
        }
        // обработчик кликов (не знаю зачем он тут, но лень удалять)
        private void clickAction(PictureBox open, int number)
        {
            if (flip_card == true)
            {
                Analogy(open, number);

            }
            if (flip_card != true)
            {
                label3.Text = "wait!";
            }

        }
        // Таймер + гифка открытия. Не лезь!!!! И так работает   
        public void wait_open(int milliseconds, PictureBox box, int number)
        {
            
            flip_card = false;
            box.Image = Image.FromFile(Application.StartupPath + @"\card\1" + number.ToString() + ".gif");
            box.SizeMode = PictureBoxSizeMode.StretchImage;

            System.Windows.Forms.Timer timer4 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            this.BeginInvoke(new Action(() => box.Enabled = true));
            timer4.Interval = milliseconds;
            timer4.Enabled = true;
            timer4.Start();
            timer4.Tick += (s, e) =>
            {
                timer4.Enabled = false;
                this.BeginInvoke(new Action(() => box.Enabled = false));
                
                flip_card = true;
                timer4.Stop();
            };
            while (timer4.Enabled)
            {
                Application.DoEvents();
            }
        }
        // Таймер + гифка закрытия. Не лезь!!!! И так работает 
        public void wait_close(int milliseconds, PictureBox open, int number)
        {
            open.Image = Image.FromFile(Application.StartupPath + @"\card\" + number.ToString() + ".gif");
            open.SizeMode = PictureBoxSizeMode.StretchImage;
            flip_card = false;
            this.BeginInvoke(new Action(() => open.Enabled = true));

            System.Windows.Forms.Timer timer4 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            timer4.Interval = milliseconds;
            timer4.Enabled = true;
            timer4.Start();
            timer4.Tick += (s, e) =>
            {
                timer4.Enabled = false;
                this.BeginInvoke(new Action(() => open.Enabled = false));
                Back_Front();
                this.BeginInvoke(new Action(() => open.Enabled = true));
                flip_card = true;
                timer4.Stop();
            };
            while (timer4.Enabled)
            {
                Application.DoEvents();
            }
        }

        // не лезь убьет. Обработчики кликов. Дальше тоже есть код 
        private void PictureBox2_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox2, 0);
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox3, 1);
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox4, 2);
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox5, 3);
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox6, 4);
        }

        private void PictureBox7_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox7, 5);
        }

        private void PictureBox8_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox8, 6);
        }

        private void PictureBox9_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox9, 7);
        }

        private void PictureBox10_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox10, 8);
        }

        private void PictureBox11_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox11, 9);
        }

        private void PictureBox12_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox12, 10);
        }

        private void PictureBox13_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox13, 11);
        }

        private void PictureBox14_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox14, 12);
        }

        private void PictureBox15_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox15, 13);
        }

        private void PictureBox16_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox16, 14);
        }

        private void PictureBox17_Click(object sender, EventArgs e)
        {
            clickAction(pictureBox17, 15);
        }
        // поле из 16 карт
        private void Button2_Click(object sender, EventArgs e)
        {
            delent_box();
            box = new PictureBox[] {pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11,
            pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17};
            cards_number = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            Back_Front();
            shuffle_cards();
        }
        // поле из 8 карт
        private void Button3_Click(object sender, EventArgs e)
        {
            delent_box();
            box = new PictureBox[] { pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9 };
            cards_number = new int[] { 1, 2, 3, 4, 1, 2, 3, 4 };
            Back_Front();
            shuffle_cards();
        }
        // поле из 4 карт
        private void Button4_Click(object sender, EventArgs e)
        {
            delent_box();
            box = new PictureBox[] { pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            cards_number = new int[] { 1, 2, 1, 2 };
            Back_Front();
            shuffle_cards();
        }
    }
    
}
