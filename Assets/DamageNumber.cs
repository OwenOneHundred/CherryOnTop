using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] List<Mesh> numbers;
    [SerializeField] GameObject baseNumberObj;
    [SerializeField] float spacing = 0.5f;
    float riseSpeed = 3;

    void Update()
    {
        transform.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
        riseSpeed -= 1 * Time.deltaTime;
    }

    public void SetDisplay(int damageNumber)
    {
        List<GameObject> numberObjs = new();
        while (damageNumber > 0)
        {
            GameObject newNumber = Instantiate(baseNumberObj, transform.position, Quaternion.identity, transform);
            newNumber.GetComponent<MeshFilter>().sharedMesh = numbers[damageNumber % 10];
            numberObjs.Add(newNumber);
            damageNumber /= 10;
        }

        int budgetEnum = 0;
        foreach (GameObject number in numberObjs)
        {
            number.transform.localPosition = new Vector3((-spacing * (numberObjs.Count / 2)) + (budgetEnum * spacing) + ((numberObjs.Count % 2 == 0) ? spacing / 2 : 0), 0, 0);
            budgetEnum += 1;
        }
    }
}
