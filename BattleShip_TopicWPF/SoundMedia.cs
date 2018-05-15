using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace BattleShip_TopicWPF
{
    class SoundMedia
    {
        //Tạo 3 array chứa 3 loại âm thanh. Bắn, Bắn trượt, Bắn trúng
        private SoundPlayer[] _arrSoundShoot;
        private SoundPlayer[] _arrSoundHit;
        private SoundPlayer[] _arrSoundMisses;

        public SoundMedia()
        {
            createArrSounds();
        }

        private void createArrSounds()
        {
            //Âm thanh bắn
            this._arrSoundShoot = new SoundPlayer[3];
            this._arrSoundShoot[0] = new SoundPlayer(Properties.Resources.shot0Sound);
            this._arrSoundShoot[1] = new SoundPlayer(Properties.Resources.shot1Sound);
            this._arrSoundShoot[2] = new SoundPlayer(Properties.Resources.shot2Sound);

            //Âm thanh bắn trúng
            this._arrSoundHit = new SoundPlayer[3];
            this._arrSoundHit[0] = new SoundPlayer(Properties.Resources.hit0Sound);
            this._arrSoundHit[1] = new SoundPlayer(Properties.Resources.hit1Sound);
            this._arrSoundHit[2] = new SoundPlayer(Properties.Resources.hit2Sound);
            

            //Âm thanh bắn trượt
            this._arrSoundMisses = new SoundPlayer[3];
            this._arrSoundMisses[0] = new SoundPlayer(Properties.Resources.splash0Sound);
            this._arrSoundMisses[1] = new SoundPlayer(Properties.Resources.splash1Sound);
            this._arrSoundMisses[2] = new SoundPlayer(Properties.Resources.splash2Sound);

        }

        //Type = 1, 2, 3. Nếu 1 phát âm thanh bắn, Nếu 2 phát âm thanh bắn trúng, Nếu 3 phát âm thanh bắn trượt
        public void playSound(int type)
        {
            //Random ra 1 vị trí để lấy âm thanh ra phát
            Random rand = new Random();
            int loc = rand.Next(0, 2);

            if (type == 1)
            {
                SoundPlayer sp = this._arrSoundShoot[loc];
                sp.Play();
                
            }
            else if (type == 2)
            {
                SoundPlayer sp = this._arrSoundHit[loc];
                sp.Play();
            }
            else if (type == 3)
            {
                SoundPlayer sp = this._arrSoundMisses[loc];
                sp.Play();
            }

        }

    }
}
