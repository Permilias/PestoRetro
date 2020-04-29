using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<Music> musics;
    public List<AudioSource> sources;

    public float musicVolume;

    public bool startsWithMusic;
    public int startingMusicIndex;

    public void Initialize()
    {
        //create sources
        foreach(Music music in musics)
        {
            GameObject newSourceGO = new GameObject();
            newSourceGO.transform.parent = transform;
            AudioSource newSource = newSourceGO.AddComponent<AudioSource>();
            newSource.clip = music.clip;
            newSource.volume = music.baseVolume;
            newSource.loop = true;
            sources.Add(newSource);
        }

        if(startsWithMusic)
        {
            PlayMusic(startingMusicIndex);
        }

        delay = CurrentPlayingSource().clip.length;
    }

    bool startedMusic;
    private void Update()
    {
        if(CurrentPlayingSourceIndex() == 1)
        {
            return;
        }

        delay -= Time.deltaTime;
        if(delay <= 0f && !startedMusic)
        {
            startedMusic = true;
            PlayMusic(1);
        }
    }

    float delay;

    public bool MusicPlaying()
    {
        bool playing = false;
        foreach(AudioSource source in sources)
        {
            if(source.isPlaying)
            {
                playing = true;
            }
        }

        return playing;
    }

    public AudioSource CurrentPlayingSource()
    {
        AudioSource currentPlayingSource = sources[0];

        foreach(AudioSource source in sources)
        {
            if(source.isPlaying)
            {
                currentPlayingSource = source;
                break;
            }
        }

        return currentPlayingSource;
    }

    public int CurrentPlayingSourceIndex()
    {
        int index = 0;

        foreach(AudioSource source in sources)
        {          
            if(source.isPlaying)
            {
                break;
            }
            index++;
        }
        return index;
    }

    public void PlayMusic(int index)
    {
        if(MusicPlaying())
        {
            CurrentPlayingSource().Stop();
        }
        sources[index].Play();
        sources[index].volume = musics[index].baseVolume * musicVolume;
    }

    public void FadeInMusic(int index, float speed, bool plays = true)
    {
        if(MusicPlaying())
        {
            FadeOutMusic(speed);
        }

        sources[index].volume = 0;
        if(plays)
        {
            sources[index].Play();
        }

        sources[index].DOFade(musics[index].baseVolume * musicVolume, speed);
    }

    public void FadeOutMusic(float speed)
    {
        if(MusicPlaying())
        {
            AudioSource toFadeOut = CurrentPlayingSource();
            toFadeOut.DOFade(0, speed).OnComplete(() =>
            {
                //toFadeOut.Stop();
            });
        }
    }

    public void UpdateMusicVolume()
    {
        if(MusicPlaying())
        {
            CurrentPlayingSource().volume = musics[CurrentPlayingSourceIndex()].baseVolume * musicVolume;
        }
    }
}
