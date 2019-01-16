using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;


namespace Game.Data
{
    public enum SweetsType
    {
        /// <summary>
        /// 空白
        /// </summary>
        EMPTY,
        /// <summary>
        /// 正常
        /// </summary>
        NORMAL,
        /// <summary>
        /// 障碍
        /// </summary>
        BARRIER,
        /// <summary>
        /// 行消除
        /// </summary>
        ROW_CLEAR,
        /// <summary>
        /// 列消除
        /// </summary>
        COLUMN_CLEAR,
        /// <summary>
        /// 爆炸
        /// </summary>
        EXPLOSION,
        /// <summary>
        /// 全消除
        /// </summary>
        RAINBOWCANDY,
        COUNT//标记类型
    }

    public enum ColorType
    {
        YELLOW,
        PURPLE,
        RED,
        BLUE,
        GREEN,
        PINK,
        ANY,
        COUNT
    }
    public  class PlayerInfo
    {
        private  Dictionary<ColorType, string> _SweetColorDict;
        public  Dictionary<ColorType, string> SweetColorDict
        {
            get { return _SweetColorDict; }
            set { _SweetColorDict = value; }
        }

        /// <summary>
        /// 游戏行数
        /// </summary>
        public static int xColumn = 10;
        /// <summary>
        /// 游戏列数
        /// </summary>
        public static int yRow = 10;

        private static PlayerInfo instance;
        public static PlayerInfo Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new PlayerInfo();
                    instance.SetDictionary();
                }
                return instance;
            }
        }

        public  void SetDictionary()
        {
            _SweetColorDict = new Dictionary<ColorType, string>();
            _SweetColorDict[ColorType.YELLOW] = "Yellow";
            _SweetColorDict[ColorType.PURPLE] = "Purple";
            _SweetColorDict[ColorType.RED] = "Red";
            _SweetColorDict[ColorType.BLUE] = "Blue";
            _SweetColorDict[ColorType.GREEN] = "Green";
            _SweetColorDict[ColorType.PINK] = "Pink";
            _SweetColorDict[ColorType.ANY] = "Colors";
            _SweetColorDict[ColorType.COUNT] = "";
        }

        public ColorType GetSweetColor()
        {
            return (ColorType)Random.Range(0, 5);
        }
        public string GetSweetColorUrl(ColorType color)
        {
            return UIPackage.GetItemURL("main", _SweetColorDict[(ColorType)color]);
        }
    }
}

