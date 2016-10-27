using System;
using ReactiveUI;
using RenanBandeira.Models;

namespace RenanBandeira.ViewModels
{
    class ToDoItemViewModel : ReactiveObject
    {
        ToDoItem _item;
        public ToDoItem Item
        {
            get { return _item; }
            set { this.RaiseAndSetIfChanged(ref _item, value);  }
        }

        public ToDoItemViewModel(ToDoItem Item, Action<ToDoItem> Update)
        {
            this.Item = Item;
            Item.Changed.Subscribe(_ => Update(this.Item));
        }
    }
}