using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject levelbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(levelbox, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(levelbox, new Vector3(400, 0, 0), Quaternion.identity);



    }


}
