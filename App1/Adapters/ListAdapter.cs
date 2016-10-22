using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using RenanBandeira.ViewModels;
using RenanBandeira.Models;
using Android.Util;
using Android.Runtime;
using Android.Views.InputMethods;

namespace RenanBandeira.Adapters
{
    enum FilterType { All, Active, Inactive };

    class ListAdapter : BaseAdapter<ListItem>
    {

        private List<ListItem> mItems;
        private Context mContext;
        private Action<ListItem> EditItem;
        private Action<ListItem> DeleteItem;
        private Action<ListItem> ToggleActive;

        public ListAdapter(Context context, Action<ListItem> Edit, Action<ListItem> Toggle,
             Action<ListItem> Delete, List<ListItem> items = null)
        {
            mContext = context;
            mItems = items == null ? new List<ListItem>() : items;
            EditItem = Edit;
            DeleteItem = Delete;
            ToggleActive = Toggle;
        }

        public override ListItem this[int position]
        {
            get
            {
                if (mItems == null)
                {
                    throw new NullReferenceException();
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
                return mItems.Count();
            }
        }

        public void SetItems(List<ListItem> Items)
        {
            mItems = Items;
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        private ListItem ToggleItemActive(int position)
        {
            ListItem Item = this[position];
            Item.IsActive = !Item.IsActive;
            NotifyDataSetChanged();
            return Item;
        }

        private void Delete(ListItem Item)
        {
            mItems.Remove(Item);
            NotifyDataSetChanged();
        }

        public void AddItem(ListItem item)
        {
            mItems.Add(item);
            NotifyDataSetChanged();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.ListItem, parent, false);
            ListItem item = this[position];
            ListItemViewModel ViewModel = new ListItemViewModel(item, EditItem, ToggleActive, DeleteItem);
            if (item == null) return convertView;

            EditText itemContentEditText = (EditText)convertView.FindViewById(Resource.Id.item_content_edittext);
            View deleteItemButton = convertView.FindViewById(Resource.Id.item_delete_button);
            CheckBox activeItemCheckBox = (CheckBox)convertView.FindViewById(Resource.Id.active_item_checkbox);

            activeItemCheckBox.Checked = !item.IsActive;
            activeItemCheckBox.Click += delegate { ViewModel.OnToggleActive(ToggleItemActive(position)); };
            deleteItemButton.Click += delegate { ViewModel.OnDelete(item); Delete(item); };
            itemContentEditText.SetImeActionLabel(mContext.GetString(Resource.String.button_edit), ImeAction.Done);
            itemContentEditText.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Done)
                {
                    item.Content = itemContentEditText.Text;
                    ViewModel.OnEdit(item);
                    InputMethodManager inputManager = (InputMethodManager)mContext.GetSystemService(Context.InputMethodService);
                    inputManager.HideSoftInputFromWindow(itemContentEditText.WindowToken, 0);
                }
            };
            itemContentEditText.SetSingleLine(true);
            itemContentEditText.Text = item.Content;
            itemContentEditText.PaintFlags = item.IsActive ?
                Android.Graphics.PaintFlags.LinearText : Android.Graphics.PaintFlags.StrikeThruText;
            return convertView;
        }
    }


}