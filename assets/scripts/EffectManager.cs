using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class EffectManager : MonoBehaviour
{
    public bool beginEffects = false;
    public FadeScreen fadeScreen;
    public TransparencyControl triangles;
    public Volume volume;
    public SpriteSpawn spriteSpawn;
    public SocketSpin socket;
    public ActionBasedController rightHand;
    public ActionBasedController leftHand;
    public UnityEvent onEffectEnabled;
    
    private float _begin = 0;
    private float _duration;
    public int lastMushroom = 0;

    // Start is called before the first frame update
    void Start()
    {
        _duration = GameManager.Instance.effectTime*60;
    }

    // Update is called once per frame
    void Update()
    {
        if (beginEffects)
        {
            beginEffects = false;
            onEffectEnabled.Invoke();
            StartCoroutine(EffectRoutine());
        }
    }

    public void BeginEffectsAfter(float delay)
    {
        Invoke("BeginEffects", delay);
    }
    public void BeginEffects()
    {
        Debug.Log("step0");
        beginEffects = true;
        _begin = Time.time;
    }

    IEnumerator EffectRoutine()
    {
        beginEffects = false;
        Debug.Log("step2");
        yield return StartCoroutine(fadeScreen.FadeOutInRoutine());
        //yield return StartCoroutine(socket.discoScript.EaseVolume(GameManager.Instance.initMusicVolume, 0.9f, 3));
        Debug.Log("step3");
        StartCoroutine(HandTrailRoutine(10));
        Debug.Log("step3.5");
        StartCoroutine(VolumeRoutine(0, 1f, _duration));
        Debug.Log("step4");
        StartCoroutine(HexagonRoutine(_duration/2, 30));
        //StopAllCoroutines();
    }

    IEnumerator VolumeRoutine(float a, float b, float duration)
    {
        float timer = 0;
        while (timer <= duration)
        {
            volume.weight = Mathf.Lerp(a, b, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        volume.weight = b;
    }

    IEnumerator HexagonRoutine(float delay, float transDur)
    {
        yield return new WaitForSeconds(delay);
        spriteSpawn.enabled = true;
        while (true)
        {
            var currMush = GameManager.Instance.eatenMushrooms;
            if (currMush != lastMushroom)
            {
                StartCoroutine(triangles.AlphaRoutine(triangles._currentAlpha, 0.3f * currMush / 5, transDur));
                lastMushroom = currMush;
            }
            yield return null;
        }
    }

    IEnumerator HandTrailRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("step3.6");
        ActivateTrail(rightHand);
        ActivateTrail(leftHand);
    }

    private void ActivateTrail(ActionBasedController hand)
    {
        hand.GetComponentInChildren<TrailRenderer>().enabled = true;
    }
}
