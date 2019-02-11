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

        private ColorType _color;
        public ColorType Color
        {
            get { return _color; }
            set
            {

                if (_type == SweetsType.BARRIER || _type == SweetsType.COUNT || _type==SweetsType.EMPTY )
                {
                    _color = ColorType.COUNT;
                }
                else
                {
                    _color = value;
                    _loader.url = PlayerInfo.Instance.GetSweetColorUrl(_color);;
                }

            }
        }
        public bool IsMove { get; private set; }
        public bool IsClear { get; private set; }

        private FairyGUI.Controller _SweetCon;
        private GLoader _loader;
        private GComponent _parents;
        private Transition _normalClearTra;

        public GameSweet()
        {
            _sweet = UIPackage.CreateObject("main", "sweet").asCom;
            _SweetCon = _sweet.GetController("SweetsType");
            _loader = _sweet.GetChild("icon").asLoader;
            _normalClearTra = _sweet.GetTransition("normal_clear");
            _sweet.visible = true;
            _sweet.onTouchBegin.Set(OnStart);
            _sweet.onTouchEnd.Set(OnEnd);
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

        public void SetSweetsType(SweetsType type)
        {
            _type = type;
            _SweetCon.SetSelectedIndex((int)type);
            switch (type)
            {
                case SweetsType.EMPTY:
                    {
                        IsMove = false;
                        IsClear = false;
                        _color = ColorType.COUNT;
                    }
                    break;
                case SweetsType.NORMAL:
                    {
                        IsMove = true;
                        IsClear = true;
                        _loader.alpha = 1;
                        Color = PlayerInfo.Instance.GetSweetColor();
                    }
                    break;
                case SweetsType.ROW_CLEAR:
                    {
                        IsMove = true;
                        IsClear = true;
                    }
                    break;
                case SweetsType.COLUMN_CLEAR:
                    {
                        IsMove = true;
                        IsClear = true;
                    }
                    break;
                case SweetsType.EXPLOSION:
                    {
                        IsMove = true;
                        IsClear = true;
                    }
                    break;
                case SweetsType.RAINBOWCANDY:
                    {
                        IsMove = true;
                        IsClear = true;
                    }
                    break;
                case SweetsType.BARRIER:
                    {
                        IsMove = false;
                        _color = ColorType.COUNT;
                    }
                    break;
            }
        }

        public void Move(int newX, int newY, float time)
        {
            Vector2 oldPos = ChangePostion(X, Y);
            Vector2 newPos = ChangePostion(newX, newY);
            X = newX;
            Y = newY;
            DOTween.To(() => oldPos, pos =>
            {
                try
                {
                    _sweet.xy = pos;
                }
                catch (Exception e)
                {
                    RGLog.Warn(e);
                }
            }, newPos, time);

        }

        /// <summary>
        ///  是否可消除
        /// </summary>
        /// <returns></returns>
        public bool IsMatch()
        {
            if (_type == SweetsType.NORMAL||_type == SweetsType.COLUMN_CLEAR ||_type == SweetsType.ROW_CLEAR||_type == SweetsType.RAINBOWCANDY||_type==SweetsType.EXPLOSION)
            {
                if (_color != ColorType.COUNT)
                {
                    return true;
                }      
            }
            return false;
        }

        private Vector2 ChangePostion(int x, int y)
        {
            return new Vector2(x * _sweet.width, y * _sweet.height);
        }

        public void Hide()
        {
            _sweet.visible = false;
            PoolsManager.Instance.HideObj(PoolType.GameSweet, this);
            _parents.RemoveChild(_sweet);
        }

        public void Clear()
        {
            if (!IsClear) return;
            switch (_type)
            {
                case SweetsType.NORMAL:
                    {
                        _normalClearTra.Play();
                    }
                    break;
                case SweetsType.ROW_CLEAR:
                    {
                        _normalClearTra.Play();
                    }
                    break;
                case SweetsType.COLUMN_CLEAR:
                    {
                        _normalClearTra.Play();
                    }
                    break;
                case SweetsType.EXPLOSION:
                    {
                        _normalClearTra.Play();
                    }
                    break;
                case SweetsType.RAINBOWCANDY:
                    {
                        _normalClearTra.Play();
                    }
                    break;
            }
            _type = SweetsType.EMPTY;
        }
        public void Reset()
        {
            _sweet.visible = true;
            _parents.AddChild(_sweet);
            SetSweetsType(SweetsType.EMPTY);
            if (_normalClearTra.playing)
            {
                _normalClearTra.Stop();
            }
            x = 0;
            y = 0;
        }

        public void OnStart()
        {
            GameManager.Instance.PressSweet(this);
            RGLog.Log(ToString());
        }

        public void OnEnd()
        {
            GameManager.Instance.EnterSweet(this);
        }

        public override string ToString()
        {
            return "[x] " + x + "[y] "+y+"[type] "+Type+"[color]"+Color+" [alpha] "+_loader.alpha;

        }
    }
}

