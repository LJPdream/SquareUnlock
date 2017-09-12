using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using SquareUnlock.view;

namespace SquareUnlock.controller
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("UnlockUIViewController")]
    public class UnlockUIViewController : UIViewController
    {
        public UnlockUIViewController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            //View = new UniversalView();

            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("image/back.jpeg"));

            UnlockUIView view = new UnlockUIView();
            view.Frame = new CoreGraphics.CGRect(30, 221.5, 314, 335.5);
            view.BackgroundColor = UIColor.Clear;

            UnlockUIView.correctPassWord = "012";
            this.View.AddSubview(view);
        }
    }
}