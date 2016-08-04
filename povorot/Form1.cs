using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
namespace povorot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int k = 0;
        int[,] arr3;
        int[,] arr_tools_1;
        int[,] arr_tools_2;
        int[,] arr_tools_3;
        int[,] arr_turn;
        int[,] arr_division;
        int pred = 0;
        int pred2 = 0;
        Color[,] colorArray;
        Color[,] colorArrayTools;
        Color[,] colorArrayTools_2;
        Color[,] colorArrayTools_3; 
        Color[,] colorArrayTurn;
        Color[,] colorArrayDivision;
        Bitmap bitmap;
        Bitmap save_bitmap;
        public static Bitmap image1, image2, image3;
        bool b = true;
        int[] arr2;
        int[,] spisok=new int[360,2];
         int[,] spisok2 = new int[360, 2];
            int s = 0;
        int ugol = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            //bitmap = new Bitmap("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\image\\blank0006.bmp");
            //initialization(bitmap);
            //filtration(bitmap);
            //binarization(bitmap);
            //pictureBox1.Image = (Bitmap)bitmap.Clone();
            chart1.ChartAreas[0].AxisX.Interval = 2;
            chart2.ChartAreas[0].AxisX.Interval = 2;
        }

        private void initialization(Bitmap bitmap)
        {
            colorArray = new Color[bitmap.Height, bitmap.Width];
            arr3 = new int[bitmap.Height, bitmap.Width];

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    colorArray[i, j] = bitmap.GetPixel(j, i);
                    arr3[i, j] = colorArray[i, j].R;
                }
            }

        }

        private void filtration(Bitmap bitmap)
        {
            int porog = 0;
            porog = 120;
            int average = 0;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if ((i + k - 1) < 0 || (j + l - 1) < 0 || (i + k - 1) > (bitmap.Height - 1) || (j + l - 1) > (bitmap.Width- 1))
                            {
                                average += 0;
                                continue;
                            }
                            average += arr3[(i + k - 1), (j + l - 1)];
                        }
                    }
                    if ((arr3[i, j] - (average / 9)) > porog)
                    {
                        arr3[i, j] = average / 9;
                    }
                    else
                    {
                        arr3[i, j] = arr3[i, j];
                    }
                    average = 0;
                    bitmap.SetPixel(j, i, Color.FromArgb(Convert.ToInt32(arr3[i, j]), Convert.ToInt32(arr3[i, j]), Convert.ToInt32(arr3[i, j])));
                }
            }
        }

        private void binarization(Bitmap bitmap)
        {
            int porog = 0;
            porog = 150;
            for (int i = 0; i < bitmap.Height; ++i)
            {
                for (int j = 0; j < bitmap.Width; ++j)
                {
                    if (arr3[i, j] < porog)
                    {
                        arr3[i, j] = 0;
                        bitmap.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        arr3[i, j] = 255;
                        bitmap.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "bmp|*.bmp|jpg|*.jpg|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK) { }
            if (openFileDialog1.FileName == "") return;
            bitmap = new Bitmap(openFileDialog1.FileName);

            initialization(bitmap);
            filtration(bitmap);
            binarization(bitmap);
            pictureBox1.Image = bitmap;
            this.chart1.Series[0].Points.Clear();
            label3.Text = "0";
        }

        private void build_gistogramm(Bitmap bit, Chart chart)
        {
            chart.Series[0].Points.Clear();
            int count = 0;

            BitmapData bits = bit.LockBits(new System.Drawing.Rectangle(0, 0, bit.Width, bit.Height), ImageLockMode.ReadOnly, bit.PixelFormat);
            IntPtr ptr = bits.Scan0;
            int bytes = bits.Stride * bit.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            int kk = bits.Stride / bit.Width;
            int sum = 0;
            for (int y = 0; y < bit.Height; y++)
            {
                for (int x = 0; x < bit.Width; x++)
                {
                    if (kk == 4)
                    {
                        if (rgbValues[sum * kk] == 0 & rgbValues[sum * kk + 3] == 0)
                            for (int i1 = 0; i1 < kk; i1++)
                                rgbValues[sum * kk + i1] = 0;
                        else if (rgbValues[sum * kk] < 150)
                        {
                            for (int i1 = 0; i1 < kk; i1++)
                                rgbValues[sum * kk + i1] = 0;
                            rgbValues[sum * kk + 3] = 255;
                        }
                        else
                            for (int i1 = 0; i1 < kk; i1++)
                                rgbValues[sum * kk + i1] = 255;
                    }
                    else
                    {
                        if (rgbValues[sum * kk] < 150)
                        {
                            for (int i1 = 0; i1 < kk; i1++)
                                rgbValues[sum * kk + i1] = 0;
                        }
                        else
                            for (int i1 = 0; i1 < kk; i1++)
                                rgbValues[sum * kk + i1] = 255;
                    }
                    sum++;
                }
            }

            Marshal.Copy(rgbValues, 0, ptr, bytes);
            bit.UnlockBits(bits);

            arr2 = new int[bit.Height];
            for (int i1 = 0; i1 < bytes / 4; i1++)
            //for (int i1 = bytes / 4 - 1; i1 > -1; i1--)
            {
                if (rgbValues[i1 * 4] == 0 && rgbValues[i1 * 4 + 1] == 0 && rgbValues[i1 * 4 + 2] == 0 && rgbValues[i1 * 4 + 3] == 255)
                {
                    count++;
                }
                if ((i1 + 1) % bit.Width == 0 && i1 != 0)
                {
                    arr2[i1 / bit.Width] = count;
                    chart.Series["Series1"].Points.AddXY((i1 + 1) / bit.Width, count);
                    count = 0;
                }
            }
        }

        private void povorot(Bitmap bit)
        {

            build_gistogramm(bit, chart1);
                    if (k == 0)
                    {
                        spisok[s, 1] = arr2.Max();
                        spisok[s, 0] = angleee;
                        if (s != 0 && spisok[s, 1] >= spisok[s - 1, 1])
                        {
                            angleee += ugol;
                        }
                        else if (s != 0)
                        {
                            pred = s - 1;
                            angleee = 0;
                            k = 1;
                            s = -1;
                        }
                        else angleee += ugol;
                    }
                    else
                    {
                        spisok2[s, 1] = arr2.Max();
                        spisok2[s, 0] = angleee;
                        if (s != 0 && spisok2[s, 1] >= spisok2[s - 1, 1])
                        {
                            angleee -= ugol;
                        }
                        else if (s != 0)
                        {
                            pred2 = s - 1;
                            angleee = 0;
                            b = false;
                        }
                        else angleee -= ugol;
                    }
                    bit.Dispose();
                    s++;
        }

        public void tools_up(Bitmap bit)
        {
            int y = 0;
            int x = 0;
            int number_string = 0;
            for (int i = 0; i < arr2.Length; i++)
            {
                if (arr2[i] == arr2.Max())
                {
                    number_string = i;
                }
            }
            if (number_string == 0)
            {
                number_string = 1;
            }

            if (number_string == bit.Height)
            {
                number_string = bit.Height - 1;
            }
                Bitmap bit_tools_1 = new Bitmap((int)bit.Width, (int)(bit.Height - number_string));
                arr_tools_1 = new int[bit.Height - number_string, bit.Width];
                colorArrayTools = new Color[bit.Height - number_string, bit.Width];
                for (int i = number_string; i < bit.Height; i++)
                {
                    for (int j = 0; j < bit.Width; j++)
                    {
                        colorArrayTools[y, x] = bit.GetPixel(j, i);
                        arr_tools_1[y, x] = colorArrayTools[y, x].R;
                        bit_tools_1.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(arr_tools_1[y, x]), Convert.ToInt32(arr_tools_1[y, x]), Convert.ToInt32(arr_tools_1[y, x])));
                        x++;
                    }
                    y++;
                    x = 0;
                }
                pictureBox2.Image = bit_tools_1;
                build_gistogramm(bit_tools_1, chart2);
        }

        public void tools_down(Bitmap bit)
        {
            int y = 0;
            int x = 0;
            int number_string = 0;
            number_string = 0;
            for (int i = 0; i < arr2.Length; i++)
            {
                if (arr2[i] == arr2.Max())
                {
                    number_string = i;
                }
            }
            if (number_string == 0)
            {
                number_string = 1;
            }

            if (number_string == bit.Height)
            {
                number_string = bit.Height - 1;
            }
                y = 0;
                x = 0;
                Bitmap bit_tools_2 = new Bitmap((int)bit.Width, (int)(number_string));
                arr_tools_2 = new int[number_string, bit.Width];
                colorArrayTools_2 = new Color[number_string, bit.Width];
                for (int i = 0; i < number_string; i++)
                {
                    for (int j = 0; j < bit.Width; j++)
                    {
                        colorArrayTools_2[y, x] = bit.GetPixel(j, i);
                        arr_tools_2[y, x] = colorArrayTools_2[y, x].R;
                        bit_tools_2.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(arr_tools_2[y, x]), Convert.ToInt32(arr_tools_2[y, x]), Convert.ToInt32(arr_tools_2[y, x])));
                        x++;
                    }
                    y++;
                    x = 0;
                }
                pictureBox2.Image = bit_tools_2;
                build_gistogramm(bit_tools_2, chart2);
            
        }

        public void tools_end(Bitmap bit)
        {
            int y = 0;
            int x = 0;
            int number_string_max1 = 0;
            int number_string_max2 = 0;
            int length = 0;
            int[] arrToolsMiddle;
            if (arr2.Length % 2 != 0)
            {
                length = arr2.Length + 1;
            }
            else length = arr2.Length;
            arrToolsMiddle = new int[length / 2];
            int k = 0;
            for (int i = arr2.Length / 4 + 1; i < arr2.Length - arr2.Length / 4 - 1; i++)
            {
                arrToolsMiddle[k] = arr2[i];
                k++;
            }
            number_string_max1 = arrToolsMiddle.Max();
            for (int i = 0; i < arrToolsMiddle.Length; i++)
            {
                if (arrToolsMiddle[i] == number_string_max1)
                {
                    number_string_max1 = arr2.Length / 4 + 1 + i;
                    for (int s = i - 5; s < i + 5; s++)
                    {
                        arrToolsMiddle[s] = 0;
                    }
                }
            }
            number_string_max2 = arrToolsMiddle.Max();
            for (int i = 0; i < arrToolsMiddle.Length; i++)
            {
                if (arrToolsMiddle[i] == number_string_max2)
                {
                    number_string_max2 = arr2.Length / 4 + 1 + i;
                }
            }
            if (number_string_max1 > 0)
            {
                y = 0;
                x = 0;

                Bitmap bit_tools_3 = new Bitmap((int)bit.Width, (int)((number_string_max1 - number_string_max2)*2));
                arr_tools_3 = new int[(number_string_max1 - number_string_max2)*2, bit.Width];
                colorArrayTools_3 = new Color[(number_string_max1 - number_string_max2)*2, bit.Width];
                for (int i = (number_string_max2) - (number_string_max1 - number_string_max2); i < number_string_max1; i++)
                {
                    for (int j = 0; j < bit.Width; j++)
                    {
                        colorArrayTools_3[y, x] = bit.GetPixel(j, i);
                        arr_tools_3[y, x] = colorArrayTools_3[y, x].R;
                        bit_tools_3.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(arr_tools_3[y, x]), Convert.ToInt32(arr_tools_3[y, x]), Convert.ToInt32(arr_tools_3[y, x])));
                        x++;
                    }
                    y++;
                    x = 0;
                }
            //if (number_string_max1 > 0)
            //{
            //    y = 0;
            //    x = 0;

            //    Bitmap bit_tools_3 = new Bitmap((int)bit.Width, (int)((bit.Height - number_string_max1) * 2));
            //    arr_tools_3 = new int[(bit.Height - number_string_max1) * 2, bit.Width];
            //    colorArrayTools_3 = new Color[(bit.Height - number_string_max1) * 2, bit.Width];
            //    for (int i = (bit.Height - (bit.Height - number_string_max1) * 2); i < bit.Height; i++)
            //    {
            //        for (int j = 0; j < bit.Width; j++)
            //        {
            //            colorArrayTools_3[y, x] = bit.GetPixel(j, i);
            //            arr_tools_3[y, x] = colorArrayTools_3[y, x].R;
            //            bit_tools_3.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(arr_tools_3[y, x]), Convert.ToInt32(arr_tools_3[y, x]), Convert.ToInt32(arr_tools_3[y, x])));
            //            x++;
            //        }
            //        y++;
            //        x = 0;
            //    }
                pictureBox2.Image = bit_tools_3;
                build_gistogramm(bit_tools_3, chart2);
            }
        }

        public void turn_right(Bitmap bit)
        {
            Bitmap bit_turn = new Bitmap((int)bit.Height, (int)(bit.Width));
            arr_turn = new int[bit.Width, bit.Height];
            colorArrayTurn = new Color[bit.Width, bit.Height];
            int y = 0;
            int x = 0;
            for (int i = bit.Height - 1; i > -1; i--)
            {
                for (int j = 0; j < bit.Width; j++)
                {
                    colorArrayTurn[y, x] = bit.GetPixel(j, i);
                    arr_turn[y, x] = colorArrayTurn[y, x].R;
                    bit_turn.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(arr_turn[y, x]), Convert.ToInt32(arr_turn[y, x]), Convert.ToInt32(arr_turn[y, x])));
                    y++;
                }
                x++;
                y=0;
            }
            pictureBox2.Image = bit_turn;
            build_gistogramm(bit_turn, chart2);
        }

        public void turn_left(Bitmap bit)
        {
            Bitmap bit_turn = new Bitmap((int)bit.Height, (int)(bit.Width));
            arr_turn = new int[bit.Width, bit.Height];
            colorArrayTurn = new Color[bit.Width, bit.Height];
            int y = 0;
            int x = 0;
            for (int i = 0; i < bit.Height; i++)
            {
                for (int j = bit.Width - 1; j > -1; j--)
                {
                    colorArrayTurn[y, x] = bit.GetPixel(j, i);
                    arr_turn[y, x] = colorArrayTurn[y, x].R;
                    bit_turn.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(arr_turn[y, x]), Convert.ToInt32(arr_turn[y, x]), Convert.ToInt32(arr_turn[y, x])));
                    y++;
                }
                x++;
                y = 0;
            }
            pictureBox2.Image = bit_turn;
            build_gistogramm(bit_turn, chart2);
        }

        public void division_areas(Bitmap bit)
        {
            int y = 0;
            int x = 0;
            int[] number_string = new int[5];
            int k = 0;
            for (int i = 0; i < arr2.Length; i++)
            {
                if (arr2[i] > 50)
                {
                    number_string[k] = i;
                    k++;
                    i += 5;
                }
            }
            if (number_string[4]==0)
            {
                number_string[4] = arr2.Length - 1;
            }
            for (int s = 0; s < number_string.Length - 1; s++)
            {
                y = 0;
                x = 0;
                Bitmap bit_division = new Bitmap((int)bit.Width, (int)(number_string[s+1] - number_string[s] - 2));
                arr_division = new int[number_string[s + 1] - number_string[s] - 2, bit.Width];
                colorArrayDivision = new Color[number_string[s + 1] - number_string[s] - 2, bit.Width];
                for (int i = number_string[s] + 2; i < number_string[s + 1]; i++)
                {
                    for (int j = 0; j < bit.Width; j++)
                    {
                        colorArrayDivision[y, x] = bit.GetPixel(j, i);
                        arr_division[y, x] = colorArrayDivision[y, x].R;
                        bit_division.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(arr_division[y, x]), Convert.ToInt32(arr_division[y, x]), Convert.ToInt32(arr_division[y, x])));
                        x++;
                    }
                    y++;
                    x = 0;
                }
                if (s == 0)
                {
                    pictureBox3.Image = bit_division;
                }
                if (s == 1)
                {
                    pictureBox4.Image = bit_division;
                }
                if (s == 2)
                {
                    pictureBox5.Image = bit_division;
                }
                if (s == 3)
                {
                    pictureBox6.Image = bit_division;
                }
            }
        }

        public void zhuk(PictureBox picture_in, PictureBox picture_out1, PictureBox picture_out2, PictureBox picture_out3)
        {
            Bitmap img_25;
            img_25 = new Bitmap(picture_in.Image);

            Bitmap novaja1;
            novaja1 = new Bitmap(picture_out1.Image);
            Bitmap ig_1;
            ig_1 = new Bitmap(picture_out1.Image);
            Bitmap novaja2;
            novaja2 = new Bitmap(picture_out2.Image);
            Bitmap ig_2;
            ig_2 = new Bitmap(picture_out2.Image);
            Bitmap novaja3;
            novaja3 = new Bitmap(picture_out3.Image);
            Bitmap ig_3;
            ig_3 = new Bitmap(picture_out3.Image);


            int[,] old_mas = new int[img_25.Height + 2, img_25.Width + 2];
            int[,] new_mas = new int[img_25.Height + 2, img_25.Width + 2];
            int vector = 3, nomer = 1;
            int a = 0, b = 0;

            //инициализация
            for (int i = 0; i < img_25.Height + 2; i++)
            {
                for (int j = 0; j < img_25.Width + 2; j++)
                {

                    old_mas[i, j] = 255;
                }
            }

            for (int i = 1; i < img_25.Height; i++)
            {
                for (int j = 1; j < img_25.Width; j++)
                {
                    if (Convert.ToInt32(img_25.GetPixel(j, i).R) > 120)
                    {
                        old_mas[i, j] = 255;
                    }
                    else
                    {
                        old_mas[i, j] = 0;
                    }
                }
            }
            int i_fin = 0, j_fin = 0, zz = 0;
            int i_max = 0, i_min = 0, j_max = 0, j_min = 0;
            // поиск
            for (int j = 1; j < img_25.Width + 1; j++)
            {
                for (int i = 1; i < img_25.Height + 1; i++)
                {
                    vector = 3;
                    if (old_mas[i, j] == 0)
                    {
                        i_fin = i;
                        j_fin = j;
                        i_min = i;
                        j_min = j;
                        i_max = i;
                        j_max = j;
                        vector--;
                        zz = 0;
                        i--;
                        while (i != i_fin || j != j_fin || zz<8)
                        {
                                zz++;
                                if (zz < img_25.Width*img_25.Height)
                                {
                                    if (old_mas[i, j] == 0)
                                    {
                                        vector--;
                                        if (vector == 1 || vector == 5 || vector == -3) { i--; }
                                        if (vector == 4 || vector == 0 || vector == 8 || vector == -4) { j--; }
                                        if (vector == 2 || vector == -2 || vector == 6 || vector == -6) { j++; }
                                        if (vector == 3 || vector == -1 || vector == 7 || vector == -5) { i++; }
                                        if (i_max < i) { i_max = i; }
                                        if (i_min > i) { i_min = i; }
                                        if (j_max < j) { j_max = j; }
                                        if (j_min > j) { j_min = j; }
                                    }
                                    if (old_mas[i, j] == 255)
                                    {
                                        vector++;
                                        if (vector == 1 || vector == 5 || vector == -3) { i--; }
                                        if (vector == 4 || vector == 0 || vector == 8 || vector == -4) { j--; }
                                        if (vector == 2 || vector == -2 || vector == 6 || vector == -6) { j++; }
                                        if (vector == 3 || vector == -1 || vector == 7 || vector == -5) { i++; }
                                        if (i_max < i) { i_max = i; }
                                        if (i_min > i) { i_min = i; }
                                        if (j_max < j) { j_max = j; }
                                        if (j_min > j) { j_min = j; }
                                    }
                                }
                                else break;
                        }

                        a = 0;

                        RectangleF fRect = new RectangleF(0, 0, j_max - j_min, i_max - i_min);



                        for (int x = i_min; x < i_max; x++)
                        {
                            
                            b = 0;
                            for (int y = j_min; y < j_max; y++)
                            {
                                
                                if (nomer == 1) {
                                    ig_1.SetPixel(b, a, Color.FromArgb(old_mas[x, y], old_mas[x, y], old_mas[x, y])); 
                                    novaja1 = ig_1.Clone(fRect, ig_1.PixelFormat); 
                                }
                                if (nomer == 2) 
                                {
                                    ig_2.SetPixel(b, a, Color.FromArgb(old_mas[x, y], old_mas[x, y], old_mas[x, y])); 
                                    novaja2 = ig_2.Clone(fRect, ig_2.PixelFormat); 
                                }
                                if (nomer == 3) 
                                {
                                    ig_3.SetPixel(b, a, Color.FromArgb(old_mas[x, y], old_mas[x, y], old_mas[x, y]));
                                    novaja3 = ig_3.Clone(fRect, ig_3.PixelFormat); 
                                }
                                b++;
                            }
                            a++;
                        }

                        j = 1; i = 1;
                        for (int x = i_min; x < i_max; x++)
                        {

                            for (int y = j_min; y < j_max; y++)
                            {
                                old_mas[x, y] = 255;
                                img_25.SetPixel(y, x, Color.FromArgb(old_mas[x, y], old_mas[x, y], old_mas[x, y]));

                            }
                        }

                        picture_in.Image = img_25;
                        if (nomer == 1) { picture_out1.Image = novaja1; }
                        if (nomer == 2) { picture_out2.Image = novaja2; }
                        if (nomer == 3) { picture_out3.Image = novaja3; }
                        if (nomer == 3)
                        {
                            i = 200;
                            j = 200;
                            break;
                        }
                        nomer++;
                    }


                }
            }

            image1 = new Bitmap(picture_out1.Image);
            image2 = new Bitmap(picture_out2.Image);
            image3 = new Bitmap(picture_out3.Image);
            //image1.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\1_rasp(3).bmp");
            //image2.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\2_rasp(2).bmp");
        }

        public void priznak1_4(PictureBox picturebox)
        {
            Bitmap img;
            img = new Bitmap(picturebox.Image);
            int[] sum = new int[4];
            int[] X = new int[4];

            for (int i = 0; i < img.Height / 2; i++)
            {
                for (int j = 0; j < img.Width / 2; j++)
                {
                    if(img.GetPixel(j, i).R<120)
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

        private void button2_Click(object sender, EventArgs e)
        {
            while (b == true)
            {
                label3.Text = Convert.ToString(angleee);
                ugol = 1;
                pictureBox1.Invalidate();
                //this.chart1.Series[0].Points.Clear();
                pictureBox1.Image = povorot_on_anglee(angleee);
                pictureBox1.Invalidate();
                Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
                bit = new Bitmap(pictureBox1.Image);
                povorot(bit);
            }
            if (b==false)
            {
                pictureBox1.Invalidate();
                if (spisok[pred, 1] > spisok2[pred2, 1])
                {
                    pictureBox1.Image = povorot_on_anglee(spisok[pred, 0]);
                 label3.Text = Convert.ToString(spisok[pred,0]);}
                else
                {
                    pictureBox1.Image = povorot_on_anglee(spisok2[pred2, 0]);
                label3.Text = Convert.ToString(spisok2[pred2,0]);}

                //this.chart1.Series[0].Points.Clear();
                Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
                bit = new Bitmap(pictureBox1.Image);
                pictureBox2.Image = pictureBox1.Image;
                build_gistogramm(bit,chart1);
                bit.Dispose();
            }
            b = true;
            k = 0;
            angleee = 0;
        }
        float newImgWidth;
        float newImgHeight;
        int angleee = 0;
       
        private Bitmap povorot_on_anglee(int anglee)
        {
            
                float angle = anglee % 360;
                float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
                float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
                newImgWidth = sin * bitmap.Height + cos * bitmap.Width;
                newImgHeight = sin * bitmap.Width + cos * bitmap.Height;
                float originX = 0f;
                float originY = 0f;

                if (angle > 0)
                {
                    if (angle <= 90)
                        originX = sin * bitmap.Height;
                    else
                    {
                        originX = newImgWidth;
                        originY = newImgHeight - sin * bitmap.Width;
                    }
                }
                else
                {
                    if (angle >= -90)
                        originY = sin * bitmap.Width;
                    else
                    {
                        originX = newImgWidth - sin * bitmap.Height;
                        originY = newImgHeight;
                    }
                }            

                Bitmap rotatedBmp = new Bitmap((int)newImgWidth, (int)newImgHeight);
                rotatedBmp.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
                Graphics g = Graphics.FromImage(rotatedBmp);
                g.TranslateTransform(originX, originY);
                g.RotateTransform(angle);           
                g.DrawImageUnscaled(bitmap, 0, 0);
                rotatedBmp.Save("D:\\promeg.bmp");                
                return rotatedBmp;         
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void сверхуМаксимальнойЧёрнойЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
            bit = new Bitmap(pictureBox2.Image);
            save_bitmap = new Bitmap(pictureBox2.Image);
            tools_up(bit);
        }

        private void снизуМаксимальнойЧёрнойЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
            bit = new Bitmap(pictureBox2.Image);
            save_bitmap = new Bitmap(pictureBox2.Image);
            tools_down(bit);
        }

        private void конечнаяТабличкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
            bit = new Bitmap(pictureBox2.Image);
            save_bitmap = new Bitmap(pictureBox2.Image);
            tools_end(bit);
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = save_bitmap;
            build_gistogramm(save_bitmap, chart2);
        }

        private void вправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
            bit = new Bitmap(pictureBox2.Image);
            save_bitmap = new Bitmap(pictureBox2.Image);
            turn_right(bit);
        }

        private void влевоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
            bit = new Bitmap(pictureBox2.Image);
            save_bitmap = new Bitmap(pictureBox2.Image);
            turn_left(bit);
        }

        private void разбитьНа4ОбластиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bit = new Bitmap((int)newImgWidth, (int)newImgHeight);
            bit = new Bitmap(pictureBox2.Image);
            save_bitmap = new Bitmap(pictureBox2.Image);
            division_areas(bit);
        }

        private void жукToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zhuk(pictureBox3, pictureBox7, pictureBox8, pictureBox9);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zhuk(pictureBox3, pictureBox7, pictureBox8, pictureBox9);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zhuk(pictureBox4, pictureBox13, pictureBox12, pictureBox10);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zhuk(pictureBox5, pictureBox16, pictureBox15, pictureBox14);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            zhuk(pictureBox6, pictureBox19, pictureBox18, pictureBox17);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox7.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp1.bmp");
            Form2 newform = new Form2(pictureBox7.Image);
            newform.Show();
            this.label1.Text = newform.label5.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pictureBox8.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp2.bmp");
            Form2 newform = new Form2(pictureBox8.Image);
            newform.Show();
            this.label1.Text += newform.label5.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pictureBox9.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp3.bmp");
            Form2 newform = new Form2(pictureBox9.Image);
            newform.Show();
            this.label1.Text += newform.label5.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pictureBox13.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp4.bmp");
            Form2 newform = new Form2(pictureBox13.Image);
            newform.Show();
            this.label6.Text = newform.label5.Text;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            pictureBox12.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp5.bmp");
            Form2 newform = new Form2(pictureBox12.Image);
            newform.Show();
            this.label6.Text += newform.label5.Text;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            pictureBox10.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp6.bmp");
            Form2 newform = new Form2(pictureBox10.Image);
            newform.Show();
            this.label6.Text += newform.label5.Text;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            pictureBox16.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp7.bmp");
            Form2 newform = new Form2(pictureBox16.Image);
            newform.Show();
            this.label9.Text = newform.label5.Text;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            pictureBox15.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp8.bmp");
            Form2 newform = new Form2(pictureBox15.Image);
            newform.Show();
            this.label9.Text += newform.label5.Text;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            pictureBox14.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp9.bmp");
            Form2 newform = new Form2(pictureBox14.Image);
            newform.Show();
            this.label9.Text += newform.label5.Text;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            pictureBox19.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp10.bmp");
            Form2 newform = new Form2(pictureBox19.Image);
            newform.Show();
            this.label12.Text = newform.label5.Text;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            pictureBox18.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp11.bmp");
            Form2 newform = new Form2(pictureBox18.Image);
            newform.Show();
            this.label12.Text += newform.label5.Text;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            pictureBox17.Image.Save("D:\\Учёба\\4-ий курс\\8 семестр\\Обработка изображений\\Курсач\\4 лаба обработка изображений\\4 лаба\\Rasp_img\\Rasp12.bmp");
            Form2 newform = new Form2(pictureBox17.Image);
            newform.Show();
            this.label12.Text += newform.label5.Text;
        }


    }
}

