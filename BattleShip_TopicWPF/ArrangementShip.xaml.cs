using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Media;

namespace BattleShip_TopicWPF
{
    /// <summary>
    /// Interaction logic for ArrangementShip.xaml
    /// </summary>
    public partial class ArrangementShip : Window
    {

        //Qui định 1: Guard ship, 2: Submarine, 3: Destroyer, 4: Cruiser, 5: Aircraft carriers
        //1: Yellow, 2: Red, 3: Green, 4: Blue, 5: Purple

        private Button[,] _arrButton;
        private int[,] _arrPutShip;                 //Lưu lại vị trí đặt các loại tàu sau khi người chơi kết thúc quá trính đặt tàu.

        private int _shipLength = 0;
        private int _direction = 1;                 //Lưu hướng sẽ đặt tàu. Hướng mặt định là nằm ngang. 1 nằm ngang, 2 nằm dọc
        private bool _isChooseShip = false;         //Nếu người chơi click vào các image để chọn tàu. Giá trị sẽ thành true
        private int _typeOfShip;        //Loại tàu. 
        private System.Windows.Media.SolidColorBrush _colorShip;                  //Lưu color của tàu
        private Dictionary<int, bool> _arrIsChooseShip;                //Lưu lại tàu nào đã được chọn  

        public int[,] arrAreaShip;                  //Lưu lại vị trí đặt tàu

        private int _row, _column;                  //Lưu lại vị trí của button trong ma trận areaShip

        public ArrangementShip()
        {
            InitializeComponent();

            createArrayButton();
            this.arrAreaShip = new int[12, 12];
            createArrIsChooseShip();

            //Nhạc nền
            music();
        }

        private void music()
        {
            SoundPlayer soudMedia = new SoundPlayer(Properties.Resources.drumSound);
            soudMedia.PlayLooping();
        }

        private void createArrIsChooseShip()
        {
            this._arrIsChooseShip = new Dictionary<int, bool>();
            this._arrIsChooseShip[1] = false;
            this._arrIsChooseShip[2] = false;
            this._arrIsChooseShip[3] = false;
            this._arrIsChooseShip[4] = false;
            this._arrIsChooseShip[5] = false;
        }

