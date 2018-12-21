using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

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
        }

        private void OnReturnClick()
        {
            UIStart start = new UIStart();
            start.Show();
            this.Dispose();
        }
    }
}
