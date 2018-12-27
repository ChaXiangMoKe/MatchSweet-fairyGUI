using System.Collections;
using System.Collections.Generic;
using FairyGUI;

namespace Game.Data
{
    public class GameSweet
    {

        private int x;
        private int y;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }


        private SweetsType _type;
        /// <summary>
        /// 糖果种类
        /// </summary>
        public SweetsType Type
        {
            get
            {
                return _type;
            }
        }

        private GComponent _sweet;
        /// <summary>
        /// 糖果组件
        /// </summary>
        public GComponent Sweet
        {
            get { return _sweet; }
            set { _sweet = value; }
        }

        private bool _IsMove;
        public bool IsMove
        {
            get { return _IsMove; }
            set { _IsMove = value; }
        }

        private FairyGUI.Controller _SweetCon;
        private GLoader _loader;
        public GameSweet (GComponent sweet)
        {
            _sweet = sweet;
            _SweetCon = _sweet.GetController("SweetsType");
            _loader = _sweet.GetChild("icon").asLoader;
        }

        private void InitSweet(SweetsType type)
        {
            _SweetCon.SetSelectedIndex((int)type);
            if(type == SweetsType.NORMAL)
            {
                _loader.url = PlayerInfo.GetSweetColor();
            }
        }

        private void Hide()
        {
            _sweet.Dispose();
        }
    }
}

