﻿using System;
using System.Drawing;

using CoreGraphics;
using Foundation;
using UIKit;
using Masonry;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SquareUnlock.view
{
    [Register("UnlockUIView")]
    public class UnlockUIView : UIView
    {
        public static string correctPassWord { get; set; }
        private List<UIButton> buttonArray;
        private List<UIButton> lineArray;
        private CGPoint currentPoint;
        private bool change;
        public UnlockUIView()
        {
            Initialize();
        }

        public UnlockUIView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            //BackgroundColor = UIColor.Red;
            buttonArray = new List<UIButton>();
            lineArray = new List<UIButton>();

            for(int i = 0;i < 9; i++)
            {
                UIButton button = new UIButton();
                button.Tag = i;
                //未选中时的图片，即默认状态，UIControlState表示状态
                button.SetBackgroundImage(UIImage.FromBundle("image/gesture_node_normal"), UIControlState.Normal);
                //选中时的图片
                button.SetBackgroundImage(UIImage.FromBundle("image/gesture_node_highlighted"), UIControlState.Selected);
                //视图是否响应用户交互事件
                button.UserInteractionEnabled = false;
                this.AddSubview(button);
                buttonArray.Add(button);
            }
        }

        //布局addSubview触发该方法
        public override void LayoutSubviews()
        {
            nfloat width = 74;
            nfloat height = 74;
            nfloat margin = (this.Frame.Size.Width - 3 * width) / 4;

            for (int i = 0; i < buttonArray.Count; i++)
            {
                nfloat x = (i % 3) * (margin + width) + margin;
                nfloat y = (i / 3) * (margin + height) + margin;

                buttonArray[i].Frame = new CoreGraphics.CGRect(x, y, width, height);
            }
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            if (lineArray.Count == 0)
                return;

            //贝塞尔曲线
            UIBezierPath path = new UIBezierPath();

            for (int i = 0; i < lineArray.Count; i++)
            {
                UIButton button = lineArray[i];
                if (i == 0)
                    path.MoveTo(button.Center);
                else
                {
                    path.AddLineTo(button.Center);
                }
            }
            path.AddLineTo(currentPoint);
            path.LineWidth = 10;
            path.LineJoinStyle = CoreGraphics.CGLineJoin.Round;
            path.LineCapStyle = CoreGraphics.CGLineCap.Round;
            UIColor.White.SetStroke();
            if (change)
                UIColor.Red.SetStroke();
            path.Stroke();

        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            UITouch t = (UITouch)touches.AnyObject;

            CGPoint p = t.LocationInView(t.View);

            for (int i = 0; i < buttonArray.Count; i++)
            {
                UIButton button = buttonArray[i];

                if (button.Frame.Contains(p))
                {
                    button.Selected = true;

                    lineArray.Add(button);

                }

            }

        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            UITouch t = (UITouch)touches.AnyObject;

            CGPoint p = t.LocationInView(t.View);

            currentPoint = p;

            for (int i = 0; i < buttonArray.Count; i++)
            {
                UIButton button = buttonArray[i];

                if (button.Frame.Contains(p))
                {
                    button.Selected = true;

                    if (!lineArray.Contains(button))
                    {

                        lineArray.Add(button);
                    }
                }

            }
            SetNeedsDisplay();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            if (lineArray.Count == 0)
                return;
            currentPoint = lineArray[lineArray.Count - 1].Center;

            SetNeedsDisplay();

            this.UserInteractionEnabled = false;

            string passwrod = "";
            for (int i = 0; i < lineArray.Count; i++)
            {
                UIButton button = lineArray[i];

                passwrod = passwrod + button.Tag;

            }
            if (passwrod == correctPassWord)
                Console.WriteLine("correct");
            else
            {
                Console.WriteLine("error");
                change = true;
                SetNeedsDisplay();
            }

            Task.Delay(2000).ContinueWith((t) =>
            {
                BeginInvokeOnMainThread(() =>
                {
                    this.UserInteractionEnabled = true;
                    clearView();
                    change = false;
                });
            });
        }

        public void clearView()
        {
            for (int i = 0; i < buttonArray.Count; i++)
            {
                UIButton button = buttonArray[i];
                button.Selected = false;

            }
            lineArray.RemoveRange(0, lineArray.Count);
            SetNeedsDisplay();

        }
    }
}