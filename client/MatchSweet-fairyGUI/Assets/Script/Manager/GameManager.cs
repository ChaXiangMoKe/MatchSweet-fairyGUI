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
        private UIStart _StartUI;
        public UIStart StartUI
        {
            get
            {
                if (_StartUI == null)
                {
                    _StartUI = new UIStart();
                }
                return _StartUI;
            }
        }
        private UIGame _GameUI;
        public UIGame GameUI
        {
            get
            {
                if (_GameUI == null)
                {
                    _GameUI = new UIGame();
                }
                return _GameUI;
            }
        }

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
            _StartUI.Show();
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

                        GameSweet sweetBelow = _sweets[x, y - 1];
                        if (sweetBelow.Type == SweetsType.EMPTY)// 垂直填充
                        {
                            PoolsManager.HideObj(PoolType.GameSweet, sweetBelow);
                            sweet.Move(x, y + 1, fillTime);
                            _sweets[x, y + 1] = sweet;
                            _sweets[x, y] = PoolsManager.GetSweetObj();
                            _sweets[x, y].InitSweet(GameUI.ParentsCom);
                            isFiledNotFinished = true;
                        }
                        else //斜向填充
                        {
                            for (int down = -1; down <= 1; down++)
                            {
                                if (down != 0)
                                {
                                    if (down != 0)
                                    {
                                        int downX = x + down;
                                        if (downX >= 0 && downX < PlayerInfo.xColumn)
                                        {
                                            GameSweet downSweet = _sweets[downX, y - 1];
                                            if (downSweet.Type == SweetsType.EMPTY)
                                            {
                                                bool canfill = true;  // 用来判断垂直填充是否满足填充需求

                                                for (int belowY = y; belowY <= PlayerInfo.yRow; belowY++)
                                                {
                                                    GameSweet sweetUnder = _sweets[downX, belowY];

                                                    if (sweetUnder.IsMove)
                                                    {
                                                        break;
                                                    }
                                                    else if (sweet.IsMove == false && sweet.Type != SweetsType.EMPTY)
                                                    {
                                                        canfill = false;
                                                        break;
                                                    }
                                                }

                                                if (!canfill)
                                                {
                                                    PoolsManager.HideObj(PoolType.GameSweet, sweetBelow);
                                                    sweet.Move(downX, y + 1, fillTime);
                                                    _sweets[downX, y + 1] = sweet;
                                                    _sweets[downX, y] = PoolsManager.GetSweetObj();
                                                    _sweets[downX, y].InitSweet(GameUI.ParentsCom);
                                                    isFiledNotFinished = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            return true;
        }
    }

}

