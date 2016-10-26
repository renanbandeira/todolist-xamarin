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
using System.Reactive.Linq;
using RenanBandeira.ViewModels;

namespace RenanBandeira.Activities
{
    [Activity(Label = "TODO List", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ReactiveActivity, TextView.IOnEditorActionListener
    {
        ListAdapter MyAdapter;
        ListView TodoList;
        TextView ItemsCount;
        StorageConnection Storage;
        int countActiveItems;

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Done)
            {
                ToDoItem item = new ToDoItem();
                item.Content = v.Text;
                MyAdapter.AddItem(item);
                v.Text = "";
                return true;
            }
            return false;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Storage = new StorageConnection("/todolist.db");

            TodoList = FindViewById<ListView>(Resource.Id.todo_listview);

            EditText ItemEditText = FindViewById<EditText>(Resource.Id.todo_item_edittext);
            ItemEditText.SetSingleLine(true);
            ItemEditText.SetImeActionLabel(GetString(Resource.String.button_add), ImeAction.Done);
            ItemEditText.SetOnEditorActionListener(this);

            FindViewById(Resource.Id.filter_all_button).Click += clearFilter;
            FindViewById(Resource.Id.filter_active_button).Click += FilterActiveItems;
            FindViewById(Resource.Id.filter_inactive_button).Click += FilterInactiveItems;
            FindViewById(Resource.Id.clear_completed_button).Click += ClearCompletedItems;

            MyAdapter = new ListAdapter(this, OnAdd, OnChange, OnRemove, Storage.GetData());
            TodoList.Adapter = MyAdapter;
            ItemsCount = FindViewById<TextView>(Resource.Id.items_count_textview);
            UpdateToDoCount();
        }

        private void OnChange(ToDoItem item)
        {
            Storage.updateData(item);
            UpdateToDoCount();
        }

        private void OnAdd(ToDoItemViewModel item)
        {
            Storage.insertData(item.Item);
            UpdateToDoCount();
        }

        private void OnRemove(ToDoItemViewModel item)
        {
            Storage.removeData(item.Item);
            UpdateToDoCount();
        }

        private void ClearCompletedItems(object sender, EventArgs e)
        {
            MyAdapter.ClearCompletedItems();
        }

        private void UpdateToDoCount()
        {
            if (MyAdapter == null) return;
            countActiveItems = MyAdapter.getActiveItemsCount();
            ItemsCount.Text = GetString(Resource.String.info_todo_count, countActiveItems);
        }

        private void clearFilter(object sender, EventArgs e)
        {
            changeButtonState(Resource.Id.filter_all_button);
            MyAdapter.setFilter(FilterType.All);
        }

        private void FilterActiveItems(object sender, EventArgs e)
        {
            changeButtonState(Resource.Id.filter_active_button);
            MyAdapter.setFilter(FilterType.Active);
        }

        private void FilterInactiveItems(object sender, EventArgs e)
        {
            changeButtonState(Resource.Id.filter_inactive_button);
            MyAdapter.setFilter(FilterType.Inactive);
        }

        private void changeButtonState(int activatedFilterButtonId)
        {
            FindViewById(Resource.Id.filter_all_button).Enabled = !(activatedFilterButtonId == Resource.Id.filter_all_button);
            FindViewById(Resource.Id.filter_active_button).Enabled = !(activatedFilterButtonId == Resource.Id.filter_active_button);
            FindViewById(Resource.Id.filter_inactive_button).Enabled = !(activatedFilterButtonId == Resource.Id.filter_inactive_button);
        }
    }
}

