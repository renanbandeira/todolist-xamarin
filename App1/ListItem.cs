using SQLite;

namespace App1
{
    class ListItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Content { get; set; }

        public bool IsActive { get; set; }

        public ListItem(int ID, string Content, bool IsActive = true)
        {
            this.ID = ID;
            this.Content = Content;
            this.IsActive = IsActive;
        }

        public ListItem()
        {
            IsActive = true;
        }

        public override string ToString()
        {
            return string.Format("[LitsItem: ID={0}, Content={1}, IsActive={2}]", ID, Content, IsActive);
        }

    }
}