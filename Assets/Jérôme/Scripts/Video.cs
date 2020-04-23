using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Video : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public Image image;
    public float VideoLength;
    float timer;

    void Start()
    {
        VideoLength = (float)videoPlayer.clip.length;
        videoPlayer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            StartVideo();

        timer -= Time.deltaTime;

        if (timer < 0)
            StopVideo();
    }

    void StartVideo()
    {
        image.color = Color.white;
        timer = VideoLength;
        videoPlayer.enabled = true;
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    void StopVideo()
    {
        image.color = new Color(1,1,1,0);
        timer = 0;
        videoPlayer.enabled = false;
    }
}