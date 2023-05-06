using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace CacheManagment
{
    public class CacheProperty
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public int LiveTimeSec { get; set; }

        public CacheProperty()
        {
            Name = String.Empty;
            FileName = String.Empty;
            LiveTimeSec = 300;
        }

        public CacheProperty(string name, int time = 300)
        {
            Name = name;
            FileName = Guid.NewGuid().ToString() + ".cache";
            LiveTimeSec = time;
        }
    }

    public class DataCache
    {
        public List<CacheProperty> Item { get; set; }

        public DataCache()
        {
            Item = new List<CacheProperty>();
        }


        private string GetFullName()
        {
            var current = Directory.GetCurrentDirectory();
            if (current.Contains("modules"))
                current = current + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".cache";
            else
                current = current + Path.DirectorySeparatorChar + ".cache";

            return current;
        }

        public bool LoadConfig()
        {
            try
            {
                var path = GetFullName() + Path.DirectorySeparatorChar + "cache.db";
                if (!File.Exists(path)) return false;
                string text = File.ReadAllText(path);

                var data = JsonSerializer.Deserialize<List<CacheProperty>>(text);
                if (data == null) return false;
                Item = data;

                return true;

            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
                return false;
            }
        }

        public bool SaveConfig()
        {
            try
            {
                var path = GetFullName() + Path.DirectorySeparatorChar + "cache.db";
                var data = JsonSerializer.Serialize(this.Item);
                using (StreamWriter outputFile = new StreamWriter(path, false))
                {
                    outputFile.WriteLine(data);
                }
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
                return false;
            }
        }



        public CacheProperty Find(string name)
        {
            try
            {
                foreach (var data in Item)
                {
                    if (data.Name == name) return data;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        public bool IsRenewCache(string name)
        {
            var item = Find(name);
            if (item == null) return true;
            return IsRenewCache(item);
        }

        public bool IsRenewCache(CacheProperty item)
        {
            if (item == null) return false;

            try
            {
                var path = GetFullName() + Path.DirectorySeparatorChar + item.FileName;
                if (!File.Exists(path)) return true;
                FileInfo info = new FileInfo(path);
                var currenttime = DateTime.Now;
                Debug.WriteLine("Curr Time: " + currenttime);

                var lastmodified = info.LastWriteTime.AddSeconds(item.LiveTimeSec);
                Debug.WriteLine("File Time: " + lastmodified);


                if (currenttime < lastmodified) return false;

                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }


        public string GetCacheValue(CacheProperty item)
        {
            if (item == null) return null;

            try
            {
                var path = GetFullName() + Path.DirectorySeparatorChar + item.FileName;
                if (!File.Exists(path)) return null;
                return File.ReadAllText(path);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UpdateCacheValue(CacheProperty item, string value)
        {
            if (item == null) return;

            try
            {
                var path = GetFullName() + Path.DirectorySeparatorChar + item.FileName;
                using (StreamWriter outputFile = new StreamWriter(path, false))
                {
                    outputFile.WriteLine(value);
                }
            }
            catch (Exception)
            {
            }
        }

        public void SaveCacheValue(CacheProperty item, string value)
        {
            if (item == null) return;

            try
            {
                Item.Add(item);
                SaveConfig();
                UpdateCacheValue(item, value);
            }
            catch (Exception)
            {
            }
        }
    }
}

