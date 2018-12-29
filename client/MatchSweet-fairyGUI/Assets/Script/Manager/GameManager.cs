using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Data;
using FairyGUI;

using System;

namespace Game
{

    public class GameManager : MonoBehaviour
    {

        // 大网格的行列数


        // 游戏时间
        public float gameTime = 60;
        // 填充时间
        public float fillTime;

        // 甜品字典
        public Dictionary<SweetsType, string> sweetPrefabDict;

        public GameSweet[,] _sweets;
        public UIStart start;

        private static GameManager instance;
        private GameManager() { }

        public static GameManager Instance
        {
            get
            {
                return instance;
            }

        }
        // Use this for initialization
        void Start()
        {
            instance = this;
            start = new UIStart();
            start.Show();
            _sweets = new GameSweet[PlayerInfo.xColumn, PlayerInfo.yRow];
        }

        /// <summary>
        /// 分布填充
        /// </summary>
        /// <returns></returns>
        public bool Fill()
        {
            // 判断本次填充是否完成
            bool isFiledNotFinished = false;

            for (int y = 0; y < PlayerInfo.yRow; y++)
            {
                for (int x = 0; x < PlayerInfo.xColumn; x++)
                {
                    GameSweet sweet = _sweets[x, y];
                    if (sweet.IsMove)
                    {

                        GameSweet sweetBelow = _sweets[x, y + 1];
                        if(sweetBelow.Type == SweetsType.EMPTY)// 垂直填充
                        {

                        }

                    }
                    else
                    {

                    }
                }
            }
            return true;
        }


    }
}

