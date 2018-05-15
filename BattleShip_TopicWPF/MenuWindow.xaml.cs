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
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();

            //Nhạc nền
            music();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnPlay":
                    ArrangementShip arrangementShip = new ArrangementShip();
                    arrangementShip.ShowDialog();
                    break;
                case "btnHelp":
                    System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Battleship_(game)");
                    break;
                case "btnExit":
                    this.Close();
                    break;
            }
        }

        private void music()
        {
            SoundPlayer soudMedia = new SoundPlayer(Properties.Resources.menuSound);
            soudMedia.PlayLooping();
        }
    }
}
