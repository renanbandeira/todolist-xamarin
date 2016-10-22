using Android.App;
using Android.OS;
using Android.Runtime;
using System.Collections.Generic;
using RenanBandeira.Models;
using RenanBandeira.Adapters;
using RenanBandeira.Storage;
using ReactiveUI;
using Android.Widget;
using Android.Views;
using Android.Views.InputMethods;
using System;
using RenanBandeira.ViewModels;

namespace RenanBandeira.Activities
{
    [Activity(Label = "TODO List", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ReactiveActivity, TextView.IOnEditorActionListener
    {
        ListAdapter MyAdapter;
        ListView TodoList;
        StorageConnection Storage;
        List<ListItem> Items;
        FilterType Filter;

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Done)
            {
                ListItem item = new ListItem();
                item.Content = v.Text;
                Storage.insertData(item);
                MyAdapter.AddItem(item);
                v.Text = "";
                return true;
            }
            return false;
        }

        private void EditItem(ListItem Item)
        {
            Items.Find(x => x.ID == Item.ID).Content = Item.Content;
            Storage.updateData(Item);
        }

        private void Toggleitem(ListItem Item)
        {
            Items.Find(x => x.ID == Item.ID).IsActive = Item.IsActive;
            Storage.updateData(Item);
            MyAdapter.SetItems(getFilteredItems());
        }

        private void DeleteItem(ListItem Item)
        {
            Items.Remove(Item);
            Storage.removeData(Item);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Storage = new StorageConnection("/todolist.db");

            TodoList = FindViewById<ListView>(Resource.Id.todo_listview);

            EditText ItemEditText = FindViewById<EditText>(Resource.Id.todo_item_edittext);
            ItemEditText.SetSingleLine(true);
            ItemEditText.SetImeActionLabel(GetString(Resource.String.button_add), Android.Views.InputMethods.ImeAction.Done);
            ItemEditText.SetOnEditorActionListener(this);

            FindViewById(Resource.Id.filter_all_button).Click += clearFilter;
            FindViewById(Resource.Id.filter_active_button).Click += FilterActiveItems;
            FindViewById(Resource.Id.filter_inactive_button).Click += FilterInactiveItems;

            Items = Storage.GetData();
            MyAdapter = new ListAdapter(this, this.EditItem, this.Toggleitem, this.DeleteItem, Items);
            TodoList.Adapter = MyAdapter;
        }

        private void clearFilter(object sender, EventArgs e)
        {
            changeButtonState(Resource.Id.filter_all_button);
            Filter = FilterType.All;
            MyAdapter.SetItems(getFilteredItems());
        }

        private void FilterActiveItems(object sender, EventArgs e)
        {
            changeButtonState(Resource.Id.filter_active_button);
            Filter = FilterType.Active;
            MyAdapter.SetItems(getFilteredItems());
        }

        private void FilterInactiveItems(object sender, EventArgs e)
        {
            changeButtonState(Resource.Id.filter_inactive_button);
            Filter = FilterType.Inactive;
            MyAdapter.SetItems(getFilteredItems());
        }

        private void changeButtonState(int activatedFilterButtonId)
        {
            FindViewById(Resource.Id.filter_all_button).Enabled = !(activatedFilterButtonId == Resource.Id.filter_all_button);
            FindViewById(Resource.Id.filter_active_button).Enabled = !(activatedFilterButtonId == Resource.Id.filter_active_button);
            FindViewById(Resource.Id.filter_inactive_button).Enabled = !(activatedFilterButtonId == Resource.Id.filter_inactive_button);
        }

        private List<ListItem> getFilteredItems()
        {
            if (Filter == FilterType.Active)
            {
                return Items.FindAll(item => item.IsActive);
            }
            if (Filter == FilterType.Inactive)
            {
                return Items.FindAll(item => !item.IsActive);
            }
            return Items;
        }
    }
}

