using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using RenanBandeira.ViewModels;
using RenanBandeira.Models;
using Android.Views.InputMethods;
using ReactiveUI;

namespace RenanBandeira.Adapters
{
    enum FilterType { All, Active, Inactive };

    class ListAdapter : BaseAdapter<ListItemViewModel>
    {

        private ReactiveList<ListItemViewModel> mItems;
        private Action<ListItem> OnChange;
        private List<ListItemViewModel> FilteredItems;
        private Context mContext;
        private FilterType Filter;

        public ListAdapter(Context context, Action<ListItemViewModel> OnAdd, Action<ListItem> OnChange, Action<ListItemViewModel> OnRemove, List<ListItem> items = null)
        {
            mContext = context;
            mItems = new ReactiveList<ListItemViewModel>();
            if (items != null)
            {
                items.ForEach(item => mItems.Add(new ListItemViewModel(item, OnChange)));
            }
            this.OnChange = OnChange;
            FilteredItems = getFilteredList();
            mItems.ChangeTrackingEnabled = true;
            mItems.ItemsAdded.Subscribe(OnAdd);
            mItems.ItemsRemoved.Subscribe(OnRemove);
            Filter = FilterType.All;
        }

        public void setFilter(FilterType Filter)
        {
            this.Filter = Filter;
            FilteredItems = getFilteredList();
            NotifyDataSetChanged();
        }

        public override ListItemViewModel this[int position]
        {
            get
            {
                return FilteredItems[position];
            }
        }

        private List<ListItemViewModel> getFilteredList()
        {
            if (mItems == null)
            {
                throw new NullReferenceException();
            }
            if (Filter == FilterType.Active)
            {
                return mItems.ToList().FindAll(x => x.IsActive);
            }
            if (Filter == FilterType.Inactive)
            {
                return mItems.ToList().FindAll(x => !x.IsActive);
            }
            return mItems.ToList();
        }

        public override int Count
        {
            get
            {
                if (FilteredItems == null)
                {
                    return 0;
                }
                return FilteredItems.Count();
            }
        }

        public void ClearCompletedItems()
        {
            List<ListItemViewModel> removeItems = mItems.ToList().FindAll(item => !item.IsActive);
            mItems.RemoveAll(removeItems);
            FilteredItems = getFilteredList();
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        private void ToggleItemActive(ListItemViewModel Item)
        {
            Item.IsActive = !Item.IsActive;
            FilteredItems = getFilteredList();
            NotifyDataSetChanged();
        }

        private void Delete(ListItemViewModel item)
        {
            mItems.Remove(item);
            FilteredItems.Remove(item);
            NotifyDataSetChanged();
        }

        public int getActiveItemsCount()
        {
            return mItems.ToList().FindAll(item => item.Item.IsActive).Count();
        }

        public void AddItem(ListItem item)
        {
            mItems.Add(new ListItemViewModel(item, OnChange));
            FilteredItems = getFilteredList();
            NotifyDataSetChanged();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.ListItem, parent, false);
            ListItemViewModel item = this[position];
            if (item == null) return convertView;

            EditText itemContentEditText = (EditText)convertView.FindViewById(Resource.Id.item_content_edittext);
            View deleteItemButton = convertView.FindViewById(Resource.Id.item_delete_button);
            CheckBox activeItemCheckBox = (CheckBox)convertView.FindViewById(Resource.Id.active_item_checkbox);
            activeItemCheckBox.Checked = !item.Item.IsActive;
            activeItemCheckBox.Events().Click.Subscribe(_ => ToggleItemActive(item));
            deleteItemButton.Events().Click.Subscribe(_ => Delete(item));
            itemContentEditText.SetImeActionLabel(mContext.GetString(Resource.String.button_edit), ImeAction.Done);
            itemContentEditText.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Done)
                {
                    item.Content = itemContentEditText.Text;
                    InputMethodManager inputManager = (InputMethodManager)mContext.GetSystemService(Context.InputMethodService);
                    inputManager.HideSoftInputFromWindow(itemContentEditText.WindowToken, 0);
                    itemContentEditText.ClearFocus();
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