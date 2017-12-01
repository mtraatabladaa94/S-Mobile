using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

namespace SMobile.Android.Fragments
{
    public class MainFragment : Fragment
    {

        ViewPager pager;

        TabsAdapter adapter;

        FragmentManager fm;

        public MainFragment(FragmentManager fm)
        {

            this.fm = fm;

        }

        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.main_fragment, container, false);

            return view;

        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {



            adapter = new TabsAdapter(Context, fm);

            pager = view.FindViewById<ViewPager>(Resource.Id.tabs_viewpager);

            var tabs = view.FindViewById<TabLayout>(Resource.Id.sliding_tabs);

            pager.Adapter = adapter;

            tabs.SetupWithViewPager(pager);

            pager.OffscreenPageLimit = 3;
            
            pager.PageSelected += (sender, args) =>
            {

                var fragment = adapter.InstantiateItem(pager, args.Position);
                
            };

        }

    }

    class TabsAdapter : FragmentStatePagerAdapter
    {
        string[] titles = new string[] {
            "Productos",
            "Ofertas",
            "Empresas",
        };

        FragmentManager fm;

        public override int Count => titles.Length;
        
        public TabsAdapter(Context context, FragmentManager fm) : base(fm)
        {

            this.fm = fm;
            //titles = context.Resources.GetTextArray(Resource.Array.sections);

        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position) =>
                            new Java.Lang.String(titles[position]);

        public override Fragment GetItem(int position)
        {
            switch (position)
            {

                case 0: return ProductsFragment.NewInstance();

                case 1: return OffersFragment.NewInstance();

                case 2: return BusinessFragment.NewInstance();

            }

            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag) => PositionNone;

    }

}