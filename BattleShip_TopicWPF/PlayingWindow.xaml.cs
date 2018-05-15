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
using System.Threading;
using System.Reflection;
using System.IO;

namespace BattleShip_TopicWPF
{
    /// <summary>
    /// Interaction logic for PlayingWindow.xaml
    /// </summary>

    public partial class PlayingWindow : Window
    {
        private double _widthHeightButton = 34.8;

        private int _qualityShipOfPlayer = 5;        //Số lượng tàu còn lại của người chơi
        private int _missesPlayer = 0;               //Số lần bắn miss của người chơi
        private int _hitRatioPlayer = 0;             //Số lần bắn trúng của người chơi

        private int _qualityShipOfComputer = 5;
        private int _missesComputer = 0;
        private int _hitRatioComputer = 0;

        private Button[,] _arrButtonPlayer;         //Lưu dưới dạng màu sắc. Thể hiện sư sắp xếp tàu của người chơi và máy tính
        private Button[,] _arrButtonComputer;       //Đồng thời thể hiện quá trình bắn của người chơi và máy tính thông qua màu
                                                    //Vị trí button chưa bắn sẽ có màu trắng trong suốt. Đã bắn sẽ là màu trắng thấy được
                                                    //Vị trí bắn trúng tàu sẽ có màu đỏ

        private int[,] _arrValuePlayer = new int[12, 12];             //Lưu giá trị ma trận dưới dạng số. 
        private int[,] _arrValueComputer = new int[12, 12];           //Thể hiện sự sắp xếp tàu của người chơi "Player"

        private Computer _cpt;                      //Đối tượng Computer. Đối tượng này sẽ chơi với người chơi

        private SoundMedia sound = new SoundMedia();        //Phát âm thanh của trò chơi

        public PlayingWindow()
        {
            InitializeComponent();

            createArrValueComputer();
            createArrButton();     

        }

        public PlayingWindow(int[,] arrValuePlayer)
        {

            InitializeComponent();

            //Gán giá trị cho mảng Value Player
            this._arrValuePlayer = arrValuePlayer;

            //Gán gái trị cho mảng Value Computer
            createArrValueComputer();

            //Khởi tạo button và gán giá trị cho các button. Add nó vào WrapPanel để hiện thị lên cho người chơi
            createArrButton();

        }

        private void createArrValueComputer()
        {
            this._arrValueComputer = new int[12, 12];
            this._cpt = new Computer();
            this._arrValueComputer = this._cpt.shipDeployment();
        }
      

        private void createArrButton()
        {
            this._arrButtonComputer = new Button[12, 12];
            this._arrButtonPlayer = new Button[12, 12];

            //Tạo mảng button cho phần bản đồ chứa tàu của người chơi
            //Phần này người chơi chỉ xem mà không thể tác động
            //Máy tính sẽ tác động "bắn" trên đây
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Button btn = new Button();
                    btn.Width = _widthHeightButton;
                    btn.Height = _widthHeightButton;

                    btn.Background = getColorByTypeOfShip(this._arrValuePlayer[i, j]);

                    //Ẩn các button đi
                    btn.Opacity = 0;

                    this._arrButtonPlayer[i, j] = btn;

                    //Add button lên WrapPanel
                    this.wrpPlayer.Children.Add(btn);

                }
            }

            //Tạo mảng button cho phần bản đồ chứa tàu của máy tính
            //Phần này máy tính không thể tác động
            //Người chơi sẽ tác động "bắn" trên đây bằng cách click vào các button
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Button btn = new Button();
                    btn.Width = _widthHeightButton;
                    btn.Height = _widthHeightButton;

                    //Xét ở arrValueComputer. Nếu giá trị tại vị trí i, j khác "0" thì nghĩa là vị trí đó có chứa tàu
                    btn.Background = getColorByTypeOfShip(this._arrValueComputer[i, j]);

                    //Thêm sự kiện click cho butotn
                    btn.Click += clickButtonInAreaShipComputer;

                    btn.Opacity = 0;

                    this._arrButtonComputer[i, j] = btn;

