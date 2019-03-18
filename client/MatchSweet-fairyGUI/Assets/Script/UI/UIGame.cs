using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Game.Data;

namespace Game.UI
{
    public class UIGame : UIBase
    {
        private GTextField time_text;
        private GTextField scote_text;
        private GButton return_button;
        private GComponent parents_com;

        public GComboBox ParentsCom
        {
            get
            {
                return ParentsCom;
            }
        }

        protected override void OnShown()
        {
            UIPackage.AddPackage("UI/main/main");
            contentPane = UIPackage.CreateObject("main", "game").asCom;
            InitCom();
            InitUI();
        }

        protected override void OnHide()
        {
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitCom()
        {
            time_text = contentPane.GetChild("time_text").asTextField;
            scote_text = contentPane.GetChild("scote_text").asTextField;
            return_button = contentPane.GetChild("return_btn").asButton;
            parents_com = contentPane.GetChild("parents").asCom;
            
        }

        /// <summary>
        /// 初始UI
        /// </summary>
        private void InitUI()
        {
            return_button.onClick.Set(OnReturnClick);
            GameManager.Instance.Sweets = new GameSweet[PlayerInfo.Instance.xColumn, PlayerInfo.Instance.yRow];
            for (int x = 0; x < PlayerInfo.Instance.xColumn; x++)
            {
                for (int y = 0; y < PlayerInfo.Instance.yRow; y++)
                {
                    GameSweet gameSweet = PoolsManager.Instance.GetSweetObj();
                    gameSweet.InitSweet(parents_com);
                    gameSweet.SetXY(x, y);
                    GameManager.Instance.Sweets[x, y] = gameSweet;
                }
            }
            GameManager.Instance.StartFill();

        }

        private void OnReturnClick()
        {
            //GameManager.Instance.StartUIShow();
            //this.Dispose();
            //GameManager.Instance.Fill();
            for (int y = 0; y < PlayerInfo.Instance.yRow; y++)
            {
                for (int x = 0; x < PlayerInfo.Instance.xColumn; x++)
                {
                    GameSweet sweet = GameManager.Instance.Sweets[x, y];
                    if (x != sweet.X || y != sweet.Y)
                    {
                        Debug.Log("x :" + x + "  y :" + y);
                        Debug.Log("sx :" + sweet.X + "  sy :" + sweet.Y);
                    }
                }
            }
        }

    }
}
