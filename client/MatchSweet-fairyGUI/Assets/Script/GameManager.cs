using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Game
{
    public enum SweetsType
    {
        EMPTY,
        NORMAL,
        BARRIER,
        ROW_CLEAR,
        COLUMN_CLEAR,
        RAINBOWCANDY,
        COUNT//标记类型
    }

    public class GameManager : MonoBehaviour
    {

        // 大网格的行列数
        public int xColumn;
        public int yRow;

        // 游戏时间
        public float gameTime = 60;
        // 填充时间
        public float fillTime;

        // 甜品字典
        public Dictionary<SweetsType, string> sweetPrefabDict;

        public UIStart start;
        // Use this for initialization
        void Start()
        {
            start = new UIStart();
            start.Show();
        }

    }
}