                    //Add button lên WrapPanel
                    this.wrpComputer.Children.Add(btn);
                }
            }
        }  

        //Truyền vào loại tàu 1, 2, 3, 4, 5. Trả về màu (1: Yellow, 2: Red, 3: Green, 4: Blue, 5: Purple)
        private System.Windows.Media.SolidColorBrush getColorByTypeOfShip(int type)
        {
            switch (type)
            {
                case 1:
                    return Brushes.Yellow;
                case 2:
                    return Brushes.Red;
                case 3:
                    return Brushes.Green;
                case 4:
                    return Brushes.Blue;
                case 5:
                    return Brushes.Purple;
                default:
                    return Brushes.White;
            }
        }
        
        //Tại đây xử lí việc Click vào khu vực đặc tàu của Computer để bắn của người chơi
        private void clickButtonInAreaShipComputer(object sender, EventArgs e)
        {
            
            //Bắt đầu âm thanh bắn
            sound.playSound(1);
            //Sleep một khoảng để chờ phát hết âm thanh
            Thread.Sleep(2000);

            Button btn = (Button)sender;

            //Hiển thị vị trí bị bắn
            btn.Opacity = 25;

            //Kiểm tra xem vừa rồi người chơi bắn miss hay hit
            if (btn.Background == Brushes.White)
            {
                this._missesPlayer++;
                this.txbPlayerMisses.Text = this._missesPlayer + "";
                this.rxbLog.AppendText("Your turn: misses\n");

                //Phát âm thanh bắn trượt. Sleep 1 khoảng
                //sound.playSound(3);
            }
            else
            {
                this._hitRatioPlayer++;
                this.txbPlayerHitRatio.Text = this._hitRatioPlayer + "";
                this.rxbLog.AppendText("Your turn: was hit by computer's ship\n");

                //Phát âm thanh bắn trúng
                //sound.playSound(2);

                //Cập nhật số lượng tàu còn lại của Computer
                if (!isHaveSameButton(btn, this._arrButtonComputer))
                {
                    this._qualityShipOfComputer -= 1;
                    this.txbComputerLeft.Text = this._qualityShipOfComputer + "";

                    //Nếu số lượng tàu còn lại của Computer = 0 thì người chơi thắng
                    if (this._qualityShipOfComputer == 0)
                    {
                        this.rxbLog.AppendText("You win");
                        MessageBox.Show("You win");
                        this.Close();
                    }
                }
            }

            //Sau khi người chơi bắn xong. Gọi phương thức để Computer thực hiện bắn
            computerFire();

        }

        //Xử lí máy tính bắn
        private void computerFire()
        {

            Point pointFire = this._cpt.fire();

            int xFire = (int)pointFire.X, yFire = (int)pointFire.Y;

            //Bắt đầu âm thanh bắn
            sound.playSound(1);
            //Sleep một khoảng để chờ phát hết âm thanh
            Thread.Sleep(2000);

            //Hiển thị vị trí bị bắn
            this._arrButtonPlayer[xFire, yFire].Opacity = 25;

            if (this._arrButtonPlayer[xFire, yFire].Background == Brushes.White)
            {
                this._missesComputer++;
                this.txbComputerMisses.Text = this._missesComputer + "";
                this.rxbLog.AppendText("Computer's turn: misses\n");
            }
            else
            {
                this._hitRatioComputer++;
                this.txbComputerHitRadio.Text = this._hitRatioComputer + "";

                //Cập nhật số lượng tàu còn lại của người chơi
                if(!isHaveSameButton(this._arrButtonPlayer[xFire, yFire], this._arrButtonPlayer))
                {
                    this._qualityShipOfPlayer--;
                    this.txbPlayerShipLeft.Text = this._qualityShipOfPlayer + "";
                    this.rxbLog.AppendText("Computer's turn: was hit by your ship\n");

                    if (this._qualityShipOfPlayer == 0)
                    {
                        this.rxbLog.AppendText("Computer win");
                        MessageBox.Show("Computer win");
                        this.Close();
                    }
                }

            }
        }

        //Truyền vào 1 button và kiểm tra xem button có màu tương tự
        //có Opacity = 0 trong array
        private bool isHaveSameButton(Button btn, Button[,] arrButton)
        {
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    if (arrButton[i, j].Background == btn.Background && arrButton[i, j].Opacity == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


