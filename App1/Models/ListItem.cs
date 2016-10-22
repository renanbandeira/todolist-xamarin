using SQLite;

namespace RenanBandeira.Models
{
    class ListItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Content { get; set; }

        public bool IsActive { get; set; }

        public ListItem()
        {
            IsActive = true;
        }

        public override string ToString()
        {
            return string.Format("[LitsItem: ID={0}, Content={1}, IsActive={2}]", ID, Content, IsActive);
        }

        public ListItem(string Content, int ID = 0, bool IsActive = true)
        {
            this.ID = ID;
            this.Content = Content;
            this.IsActive = IsActive;
        }

        public override bool Equals(object item)
        {
           return (item is ListItem && ((ListItem)item).ID == ID);
        }

    }
}