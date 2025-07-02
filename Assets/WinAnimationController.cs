using System.Collections;
using UnityEngine;

public class WinAnimationController : MonoBehaviour
{
    [SerializeField] GameObject youWinUI;
    [SerializeField] float waitTime;

    void Start()
    {
        PlayWinAnimation();
    }
    public void PlayWinAnimation()
    {
        StartCoroutine(WinAnimation());
    }

    private IEnumerator WinAnimation()
    {
        yield return new WaitForSeconds(waitTime);

        youWinUI.SetActive(true);
    }
}
