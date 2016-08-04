using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace povorot
{
    public partial class Form2 : Form
    {
         public int n = 4, m = 9;
        public OpenFileDialog open_image;
        public SaveFileDialog save_image;
        public Bitmap img, img2, img3, img4, bmp_open;
        public Bitmap img_21;
        public double summ = 0;
        public Bitmap[] img_mass = new Bitmap[31];
        public Random rand = new Random();
        public int nom1 = 0, nom2 = 0;
        public double temp_sum = 0, temp_sum1 = 0, temp_sum2 = 0;
        public bool secush = false;
        public bool check = false;
        public int t1 = 0, tim = 0, t = 0, obuch = 0;
        public int r1 = -2, r2 = 2, ka = 0;

        public int[] X = new int[4];
        public int[,] new_center;
        public int ras_index = 0;
        public int machine = 50; // количество обучений
        public int[] number1 = new int[11];

        public int[,] kontur;
        public int[,] new_kontur;


        public Form2(Image picture)
        {
            InitializeComponent();
            for (int i = 0; i < 11; i++) { number1[i] = 0; }

                for (int i = 0; i < machine; i++)
                {
                    pictureBox25.Image = picture;
                    dataGridView10.Columns.Clear();
                    dataGridView10.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                    dataGridView2.Columns.Clear();
                    dataGridView2.Rows.Clear();
                    dataGridView3.Columns.Clear();
                    dataGridView3.Rows.Clear();
                    dataGridView21.Columns.Clear();
                    dataGridView21.Rows.Clear();
                    dataGridView22.Columns.Clear();
                    dataGridView22.Rows.Clear();
                    dataGridView23.Columns.Clear();
                    dataGridView23.Rows.Clear();

                    nom1 = 0; nom2 = 0;
                    temp_sum = 0; temp_sum1 = 0; temp_sum2 = 0;
                    secush = false;
                    check = false;
                    t1 = 0; tim = 0; t = 0; obuch = 0;
                    r1 = -2; r2 = 2; ka = 5;

                    func_initialize();
                    func_vicherkivanie();
                    func_razryady();
                    func_index();
                    func_img();
                    func_raspoznavanie();
                    number1[ras_index]++;

                }
                    //label4.Text = "Изображение принадлежит \n классу №: " + ras_index.ToString();

                label4.Text = "Колличество голосов: " + number1[1].ToString() + ", " + number1[2].ToString() + ", " + number1[3].ToString() + "\n" + number1[4].ToString() + ", " + number1[5].ToString() + ", " + number1[6].ToString() + "," + number1[7].ToString() + ", " + number1[8].ToString() + ", " + number1[9].ToString() + "," + number1[10].ToString();
                for (int i = 1; i < 11; i++)
                {
                    if (number1[i] == number1.Max())
                    {
                        label5.Text = i.ToString();
                    }
                }
                //pictureBox25.Image.Dispose();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            img_21.Save(save_image.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
            pictureBox25.ImageLocation = open_image.FileName;
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            open_image.ShowDialog();
        }

        private void открытьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_image.ShowDialog();
        }

        private void сохранитьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_image.FileName = "img.bmp";
            save_image.ShowDialog();
        }


        public void priznak1_4(Bitmap img)
        {
            int[] sum = new int[4];
            

            for (int i = 0; i < img.Height / 2; i++)
            {
                for (int j = 0; j < img.Width / 2; j++)
                {
                    if (img.GetPixel(j, i).R < 120)
                    {
                        sum[0]++;
                    }
                }
            }

            for (int i = 0; i < img.Height / 2; i++)
            {
                for (int j = img.Width / 2; j < img.Width; j++)
                {
                    if (img.GetPixel(j, i).R < 120)
                    {
                        sum[1]++;
                    }
                }
            }

            for (int i = img.Height / 2; i < img.Height; i++)
            {
                for (int j = img.Width / 2; j < img.Width; j++)
                {
                    if (img.GetPixel(j, i).R < 120)
                    {
                        sum[2]++;
                    }
                }
            }

            for (int i = img.Height / 2; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width / 2; j++)
                {
                    if (img.GetPixel(j, i).R < 120)
                    {
                        sum[3]++;
                    }
                }
            }

            X[0] = sum[0] * 100 / (sum[0] + sum[1] + sum[2] + sum[3]);
            X[1] = sum[1] * 100 / (sum[0] + sum[1] + sum[2] + sum[3]);
            X[2] = sum[2] * 100 / (sum[0] + sum[1] + sum[2] + sum[3]);
            X[3] = sum[3] * 100 / (sum[0] + sum[1] + sum[2] + sum[3]);
        }
  

        public void func_initialize()
        {
            dataGridView10.Visible = true;
            dataGridView10.Columns.Clear();

            img_mass[0] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\1(1).bmp");
            img_mass[1] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\1(2).bmp");
            img_mass[2] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\1(3).bmp");
            img_mass[3] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\2(1).bmp");
            img_mass[4] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\2(2).bmp");
            img_mass[5] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\2(3).bmp");
            img_mass[6] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\3(1).bmp");
            img_mass[7] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\3(2).bmp");
            img_mass[8] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\3(3).bmp");
            img_mass[9] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\4(1).bmp");
            img_mass[10] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\4(2).bmp");
            img_mass[11] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\4(3).bmp");
            img_mass[12] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\5(1).bmp");
            img_mass[13] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\5(2).bmp");
            img_mass[14] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\5(3).bmp");
            img_mass[15] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\6(1).bmp");
            img_mass[16] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\6(2).bmp");
            img_mass[17] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\6(3).bmp");
            img_mass[18] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\7(1).bmp");
            img_mass[19] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\7(2).bmp");
            img_mass[20] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\7(3).bmp");
            img_mass[21] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\8(1).bmp");
            img_mass[22] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\8(2).bmp");
            img_mass[23] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\8(3).bmp");
            img_mass[24] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\9(1).bmp");
            img_mass[25] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\9(2).bmp");
            img_mass[26] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\9(3).bmp");
            img_mass[27] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\0(1).bmp");
            img_mass[28] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\0(2).bmp");
            img_mass[29] = new Bitmap("d:\\учёба\\4-ий курс\\8 семестр\\обработка изображений\\курсач\\4 лаба обработка изображений\\4 лаба\\0(3).bmp");

            //img_mass[0] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\1\\1111.bmp");
            //img_mass[1] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\1\\111.bmp");
            //img_mass[2] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\1\\11.bmp");
            //img_mass[3] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\2\\22222.bmp");
            //img_mass[4] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\2\\2222.bmp");
            //img_mass[5] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\2\\22.bmp");
            //img_mass[6] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\3\\333333.bmp");
            //img_mass[7] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\3\\3333.bmp");
            //img_mass[8] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\3\\33.bmp");
            //img_mass[9] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\4\\444444.bmp");
            //img_mass[10] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\4\\444.bmp");
            //img_mass[11] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\4\\44.bmp");
            //img_mass[12] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\5\\55555.bmp");
            //img_mass[13] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\5\\5555.bmp");
            //img_mass[14] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\5\\555.bmp");
            //img_mass[15] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\6\\6666.bmp");
            //img_mass[16] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\6\\66.bmp");
            //img_mass[17] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\6\\6.bmp");
            //img_mass[18] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\7\\77777.bmp");
            //img_mass[19] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\7\\777.bmp");
            //img_mass[20] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\7\\77.bmp");
            //img_mass[21] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\8\\88888.bmp");
            //img_mass[22] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\8\\888.bmp");
            //img_mass[23] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\8\\88.bmp");
            //img_mass[24] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\9\\99999.bmp");
            //img_mass[25] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\9\\999.bmp");
            //img_mass[26] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\9\\9.bmp");
            //img_mass[27] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\0\\0000.bmp");
            //img_mass[28] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\0\\0000.bmp");
            //img_mass[29] = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\obuch\\0\\0.bmp");

            tim = 0;
            secush = false;
            while (secush == false)
            {
                if (tim == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        dataGridView10.Columns.Add(i.ToString(), (i - 1).ToString());

                    }
                    for (int i = 0; i < 30; i++)//++
                    { dataGridView10.Rows.Add(); }
                    dataGridView10.Columns[0].HeaderCell.Value = "N изобр.";
                    dataGridView10.Columns[1].HeaderCell.Value = "Класс";


                    for (int i = 0; i < 30; i++)//++
                    {
                        dataGridView10.Rows[i].Cells[0].Value = (i + 1).ToString();
                        if (i < 3)
                        { dataGridView10.Rows[i].Cells[1].Value = "1"; }
                        if (i < 6 && i >= 3)
                        { dataGridView10.Rows[i].Cells[1].Value = "2"; }
                        if (i < 9 && i >= 6)
                        { dataGridView10.Rows[i].Cells[1].Value = "3"; }
                        if (i < 12 && i >= 9)
                        { dataGridView10.Rows[i].Cells[1].Value = "4"; }
                        if (i < 15 && i >= 12)
                        { dataGridView10.Rows[i].Cells[1].Value = "5"; }
                        if (i < 18 && i >= 15)
                        { dataGridView10.Rows[i].Cells[1].Value = "6"; }
                        if (i < 21 && i >= 18)
                        { dataGridView10.Rows[i].Cells[1].Value = "7"; }
                        if (i < 24 && i >= 21)
                        { dataGridView10.Rows[i].Cells[1].Value = "8"; }
                        if (i < 27 && i >= 24)
                        { dataGridView10.Rows[i].Cells[1].Value = "9"; }
                        if (i < 30 && i >= 27)
                        { dataGridView10.Rows[i].Cells[1].Value = "0"; }
                    }

                    // иксы
                    for (int i = 0; i < 4; i++) //++
                    {
                        dataGridView1.Columns.Add(i.ToString(), "X" + (i + 1).ToString());
                    }
                    for (int i = 0; i < 30; i++) //++
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                    }


                    for (int i = 0; i < 30; i++)
                    {
                        t = 0;
                        priznak1_4(img_mass[i]);
                            for (int x = 0; x < 4; x++)
                            {
                                dataGridView1.Rows[i].Cells[t].Value = X[x];
                                t++;
                            }
                    }


                    //лямды

                    for (int i = 0; i < 4; i++) //++
                    {
                        dataGridView2.Columns.Add(i.ToString(), "lambda" + (i + 1).ToString());

                    }

                    //секущие + суммы

                    dataGridView3.Columns.Add("", "N сек.");
                    dataGridView3.Columns.Add("", "");
                    dataGridView3.Columns.Add("", "Сумма 1");
                    dataGridView3.Columns.Add("", "Сумма 2");
                    dataGridView3.Columns.Add("", "lambdan+1");
                    dataGridView3.Columns.Add("", "G1");
                    dataGridView3.Columns.Add("", "G2");
                    dataGridView3.Rows.Add();


                    //заполнение таблицы знаков


                    for (int i = 0; i < dataGridView10.RowCount - 1; i++)
                    {
                        for (int j = 0; j < dataGridView10.RowCount - 1; j++)
                        {
                            if (dataGridView10.Rows[i].Cells[1].Value != dataGridView10.Rows[j].Cells[1].Value)
                            {
                                nom1 = i;
                                nom2 = j;
                                dataGridView10.Columns.Add("", (i + 1).ToString() + "-" + (j + 1).ToString());

                                break;
                            }

                        }
                        if (nom1 != 0 || nom2 != 0)
                        { break; }
                    }



                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[tim].HeaderCell.Value = (tim + 1).ToString();
                    for (int y = 0; y < 30; y++) //++
                    {
                        t1 = 0;
                        for (int x = 0; x < 4; x++) //++
                        {

                            dataGridView2.Rows[0].Cells[t1].Value = rand.Next(r1, r2);
                            t1++;
                        }
                    }

                    //первое заполнение 4ой таблицы
                    dataGridView3.Rows[tim].Cells[0].Value = tim + 1;
                    dataGridView3.Rows[tim].Cells[1].Value = (nom1 + 1).ToString() + "-" + (nom2 + 1).ToString();
                    while (true)
                    {
                        temp_sum1 = 0;
                        temp_sum2 = 0;
                        for (int x = 0; x < dataGridView1.ColumnCount; x++)
                        {
                            temp_sum1 += Convert.ToDouble(dataGridView1.Rows[nom1].Cells[x].Value) * Convert.ToDouble(dataGridView2.Rows[tim].Cells[x].Value);
                            temp_sum2 += Convert.ToDouble(dataGridView1.Rows[nom2].Cells[x].Value) * Convert.ToDouble(dataGridView2.Rows[tim].Cells[x].Value);
                        }
                        if (Math.Abs(temp_sum1) - Math.Abs(temp_sum2) > ka) { break; }
                        for (int y = 0; y < 30; y++) //++
                        {
                            t1 = 0;
                            for (int x = 0; x < 4; x++) //++
                            {

                                dataGridView2.Rows[0].Cells[t1].Value = rand.Next(r1, r2);
                                t1++;
                            }
                        }

                    }
                    dataGridView3.Rows[tim].Cells[2].Value = temp_sum1;
                    dataGridView3.Rows[tim].Cells[3].Value = temp_sum2;
                    dataGridView3.Rows[tim].Cells[4].Value = (temp_sum1 + temp_sum2) / 2.0;

                    if (temp_sum1 - ((temp_sum1 + temp_sum2) / 2.0) <= 0) { dataGridView3.Rows[tim].Cells[5].Value = 0; } else { dataGridView3.Rows[tim].Cells[5].Value = 1; }
                    if (temp_sum2 - ((temp_sum1 + temp_sum2) / 2.0) <= 0) { dataGridView3.Rows[tim].Cells[6].Value = 0; } else { dataGridView3.Rows[tim].Cells[6].Value = 1; }

                    //цикл для создания знаков для одной секущей
                    temp_sum = 0;
                    for (int i = 0; i < dataGridView10.RowCount - 1; i++)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            temp_sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value) * Convert.ToDouble(dataGridView2.Rows[tim].Cells[j].Value);
                        }
                        if (temp_sum - ((temp_sum1 + temp_sum2) / 2.0) <= 0) { dataGridView10.Rows[i].Cells[dataGridView10.ColumnCount - 1].Value = 0; }
                        else { dataGridView10.Rows[i].Cells[dataGridView10.ColumnCount - 1].Value = 1; }
                        temp_sum = 0;
                    }


                }//tim == 0
                else
                {
                    //заполнение таблицы знаков динамическое
                    nom1 = 0; nom2 = 0; check = false;
                    for (int i = 0; i < dataGridView10.RowCount - 1; i++)
                    {
                        for (int j = 0; j < dataGridView10.RowCount - 1; j++)
                        {
                            if (Convert.ToDouble(dataGridView10.Rows[i].Cells[2].Value) == Convert.ToDouble(dataGridView10.Rows[j].Cells[2].Value))
                            {
                                if (3 < dataGridView10.ColumnCount)
                                {
                                    check = false;
                                    for (int k = 3; k < dataGridView10.ColumnCount; k++)
                                    {

                                        if (Convert.ToDouble(dataGridView10.Rows[i].Cells[k].Value) != Convert.ToDouble(dataGridView10.Rows[j].Cells[k].Value))
                                        { check = true; break; }
                                    }
                                }
                                if (check == false)
                                {
                                    if (Convert.ToDouble(dataGridView10.Rows[i].Cells[1].Value) != Convert.ToDouble(dataGridView10.Rows[j].Cells[1].Value))
                                    {
                                        nom1 = i;
                                        nom2 = j;
                                        dataGridView10.Columns.Add("", (i + 1).ToString() + "-" + (j + 1).ToString());

                                        break;
                                    }
                                }

                            }
                            if (nom1 != 0 || nom2 != 0) { break; }
                        }
                        if (nom1 != 0 || nom2 != 0) { break; }
                    }
                    if (nom1 == 0 && nom2 == 0) { secush = true; }
                    if (secush == true) { break; }


                    //лямды динамически
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[tim].HeaderCell.Value = (tim + 1).ToString();
                    for (int y = 0; y < 30; y++) //++
                    {
                        t1 = 0;
                        for (int x = 0; x < 4; x++) //++
                        {

                            dataGridView2.Rows[tim].Cells[t1].Value = rand.Next(r1, r2);
                            t1++;
                        }
                    }


                    //динамическое заполнение 4ой таблицы
                    dataGridView3.Rows.Add();
                    dataGridView3.Rows[tim].Cells[0].Value = tim + 1;
                    dataGridView3.Rows[tim].Cells[1].Value = (nom1 + 1).ToString() + "-" + (nom2 + 1).ToString();

                    while (true)
                    {
                        temp_sum1 = 0;
                        temp_sum2 = 0;
                        for (int x = 0; x < dataGridView1.ColumnCount; x++)
                        {
                            temp_sum1 += Convert.ToDouble(dataGridView1.Rows[nom1].Cells[x].Value) * Convert.ToDouble(dataGridView2.Rows[tim].Cells[x].Value);
                            temp_sum2 += Convert.ToDouble(dataGridView1.Rows[nom2].Cells[x].Value) * Convert.ToDouble(dataGridView2.Rows[tim].Cells[x].Value);
                        }
                        if (Math.Abs(temp_sum1) - Math.Abs(temp_sum2) > ka) { break; }
                        
                        for (int y = 0; y < 30; y++) //++
                        {
                            t1 = 0;
                            for (int x = 0; x < 4; x++) //++
                            {

                                dataGridView2.Rows[tim].Cells[t1].Value = rand.Next(r1, r2);
                                t1++;
                            }
                        }
                    }
                    dataGridView3.Rows[tim].Cells[2].Value = temp_sum1;
                    dataGridView3.Rows[tim].Cells[3].Value = temp_sum2;
                    dataGridView3.Rows[tim].Cells[4].Value = (temp_sum1 + temp_sum2) / 2.0;

                    if (temp_sum1 - ((temp_sum1 + temp_sum2) / 2.0) <= 0) { dataGridView3.Rows[tim].Cells[5].Value = 0; } else { dataGridView3.Rows[tim].Cells[5].Value = 1; }
                    if (temp_sum2 - ((temp_sum1 + temp_sum2) / 2.0) <= 0) { dataGridView3.Rows[tim].Cells[6].Value = 0; } else { dataGridView3.Rows[tim].Cells[6].Value = 1; }


                    //цикл для создания занаков для остальных секущих
                    temp_sum = 0;
                    for (int i = 0; i < dataGridView10.RowCount - 1; i++)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            temp_sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value) * Convert.ToDouble(dataGridView2.Rows[tim].Cells[j].Value);
                        }
                        if (temp_sum - ((temp_sum1 + temp_sum2) / 2.0) <= 0) { dataGridView10.Rows[i].Cells[dataGridView10.ColumnCount - 1].Value = 0; }
                        else { dataGridView10.Rows[i].Cells[dataGridView10.ColumnCount - 1].Value = 1; }
                        temp_sum = 0;
                    }

                    // secush = true;
                    if (secush == true) { break; }
                }

                if (secush == true) { break; }



                tim++;
            }// for
            obuch = dataGridView10.RowCount;


            for (int i = 0; i < dataGridView10.ColumnCount; i++)
            {
                // column_1 = new DataGridViewTextBoxColumn();
                dataGridView21.Columns.Add(i.ToString(), (i - 1).ToString());

            }
            for (int i = 0; i < dataGridView10.RowCount + 3; i++)//++
            { dataGridView21.Rows.Add(); }
            //dataGridView1.Rows[1].HeaderCell.Value = 1.ToString();
            dataGridView21.Columns[0].HeaderCell.Value = "N изобр.";
            dataGridView21.Columns[1].HeaderCell.Value = "Класс";

            for (int i = 0; i < dataGridView10.RowCount; i++)
            {
                for (int j = 0; j < dataGridView10.ColumnCount; j++)
                {
                    dataGridView21.Rows[i].Cells[j].Value = dataGridView10.Rows[i].Cells[j].Value;
                }
            }

            n = dataGridView21.ColumnCount;
            m = dataGridView21.RowCount - 3;
        }

        public void func_vicherkivanie()
        {
            int schet = 0;
            int kom = 0;
            //поиск лишних столбцов
            for (int st = 2; st < n; st++)
            {
                for (int ct = 0; ct < m - 1; ct++)
                {
                    for (int i = ct + 1; i < m; i++)
                    {

                        schet = 0;
                        kom = 0;
                        if (Convert.ToString(dataGridView21.Rows[ct].Cells[1].Value) != Convert.ToString(dataGridView21.Rows[i].Cells[1].Value))
                        {
                            for (int j = 2; j < n; j++)
                            {
                                if (j == st) { j++; }

                                if (j < n)
                                {
                                    if (Convert.ToString(dataGridView21.Rows[m].Cells[j].Value) == "d" || st < j)
                                    {
                                        if (Convert.ToString(dataGridView21.Rows[ct].Cells[j].Value) == Convert.ToString(dataGridView21.Rows[i].Cells[j].Value)) { schet++; }
                                        else { break; }
                                    }
                                    else { kom++; }
                                }


                                if (schet == n - 3 - kom) { dataGridView21.Rows[m].Cells[st].Value = 'd'; }
                            }
                        }
                    }
                }
            }
            // удаление лишних столбцов
            int del = 0;
            int tmp_n = n;
            dataGridView21.Rows[m].Cells[0].Value = 'd'; dataGridView21.Rows[m].Cells[1].Value = 'd';
            for (int gg = 2; gg < tmp_n - del; gg++)
            {
                if (Convert.ToString(dataGridView21.Rows[m].Cells[gg].Value) != "d")
                {
                    dataGridView21.Columns.RemoveAt(gg);


                    del++; gg--; n--;
                    dataGridView10.Columns.RemoveAt(gg + 1);
                    dataGridView2.Rows.RemoveAt(gg - 1);
                    dataGridView3.Rows.RemoveAt(gg - 1);
                }
            }
            dataGridView21.Rows.RemoveAt(m);
            // поиск лишних строк
            for (int ct = 0; ct < m - 1; ct++)
            {
                for (int i = ct + 1; i < m; i++)
                {

                    schet = 0;
                    if (Convert.ToString(dataGridView21.Rows[ct].Cells[1].Value) == Convert.ToString(dataGridView21.Rows[i].Cells[1].Value))
                    {
                        for (int j = 2; j < n; j++)
                        {

                            if (Convert.ToString(dataGridView21.Rows[ct].Cells[j].Value) == Convert.ToString(dataGridView21.Rows[i].Cells[j].Value)) { schet++; }
                            else { break; }
                            if (schet == n - 2) { dataGridView21.Rows[i].Cells[0].Value = 'd'; }
                        }
                    }
                }
            }


            // удаление лишних строк
            int delete = 0;
            int tmp_m = m;
            for (int gg = 0; gg < tmp_m - delete; gg++)
            { if (Convert.ToString(dataGridView21.Rows[gg].Cells[0].Value) == "d") { dataGridView21.Rows.RemoveAt(gg); delete++; gg--; m--; } }
        }

        public void func_razryady()
        {
            // 2 table
            for (int i = 0; i < n; i++)
            {
                dataGridView22.Columns.Add(i.ToString(), (i - 1).ToString());

            }
            for (int i = 0; i < m + 1; i++)
            { dataGridView22.Rows.Add(); }
            dataGridView22.Columns[0].HeaderCell.Value = "N изобр.";
            dataGridView22.Columns[1].HeaderCell.Value = "Класс";

            // инициализация массива
            for (int i = 0; i < m; i++) { dataGridView22.Rows[i].Cells[0].Value = dataGridView21.Rows[i].Cells[0].Value; }
            for (int i = 0; i < m; i++) { dataGridView22.Rows[i].Cells[1].Value = dataGridView21.Rows[i].Cells[1].Value; }
            for (int i = 0; i < m - 1; i++)
            {
                for (int j = 2; j < n; j++)
                {
                    dataGridView22.Rows[i].Cells[j].Value = 1;
                }
            }

            //заполнение таблицы разрядов
            int schet = 0;
            for (int st = 2; st < n - 1; st++)
            {
                for (int ct = 0; ct < m - 1; ct++)
                {
                    for (int i = ct + 1; i < m; i++)
                    {
                        // 1й столбец
                        if (st == 2)
                        {
                            schet = 0;
                            if (Convert.ToString(dataGridView21.Rows[ct].Cells[1].Value) != Convert.ToString(dataGridView21.Rows[i].Cells[1].Value))
                            {
                                for (int j = 2; j < n; j++)
                                {
                                    if (j == st) { j++; }
                                    if (j < n)
                                    {
                                        if (Convert.ToString(dataGridView21.Rows[ct].Cells[j].Value) == Convert.ToString(dataGridView21.Rows[i].Cells[j].Value)) { schet++; }
                                        else { break; }
                                    }
                                    if (schet == n - 3) { dataGridView22.Rows[ct].Cells[st].Value = 0; dataGridView22.Rows[i].Cells[st].Value = 0; }
                                }
                            }
                        }
                        else
                        {
                            schet = 0;
                            if (Convert.ToString(dataGridView21.Rows[ct].Cells[1].Value) != Convert.ToString(dataGridView21.Rows[i].Cells[1].Value))
                            {
                                for (int j = st + 1; j < n; j++)
                                {
                                    if (Convert.ToString(dataGridView21.Rows[ct].Cells[j].Value) == Convert.ToString(dataGridView21.Rows[i].Cells[j].Value)) { schet++; }
                                    else { break; }
                                    // открываем нули
                                    if (schet == n - st - 1)
                                    {
                                        for (int y = st - 1; y > 1; y--)
                                        {
                                            if (Convert.ToString(dataGridView22.Rows[ct].Cells[y].Value) == "0" && Convert.ToString(dataGridView22.Rows[i].Cells[y].Value) == "0")
                                            {
                                                if (Convert.ToString(dataGridView21.Rows[ct].Cells[y].Value) != Convert.ToString(dataGridView21.Rows[i].Cells[y].Value)) { break; }
                                            }
                                            else { dataGridView22.Rows[ct].Cells[st].Value = 0; dataGridView22.Rows[i].Cells[st].Value = 0; }
                                            if (y == 2) { dataGridView22.Rows[ct].Cells[st].Value = 0; dataGridView22.Rows[i].Cells[st].Value = 0; }
                                        }

                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        public void func_index()
        {
            // 3 table
            for (int i = 0; i < n; i++)
            {
                dataGridView23.Columns.Add(i.ToString(), (i - 1).ToString());

            }
            for (int i = 0; i < m + 1; i++)
            { dataGridView23.Rows.Add(); }
            dataGridView23.Columns[0].HeaderCell.Value = "N изобр.";
            dataGridView23.Columns[1].HeaderCell.Value = "Класс";

            // инициализация массива
            for (int i = 0; i < m; i++) { dataGridView23.Rows[i].Cells[0].Value = dataGridView21.Rows[i].Cells[0].Value; }
            for (int i = 0; i < m; i++) { dataGridView23.Rows[i].Cells[1].Value = dataGridView21.Rows[i].Cells[1].Value; }
            for (int i = 0; i < m - 1; i++)
            {
                for (int j = 2; j < n; j++)
                {
                    if (Convert.ToString(dataGridView22.Rows[i].Cells[j].Value) == "0") { dataGridView23.Rows[i].Cells[j].Value = Convert.ToString(dataGridView21.Rows[i].Cells[j].Value); }
                    else { dataGridView23.Rows[i].Cells[j].Value = "1/0"; }
                }
            }
            // заполняем последний столбец в разрядах
            int schet = 0;

            for (int ct = 0; ct < m - 1; ct++)
            {
                for (int i = ct + 1; i < m; i++)
                {
                    schet = 0;
                    if (Convert.ToString(dataGridView21.Rows[ct].Cells[1].Value) != Convert.ToString(dataGridView21.Rows[i].Cells[1].Value))
                    {
                        for (int j = 2; j < n; j++)
                        {
                            if (Convert.ToString(dataGridView23.Rows[ct].Cells[j].Value) == Convert.ToString(dataGridView23.Rows[i].Cells[j].Value)
                               || Convert.ToString(dataGridView23.Rows[ct].Cells[j].Value) == "1/0"
                               || Convert.ToString(dataGridView23.Rows[i].Cells[j].Value) == "1/0") { schet++; }
                            else { break; }

                            if (schet == n - 3) { dataGridView22.Rows[ct].Cells[n - 1].Value = 0; dataGridView22.Rows[i].Cells[n - 1].Value = 0; }
                        }
                    }

                }
            }

            // заполняем последний столбец в индексах
            for (int i = 0; i < m - 1; i++)
            {
                if (Convert.ToString(dataGridView22.Rows[i].Cells[n - 1].Value) == "0") { dataGridView23.Rows[i].Cells[n - 1].Value = Convert.ToString(dataGridView21.Rows[i].Cells[n - 1].Value); }
                else { dataGridView23.Rows[i].Cells[n - 1].Value = "1/0"; }
            }


            // вычеркиваем одинаковые строки

            for (int ct = 0; ct < m - 1; ct++)
            {
                for (int i = ct + 1; i < m; i++)
                {

                    schet = 0;
                    if (Convert.ToString(dataGridView23.Rows[ct].Cells[1].Value) == Convert.ToString(dataGridView23.Rows[i].Cells[1].Value))
                    {
                        for (int j = 2; j < n; j++)
                        {

                            if (Convert.ToString(dataGridView23.Rows[ct].Cells[j].Value) == Convert.ToString(dataGridView23.Rows[i].Cells[j].Value)) { schet++; }
                            else { break; }
                            if (schet == n - 2) { dataGridView23.Rows[i].Cells[0].Value = 'd'; }
                        }
                    }
                }
            }

            // удаление лишних строк
            int delete = 0;
            int tmp_m = m;
            for (int gg = 0; gg < tmp_m - delete; gg++)
            { if (Convert.ToString(dataGridView23.Rows[gg].Cells[0].Value) == "d") { dataGridView23.Rows.RemoveAt(gg); delete++; gg--; m--; } }
        }

        public void func_raspoznavanie()
        {
            // распознавание изображжения
            int schet = 0;
            for (int j = 0; j < n; j++)
            {
                dataGridView23.Rows[m - 1].Cells[j].Value = Convert.ToString(dataGridView10.Rows[dataGridView10.RowCount - 1].Cells[j].Value);
            }

            for (int i = 0; i < m - 1; i++)
            {
                schet = 0;
                for (int j = 2; j < n; j++)
                {
                    if (Convert.ToString(dataGridView23.Rows[m - 1].Cells[j].Value) == Convert.ToString(dataGridView23.Rows[i].Cells[j].Value)
                   || Convert.ToString(dataGridView23.Rows[m - 1].Cells[j].Value) == "1/0"
                   || Convert.ToString(dataGridView23.Rows[i].Cells[j].Value) == "1/0") { schet++; }
                    else { break; }
                    if (schet == n - 2) { 
                    dataGridView23.Rows[m - 1].Cells[1].Value = Convert.ToString(dataGridView23.Rows[i].Cells[1].Value);
                    ras_index = Convert.ToInt32(dataGridView23.Rows[i].Cells[1].Value); 
                    }
                }
            }
        }

 
        public void func_img()
        {
            img_21 = new Bitmap(pictureBox25.Image);

            /*бинаризация*/
            int bin = 0;
            for (int y = 0; y < img_21.Height; y++)
            {
                for (int x = 0; x < img_21.Width; x++)
                {
                    if ((Convert.ToInt32(img_21.GetPixel(x, y).R) > 0) && (Convert.ToInt32(img_21.GetPixel(x, y).R) < 255))
                    {
                        bin = 1;
                        break;
                    }

                }
                if (bin == 1) { break; }
            }

            if (bin == 1)
            {
                double porog = 0;
                porog = 0.45;
                porog = 255.0 * porog;

                for (int y = 0; y < img_21.Height; y++)
                {
                    for (int x = 0; x < img_21.Width; x++)
                    {
                        if (Convert.ToInt32(img_21.GetPixel(x, y).R) >= porog)
                        {
                            img_21.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            img_21.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                    }
                }
            }
            /* конец бинаризации */

            img_mass[30] = img_21;

            pictureBox25.Image = img_21;

            for (int i = 0; i < obuch; i++)
            {
                t = 0;
                priznak1_4(img_mass[30]);
                for (int x = 0; x < 4; x++)
                {
                    dataGridView1.Rows[obuch - 1].Cells[t].Value = X[x];
                    t++;
                }

            }

            temp_sum = 0;
            dataGridView10.Rows[obuch - 1].Cells[0].Value = obuch;
            for (int i = 2; i < dataGridView10.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    temp_sum += Convert.ToDouble(dataGridView1.Rows[obuch - 1].Cells[j].Value) * Convert.ToDouble(dataGridView2.Rows[i - 2].Cells[j].Value);
                }


                if (temp_sum - ((Convert.ToDouble(dataGridView3.Rows[i - 2].Cells[2].Value) + Convert.ToDouble(dataGridView3.Rows[i - 2].Cells[3].Value)) / 2.0) <= 0) { dataGridView10.Rows[obuch - 1].Cells[i].Value = 0; }
                else { dataGridView10.Rows[obuch - 1].Cells[i].Value = 1; }
                temp_sum = 0;
            }
        }

    }
}
