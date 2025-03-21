using System.Collections.Generic;
using UnityEngine;


namespace GameSaves
{
    public class SaveData
    {
        Dictionary<string, SaveData> data;
    }

    public abstract class BaseDataEntry
    {
        
    }

    [System.Serializable]
    public abstract class DataEntry <T> 
    {
        [SerializeField] protected T _data;
        public T Data { get { return _data; } set { _data = value; } }
        [SerializeField] public string _entryName;
        public string EntryName { get { return _entryName; } }

        public DataEntry(string entryName, T data)
        {
            _entryName = entryName;
            _data = data;
        }

        public abstract string WriteData();
        public abstract void ReadData(string text);

    }

}