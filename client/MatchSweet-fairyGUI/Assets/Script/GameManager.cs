using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Data;
using FairyGUI;
using DG.Tweening;
using System;

namespace Game
{

    public class GameManager : MonoBehaviour
    {

        // 大网格的行列数
        public int xColumn = 10;
        public int yRow = 10;

        // 游戏时间
        public float gameTime = 60;
        // 填充时间
        public float fillTime;

        // 甜品字典
        public Dictionary<SweetsType, string> sweetPrefabDict;

        public GameSweet[,] SweetList;
        public UIStart start;

        private static GameManager gameManager;
        private GameManager() { }

        public static GameManager Instance
        {
            get {
                if (gameManager == null)
                {
                    gameManager = new GameManager();
                }
                return gameManager;
            }

        }
        // Use this for initialization
        void Start()
        {
            start = new UIStart();
            start.Show();
            SweetList = new GameSweet[xColumn,yRow];
        }

        public void Move(GameSweet sweet, int x, int y, float time)
        {
            GComponent component = sweet.Sweet;
            Vector2 newPos = new Vector2(sweet.X, sweet.Y);
            sweet.X = x;
            sweet.Y = y;
            Vector2 oldPos = new Vector2(sweet.X, sweet.Y);

            Tween _tween = DOTween.To(() => oldPos, pos =>
            {
                try
                {
                    component.xy = pos;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }, newPos, time).OnComplete(() =>
            {

            });

        }
    }
}

