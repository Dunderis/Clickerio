using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Clicker : MonoBehaviour
{
    [Header("Animation settings")]
    public float scale = 1.2f;
    public float duration = 0.1f;
    public Ease ease = Ease.OutElastic;

    [Header("Audio")]
    public AudioClip clickSound;

    [Header("VFX")]

    public ParticleSystem clickVFX;
    [HideInInspector]public int clickps;


    [HideInInspector]public int clicks = 0;
    private AudioSource audioSource;

    private int oldClicks = 0;
    private int clickCount = 0;
    public MeshFilter filter;
    private Mesh startMesh;
    public Mesh mesh;
    public GameObject player;
    

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("CountClicks", 1, 1);
        InvokeRepeating("ChangeMesh", 1, 1);
        clicks = PlayerPrefs.GetInt("clicks", 0);
        filter = player.GetComponent<MeshFilter>();
        
        
    }

    private void OnMouseDown() 
    {
        clickVFX.Emit(1);
        clicks++;
        clickCount++;
        clickps++;
        //Debug.Log("Clicks: " + clicks);
        UiManager.instance.UpdateClicks(clicks);
        

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clickSound);

        transform
            .DOScale(1, duration)
            .ChangeStartValue(scale * Vector3.one)
            .SetEase(ease);
            //.SetLoops(2, LoopType.Yoyo);
    }

    private void CountClicks()
    {
        var cps = clicks - oldClicks;
        oldClicks = clicks;
        UiManager.instance.UpdateCps(cps);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) {
            Save();
        }
    }
    private void OnApplicationQuit()
    {
       Save();
    }

    public void Save()
    {
        
        PlayerPrefs.SetInt("clicks", clicks);
        PlayerPrefs.Save();
    }
    public void ChangeMesh()
    {
        if(clickCount == 1)
        {
            clickCount = 0;
            startMesh = filter.mesh;
            filter.mesh = mesh;
            mesh = startMesh;
        }
    }
}
