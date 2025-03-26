using UnityEngine;

public class MoneyChangeDisplay : MonoBehaviour
{
    float timeActive = 0;
    bool active = false;
    int displayedMoney = 0;
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] GameObject coinPSPrefab;

    private void Update()
    {
        if (active)
        {
            timeActive += Time.deltaTime;

            if (timeActive > 3.5f)
            {
                gameObject.SetActive(false);
                displayedMoney = 0;
            }
        }
    }   

    public void AddToDisplay(int moneyChange)
    {
        displayedMoney += moneyChange;
        active = true;
        timeActive = 0;
        gameObject.SetActive(true);
        text.text = (displayedMoney < 0 ? "" : "+") + displayedMoney;

        if (moneyChange > 0)
        {
            Destroy(Instantiate(coinPSPrefab, Camera.main.transform), 4);
        }
    }
}
