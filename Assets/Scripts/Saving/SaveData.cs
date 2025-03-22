using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace GameSaves
{
    public class SaveData
    {
        public static string _saveFolderName = "Saves";

        public static string AssertSavesFolderExists()
        {
            string savesPath = Application.persistentDataPath + Path.DirectorySeparatorChar + _saveFolderName;
            if (!Directory.Exists(savesPath))
            {
                Directory.CreateDirectory(savesPath);
                Debug.Log("Saves folder created at path: " + savesPath);
            }
            return savesPath;
        }

        public static string GetSaveFilePath(string filename)
        {
            return AssertSavesFolderExists() + Path.DirectorySeparatorChar + filename + _saveFileExtension;
        }

        public static string _defaultSaveFile = "savefile";
        public static string _saveFileExtension = ".cot";

        public static Dictionary<string, string> ReadJson(string filename)
        {
            Dictionary<string, string> valueDict = new Dictionary<string, string>();
            string filepath = GetSaveFilePath(filename);
            if (!File.Exists(filepath))
            {
                Debug.LogError("Filepath does not exists! Using new SaveData. Given filepath: " + filepath);
                return valueDict;
            }
            DataWrapper wrapper = JsonUtility.FromJson<DataWrapper>(File.ReadAllText(filepath));
            foreach (StringDataEntry s in wrapper.entries)
            {
                valueDict.Add(s.dataName, s.dataValue);
            }

            Debug.Log("Finished reading save data from filepath: " + filepath);

            return valueDict;
        }

        public static void WriteJson(SaveData data, string filename)
        {
            //Dictionary<string, string> valueDict = new Dictionary<string, string>();
            DataWrapper wrapper = new DataWrapper();
            foreach (string key in data.data.Keys)
            {
                DataEntry entry = data.data[key];
                if (entry == null) continue;
                string val = WriteDataFrom(entry);
                StringDataEntry s = new StringDataEntry();
                s.dataName = key;
                s.dataValue = val;
                wrapper.entries.Add(s);
            }

            string filepath = GetSaveFilePath(filename);
            string json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(filepath, json);
        }

        public static bool TryReadDataAs<T> (string data, out T dataEntry) where T:DataEntry
        {
            try
            {
                dataEntry = JsonUtility.FromJson<T>(data);
                return true;
            } catch (Exception e)
            {
                dataEntry = default;
                return false;
            }
        }

        public static string WriteDataFrom<T> (T dataEntry) where T:DataEntry
        {
            string json = JsonUtility.ToJson(dataEntry);
            return json;
        }

        public bool TryGetData<T>(string dataName, out T typedValue) where T:DataEntry
        {
            if (data.TryGetValue(dataName, out DataEntry untypedValue))
            {
                typedValue = untypedValue as T;
                return typedValue != null;
            } else if (rawData.TryGetValue(dataName, out string raw) && TryReadDataAs(raw, out typedValue))
            {
                data.Add(dataName, typedValue);
                return true;
            }
            typedValue = null;
            return false;
        }

        public T GetData<T>(string dataName) where T:DataEntry
        {
            if (TryGetData<T>(dataName, out T typedValue))
            {
                return typedValue;
            }
            return null;
        }

        public DataEntry SetData<T>(T dataEntry, bool overwrite = false) where T : DataEntry
        {
            if (dataEntry == null) return null;
            return SetData(dataEntry.DataName, dataEntry, overwrite);
        }

        public DataEntry SetData<T>(string dataName, T dataEntry, bool overwrite = false) where T:DataEntry
        {
            if (dataEntry == null) return null;
            if (data.TryGetValue(dataEntry.DataName, out DataEntry previousEntry))
            {
                if (overwrite)
                {
                    data.Remove(dataName);
                    data.Add(dataName, dataEntry);
                    return previousEntry;
                } else
                {
                    return null;
                }
            }
            data.Add(dataName, dataEntry);
            return null;
        }

        public static SaveData CreateData(string datapathName)
        {
            SaveData data = new SaveData(datapathName);
            return data;
        }

        public static SaveData LoadData(string datapathName)
        {
            SaveData data = new SaveData(datapathName, true);
            return data;
        }

        public static void WriteData(SaveData data)
        {
            WriteData(data, data.saveFile);
        }

        public static void WriteData(SaveData data, string datapathName)
        {
            WriteJson(data, datapathName);
        }

        protected string saveFile;
        protected SaveData(string saveFile, bool load = false)
        {
            this.saveFile = saveFile;
            rawData = load ? ReadJson(saveFile) : new Dictionary<string, string>();
            data = new Dictionary<string, DataEntry>();
        }

        Dictionary<string, string> rawData;
        Dictionary<string, DataEntry> data;


    }

    [System.Serializable]
    public class DataWrapper
    {
        [SerializeField] public List<StringDataEntry> entries = new List<StringDataEntry>();
    }

    [System.Serializable]
    public class StringDataEntry
    {
        [SerializeField] public string dataName = "";
        [SerializeField] public string dataValue = "";
    }

    [System.Serializable]
    public class DataEntry
    {
        [SerializeField] protected string _dataName = "Data Entry";
        public string DataName { get { return _dataName; } }
        public DataEntry(string dataName)
        {
            _dataName = dataName;
        }

        public bool TryAs<T>(out T value) where T:DataEntry {
            value = this as T;
            return value != null;
        }
    }

    [System.Serializable]
    public class DEPosition : DataEntry
    {
        [SerializeField] public Vector3 positionData;
        public DEPosition(string dataName, Vector3 positionData) : base(dataName)
        {
            this.positionData = positionData;
        }
    }

}