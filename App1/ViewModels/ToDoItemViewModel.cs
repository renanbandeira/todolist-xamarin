using System;
using ReactiveUI;
using RenanBandeira.Models;

namespace RenanBandeira.ViewModels
{
    class ToDoItemViewModel : ReactiveObject
    {
        private ToDoItem _item;
        public ToDoItem Item
        {
            get { return _item; }
            set { this.RaiseAndSetIfChanged(ref _item, value);  }
        }

        public ToDoItemViewModel(ToDoItem Item, Action<ToDoItem> Update)
        {
            this.Item = Item;
            Item.Changed.Subscribe(_ => Update(this.Item));
            //this.WhenAnyValue(i => i.IsActive, i => i.Content).Subscribe(_ => Update(this.Item));
        }
    }
}