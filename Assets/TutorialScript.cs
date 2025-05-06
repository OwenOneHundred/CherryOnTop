using System.Collections;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public void Start()
    {
        StartTutorial();
    }

    public void StartTutorial()
    {

    }

    private IEnumerator DisplayText(TextBox textBox)
    {
        yield return null;
    }

    private class TextBox
    {
        public Vector3 location;
        public string text;
        public TextBox(string text, Vector3 location = default)
        {
            this.text = text;
            this.location = location;
        }
    }
}
