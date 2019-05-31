using UnityEngine;
using System.Collections;

public class RippleEffect : MonoBehaviour
{
    public AnimationCurve waveform = new AnimationCurve(
        new Keyframe(0.00f, 0.50f, 0, 0),
        new Keyframe(0.05f, 1.00f, 0, 0),
        new Keyframe(0.15f, 0.10f, 0, 0),
        new Keyframe(0.25f, 0.80f, 0, 0),
        new Keyframe(0.35f, 0.30f, 0, 0),
        new Keyframe(0.45f, 0.60f, 0, 0),
        new Keyframe(0.55f, 0.40f, 0, 0),
        new Keyframe(0.65f, 0.55f, 0, 0),
        new Keyframe(0.75f, 0.46f, 0, 0),
        new Keyframe(0.85f, 0.52f, 0, 0),
        new Keyframe(0.99f, 0.50f, 0, 0)
    );

    public bool onlyOnStart = true;

    [Range(0.01f, 5.0f)]
    public float refractionStrength = 0.5f;

    public Color reflectionColor = Color.gray;

    [Range(0.01f, 5.0f)]
    public float reflectionStrength = 0.7f;

    [Range(0.1f, 3.0f)]
    public float waveSpeed = 1.25f;

    public float dropInterval = 0.5f;

    [SerializeField]
    private Camera cam = null;

    [SerializeField]
    private Shader shader = null;

    class Droplet
    {
        Vector2 position;
        float time;

        public Droplet()
        {
            time = 1000;
        }

        public void Reset(Vector2 pos)
        {
            position = pos;// new Vector2(Random.value, Random.value);
            time = 0;
        }

        public void Update()
        {
            time += Time.deltaTime;
        }

        public Vector4 MakeShaderParameter(float aspect)
        {
            return new Vector4(position.x * aspect, position.y, time, 0);
        }
    }

    Droplet[] droplets;
    Texture2D gradTexture;
    Material material;
    float timer;
    int dropCount;

    void UpdateShaderParameters()
    {
        material.SetVector("_Drop1", droplets[0].MakeShaderParameter(cam.aspect));
        material.SetVector("_Drop2", droplets[1].MakeShaderParameter(cam.aspect));
        material.SetVector("_Drop3", droplets[2].MakeShaderParameter(cam.aspect));

        material.SetColor("_Reflection", reflectionColor);
        material.SetVector("_Params1", new Vector4(cam.aspect, 1, 1 / waveSpeed, 0));
        material.SetVector("_Params2", new Vector4(1, 1 / cam.aspect, refractionStrength, reflectionStrength));
    }

    void Awake()
    {
        /*if (goToCamera)
        {
            RippleEffect ripple = cam.gameObject.AddComponent(typeof(RippleEffect)) as RippleEffect;
            ripple = this;
            Destroy(this);
            return;
        }*/

        droplets = new Droplet[3];
        droplets[0] = new Droplet();
        droplets[1] = new Droplet();
        droplets[2] = new Droplet();

        gradTexture = new Texture2D(2048, 1, TextureFormat.Alpha8, false);
        gradTexture.wrapMode = TextureWrapMode.Clamp;
        gradTexture.filterMode = FilterMode.Bilinear;
        for (var i = 0; i < gradTexture.width; i++)
        {
            var x = 1.0f / gradTexture.width * i;
            var a = waveform.Evaluate(x);
            gradTexture.SetPixel(i, 0, new Color(a, a, a, a));
        }
        gradTexture.Apply();

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        material.SetTexture("_GradTex", gradTexture);

        UpdateShaderParameters();
    }

    /// <summary>
    /// au start, emit l'objet une fois !
    /// </summary>
    public void StartPlay(Vector2 pos)
    {
        Emit(pos);
    }

    void Update()
    {
        if (dropInterval > 0 && !onlyOnStart)
        {
            timer += Time.deltaTime;
            while (timer > dropInterval)
            {
                Emit(new Vector2(0.5f, 0.5f));
                timer -= dropInterval;
            }
        }
        

        foreach (var d in droplets) d.Update();

        UpdateShaderParameters();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    public void Emit(Vector2 pos)
    {
        droplets[dropCount++ % droplets.Length].Reset(pos);
    }
}
