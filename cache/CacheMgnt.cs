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
        public bool Enabled { get; set; }
        public string FileName { get; set; }
        public int LiveTimeSec { get; set; }

        public CacheProperty()
        {
            Name = String.Empty;
            FileName = String.Empty;
            Enabled = true;
            LiveTimeSec = 300;
        }

        public CacheProperty(string name, int time = 300)
        {
            Name = name;
            FileName = Guid.NewGuid().ToString() + ".cache";
            Enabled = true;
            LiveTimeSec = time;
        }

        public string ConvertToMinutes()
        {
            if (this.LiveTimeSec >= 60)
            {
                var minutes = (this.LiveTimeSec / 60);
                var secundes = (this.LiveTimeSec % 60);

                return string.Format("{0}.{1} minutes", minutes, secundes);
            }
            else
                return string.Format("{0} sec", this.LiveTimeSec);
        }
    }

    public class DataCache
    {
        public List<CacheProperty> Items { get; set; }

        public DataCache()
        {
            Items = new List<CacheProperty>();
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

        public bool Clear()
        {
            var path = GetFullName();
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                return true;
            }
            catch { throw; }

            return false;

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
                Items = data;

                return true;

            }
            catch (Exception error)
            {
                throw;
                return false;
            }
        }

        public bool SaveConfig()
        {
            try
            {
                var path = GetFullName() + Path.DirectorySeparatorChar + "cache.db";
                var data = JsonSerializer.Serialize(this.Items);
                using (StreamWriter outputFile = new StreamWriter(path, false))
                {
                    outputFile.WriteLine(data);
                }
                return true;
            }
            catch (Exception error)
            {
                throw;
                return false;
            }
        }



        public CacheProperty Find(string name)
        {
            try
            {
                foreach (var data in Items)
                {
                    if (data.Name == name) return data;
                }
            }
            catch (Exception)
            {
                throw;
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
            if (item == null) return true;
            if (!item.Enabled) return true;

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
                throw;
                return true;
            }
        }

        public bool RemoveCacheFile(string name)
        {
            var item = Find(name);
            if (item == null) return false;

            var path = GetFullName() + Path.DirectorySeparatorChar + item.FileName;
            try
            {
                File.Delete(path);
                if (this.Items.Remove(item))
                {
                    SaveConfig();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
                return false;
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
                throw;
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
                throw;
            }
        }

        public void SaveCacheValue(CacheProperty item, string value)
        {
            if (item == null) return;

            try
            {
                Items.Add(item);
                SaveConfig();
                UpdateCacheValue(item, value);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

