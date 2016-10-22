using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace App1
{
    enum FilterType { All, Active, Inactive };

    class ListAdapter : BaseAdapter<ListItem>
    {
        
        private List<ListItem> mItems;
        private Context mContext;
        private FilterType mFilter;


        public ListAdapter(Context context, List<ListItem> items = null)
        {
            mContext = context;
            mItems = items == null ? new List<ListItem>() : items;
            mFilter = FilterType.All;
        }

        public override ListItem this[int position]
        {
            get
            {
                if (mItems == null || mItems.Count <= position)
                {
                    throw new IndexOutOfRangeException();
                }

                if (mFilter.Equals(FilterType.Active))
                {
                    //TODO add filtering
                }
                if(mFilter.Equals(FilterType.Inactive)) {
                    //TODO add Filtering
                }
                return mItems[position];
            }
        }

        public override int Count
        {
            get
            {
                if (mItems == null)
                {
                    return 0;
                }
                if (mFilter.Equals(FilterType.Active))
                {
                    //TODO add filtering
                }
                if (mFilter.Equals(FilterType.Inactive)) {
                    //TODO add Filtering
                }
                return mItems.Count();
            }
        }

        public void setFilter(FilterType Filter)
        {
            mFilter = Filter;
            NotifyDataSetChanged();
        }

        public void ToggleItemActive(int position)
        {
            ListItem Item = this[position];
            Item.IsActive = !Item.IsActive;
            mItems[position] = Item;
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public void Remove(int position)
        {
            mItems.RemoveAt(position);
            NotifyDataSetChanged();
        }

        public void AddItem(ListItem item)
        {
            mItems.Add(item);
            NotifyDataSetChanged();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.ListItem, parent, false);
            }
            ListItem item = this[position];
            if (item == null) return convertView;

            TextView itemTextView = (TextView)convertView.FindViewById(Resource.Id.item_content_textview);
            CheckBox activeItemCheckBox = (CheckBox)convertView.FindViewById(Resource.Id.active_item_checkbox);

            activeItemCheckBox.Checked = !item.IsActive;
            itemTextView.Text = item.Content;
            itemTextView.PaintFlags = item.IsActive ? 
                Android.Graphics.PaintFlags.LinearText : Android.Graphics.PaintFlags.StrikeThruText;

            return convertView;
        }
    }
}