using ReactiveUI;
using SQLite;

namespace RenanBandeira.Models
{
    class ToDoItem : ReactiveObject
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        string _content;
        public string Content { get { return _content; } set { this.RaiseAndSetIfChanged(ref _content, value); } }

        bool _isActive;
        public bool IsActive { get { return _isActive; } set { this.RaiseAndSetIfChanged(ref _isActive, value); } }

        public ToDoItem()
        {
            IsActive = true;
        }

        public override string ToString()
        {
            return string.Format("[LitsItem: ID={0}, Content={1}, IsActive={2}]", ID, Content, IsActive);
        }

        public ToDoItem(string Content, int ID = 0, bool IsActive = true)
        {
            this.ID = ID;
            this.Content = Content;
            this.IsActive = IsActive;
        }

        public override bool Equals(object item)
        {
            return (item is ToDoItem && ((ToDoItem)item).ID == ID);
        }

        public override int GetHashCode()
        {
            return ID;
        }

    }
}