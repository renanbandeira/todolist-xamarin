using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Runtime;
using Android.Views.InputMethods;
using System.Collections.Generic;
using Akavache;
using System.Reactive.Linq;

namespace App1
{
    [Activity(Label = "TODO List", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, Android.Widget.TextView.IOnEditorActionListener
    {
        ListAdapter MyAdapter;
        List<ListItem> mItems;
        ListView TodoList;
        StorageConnection Storage;

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

        public void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ListItem Item = MyAdapter[e.Position];
            Item.IsActive = !Item.IsActive;
            Storage.updateData(Item);
            MyAdapter.NotifyDataSetChanged();
        }

        public void OnItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            ListItem Item = MyAdapter[e.Position];
            Storage.removeData(Item);
            MyAdapter.Remove(e.Position);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Storage = new StorageConnection("/todolist.db");

            TodoList = FindViewById<ListView>(Resource.Id.todo_listview);
            TodoList.ItemClick += OnItemClick;
            TodoList.ItemLongClick += OnItemLongClick;

            EditText ItemEditText = FindViewById<EditText>(Resource.Id.todo_item_edittext);
            ItemEditText.SetSingleLine(true);
            ItemEditText.SetImeActionLabel(GetString(Resource.String.button_add), Android.Views.InputMethods.ImeAction.Done);
            ItemEditText.SetOnEditorActionListener(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            mItems = Storage.GetData();

            MyAdapter = new ListAdapter(this, mItems);
            TodoList.Adapter = MyAdapter;
        }
    }
}

