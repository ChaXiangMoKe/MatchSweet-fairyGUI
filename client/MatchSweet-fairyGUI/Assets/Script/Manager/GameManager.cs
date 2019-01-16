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
        // 游戏时间
        public float gameTime = 60;
        // 填充时间
        public float fillTime;

        // 甜品字典
        public Dictionary<SweetsType, string> sweetPrefabDict;

        private GameSweet[,] _sweets;
        public GameSweet[,] Sweets
        {
            get { return _sweets; }
            set { _sweets = value; }
        }
        private UIStart _StartUI;
        public UIStart StartUI
        {
            get { return _StartUI; }
            set { _StartUI = value; }
        }
        private UIGame _GameUI;
        public UIGame GameUI
        {
            get { return _GameUI; }
            set { _GameUI = value; }

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
            Init();
        }

        public void Init()
        {
            StartUIShow();
            _sweets = new GameSweet[PlayerInfo.xColumn, PlayerInfo.yRow];
        }

        public void StartUIShow()
        {
            _StartUI = new UIStart();
            _StartUI.Show();
        }

        public void GameUIShow()
        {
            _GameUI = new UIGame();
            _GameUI.Show();
        }
        public void StartFill()
        {
            StartCoroutine(AllFill());
        }

        public IEnumerator AllFill()
        {
            bool needRefill = true;
            while (needRefill)
            {
                yield return new WaitForSeconds(fillTime);
                while (Fill())
                {
                    yield return new WaitForSeconds(fillTime);
                }
                needRefill = ClearAllMatchedSweet();
            }
        }

        /// <summary>
        /// 分布填充
        /// </summary>
        /// <returns></returns>
        public bool Fill()
        {
            // 判断本次填充是否完成
            bool isFiledNotFinished = false;

            for (int y = PlayerInfo.yRow - 2; y >= 0; y--)
            {
                for (int x = 0; x < PlayerInfo.xColumn; x++)
                {
                    GameSweet sweet = _sweets[x, y];
                    if (sweet.IsMove)
                    {
                        GameSweet sweetBelow = _sweets[x, y + 1];
                        if (sweetBelow.Type == SweetsType.EMPTY)// 垂直填充
                        {
                            sweetBelow.Hide();
                            sweet.Move(x, y + 1, fillTime);
                            _sweets[x, y + 1] = sweet;
                            _sweets[x, y] = PoolsManager.Instance.GetSweetObj();
                            _sweets[x, y].SetXY(x, y);
                            isFiledNotFinished = true;
                        }
                        //else //斜向填充
                        //{
                        //    for (int down = -1; down <= 1; down++)
                        //    {
                        //        if (down != 0)
                        //        {
                        //            if (down != 0)
                        //            {
                        //                int downX = x + down;
                        //                if (downX >= 0 && downX < PlayerInfo.xColumn)
                        //                {
                        //                    GameSweet downSweet = _sweets[downX, y - 1];
                        //                    if (downSweet.Type == SweetsType.EMPTY)
                        //                    {
                        //                        bool canfill = true;  // 用来判断垂直填充是否满足填充需求

                        //                        for (int belowY = y; belowY <= PlayerInfo.yRow; belowY++)
                        //                        {
                        //                            GameSweet sweetUnder = _sweets[downX, belowY];

                        //                            if (sweetUnder.IsMove)
                        //                            {
                        //                                break;
                        //                            }
                        //                            else if (sweet.IsMove == false && sweet.Type != SweetsType.EMPTY)
                        //                            {
                        //                                canfill = false;
                        //                                break;
                        //                            }
                        //                        }

                        //                        if (!canfill)
                        //                        {
                        //                            sweetBelow.Hide();
                        //                            sweet.Move(downX, y + 1, fillTime);
                        //                            _sweets[downX, y + 1] = sweet;
                        //                            _sweets[downX, y] = PoolsManager.Instance.GetSweetObj();
                        //                            isFiledNotFinished = true;
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }

            }
            //最上排的特殊情况
            for (int x = 0; x < PlayerInfo.xColumn; x++)
            {
                GameSweet sweet = _sweets[x, 0];
                if (sweet.Type == SweetsType.EMPTY)
                {
                    sweet.SetSweetsType(SweetsType.NORMAL);
                    sweet.SetXY(x, -1);
                    sweet.Move(x, 0, fillTime);
                    isFiledNotFinished = true;
                }
            }
            return isFiledNotFinished;
        }


        public List<GameSweet> MatchSweets(GameSweet sweet)
        {
            List<GameSweet> finishedList = new List<GameSweet>();
            if (!sweet.IsClear)
            {
                return finishedList;
            }

            ColorType color = sweet.Color;
            List<GameSweet> columnList = new List<GameSweet>();
            List<GameSweet> rowList = new List<GameSweet>();
            // 列匹配
            rowList = FindRowSameSweet(sweet, color);
            rowList.Add(sweet);

            if (rowList.Count == 3)
            {
                for (int count = 0; count < rowList.Count; count++)
                {
                    GameSweet tempSweet = rowList[count];
                    // L T性匹配
                    columnList.Clear();
                    columnList = FindColumnSameSweet(tempSweet, color);
                    if (columnList.Count >= 2)
                    {
                        tempSweet.SetSweetsType(SweetsType.EXPLOSION);
                        for (int k = 0; k < columnList.Count; k++)
                        {
                            finishedList.Add(columnList[k]);
                        }
                    }
                    else
                    {
                        finishedList.Add(tempSweet);
                    }
                }
            }
            else if (rowList.Count == 4)
            {
                bool isSuccess = false;
                for (int count = 0; count < rowList.Count; count++)
                {
                    GameSweet tempSweet = rowList[count];
                    // L T性匹配
                    columnList.Clear();
                    columnList = FindColumnSameSweet(tempSweet, color);
                    if (columnList.Count >= 2)
                    {
                        isSuccess = true;
                        tempSweet.SetSweetsType(SweetsType.EXPLOSION);
                        for (int k = 0; k < columnList.Count; k++)
                        {
                            finishedList.Add(columnList[k]);
                        }
                    }
                    else
                    {
                        finishedList.Add(tempSweet);
                    }
                }
                if (!isSuccess)
                {
                    finishedList.Remove(sweet);
                    sweet.SetSweetsType(SweetsType.COLUMN_CLEAR);
                }
            }
            else if (rowList.Count >= 5)
            {
                for (int count = 0; count < rowList.Count; count++)
                {
                    GameSweet tempSweet = rowList[count];
                    finishedList.Add(tempSweet);
                    // L T性匹配
                    columnList.Clear();
                    columnList = FindColumnSameSweet(tempSweet, color);
                    if (columnList.Count >= 2)
                    {
                        for (int k = 0; k < columnList.Count; k++)
                        {
                            finishedList.Add(columnList[k]);
                        }
                    }
                }
                finishedList.Remove(sweet);
                sweet.SetSweetsType(SweetsType.RAINBOWCANDY);
            }

            if (finishedList.Count >= 3)
            {
                return finishedList;
            }

            columnList.Clear();
            rowList.Clear();
            finishedList.Clear();

            columnList = FindColumnSameSweet(sweet, color);
            columnList.Add(sweet);
            if (columnList.Count == 3)
            {
                for (int count = 0; count < columnList.Count; count++)
                {
                    GameSweet tempSweet = columnList[count];
                    rowList.Clear();
                    rowList = FindRowSameSweet(tempSweet, color);
                    if (rowList.Count >= 2)
                    {
                        tempSweet.SetSweetsType(SweetsType.EXPLOSION);
                        for (int k = 0; k < rowList.Count; k++)
                        {
                            finishedList.Add(rowList[k]);
                        }
                    }
                }

            }
            else if (columnList.Count == 4)
            {
                bool isSuccess = false;
                for (int count = 0; count < columnList.Count; count++)
                {
                    GameSweet tempSweet = columnList[count];
                    rowList.Clear();
                    rowList = FindRowSameSweet(tempSweet, color);
                    if (rowList.Count >= 2)
                    {
                        isSuccess = true;
                        tempSweet.SetSweetsType(SweetsType.EXPLOSION);
                        for (int k = 0; k < rowList.Count; k++)
                        {
                            finishedList.Add(rowList[k]);
                        }
                    }
                }
                if (!isSuccess)
                {
                    sweet.SetSweetsType(SweetsType.ROW_CLEAR);
                }
            }
            else if (columnList.Count >= 5)
            {
                for (int count = 0; count < columnList.Count; count++)
                {
                    GameSweet tempSweet = columnList[count];
                    rowList.Clear();
                    rowList = FindRowSameSweet(tempSweet, color);
                    if (rowList.Count >= 2)
                    {
                        for (int k = 0; k < rowList.Count; k++)
                        {
                            finishedList.Add(rowList[k]);
                        }
                    }
                }
                sweet.SetSweetsType(SweetsType.RAINBOWCANDY);
            }

            if (finishedList.Count <= 2)
            {

                finishedList.Clear();
            }
            return finishedList;
        }


        /// <summary>
        /// 行匹配
        /// </summary>
        /// <param name="sweet">糖果本身</param>
        /// <param name="type">糖果颜色</param>
        /// <returns>匹配结果</returns>
        public List<GameSweet> FindColumnSameSweet(GameSweet sweet, ColorType type)
        {
            int currentX = sweet.X;
            int x;
            List<GameSweet> list = new List<GameSweet>();
            for (int i = 0; i <= 1; i++)
            {
                for (int xDistance = 1; xDistance < PlayerInfo.xColumn; xDistance++)
                {
                    if (i == 0)
                    {
                        x = currentX - xDistance;
                    }
                    else
                    {
                        x = currentX + xDistance;
                    }
                    if (x < 0 || x >= PlayerInfo.xColumn)
                    {
                        break;
                    }
                    if (sweet.IsMatch() && _sweets[x, sweet.Y].Color == type)
                    {
                        list.Add(_sweets[x, sweet.Y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 列匹配
        /// </summary>
        /// <param name="sweet">糖果本身</param>
        /// <param name="type">糖果颜色</param>
        /// <returns>匹配结果</returns>
        public List<GameSweet> FindRowSameSweet(GameSweet sweet, ColorType type)
        {
            int currentY = sweet.Y;
            int y;
            List<GameSweet> list = new List<GameSweet>();
            for (int i = 0; i <= 1; i++)
            {
                for (int yDistance = 1; yDistance < PlayerInfo.yRow; yDistance++)
                {
                    if (i == 0)
                    {
                        y = currentY - yDistance;
                    }
                    else
                    {
                        y = currentY + yDistance;
                    }
                    if (y < 0 || y >= PlayerInfo.yRow)
                    {
                        break;
                    }
                    if (sweet.IsMatch() && _sweets[sweet.X, y].Color == type)
                    {
                        list.Add(_sweets[sweet.X, y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 清除全部完成匹配的甜品
        /// </summary>
        /// <returns></returns>
        public bool ClearAllMatchedSweet()
        {
            bool needRefill = false;
            for (int y = 0; y < PlayerInfo.yRow; y++)
            {
                for (int x = 0; x < PlayerInfo.xColumn; x++)
                {
                    if (_sweets[x, y].IsClear)
                    {
                        List<GameSweet> clearList = new List<GameSweet>();
                        clearList = MatchSweets(_sweets[x, y]);
                        if (clearList != null && clearList.Count > 0)
                        {
                            for (int i = 0; i < clearList.Count; i++)
                            {
                                clearList[i].Clear();
                                needRefill = true;
                            }
                        }
                    }
                }
            }
            return needRefill;
        }
    }

}

