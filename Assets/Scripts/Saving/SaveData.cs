using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace GameSaves
{
    public class SaveData
    {
        public static string _saveFolderName = "Saves";

        public static void AssertSavesFolderExists()
        {
            string savesPath = Application.persistentDataPath + _saveFolderName;
            if (!Directory.Exists(savesPath))
            {
                Directory.CreateDirectory(savesPath);
                Debug.Log("Saves folder created at path: " + savesPath);
            }
        }

        public static string _defaultSaveFile = "savefile";
        public static string _saveFileExtension = "cot";

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

        public static SaveData CreateData()
        {
            return null;
        }

        public static SaveData LoadData()
        {
            return null;
        }

        protected SaveData(string saveFile, bool load = false)
        {

        }

        Dictionary<string, string> rawData;
        Dictionary<string, DataEntry> data;


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
    public class DETransform : DataEntry
    {
        [SerializeField] public Transform transformData;
        public DETransform(string dataName, Transform transform) : base(dataName)
        {
            transformData = transform;
        }
    }

}