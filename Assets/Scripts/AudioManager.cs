using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Our array of  background audios and sound effects
    public AudioSource[] sfx;
    public AudioSource[] bgm;

    //Making an instance so there will only be 1 audiomanager
    public static AudioManager instance;

    /**/
    /*
    AudioManager.cs --- Start()
    NAME
            Start() - In Unity,  Start is called before the first frame update
    SYNOPSIS
            Assigning value to instance and telling Unity not to destroy on load
    DESCRIPTION
            Setting instance to be the current object
            DontDestroyOnLoad - makes it so we do not destroy the target Object(in this case AudioManager) when loading a new Scene.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /**/
    /*
    AudioManager.cs --- Update()
    NAME
            Update() - In Unity,   Update is called once per frame
    SYNOPSIS
           Used for testing audio
    DESCRIPTION
            When the game is running and I hit the t key I try to play the music and a sound effect.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlaySFX(4);
            PlayBGM(3);
        }
    }
    /**/
    /*
    AudioManager.cs --- PlaySFX()
    NAME
            PlaySFX(int soundToPlay)
    SYNOPSIS
           Play a sound effect
    DESCRIPTION
            1. Validate array index 
            2. If it is a valid array index then we call the play function for the specified index
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void PlaySFX(int soundToPlay)
    {
        if (soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }
    /**/
    /*
    AudioManager.cs --- PlayBGM()
    NAME
            PlayBGM(int musicToPlay)
    SYNOPSIS
           Play some background music
    DESCRIPTION
            1. If it is true we check if there is no music playing OR if there is currently music playing 
                because if the track is already playing (say we enter the shop) we want to continue the music, not restart it
            2.  Validate array index 
            3.  If valid array we play the music at the background music array index.
            
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void PlayBGM(int musicToPlay)
    {
 
        if (!bgm[musicToPlay].isPlaying)
        {
            StopMusic();
            if (musicToPlay < bgm.Length)
            {
                bgm[musicToPlay].Play();
            }
        }

    }
    /**/
    /*
    AudioManager.cs --- StopMusic()
    NAME
            StopMusic()
    SYNOPSIS
           Stop music from playing
    DESCRIPTION
            Loop through our indexes and stop everything from playing
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void StopMusic()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
