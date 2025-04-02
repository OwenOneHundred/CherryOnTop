using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


namespace GameSaves
{
    public class SaveData
    {

        protected static bool TryReadDataEntryAs<T> (string data, out T dataEntry) where T:DataEntry
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

        public bool TryGetDataEntry<T>(string dataName, out T typedValue) where T:DataEntry
        {
            if (data.TryGetValue(dataName, out DataEntry untypedValue))
            {
                typedValue = untypedValue as T;
                return typedValue != null;
            } else if (rawData.TryGetValue(dataName, out string raw) && TryReadDataEntryAs(raw, out typedValue))
            {
                data.Add(dataName, typedValue);
                return true;
            }
            typedValue = null;
            return false;
        }

        public T GetDataEntry<T>(string dataName) where T:DataEntry
        {
            if (TryGetDataEntry<T>(dataName, out T typedValue))
            {
                return typedValue;
            }
            return null;
        }

        public DataEntry SetDataEntry<T>(T dataEntry, bool overwrite = true) where T : DataEntry
        {
            if (dataEntry == null) return null;
            return SetDataEntry(dataEntry.DataName, dataEntry, overwrite);
        }

        public DataEntry SetDataEntry<T>(string dataName, T dataEntry, bool overwrite = true) where T:DataEntry
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

        public string saveFileName { get; protected set; }
        public string saveLevelName { get;protected set; }
        public SaveData(string saveFile, bool load = false) : this(saveFile, null, load)
        {

        }
        public SaveData(string saveFile, string saveLevelName, bool load = false)
        {
            saveFileName = saveFile;
            this.saveLevelName = saveLevelName;
            rawData = load ? SaveDataFileUtility.ReadJson(saveFile, saveLevelName) : new Dictionary<string, string>();
            data = new Dictionary<string, DataEntry>();
        }

        Dictionary<string, string> rawData;
        Dictionary<string, DataEntry> data;

        public ICollection<KeyValuePair<string, string>> ReadRawData()
        {
            return rawData.AsReadOnlyCollection();
        }
        public ICollection<KeyValuePair<string, DataEntry>> ReadData()
        {
            return data.AsReadOnlyCollection();
        }
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
        [SerializeField] public Vector3 eulers;
        public DEPosition(string dataName, Vector3 positionData, Vector3 eulers) : base(dataName)
        {
            this.positionData = positionData;
            this.eulers = eulers;
        }
    }

    [System.Serializable]
    public class DETowerPlaced : DataEntry
    {
        [SerializeField] public int towerIndex;
        [SerializeField] public DEPosition pos;
        [SerializeField] public string towerID;
        public DETowerPlaced(string dataName, int towerIndex, DEPosition pos, string towerID) : base(dataName)
        {
            this.towerIndex = towerIndex;
            this.pos = pos;
            this.towerID = towerID;
        }
    }

    [System.Serializable]
    public class DEAllTowers : DataEntry
    {
        [SerializeField] public List<DETowerPlaced> towers;
        public DEAllTowers(string dataName, List<DETowerPlaced> towers) : base(dataName)
        {
            this.towers = towers;
        }
    }

    [System.Serializable]
    public class DEItemInventory : DataEntry
    {
        [SerializeField] public int itemIndex;
        [SerializeField] public string itemID;
        public DEItemInventory(string dataName, int itemIndex, string itemID) : base(dataName)
        {
            this.itemIndex = itemIndex;
            this.itemID = itemID;
        }
    }

    [System.Serializable]
    public class DEAllItemsInventory : DataEntry
    {
        [SerializeField] public List<DEItemInventory> items;
        public DEAllItemsInventory(string dataName, List<DEItemInventory> items) : base(dataName)
        {
            this.items = items;
        }
    }

    [System.Serializable]
    public class DEIntEntry : DataEntry
    {
        [SerializeField] public int value;
        public DEIntEntry(string dataName, int value) : base(dataName)
        {
            this.value = value;
        }
    }

    [System.Serializable]
    public class DEItemIDProvider : DataEntry
    {
        [SerializeField] public int nextValue;
        public DEItemIDProvider(string dataName) : base(dataName)
        {
            nextValue = 0;
        }

        public int GetNextID()
        {
            return nextValue++;
        }
    }

}