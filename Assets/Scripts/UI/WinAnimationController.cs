using System.Collections;
using UnityEngine;

public class WinAnimationController : MonoBehaviour
{
    [SerializeField] GameObject youWinUI;
    [SerializeField] GameObject veil;
    CameraControl cameraControl;
    Vector3 cameraInitialPosition;
    void Awake()
    {
        cameraControl = Camera.main.transform.root.GetComponent<CameraControl>();
    }
    public void PlayWinAnimation()
    {
        cameraControl.Locked = true;
        gameObject.SetActive(true);
        StartCoroutine(WinAnimation());
    }

    private IEnumerator WinAnimation()
    {
        veil.SetActive(true);
        Transform cameraTransform = Camera.main.transform;
        Transform cameraSwivel = cameraTransform.root;
        Vector3 cameraZoomPosition = new Vector3(0, 9, -9);
        cameraInitialPosition = Camera.main.transform.localPosition;

        while (Vector3.Distance(cameraTransform.localPosition, cameraZoomPosition) > 0.01f || cameraSwivel.localRotation.eulerAngles.x != 0)
        {
            cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, cameraZoomPosition, 4 * Time.deltaTime);
            cameraSwivel.localRotation = Quaternion.Euler(new Vector3(
                Mathf.MoveTowards(cameraSwivel.localRotation.eulerAngles.x, 0, 30 * Time.deltaTime),
                cameraSwivel.localRotation.eulerAngles.y,
                cameraSwivel.localRotation.eulerAngles.z)
                );

            yield return null;
        }

        yield return new WaitForSeconds(1);

        float totalRotationTraveled = 0;
        while (totalRotationTraveled < 360)
        {
            cameraSwivel.Rotate(new Vector3(0, Time.deltaTime * 75f, 0));
            totalRotationTraveled += Time.deltaTime * 75f;

            yield return null;
        }

        yield return new WaitForSeconds(1);

        youWinUI.SetActive(true);
        youWinUI.transform.localScale = Vector3.zero;
        while (youWinUI.transform.localScale.x < 1)
        {
            youWinUI.transform.localScale += Vector3.one * Time.deltaTime * 5;

            yield return null;
        }
    }

    public void OnCloseWinPanel()
    {
        StartCoroutine(Close());
    }

    private IEnumerator Close()
    {
        Transform cameraTransform = Camera.main.transform;
        veil.SetActive(false);
        cameraControl.Locked = false;
        while (youWinUI.transform.localScale.x < 1)
        {
            youWinUI.transform.localScale += Vector3.one * Time.deltaTime * 50;

            yield return null;
        }
        while (Vector3.Distance(cameraTransform.localPosition, cameraInitialPosition) > 0.01f)
        {
            cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, cameraInitialPosition, 14 * Time.deltaTime);

            yield return null;
        }
        youWinUI.SetActive(false);
    }
}
