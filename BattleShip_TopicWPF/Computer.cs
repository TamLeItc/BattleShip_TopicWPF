using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleShip_TopicWPF
{
    class Computer
    {
        public int[,] arrPutShip = new int[12, 12];

        public Computer() { }

        //AI đặt tàu của mình
        //1 tàu có độ dài 2, 2 tàu có độ dài 3, 1 tàu có độ dài 4, 1 tàu có độ dài 5
        public int[,] shipDeployment()
        {


            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    arrPutShip[i, j] = 0;
                }
            }

            int qualityShip = 0, x, y, length = 2, typeOfShip = 1;
            while (qualityShip < 5)
            {
                //Xác định vị trí đặt tàu
                //Con tàu cần vị trí có chiều rộng = 1 và chiều dài bằng lenght
                //Và mỗi con tàu phải cách nhau là 1.
                //Nên 1 vị trí có chiều rộng ít nhất = 3 và chiều dài ít nhất bằng length +2 thì mới đặt được tàu
                while (true)
                {
                    Random rand = new Random();
                    x = rand.Next(1, 11);
                    y = rand.Next(1, 11);

                    //Nếu vị trí được đặt chưa có tàu
                    if (arrPutShip[x, y] != 1)
                    {
                        //Kiểm tra xem có thể đặt tàu không
                        //Từ vị trí x, y chạy qua phải và xuống dưới. Xem có hướng nào có thể đặt tàu hay không
                        //right, bottom thể hiện hướng đặt. giá trị 1 có thể đặt tàu theo hướng đó.
                        int right = 1, bottom = 1;
                        if (x + length <= 10)
                        {

                            for (int i = x; i <= x + length; i++)
                            {
                                if (arrPutShip[i, y] != 0)
                                {
                                    right = 0;
                                    break;
                                }

                                //Kiểm tra hai bên có đảm bảo không
                                if (y > 1)
                                {
                                    if (arrPutShip[i, y - 1] != 0)
                                    {
                                        right = 0;
                                        break;
                                    }
                                }
                                if (y < 10 && arrPutShip[i, y + 1] != 0)
                                {
                                    right = 0;
                                    break;
                                }
                            }

                            //Kiểm tra hai đầu
                            if (x > 1 && arrPutShip[x - 1, y] != 0) { right = 0; break; }
                            if (x > 1 && arrPutShip[x - 1, y] != 0) { right = 0; break; }
                        }
                        else
                        {
                            right = 0;
                        }
                        if (y + length <= 10)
                        {
                            for (int i = y; i <= y + length; i++)
                            {
                                if (arrPutShip[x, i] != 0)
                                {
                                    bottom = 0;
                                    break;
                                }

                                //Kiểm tra hai bên có đảm bảo không
                                if (x > 1)
                                {
                                    if (arrPutShip[x - 1, i] != 0)
                                    {
                                        bottom = 0;
                                        break;
                                    }
                                }
                                if (x < 10 && arrPutShip[x + 1, i] != 0)
                                {
                                    bottom = 0;
                                    break;
                                }
                            }

                            //Kiểm tra hai đầu
                            if (y > 1 && arrPutShip[y - 1, y] != 0) { bottom = 0; break; }
                            if (y > 1 && arrPutShip[y - 1, y] != 0) { bottom = 0; break; }
                        }
                        else
                        {
                            bottom = 0;
                        }

                        //Xác định hướng và đặt tàu
                        if (right != 0 || bottom != 0)
                        {
                            //Xác định hướng đặt tàu
                            //1 sang phải, 2 xuống dưới
                            int dir = 0;
                            if (right == 1 && bottom == 1)
                            {
                                dir = new Random().Next(1, 3);
                            }
                            else if (right == 1 && bottom != 1)
                            {
                                dir = 1;
                            }
                            else if (right != 1 && bottom == 1)
                            {
                                dir = 2;
                            }

                            //Đặt tàu
                            if (dir == 1)
                            {
                                for (int i = x; i < x + length; i++)
                                {
                                    arrPutShip[i, y] = typeOfShip;
                                }
                            }
                            else
                            {
                                for (int i = y; i < y + length; i++)
                                {
                                    arrPutShip[x, i] = typeOfShip;

                                }
                            }

                            qualityShip++;
                            typeOfShip++;

                            if (qualityShip == 2)
                            {
                                length = 3;
                            }
                            else
                            {
                                length++;
                            }
                            break;
                        }
                    }
                }
            }
            return this.arrPutShip;
        }

        //a = giá trị của area tại một vị trí
        //nếu a = 1. Vị trí đã được bắn
        //nếu a = 0. Vị trí chưa được bắn.
        public Point fire()
        {
            Random rand = new Random();
            return new Point(rand.Next(1, 11), rand.Next(1, 11));

        }
    }
}
