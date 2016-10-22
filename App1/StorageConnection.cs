using System.Collections.Generic;
using SQLite;
using System;

namespace App1
{
    class StorageConnection
    {
        private string Path;

        public StorageConnection(string path)
        {
            Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Path += path;
            createDatabase();
        }
        private string createDatabase()
        {
            try
            {
                var connection = new SQLiteConnection(Path);
                connection.CreateTable<ListItem>();
                connection.Close();
                return "Database created";

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public void insertData(ListItem data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                db.Insert(data);
                db.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void updateData(ListItem data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                db.Update(data);
                db.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void removeData(ListItem data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                db.Delete(data);
                db.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<ListItem> GetData()
        {
            try
            {
                var db = new SQLiteConnection(Path);
                List<ListItem> Items = db.Query<ListItem>("SELECT * FROM ListItem");
                db.Close();
                return Items;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}