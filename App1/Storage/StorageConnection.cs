using System.Collections.Generic;
using SQLite;
using System;
using RenanBandeira.Models;

namespace RenanBandeira.Storage
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
        private void createDatabase()
        {
            try
            {
                var connection = new SQLiteConnection(Path);
                connection.CreateTable<ToDoItem>();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void insertData(ToDoItem data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                db.Insert(data);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void updateData(ToDoItem data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                db.Update(data);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void removeData(ToDoItem data)
        {
            try
            {
                var db = new SQLiteConnection(Path);
                db.Delete(data);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<ToDoItem> GetData()
        {
            try
            {
                var db = new SQLiteConnection(Path);
                return db.Query<ToDoItem>("SELECT * FROM ListItem");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}