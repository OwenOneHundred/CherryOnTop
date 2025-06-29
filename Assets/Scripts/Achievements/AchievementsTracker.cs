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
                //Debug.Log("Initialized achievement save data file name: " + saveFileName);
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
        List<CompletedLevelDifficulty> levels = new List<CompletedLevelDifficulty>();
        CompletedLevelsDE levelsDE = saveData.GetOrDefault(completedLevelName, new CompletedLevelsDE(completedLevelName, new List<CompletedLevelDifficulty>()));
        levels = levelsDE.completedLevels;
        //Debug.Log("Initialized Achievements. Current levels completed: " + string.Join(", ", levels));

        _initialized = true;
    }

    public List<CompletedLevelDifficulty> GetCompletedLevels()
    {
        CompletedLevelsDE levelsDE = saveData.GetOrDefault(completedLevelName, new CompletedLevelsDE(completedLevelName, new List<CompletedLevelDifficulty>()));
        return levelsDE.completedLevels;
    }

    /// <summary>
    /// Get if the given level, at the given difficulty, with the given batter is completed, where -1 batter means no/normal batter.
    /// </summary>
    /// <param name="level">Level index</param>
    /// <param name="difficulty">Difficulty... number??</param>
    /// <param name="batter">Batter index, where -1 means no/default batter</param>
    /// <returns></returns>
    public bool HasCompletedLevel(int level, int difficulty, int batter)
    {
        CompletedLevelDifficulty levelDiff = new CompletedLevelDifficulty(level, difficulty, batter);
        if (saveData.TryGetDataEntry(completedLevelName, out CompletedLevelsDE levelsDE))
        {
            if (batter == -1)
            {
                if (levelsDE.completedLevels.Find(l => l.level == levelDiff.level && l.difficulty == levelDiff.difficulty) != null)
                {
                    return true;
                }
            }
            else
            {
                if (levelsDE.completedLevels.Find(l => l.level == levelDiff.level && l.batter == levelDiff.batter) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void MarkLevelAsCompleted(int level, int difficulty, int batter, bool isCompleted = true)
    {
        bool hasCompleted = HasCompletedLevel(level, difficulty, batter);
        if (isCompleted == hasCompleted) return;
        CompletedLevelDifficulty levelDiff = new CompletedLevelDifficulty(level, difficulty, batter);
        CompletedLevelsDE levelsDE = saveData.GetOrDefault(completedLevelName, new CompletedLevelsDE(completedLevelName, new List<CompletedLevelDifficulty>()));
        CompletedLevelDifficulty existing = levelsDE.completedLevels.Find(l => l.level == levelDiff.level && l.difficulty == levelDiff.difficulty && l.batter == batter);
        if (existing != null)
        {
            levelsDE.completedLevels.Remove(existing);
        } else
        {
            levelsDE.completedLevels.Add(levelDiff);
        }
        UpdateSaveData();
    }

    public void UpdateSaveData()
    {
        SaveDataUtility._useEncryptions = _encryptData;
        SaveDataUtility.WriteSaveData(saveData);
    }

    [System.Serializable]
    public class CompletedLevelsDE : DataEntry
    {
        [SerializeField] public List<CompletedLevelDifficulty> completedLevels;
        public CompletedLevelsDE(string dataName, List<CompletedLevelDifficulty> levels) : base(dataName)
        {
            completedLevels = levels;
        }
    }

    [System.Serializable]
    public class CompletedLevelDifficulty
    {
        [SerializeField] public int difficulty;
        [SerializeField] public int level;
        [SerializeField] public int batter; // where -1 is no batter and anything else is a batter index
        public CompletedLevelDifficulty(int level, int difficulty, int batter)
        {
            this.difficulty = difficulty;
            this.level = level;
            this.batter = batter;
        }
    }
}
