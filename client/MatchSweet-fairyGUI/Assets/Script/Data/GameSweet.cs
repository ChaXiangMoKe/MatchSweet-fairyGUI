using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game.Data
{
    public class GameSweet : IResetable
    {

        private int x;
        private int y;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }


        private SweetsType _type;
        /// <summary>
        /// 糖果种类
        /// </summary>
        public SweetsType Type
        {
            get
            {
                return _type;
            }
        }

        private GComponent _sweet;
        /// <summary>
        /// 糖果组件
        /// </summary>
        public GComponent Sweet
        {
            get { return _sweet; }
            set { _sweet = value; }
        }

        private bool _IsMove;
        public bool IsMove
        {
            get { return _IsMove; }
            set { _IsMove = value; }
        }

        private FairyGUI.Controller _SweetCon;
        private GLoader _loader;
        public GameSweet()
        {
            _type = SweetsType.EMPTY;
            _sweet = UIPackage.CreateObject("main", "sweet").asCom;
            _SweetCon = _sweet.GetController("SweetsType");
            _loader = _sweet.GetChild("icon").asLoader;
        }

        public void InitSweet(SweetsType type,GComponent parents)
        {
            parents.AddChild(_sweet);
            SetSweetsType(type);
        }

        /// <summary>
        ///  设置坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public void SetXY(int x,int y)
        {
            X = x;
            Y = y;
            _sweet.xy = new Vector2(x * _sweet.width, y * _sweet.height);
        }

        public void SetColor(ColorType color)
        {
            _loader.url = PlayerInfo.SweetColorDict[color];
        }
        public void SetSweetsType(SweetsType type)
        {
            _SweetCon.SetSelectedIndex((int)type);
            if (type == SweetsType.NORMAL)
            {
                _loader.url = PlayerInfo.GetSweetColor();
            }
        }


        public void Move(int newX,int newY,float time)
        {
            
            Vector2 oldPos  = ChangePostion(X,Y);
            Vector2 newPos = ChangePostion(newX, newY);

            Tween _tween = DOTween.To(() => oldPos, pos =>
            {
                try
                {
                    _sweet.xy = pos;
                }
                catch (Exception e)
                {
                    RGLog.Warn(e);
                }
            }, newPos, time).OnComplete(() =>
            {
                X = x;
                Y = y;
            });
        }

        private Vector2 ChangePostion(int x ,int y)
        {
            return new Vector2(x * _sweet.width, y * _sweet.height);
        }

        private void Hide()
        {
            _sweet.Dispose();
        }

        public void Reset()
        {
            SetSweetsType(SweetsType.EMPTY);
            x = 0;
            y = 0;
        }
    }
}

