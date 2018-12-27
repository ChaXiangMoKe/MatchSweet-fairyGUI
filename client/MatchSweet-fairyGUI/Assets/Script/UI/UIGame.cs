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
        }

        /// <summary>
        /// 初始UI
        /// </summary>
        private void InitUI()
        {
            return_button.onClick.Set(OnReturnClick);
            Debug.Log(" xColumn " + GameManager.Instance.xColumn);
            for(int x = 0; x < GameManager.Instance.xColumn; x++)
            {
                for(int y = 0; y < GameManager.Instance.yRow; y++)
                {
                    GComponent gSweet = UIPackage.CreateObject("main", "sweet").asCom;
                    contentPane.AddChild(gSweet);
                    GameSweet sweet = new GameSweet(gSweet);
                    GameManager.Instance.SweetList[x, y] = sweet;
                    Debug.Log("x :" + x + " y :" + y);
                }
            }
        }

        private void OnReturnClick()
        {
            UIStart start = new UIStart();
            start.Show();
            this.Dispose();
        }
    }
}
