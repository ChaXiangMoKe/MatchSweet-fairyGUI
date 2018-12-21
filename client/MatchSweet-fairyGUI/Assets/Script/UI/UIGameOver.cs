using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Game.UI
{
    public class UIGameOver : UIBase
    {

        private GButton return_main_btn;
        private GButton replay_btn;
        private GTextField finalScore_text;
        protected override void OnShown()
        {
            UIPackage.AddPackage("UI/main/main");
            contentPane = UIPackage.CreateObject("main", "gameover").asCom;
        }

        protected override void OnHide()
        {
            
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitCom()
        {
            return_main_btn = contentPane.GetChild("return_main").asButton;
            replay_btn = contentPane.GetChild("quit_btn").asButton;
            finalScore_text = contentPane.GetChild("finalScore_text").asTextField;
        }

        /// <summary>
        /// 初始UI
        /// </summary>
        private void InitUI()
        {
            return_main_btn.onClick.Set(OnStartClick);
            replay_btn.onClick.Set(OnQuitClick);
        }

        private void OnReturnMainClick()
        {
            this.Dispose();
            UIStart start = new UIStart();
            start.Show();
        }

        private void OnRepalyClick()
        {
            this.Dispose();
            UIGame game = new UIGame();
            game.Show();
        }
    }

}
