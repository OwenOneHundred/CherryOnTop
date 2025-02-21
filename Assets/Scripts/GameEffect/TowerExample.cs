using UnityEngine;

public class TowerExample : MonoBehaviour
{
    public int attackPower = 0;
    [SerializeField] protected GameEffectExample gameEffect;

    private void Awake()
    {
        if (gameEffect != null)
        {
            GameEffectExample example = Instantiate(gameEffect);
            example.RegisterEffects(this);
        }
    }
}
