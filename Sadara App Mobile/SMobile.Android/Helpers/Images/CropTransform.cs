using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Square.Picasso;
using Android.Support.V4.Graphics.Drawable;
using Android.Content.Res;

namespace SMobile.Android.Helpers.Images
{

    public class CropTransform : Java.Lang.Object, ITransformation
    {

        #region Components for view
        private Resources resources;
        private int width, height, x = 0, y = 0;
        #endregion

        public CropTransform(Resources res, int width, int height)
        {

            if (width <= 0 || height <= 0)
                throw new Exception("Ancho y alto deben ser números positivos");

            this.width = width;

            this.height = height;

            this.resources = res;

        }

        public CropTransform(Resources res, int width, int height, int x, int y)
        {

            if (width <= 0 || height <= 0)
                throw new Exception("Ancho y alto deben ser números positivos");

            if (x < 0 || y < 0)
                throw new Exception("X y Y no pueden ser números negativos");

            this.width = width;

            this.height = height;
            
            this.x = x;

            this.y = y;

            this.resources = res;
            
        }

        public static RoundedBitmapDrawable OnlyCircleImage(Bitmap bitmap, Resources resources)
        {

            RoundedBitmapDrawable roundedBitmap = RoundedBitmapDrawableFactory.Create(resources, bitmap);

            roundedBitmap.Circular = true;

            return roundedBitmap;

        }

        string ITransformation.Key => "square()";

        Bitmap ITransformation.Transform(Bitmap bitmap)
        {
            
            return this.CropImage(bitmap, this.x, this.y, this.width, this.height);

        }

        public RoundedBitmapDrawable GetCircleImage(Bitmap bitmap, int x, int y, int width, int height)
        {

            return this.CropCircleImage(bitmap, x, y, width, height);

        }

        #region Private functions

        private Bitmap CropImage(Bitmap bitmap, int x, int y, int width, int height)
        {

            Bitmap result = Bitmap.CreateBitmap(bitmap, x, y, width, height);

            if (result != bitmap)
                bitmap.Recycle();

            return result;

        }

        private RoundedBitmapDrawable CropCircleImage(Bitmap bitmap, int x, int y, int width, int height)
        {

            var bitmapCrop = this.CropImage(bitmap, x, y, width, height);

            RoundedBitmapDrawable roundedBitmap = RoundedBitmapDrawableFactory.Create(this.resources, bitmapCrop);

            roundedBitmap.Circular = true;

            return roundedBitmap;

        }

        #endregion

    }

}