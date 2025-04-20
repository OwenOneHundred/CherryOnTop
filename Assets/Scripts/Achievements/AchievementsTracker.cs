using System.Collections.Generic;
using GameSaves;
using UnityEngine;

public class AchievementsTracker : MonoBehaviour
{
    protected static AchievementsTracker _instance = null;
    public static AchievementsTracker Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                _instance = obj.AddComponent<AchievementsTracker>();
                _instance.InitializeAchievements();
                obj.name = "AchievementTracker";
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    protected SaveData _saveData = null;
    public SaveData saveData
    {
        get
        {
            if (_saveData == null)
            {
                string saveLevelName = "achievements";
                if (SaveDataUtility.GetSaveFileNameIfExists(saveLevelName, out string saveFilePath, out string saveFileName, saveLevelName))
                {
                    _saveData = SaveDataUtility.LoadSaveData(saveLevelName, saveLevelName);
                }
                else
                {
                    Debug.LogWarning("No achievements file found, creating a new instance...");
                    _saveData = SaveDataUtility.CreateSaveData(saveLevelName, saveLevelName);
                }
                Debug.Log("Initialized achievement save data file name: " + saveFileName);
            }
            return _saveData;
        }
    }
    protected bool _initialized = false;
    protected string completedLevelName = "levelsComleted";
    public bool _encryptData = true;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            InitializeAchievements();
        } else if (_instance != this)
        {
            Debug.LogWarning("AchievementTracker instance already exists, deleting new one that was created...");
        }
    }

    public void InitializeAchievements()
    {
        if (_initialized) return;

        SaveData data = saveData;
        List<int> levels = new List<int>();
        CompletedLevelsDE levelsDE = saveData.GetOrDefault(completedLevelName, new CompletedLevelsDE(completedLevelName, new List<int>()));
        levels = levelsDE.completedLevels;
        Debug.Log("Initialized Achievements. Current levels completed: " + string.Join(", ", levels));

        _initialized = true;
    }

    public List<int> GetCompletedLevels()
    {
        CompletedLevelsDE levelsDE = saveData.GetOrDefault(completedLevelName, new CompletedLevelsDE(completedLevelName, new List<int>()));
        return levelsDE.completedLevels;
    }

    public bool HasCompletedLevel(int level)
    {
        if (saveData.TryGetDataEntry(completedLevelName, out CompletedLevelsDE levelsDE))
        {
            return levelsDE.completedLevels.Contains(level);
        }
        return false;
    }

    public void MarkLevelAsCompleted(int level, bool isCompleted = true)
    {
        bool hasCompleted = HasCompletedLevel(level);
        if (isCompleted == hasCompleted) return;
        CompletedLevelsDE levelsDE = saveData.GetOrDefault(completedLevelName, new CompletedLevelsDE(completedLevelName, new List<int>()));
        if (isCompleted)
            levelsDE.completedLevels.Add(level);
        else
            levelsDE.completedLevels.Remove(level);
        UpdateSaveData();
    }

    public void UpdateSaveData()
    {
        SaveDataUtility._useEncryptions = _encryptData;
        SaveDataUtility.WriteSaveData(saveData);
        Debug.Log("Updated achievements data!");
    }

    [System.Serializable]
    public class CompletedLevelsDE : DataEntry
    {
        [SerializeField] public List<int> completedLevels;
        public CompletedLevelsDE(string dataName, List<int> levels) : base(dataName)
        {
            completedLevels = levels;
        }
    }
}
