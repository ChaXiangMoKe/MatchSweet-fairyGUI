using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private int Y
        {
            get { return y; }
            set { y = value; }
        }


        private SweetsType type;
        /// <summary>
        /// 糖果种类
        /// </summary>
        public SweetsType Type
        {
            get
            {
                return type;
            }
        }

        private GComponent sweet;
        public GComponent Sweet
        {
            get { return sweet; }
            set { sweet = value; }
        }
        public GameSweet (SweetsType type)
        {
            this.type = type;
        }

        private void InitSweet()
        {
            GComponent sweet = 
        }
    }
}

