using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Square.Picasso;
using Android.Content.Res;
using Android.Support.V4.Graphics.Drawable;

namespace SMobile.Android.Helpers.Images
{
    public class CircleCropSquareTransformation : Java.Lang.Object, ITransformation
    {


        private Resources resources;



        public CircleCropSquareTransformation(Resources resources)
        {

            this.resources = resources;

        }

        string ITransformation.Key => "square()";

        Bitmap ITransformation.Transform(Bitmap p0)
        {

            //Canvas canvas = new Canvas(p0);

            //Paint paint = new Paint
            //{
            //    AntiAlias = true
            //};

            //canvas.DrawCircle(p0.Width, p0.Height, p0.Width, paint);

            ////paint.SetXfermode(new PorterDuffXfermode(Mode.SRC_IN));

            RoundedBitmapDrawable roundedBitmap = RoundedBitmapDrawableFactory.Create(this.resources, p0);

            roundedBitmap.Circular = true;

            return roundedBitmap.Bitmap;

        }

    }

}