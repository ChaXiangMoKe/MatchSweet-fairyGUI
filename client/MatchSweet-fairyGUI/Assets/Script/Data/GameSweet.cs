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
        }

        private FairyGUI.Controller _SweetCon;
        private GLoader _loader;
        private GComponent _parents;
        public GameSweet()
        {
            _sweet = UIPackage.CreateObject("main", "sweet").asCom;
            _SweetCon = _sweet.GetController("SweetsType");
            _loader = _sweet.GetChild("icon").asLoader;
            _sweet.visible = true;
        }

        public void InitSweet(GComponent parents)
        {
            _parents = parents;
            _sweet.visible = true;
            _type = SweetsType.EMPTY;
            parents.AddChild(_sweet);
            SetSweetsType(_type);
        }

        /// <summary>
        ///  设置坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public void SetXY(int x, int y)
        {
            X = x;
            Y = y;
            _sweet.xy = ChangePostion(x, y);
        }

        public void SetColor(ColorType color)
        {
            _loader.url = PlayerInfo.Instance.SweetColorDict[color];
        }
        public void SetSweetsType(SweetsType type)
        {
            _SweetCon.SetSelectedIndex((int)type);
            switch (type)
            {
                case SweetsType.EMPTY:
                    _IsMove = true;
                    break;
                case SweetsType.NORMAL:
                    _IsMove = true;
                    _loader.url = PlayerInfo.Instance.GetSweetColor();
                    break;
            }
        }

        public void Move(int newX, int newY, float time)
        {

            Vector2 oldPos = ChangePostion(X, Y);
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

        private Vector2 ChangePostion(int x, int y)
        {
            return new Vector2(x * _sweet.width, y * _sweet.height);
        }

        public void Hide()
        {
            _sweet.visible = false;
            PoolsManager.Instance.HideObj(PoolType.GameSweet,this);
        }

        public void Reset()
        {
            _sweet.visible = true;
            _parents.AddChild(_sweet);
            SetSweetsType(SweetsType.EMPTY);
            x = 0;
            y = 0;
        }
    }
}

