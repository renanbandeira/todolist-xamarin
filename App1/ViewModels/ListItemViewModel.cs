using System;
using ReactiveUI;
using RenanBandeira.Models;

namespace RenanBandeira.ViewModels
{
    class ListItemViewModel : ReactiveObject
    {
        private string _content;
        public string Content
        {
            get { return _content; }
            set { this.RaiseAndSetIfChanged(ref _content, value); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { this.RaiseAndSetIfChanged(ref _isActive, value); }
        }

        public ListItem Item
        {
            get { return new ListItem(Content, ID, IsActive); }
        }

        readonly int ID;

        public ListItemViewModel(ListItem Item, Action<ListItem> Update)
        {
            this.Content = Item.Content;
            this.IsActive = Item.IsActive;
            ID = Item.ID;
            this.WhenAnyValue(i => i.IsActive, i => i.Content).Subscribe(_ => Update(this.Item));
        }
    }
}