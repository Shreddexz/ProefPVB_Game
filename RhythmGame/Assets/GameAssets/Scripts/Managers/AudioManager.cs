using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using FMOD;
using FMOD.Studio;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using Melanchall.DryWetMidi.Multimedia;
using UnityEngine.Networking;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
#region Variables
    public static bool paused;
    public static AudioManager instance;
    public static bool infoSet;

#region MIDIVars
    public static MidiFile songChart;
    public string chartName;
    public static string chartDir;
#endregion


#region FMODVars
    EVENT_CALLBACK beatCallback;
    public static PLAYBACK_STATE playbackState;
    public static EventInstance musicPlayer;
    public static ChannelGroup masterChannelGroup;
    int masterSampleRate;
    double currentSamples;
    double currentTime;
    ulong dspClock;
    ulong parentDSP;
    ulong cachedSamples;
    public string playTimeString;
    public double playbackTime;
    static string songDuration;
    Bus sfxBus;
    Bus musicBus;
#endregion

#region EventVars
    public delegate void VolumeChangedDelegate();

    public delegate void MusicStateChange();

    public delegate void TimelineInfoSet();

    public static VolumeChangedDelegate SfxVolumeChangedDelegate;
    public static VolumeChangedDelegate MusicVolumeChangedDelegate;

    public static MusicStateChange onMusicStart;

    public static TimelineInfoSet onInfoReceived;
#endregion

#region VolumeMixerVars
    [SerializeField] [Range(0f, 1f)] float sfxVolume = 0f;
    float c_sfxVolume;

    [SerializeField] [Range(0f, 1f)] float musicVolume = 0f;
    float c_musicVolume;
#endregion

    [StructLayout(layoutKind: LayoutKind.Sequential)] //this places the variables sequentially
    //in the memory to access it quicker and more easily.
    public class SongInfo
    {
        public int currentBeat = 0;
    }

    static SongInfo songInfo;

    public SongInfo info;
    GCHandle timelineHandle;

    public static int beat = 0;
    public static float bpm;
    public static uint songLength;

    public GameObject audioObject;

    public float songDelaySeconds;
    public int inputDelayMilliseconds;
    public float noteTime;
    public float noteOffset;
    public double marginOfErrorSeconds;

    public Lane[] lanes;

    /// <summary>
    /// A simple input method for referring to an FMOD event.
    /// </summary>
    string FMODEvent(string eventName)
    {
        string output = $"event:/{eventName}";
        return output;
    }
#endregion

#region Start/Exit calls
    void OnEnable()
    {
        MusicVolumeChangedDelegate += MusicVolumeChanged;
        SfxVolumeChangedDelegate += SFXVolumeChanged;
        onMusicStart += OnMusicStart;

        onInfoReceived += OnInfoReceived;
    }

    void OnDisable()
    {
        MusicVolumeChangedDelegate -= MusicVolumeChanged;
        SfxVolumeChangedDelegate -= SFXVolumeChanged;
        onMusicStart -= OnMusicStart;
        onInfoReceived -= OnInfoReceived;
        musicPlayer.stop(STOP_MODE.ALLOWFADEOUT);
        musicPlayer.release();
    }

    void Awake()
    {
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        musicVolume = 0.5f;
        sfxVolume = 0.5f;

        c_musicVolume = 0.5f;
        c_sfxVolume = 0.5f;

        musicBus.setVolume(0.5f);
        sfxBus.setVolume(0.5f);
        // musicBus.getVolume(out c_musicVolume);
        // sfxBus.getVolume(out c_sfxVolume);

        RuntimeManager.CoreSystem.getMasterChannelGroup(out masterChannelGroup);
        RuntimeManager.CoreSystem.getSoftwareFormat(out masterSampleRate, out SPEAKERMODE mode, out int speakerNum);
    }

    void Start()
    {
        if (!instance)
            instance = this;
        else
        {
            Debug.Log("Audiomanager instance already exists. The new instance will be destroyed");
            Destroy(this);
        }

        // songInfo = new SongInfo();
    }

    void OnDestroy()
    {
        musicPlayer.setUserData(IntPtr.Zero);
        musicPlayer.stop(STOP_MODE.ALLOWFADEOUT);
        musicPlayer.release();
        timelineHandle.Free();
    }
#endregion

    void Update()
    {
        musicPlayer.getPlaybackState(out playbackState);
        switch (playbackState)
        {
            case PLAYBACK_STATE.PLAYING:
                paused = false;
                break;

            case PLAYBACK_STATE.STOPPED:
                paused = true;
                break;

            default:
                paused = true;
                break;
        }

        if (musicVolume != c_musicVolume)
            MusicVolumeChangedDelegate?.Invoke();

        if (sfxVolume != c_sfxVolume)
            SfxVolumeChangedDelegate?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onMusicStart?.Invoke();
        }

        masterChannelGroup.getDSPClock(out dspClock, out parentDSP);

        if (!paused)
            UpdateDSPTime();

        TimeSpan playTimeTS = TimeSpan.FromSeconds(currentTime);
        playTimeString = playTimeTS.ToString("mm':'ss");
        playbackTime = currentTime;
    }

