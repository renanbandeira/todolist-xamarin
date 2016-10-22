using System;
using ReactiveUI;
using RenanBandeira.Models;

namespace RenanBandeira.ViewModels
{
    class ListItemViewModel : ReactiveObject
    {
        public ListItem Item;

        public Action<ListItem> OnEdit { get; private set; }
        public Action<ListItem> OnToggleActive { get; private set; }
        public Action<ListItem> OnDelete { get; private set; }

        public ListItemViewModel(ListItem Item, Action<ListItem> OnEdit, 
            Action<ListItem> OnToggleActive, Action<ListItem> OnDelete)
        {
            this.OnEdit = OnEdit;
            this.OnToggleActive = OnToggleActive;
            this.OnDelete = OnDelete;
        }

    }
}