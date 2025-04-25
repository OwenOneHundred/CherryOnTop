using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControl : MonoBehaviour
{
    public static GameOverControl gameOverControl;
    [SerializeField] Vector3 cherryPositionAtGameEnd;
    [SerializeField] float cameraSpeed;
    [SerializeField] Song gameOverSong;
    public bool isGameOver = false;
    private void Awake()
    {
        if (gameOverControl == null)
        {
            gameOverControl = this;
        }
        else if (gameOverControl != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnGameOver(GameObject cherry)
    {
        isGameOver = true;
        StartCoroutine(GameOverCoroutine(cherry));
    }

    public IEnumerator GameOverCoroutine(GameObject cherry)
    {
        SoundEffectManager.sfxmanager.transform.root.GetComponentInChildren<MusicController>().Pause();

        float timeScaleBefore = Time.timeScale;
        Time.timeScale = 0f;

        Transform camera = Camera.main.transform;
        Vector3 cameraGoalPos = Vector3.Lerp(cherryPositionAtGameEnd, camera.position, 0.85f) + new Vector3(0, 2, 0);
        while (camera.position != cameraGoalPos)
        {
            camera.position = Vector3.MoveTowards(camera.position, cameraGoalPos, cameraSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        StartCherryJump(cherry.transform, cherryPositionAtGameEnd);

        while (!WhileCherryJumping(cherry.transform))
        {
            yield return null;
        }

        OnCherryLand();

        yield return new WaitForSecondsRealtime(2);

        Time.timeScale = timeScaleBefore;

        transform.GetChild(0).gameObject.SetActive(true);

        SoundEffectManager.sfxmanager.transform.root.GetComponentInChildren<AudioManager>().SetLowpass(0);
        SoundEffectManager.sfxmanager.transform.root.GetComponentInChildren<MusicController>().ChangeSong(gameOverSong);
    }

    private void OnCherryLand()
    {
        Camera.main.transform.root.GetComponentInChildren<CameraControl>().ApplyCameraShake(0.2f, 0.1f);
    }

    Vector3 goalJumpPosition;
    Vector3 jumpStartPosition;
    float goalJumpY;
    float goalJumpTime;
    readonly float verticalJumpClearance = 1.5f;
    float velocityY;
    float jumpHorizontalSpeed;
    float gravity = 9.8f;
    float timer = 0;
    void StartCherryJump(Transform cherry, Vector3 jumpTarget) // set goal height, decceleration, calculate time based on distance, calculate initial velocity based on others
    {
        timer = 0; 

        goalJumpPosition = jumpTarget;
        jumpStartPosition = cherry.position;

        float heighestYBetweenGoalAndStart = (jumpStartPosition.y > goalJumpPosition.y) ? jumpStartPosition.y : goalJumpPosition.y;
        goalJumpY = heighestYBetweenGoalAndStart + verticalJumpClearance;

        goalJumpTime = Mathf.Clamp(Vector3.Distance(goalJumpPosition, jumpStartPosition), 1.5f, 8) / 3;
        
        velocityY = CalculateInitialVelocityKinFormula(Mathf.Abs(goalJumpY - jumpStartPosition.y), -gravity, goalJumpTime);

        jumpHorizontalSpeed = Vector2.Distance(new Vector2(goalJumpPosition.x, goalJumpPosition.z), new Vector2(jumpStartPosition.x, jumpStartPosition.z)) / goalJumpTime;

        static float CalculateInitialVelocityKinFormula(float distance, float acceleration, float time)
        {
            return (distance - (0.5f * acceleration * (time * time))) / time;
        }
    }

    bool WhileCherryJumping(Transform cherry)
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= goalJumpTime * 0.9f && cherry.position.y < goalJumpPosition.y)
        {
            cherry.position = goalJumpPosition;
            return true;
        }

        Vector2 xzMovement = (new Vector2(goalJumpPosition.x, goalJumpPosition.z) - new Vector2(cherry.position.x, cherry.position.z)).normalized * jumpHorizontalSpeed * Time.unscaledDeltaTime;
        cherry.position += new Vector3(xzMovement.x, velocityY * Time.unscaledDeltaTime, xzMovement.y);
        velocityY -= gravity * Time.unscaledDeltaTime;
        return false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        TransitionManager.transitionManager.LoadScene("MenuScene");
    }
}