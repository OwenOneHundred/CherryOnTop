using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] float charactersPerSecond;
    [SerializeField] TextBox welcome;
    [SerializeField] TextBox gameSummary;
    [SerializeField] TextBox whatAreToppings;
    [SerializeField] TextBox openTheShop;
    [SerializeField] TextBox readDescriptionsAndBuySomething;
    [SerializeField] TextBox onceSatisfiedCloseShop;
    [SerializeField] TextBox dragToppingsOntoCake;
    [SerializeField] TextBox startWhenReady;
    [SerializeField] GameObject textBoxGameObject;
    [SerializeField] Image textBoxImage;
    [SerializeField] GameObject arrowObject;
    [SerializeField] AudioFile characterAppearSound;
    [SerializeField] AudioFile dismissTextSound;
    Button startButton;
    Button shopButton;
    public void Start()
    {
        startButton = GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>();
        shopButton = GameObject.FindGameObjectWithTag("ShopButton").GetComponent<Button>();
        StartCoroutine(Tutorial());
    }

    private IEnumerator Tutorial()
    {
        yield return null;

        int initialInventoryItems = Inventory.inventory.ownedItems.Count;

        startButton.interactable = false;
        shopButton.interactable = false;

        yield return StartCoroutine(DisplayText(welcome));
        yield return StartCoroutine(WaitUntilClick());
        yield return StartCoroutine(DisplayText(gameSummary));
        yield return StartCoroutine(WaitUntilClick());
        yield return StartCoroutine(DisplayText(whatAreToppings));
        yield return StartCoroutine(WaitUntilClick());
        shopButton.interactable = true;
        yield return StartCoroutine(DisplayText(openTheShop));
        while (!Shop.shop.Open)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(DisplayText(readDescriptionsAndBuySomething));
        yield return StartCoroutine(WaitUntilClick());
        textBoxGameObject.SetActive(false);
        while (Inventory.inventory.ownedItems.Count == initialInventoryItems)
        {
            yield return null;
        }
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(DisplayText(onceSatisfiedCloseShop));
        yield return StartCoroutine(WaitUntilClick());
        textBoxGameObject.SetActive(false);
        while (Shop.shop.Open)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(DisplayText(dragToppingsOntoCake));
        yield return StartCoroutine(WaitUntilClick());
        textBoxGameObject.SetActive(false);
        while (ToppingRegistry.toppingRegistry.PlacedToppings.Count == 0)
        {
            yield return null;
        }

        startButton.interactable = true;

        yield return StartCoroutine(DisplayText(startWhenReady));
        yield return StartCoroutine(WaitUntilClick());
        textBoxGameObject.SetActive(false);
        while (RoundManager.roundManager.roundState != RoundManager.RoundState.cherries)
        {
            yield return null;
        }

        OnTutorialEnd();
    }

    void Update()
    {
        if (Input.mousePosition.y > 730)
        {
            Color color = textBoxImage.color;
            color.a = 0.75f;
            textBoxImage.color = color;
        }
        else
        {
            Color color = textBoxImage.color;
            color.a = 1f;
            textBoxImage.color = color;
        }
    }

    private IEnumerator DisplayText(TextBox textBox)
    {
        textBoxGameObject.SetActive(true);
        text.text = "";
        text.fontSize = textBox.textSize;
        float waitTime = 1 / charactersPerSecond;
        for (int i = 0; i < textBox.text.Count(); i++)
        {
            if (Input.GetMouseButtonDown(0))
            {
                text.text = textBox.text;
                break;
            }
            SoundEffectManager.sfxmanager.PlayOneShot(characterAppearSound);
            text.text += textBox.text[i];
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator WaitUntilClick()
    {
        arrowObject.SetActive(true);
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        yield return null;
        SoundEffectManager.sfxmanager.PlayOneShot(dismissTextSound);
        arrowObject.SetActive(false);
    }

    void OnTutorialEnd()
    {
        PlayerPrefs.SetInt("TutorialFinished", 1);

        Destroy(gameObject);
    }

    [System.Serializable]
    private class TextBox
    {
        public Vector3 location;
        [TextArea] public string text;
        public float textSize = 40;
        public TextBox(string text, Vector3 location = default)
        {
            this.text = text;
            this.location = location;
        }
    }
}
