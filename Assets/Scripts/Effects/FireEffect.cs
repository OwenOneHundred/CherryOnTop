using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireEffect : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    private List<Effect> activeEffects = new List<Effect>();

    void Start()
    {
        StartNewIndefiniteFireEffect("TheFire", UnityEngine.Random.Range(0, 2) == 0 ? Color.red : Color.blue);
    }
    private void RemoveEffect(Effect effect)
    {
        activeEffects.Remove(effect);
        effect.Stop();
    }

    public Effect StartNewIndefiniteFireEffect(string name, Color color)
    {
        Effect effect = new Effect(prefab, name, transform.root, color, -1);
        activeEffects.Add(effect);
        return effect;
    }

    public Effect StartNewFireEffect(string name, Color color, float lifetime)
    {
        Effect effect = new Effect(prefab, name, transform.root, color, lifetime);
        activeEffects.Add(effect);
        return effect;
    }

    public bool StopFireEffect(string name)
    {
        List<Effect> effects = activeEffects.Where(x => x.name == name).ToList();
        foreach (Effect effect in effects)
        {
            RemoveEffect(effect);
        }
        return effects.Count > 0;
    }

    public bool StopFireEffect(Effect effect)
    {
        Effect foundEffect = activeEffects.FirstOrDefault(p => p == effect);
        if (foundEffect != null)
        {
            RemoveEffect(foundEffect);
            return true;
        }
        else return false;
    }

    public Effect GetEffect(string name)
    {
        return activeEffects.FirstOrDefault(x => x.name == name);
    }

    public Effect GetEffect(Color color)
    {
        return activeEffects.FirstOrDefault(x => x.color.Equals(color));
    }

    public bool GetEffectExists(string name)
    {
        return activeEffects.Any(x => x.name == name);
    }

    public int GetNumberOfActiveEffects()
    {
        return activeEffects.Count;
    }

    private void Update()
    {
        List<Effect> toRemove = new();
        foreach (Effect effect in activeEffects)
        {
            if (!effect.indefinite)
            {
                effect.lifetime -= Time.deltaTime;
                if (effect.lifetime <= 0)
                {
                    toRemove.Add(effect);
                }
            }
        }

        foreach (Effect effect in toRemove)
        {
            RemoveEffect(effect);
        }
    }

    public class Effect
    {
        public Effect(GameObject prefab, string name, Transform parent, Color color, float lifetime)
        {
            this.name = name;
            this.color = color;
            this.indefinite = lifetime < 0;
            this.lifetime = lifetime;
            
            effect = Instantiate(prefab, parent);
            effect.transform.localPosition = Vector3.zero;

            ps = effect.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule main = ps.main;
            main.startColor = color;

            var mesh = parent.GetComponentInChildren<MeshFilter>();

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Mesh;
            shape.mesh = mesh.sharedMesh;

            effect.transform.localScale = mesh.transform.lossyScale;

            ps.Play();
        }

        public void Stop()
        {
            ps.Stop();
            active = false;
            Destroy(effect, 3);
        }

        public string name;
        public GameObject effect;
        public ParticleSystem ps;
        public Color color;
        public bool indefinite;
        public float lifetime;
        public bool active = true;
    }
}
