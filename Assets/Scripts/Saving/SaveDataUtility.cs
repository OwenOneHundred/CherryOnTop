using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace GameSaves {
    public class SaveDataUtility
    {
        public static string _defaultSaveFile = "savefile";
        public static bool _useEncryptions = true;

        public static SaveData CreateSaveData(string saveLevelName)
        {
            return CreateSaveData(_defaultSaveFile, saveLevelName);
        }

        public static SaveData CreateSaveData(string saveFileName, string saveLevelName)
        {
            SaveData data = new SaveData(saveFileName, saveLevelName);
            return data;
        }

        public static SaveData LoadSaveData(string saveFileName, string saveLevelName)
        {
            SaveData data = new SaveData(saveFileName, saveLevelName, true);
            return data;
        }

        public static void WriteSaveData(SaveData data)
        {
            WriteSaveData(data, data.saveFileName, data.saveLevelName);
        }

        public static void WriteSaveData(SaveData data, string fileName, string levelName)
        {
            SaveDataFileUtility.WriteJson(data.ReadData(), fileName, levelName, _useEncryptions);
        }

        public static string[] GetSaveFileNames(string levelName, out string levelPath)
        {
            return SaveDataFileUtility.GetSaveFileNames(levelName, out levelPath);
        }

        public static string GetSaveFileName(string levelName, out string saveFilePath, string defaultSaveFileName = "")
        {
            saveFilePath = SaveDataFileUtility.GetSaveFilePath(string.IsNullOrEmpty(defaultSaveFileName) ? _defaultSaveFile : defaultSaveFileName, levelName);
            string saveFileName = Path.GetFileName(saveFilePath);
            return saveFileName;
        }

        public static string GetSaveFileName(string levelName, string defaultSaveFileName = "")
        {
            return GetSaveFileName(levelName, out string saveFilePath, defaultSaveFileName);
        }

        public static bool GetSaveFileNameIfExists(string levelName, out string saveFilePath, out string saveFileName, string defaultSaveFileName = "")
        {
            saveFileName = GetSaveFileName(levelName, out saveFilePath, defaultSaveFileName);
            return !string.IsNullOrEmpty(saveFileName) && File.Exists(saveFilePath);
        }
    }

    public class SaveDataFileUtility
    {
        public static string _saveFolderName = "Saves";
        public static string _saveFileExtension = ".cot";

        public static string AssertSavesFolderExists(string levelName)
        {
            string savesPath = Application.persistentDataPath + Path.DirectorySeparatorChar + _saveFolderName;
            if (!Directory.Exists(savesPath))
            {
                Directory.CreateDirectory(savesPath);
                Debug.Log("Saves folder created at path: " + savesPath);
            }
            if (!string.IsNullOrEmpty(levelName))
            {
                string levelSavesPath = savesPath + Path.DirectorySeparatorChar + levelName;
                if (!Directory.Exists(levelSavesPath))
                {
                    Directory.CreateDirectory(levelSavesPath);
                    Debug.Log("Created level saves folder at path: " + levelSavesPath);
                }
                savesPath = levelSavesPath;
            }
            return savesPath;
        }

        public static string GetSaveFilePath(string fileName, string levelName)
        {
            return AssertSavesFolderExists(levelName) + Path.DirectorySeparatorChar + fileName + _saveFileExtension;
        }

        public static string[] GetSaveFileNames(string levelName, out string levelPath)
        {
            levelPath = AssertSavesFolderExists(levelName);
            string[] filePaths = Directory.GetFiles(levelPath);
            string[] fileNames = new string[filePaths.Length];
            for (int f = 0; f < filePaths.Length; f++)
            {
                fileNames[f] = Path.GetFileName(filePaths[f]);
            }
            return fileNames;
        }

        public static Dictionary<string, string> ReadJson(string filename, string levelName)
        {
            Dictionary<string, string> valueDict = new Dictionary<string, string>();
            string filepath = GetSaveFilePath(filename, levelName);
            //Debug.Log("Reading save data from filepath... " + filepath);
            if (!File.Exists(filepath))
            {
                Debug.LogWarning("Filepath does not exists! Using new SaveData. Given filepath: " + filepath);
                return valueDict;
            }
            byte[] encryptedData = File.ReadAllBytes(filepath);
            string jsonText = EncryptionUtility.DecryptFile(encryptedData, out byte fileFlags);
            DataWrapper wrapper = JsonUtility.FromJson<DataWrapper>(jsonText);
            foreach (StringDataEntry s in wrapper.entries)
            {
                valueDict.Add(s.dataName, s.dataValue);
            }
            //Debug.Log("Finished reading save data from filepath: " + filepath);
            return valueDict;
        }

        public static void WriteJson(Dictionary<string, DataEntry> data, string filename, string levelName, bool useEncryption = true)
        {
            WriteJson(data.AsReadOnlyCollection(), filename, levelName, useEncryption);
        }

        public static void WriteJson(ICollection<KeyValuePair<string, DataEntry>> data, string filename, string levelName, bool useEncryption = true)
        {
            string filepath = GetSaveFilePath(filename, levelName);
            //Debug.Log("Writing save data from filepath... " + filepath);
            DataWrapper wrapper = new DataWrapper();
            foreach (KeyValuePair<string, DataEntry> keyPair in data)
            {
                if (keyPair.Value == null) continue;
                string val = WriteDataEntryFrom(keyPair.Value);
                StringDataEntry s = new StringDataEntry();
                s.dataName = keyPair.Key;
                s.dataValue = val;
                wrapper.entries.Add(s);
            }

            string json = JsonUtility.ToJson(wrapper);
            byte[] encryptedData = EncryptionUtility.EncryptFile(json, useEncryption);
            File.WriteAllBytes(filepath, encryptedData);
            //Debug.Log("Finished writing save data from filepath: " + filepath);
        }

        public static string WriteDataEntryFrom<T>(T dataEntry) where T : DataEntry
        {
            string json = JsonUtility.ToJson(dataEntry);
            return json;
        }
    }
}
