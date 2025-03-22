using UnityEngine;
using GameSaves;

public class SaveDataTester : MonoBehaviour
{

    [SerializeField] string savename = "save1";
    protected SaveData _saveData = null;
    public SaveData saveData
    {
        get
        {
            if (_saveData == null)
            {
                _saveData = SaveData.LoadData(savename);
            }
            return _saveData;
        }
    }

    [SerializeField] protected Vector3 writeThis = new Vector3(5, 2, 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (saveData.TryGetData("pos", out DEPosition pos))
        {
            Debug.Log("Found pos with data: " + pos.positionData.ToString());
        } else
        {
            saveData.SetData("pos", new DEPosition("pos", writeThis));
        }
        SaveData.WriteData(saveData);
    }

    [System.Serializable]
    class SomeDataEntry : DataEntry
    {
        [SerializeField] public string someVariable = "this";
        public SomeDataEntry(string dataName, string someVariable) : base(dataName)
        {
            this.someVariable = someVariable;
        }
    }
}
