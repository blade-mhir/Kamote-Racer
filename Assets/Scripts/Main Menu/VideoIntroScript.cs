using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class VideoIntroScript: MonoBehaviour

{

    public VideoPlayer videoPlayer;

    public string nextSceneName = "MainGame";

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached (VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }


}