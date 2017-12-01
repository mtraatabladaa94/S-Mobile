using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Java.Lang;

namespace SMobile.Android.Helpers.SliderImages
{
    public class SliderAdapter : PagerAdapter
    {

        private Context context;

        //private int[] imageList = {

        //    Resource.Drawable.img_slider1,

        //    //Resource.Drawable.img_slider2,

        //};

        private int[] colorList = {
            Resource.Color.slide1,
            Resource.Color.slide2,
            Resource.Color.slide3,
        };

        public SliderAdapter(Context context)
        {

            this.context = context;

        }

        public override int Count => this.colorList.Count();

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {

            //return view == (ImageView)@object;
            return view == (View)@object;

        }

        public override Java.Lang.Object InstantiateItem(View container, int position)
        {

            //ImageView imageView = new ImageView(context);

            //imageView.SetScaleType(ImageView.ScaleType.CenterCrop);

            //imageView.SetImageResource(this.imageList[position]);

            //((ViewPager)container).AddView(imageView, 0);

            //return imageView;

            View view = new View(context);

            view.SetBackgroundResource(this.colorList[position]);

            ((ViewPager)container).AddView(view, 0);

            return view;

        }

        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
        {

            ((ViewPager)container).RemoveView((View)@object);

        }

    }

}