        //Tạo các button cho vào ma trận để các phương thức sau sẽ xử lí
        //Bỏ các button đó vào WrapPanel để hiển thị. Giúp người chơi có thể đặt tàu của mình
        private void createArrayButton()
        {
            this._arrButton = new Button[12, 12];
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Button btn = new Button();
                    btn.Width = 32.5;
                    btn.Height = 32.5;
                    btn.Opacity = 0;
                    btn.Background = Brushes.White;
                    //btn.MouseEnter += mouseEnterLocationPutShip;
                    //btn.MouseLeave += mouseLeaveLocationPutShip;
                    btn.Click += mouseClickButtonPutShip;

                    this._arrButton[i, j] = btn;

                    this.wrpAreaPutShip.Children.Add(btn);

                }
            }
        }

        //Khi người chơi ấn chọn một con tàu để đặt
        //Sẽ lưu lại kích thước của con tàu 
        private void ship_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;

            //Nếu tàu chưa được chọn. Nghĩa là backgroud vẫn là mà trắng


            switch (image.Name)
            {
                case "imgGuardShip":
                    this._shipLength = 2;
                    this._typeOfShip = 1;
                    this._colorShip = Brushes.Yellow;
                    break;
                case "imgSubmarine":
                    this._shipLength = 3;
                    this._typeOfShip = 2;
                    this._colorShip = Brushes.Red;
                    break;
                case "imgDestroyer":
                    this._shipLength = 3;
                    this._typeOfShip = 3;
                    this._colorShip = Brushes.Green;
                    break;
                case "imgCruiser":
                    this._shipLength = 4;
                    this._typeOfShip = 4;
                    this._colorShip = Brushes.Blue;
                    break;
                case "imgAircraftCarries":
                    this._shipLength = 5;
                    this._typeOfShip = 5;
                    this._colorShip = Brushes.Purple;
                    break;
            }

            //Tàu đã được đặt rồi. Không thể đặt nữa
            if (this._arrIsChooseShip[this._typeOfShip])
            {
                return;
            }

            this._isChooseShip = true;
        }

        //Nếu người dùng đã chọn tàu và sau đó đưa chuột vào "areaShip" ấn vào một trong các button trong vùng này
        private void mouseClickButtonPutShip(object sender, EventArgs e)
        {

            //Nếu chưa chọn tàu thì không chạy
            if (_isChooseShip == false)
            {
                return; 
            }

            //Kiểm tra tàu vừa chọn đã được đặt chưa
            if (this._arrIsChooseShip[this._typeOfShip] == true)
            {
                return;
            }

            if (isCanPutShip())
            {
                //Đặt tàu theo chiều ngang
                if (this._direction == 1)
                {
                    for (int i = this._column; i < this._column + this._shipLength; i++)
                    {
                        this._arrButton[this._row, i].Background = this._colorShip;
                        this._arrButton[this._row, i].Opacity = 50;
                    }
                }
                if (this._direction == 2)
                {
                    for (int i = this._row; i < this._row + this._shipLength; i++)
                    {
                        this._arrButton[i, this._column].Background = this._colorShip;
                        this._arrButton[i, this._column].Opacity = 50;
                    }
                }

                //Xét lại giá trị trong "_arrIsChooseShip". Tàu đã được chọn
                this._arrIsChooseShip[this._typeOfShip] = true;

                //Tàu được đặt xong. Người chơi phải chọn tàu mới để có thể đặt tiếp
                this._isChooseShip = false;

            }
        }

        //Xem xét có thể đặt tàu tại "row", "column" với độ dài "shipLength" không.
        private bool isCanPutShip()
        {

            //Không đủ không gian để đặt tàu
            if (this._direction == 2 && (this._row + this._shipLength) - 1 > 10 || this._direction == 1 && (this._column + this._shipLength) - 1 > 10)
            {
                return false;
            }

            //Kiểm tra theo chiều ngang
            if (this._direction == 1)
            {
                for (int i = this._column; i < this._column + this._shipLength; i++)
                {
                    if (this._arrButton[this._row, i].Background != Brushes.White)
                    {
                        return false;
                    }
                    if (this._row > 1 && this._arrButton[this._row - 1, i].Background != Brushes.White)
                    {
                        return false;
                    }
                    if (this._row < 10 && this._arrButton[this._row + 1, i].Background != Brushes.White)
                    {
                        return false;
                    }
                }
                //Kiểm tra hai đầu
                if (this._column > 1 && this._arrButton[this._row, this._column - 1].Background != Brushes.White)
                {
                    return false;
                }
                //Kiểm tra hai đầu
                if (this._column + this._shipLength <= 10 && this._arrButton[this._row, this._column + this._shipLength].Background != Brushes.White)
                {
                    return false;
                }
            }

            //Kiểm tra theo chiều dọc
            if (this._direction == 2)
            {
                //Kiểm tra phần thân
                for (int i = this._row; i < this._row + this._shipLength; i++)
                {
                    if (this._arrButton[i, this._column].Background != Brushes.White)
                    {
                        return false;
                    }
                    if (this._column > 1 && this._arrButton[i, this._column - 1].Background != Brushes.White)
                    {
                        return false;
                    }
                    if (this._column < 10 && this._arrButton[i, this._column + 1].Background != Brushes.White)
                    {
                        return false;
                    }
                }
                //Kiểm tra hai đầu
                if (this._row > 1 && this._arrButton[this._row - 1, this._column].Background != Brushes.White)
                {
                    return false;
                }
                //Kiểm tra hai đầu
                if (this._row + this._shipLength <= 10 && this._arrButton[this._row + this._shipLength, this._column].Background != Brushes.White)
                {
                    return false;
                }
            }

            return true;
        }

        //Bắt sự kiện chuột di chuyển trên WrapPanel
        //Từ đó lấy ra tọa độ. Tính toán để lấy ra vị trí của Button trong ma trận "areaShip" mà Mouse đang Enter;
        private void wrpAreaPutShip_MouseMove(object sender, MouseEventArgs e)
        {

            //Reset lại Opacity của ma trận "areaShip". Nếu backgroud = White thì Opacity = 0
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Button btn = this._arrButton[i, j];
                    if (btn.Background == Brushes.White)
                    {
                        btn.Opacity = 0;
                    }
                }
            }

            WrapPanel wrapPanel = (WrapPanel)sender;

            //Lấy ra vị trí của chuột
            Point point = e.GetPosition(wrapPanel);

            //WrapPanel chiếm 325 - 325 kích thước. Button chiếm 32.5 kích thước
            //Như vậy lấy ra vị trí của chuột chia đi 32.5 ta sẽ lấy được vị trí của button trong ma trận "areaShip"
            this._row = (int)(point.Y / 32.5) + 1;
            this._column = (int)(point.X / 32.5) + 1;
            if (this._row > 10)
            {
                this._row -= 1;
            }
            if (this._column > 10)
            {
                this._column -= 1;
            }

            //Nếu tàu đã được chọn. Hiển thị vị trí của tàu sẽ được đặt lên màn hình cho người chơi xem. Trước khi chọn vị trí sẽ đặt

            if (this._isChooseShip)
            {
                //--Nếu hướng là chiều ngang
                if (this._direction == 1)
                {
                    for (int i = this._column; i < this._column + this._shipLength && i <= 10; i++)
                    {
                        Button btn = this._arrButton[this._row, i];

                        if (btn.Background == Brushes.White)
                        {
                            btn.Opacity = 25;
                        }
                    }
                }
                //--Nếu hướng là chiều dọc
                else
                {
                    for (int i = this._row; i < this._row + this._shipLength && i <= 10; i++)
                    {
                        Button btn = this._arrButton[i, this._column];
                        if (btn.Background == Brushes.White)
                        {
                            btn.Opacity = 25;
                        }
                    }
                }
            }


        }

        private void wrpAreaPutShip_MouseLeave(object sender, MouseEventArgs e)
        {
            //Reset lại Opacity của ma trận "areaShip". Nếu backgroud = White thì Opacity = 0
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Button btn = this._arrButton[i, j];
                    if (btn.Background == Brushes.White)
                    {
                        btn.Opacity = 0;
                    }
                }
            }
        }

        private void stpShip_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel stp = (StackPanel)sender;

            if (stp.Background == Brushes.White)
            {
                stp.Background = Brushes.Gray;
            }
        }

        private void stpShip_MouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel stp = (StackPanel)sender;
            if (stp.Background == Brushes.Gray)
            {
                stp.Background = Brushes.White;
            }
        }

        private void imgRotate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this._direction == 1)
            {
                this._direction = 2;
            }
            else
            {
                this._direction = 1;
            }
        }

        private void imgDone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Biến đếm xem có bao nhiêu tàu đã được đặt
            //Nếu chưa đặt đủ tàu thì chưa thể chơi
            //1 tàu 2, 2 tàu 3, 1 tàu 4, 1 tàu 5
            //Tổng cộng nếu đủ 17 thì đã đặt đủ tàu
            int quality = 0;

            this._arrPutShip = new int[12, 12];
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {

                    Button btn = this._arrButton[i, j];

                    //Xét màu của button để lưu giá trị loại tàu

                    if (btn.Background == Brushes.Yellow)
                    {
                        this._arrPutShip[i, j] = 1;
                        quality++;
                    }
                    else if (btn.Background == Brushes.Red)
                    {
                        this._arrPutShip[i, j] = 2;
                        quality++;
                    }
                    else if (btn.Background == Brushes.Green)
                    {
                        this._arrPutShip[i, j] = 3;
                        quality++;
                    }
                    else if (btn.Background == Brushes.Blue)
                    {
                        this._arrPutShip[i, j] = 4;
                        quality++;
                    }
                    else if (btn.Background == Brushes.Purple)
                    {
                        this._arrPutShip[i, j] = 5;
                        quality++;
                    }

                }
            }

            if (quality != 17)
            {
                MessageBox.Show("You have not organize enough your ship yet!!!");
            }
            else
            {
                //Mở window để bắt đầu trò chơi
                PlayingWindow playingWindow = new PlayingWindow(this._arrPutShip);
                playingWindow.Show();
                this.Close();
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    this._arrButton[i, j].Background = Brushes.White;
                    this._arrButton[i, j].Opacity = 0;
                }
            }
            resetValue();
        }

        private void resetValue()
        {
            this._shipLength = 0;
            this._direction = 1;
            this._isChooseShip = false;
            this._typeOfShip = 0;
            this._colorShip = new System.Windows.Media.SolidColorBrush();
            createArrIsChooseShip();
        }


    }
}