// #region Midi
//     /// <summary>
//     /// Locates and assigns the MIDI(.mid) file
//     /// so that the notes can be read and used.
//     /// </summary>
//     void ReadFromFile()
//     {
//         chartDir = $"{Application.dataPath}/StreamingAssets/{chartName}.mid";
//         songChart = MidiFile.Read(chartDir);
//         GetMidiData();
//     }
//
//     /// <summary>
//     /// Reads the data from the assigned MIDI file,
//     /// placing all the notes in an array.
//     /// </summary>
//     void GetMidiData()
//     {
//         var notes = songChart.GetNotes();
//         var notesArray = new Note[notes.Count];
//         notes.CopyTo(notesArray, 0);
//
//         foreach (var lane in lanes)
//         {
//             lane.SetTimestamp(notesArray);
//         }
//     }
// #endregion


#region Music
    /// <summary>
    /// Creates an instance of the music event, and starts it.
    /// If another instance is already playing, it stops that instance and creates a new one in its place
    /// </summary>
    void StartSongPlayback()
    {
        if (playbackState == PLAYBACK_STATE.PLAYING)
        {
            musicPlayer.release();
            musicPlayer.stop(STOP_MODE.ALLOWFADEOUT);
            RuntimeManager.DetachInstanceFromGameObject(musicPlayer);
        }

        musicPlayer = RuntimeManager.CreateInstance(FMODEvent("PlayGH"));
        RuntimeManager.AttachInstanceToGameObject(musicPlayer, audioObject.transform);
        if (!infoSet)
            GetSongInfo();
        musicPlayer.start();
        musicPlayer.setPaused(false);
    }

    void OnMusicStart()
    {
        paused = false;
        playbackState = PLAYBACK_STATE.PLAYING;
        cachedSamples = dspClock;
        StartSongPlayback();
    }

    void OnInfoReceived()
    {
        infoSet = true;
    }
#endregion

#region Mixer
    void MusicVolumeChanged()
    {
        c_musicVolume = musicVolume;
        musicBus.setVolume(c_musicVolume);
    }

    void SFXVolumeChanged()
    {
        c_sfxVolume = sfxVolume;
        sfxBus.setVolume(c_sfxVolume);
    }
#endregion

#region FMOD
    /// <summary>
    /// Sets up the variables for retrieving data from the FMOD timeline,
    /// and pins the SongInfo instance to the memory to prevent garbage collection
    /// from removing it.
    /// </summary>
    void GetSongInfo()
    {
        if (info == null)
            info = new SongInfo();
        beatCallback = BeatEventCallback;
        timelineHandle = new GCHandle();
        timelineHandle = GCHandle.Alloc(info, GCHandleType.Pinned);
        musicPlayer.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicPlayer.setCallback(beatCallback,
                                EVENT_CALLBACK_TYPE.TIMELINE_BEAT | EVENT_CALLBACK_TYPE.SOUND_PLAYED);
    }

    /// <summary>
    /// Updates the variables that keep track of the current playback time.
    /// </summary>
    void UpdateDSPTime()
    {
        currentSamples = dspClock;
        currentTime = (currentSamples - cachedSamples) / masterSampleRate;
    }

    /// <summary>
    /// Creates callbacks to the FMOD timeline, returning and setting the data
    /// so that it can be used by other systems.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="instancePtr"></param>
    /// <param name="parameterPtr"></param>
    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static RESULT BeatEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        EventInstance instance = new EventInstance(instancePtr);
        RESULT result = instance.getUserData(out var infoPtr);
        if (result != RESULT.OK)
        {
            Debug.LogError($"Timeline Callback Error: {result}");
        }
        else if (infoPtr != IntPtr.Zero)
        {
            switch (type)
            {
                case EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    if (songInfo == null)
                        songInfo = new SongInfo();
                    GCHandle timelineHandle = GCHandle.FromIntPtr(infoPtr);
                    songInfo = (SongInfo) timelineHandle.Target;
                    var songVars =
                        (TIMELINE_BEAT_PROPERTIES) Marshal.PtrToStructure(parameterPtr,
                                                                          typeof(TIMELINE_BEAT_PROPERTIES));
                    beat = songVars.beat;
                    bpm = songVars.tempo;
                    if(!infoSet)
                        onInfoReceived?.Invoke();
                    break;

                case EVENT_CALLBACK_TYPE.SOUND_PLAYED:
                    Sound sound = new Sound(parameterPtr);

                    sound.getLength(out songLength, TIMEUNIT.MS);
                    TimeSpan songTS = TimeSpan.FromMilliseconds(songLength * 48000 / 1000 / 4.78f);
                    songDuration = songTS.ToString("mm':'ss");
                    break;

                default:
                    break;
            }
        }

        return RESULT.OK;
    }
#endregion

#if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle boxStyle = new GUIStyle
        {
            fontSize = 24
        };
        GUILayout.Box($"Current beat: {beat} \nSong BPM: {bpm}\nPlaytime: {playTimeString}/{songDuration}", boxStyle,
                      GUILayout.Width(400f), GUILayout.Height(200f));
    }
#endif
}