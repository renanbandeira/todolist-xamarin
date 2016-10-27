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

    class ListAdapter : BaseAdapter<ToDoItemViewModel>
    {

        private ReactiveList<ToDoItemViewModel> mItems;
        private Action<ToDoItem> OnChange;
        private List<ToDoItemViewModel> FilteredItems;
        private Context mContext;
        private FilterType Filter;

        public ListAdapter(Context context, Action<ToDoItemViewModel> OnAdd, Action<ToDoItem> OnChange, Action<ToDoItemViewModel> OnRemove, List<ToDoItem> items = null)
        {
            mContext = context;
            mItems = new ReactiveList<ToDoItemViewModel>();
            if (items != null)
            {
                items.ForEach(item => mItems.Add(new ToDoItemViewModel(item, OnChange)));
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

        public override ToDoItemViewModel this[int position]
        {
            get
            {
                return FilteredItems[position];
            }
        }

        private List<ToDoItemViewModel> getFilteredList()
        {
            if (mItems == null)
            {
                throw new NullReferenceException();
            }
            if (Filter == FilterType.Active)
            {
                return mItems.ToList().FindAll(x => x.Item.IsActive);
            }
            if (Filter == FilterType.Inactive)
            {
                return mItems.ToList().FindAll(x => !x.Item.IsActive);
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
            var removeItems = mItems.ToList().FindAll(item => !item.Item.IsActive);
            mItems.RemoveAll(removeItems);
            FilteredItems = getFilteredList();
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return this[position].Item.ID;
        }

        private void ToggleItemActive(ToDoItemViewModel Item)
        {
            Item.Item.IsActive = !Item.Item.IsActive;
            FilteredItems = getFilteredList();
            NotifyDataSetChanged();
        }

        private void Delete(ToDoItemViewModel item)
        {
            mItems.Remove(item);
            FilteredItems.Remove(item);
            NotifyDataSetChanged();
        }

        public int getActiveItemsCount()
        {
            return mItems.ToList().FindAll(item => item.Item.IsActive).Count();
        }

        public void AddItem(ToDoItem item)
        {
            mItems.Add(new ToDoItemViewModel(item, OnChange));
            FilteredItems = getFilteredList();
            NotifyDataSetChanged();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.ListItem, parent, false);
            ToDoItemViewModel item = this[position];
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
                    item.Item.Content = itemContentEditText.Text;
                    InputMethodManager inputManager = (InputMethodManager)mContext.GetSystemService(Context.InputMethodService);
                    inputManager.HideSoftInputFromWindow(itemContentEditText.WindowToken, 0);
                    itemContentEditText.ClearFocus();
                }
            };
            itemContentEditText.SetSingleLine(true);
            itemContentEditText.Text = item.Item.Content;
            itemContentEditText.PaintFlags = item.Item.IsActive ?
                Android.Graphics.PaintFlags.LinearText : Android.Graphics.PaintFlags.StrikeThruText;
            return convertView;
        }
    }


}