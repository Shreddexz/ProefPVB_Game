using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using Debug = UnityEngine.Debug;

public class NotePooler : MonoBehaviour
{
    public int notePoolLimit = 15;
    public GameObject noteEnemy;
    float spawnOffset;
    public static Note[] notesArray;
    public List<NoteEnemy> pooledNotes = new();
    static double audioLatencyMS;
    bool bpmSet;
    int noteIndex;
    static string noteConcat;
    public static Lane[] lanes;
    public Lane[] lanesCopy;
    static int laneIndex;
    NoteManager manager;

    void OnEnable()
    {
        AudioManager.onMusicStart += OnInfoReceived;
    }

    void OnDisable()
    {
        AudioManager.onMusicStart -= OnInfoReceived;
    }

    /// <summary>
    /// When info is received from the FMOD timeline, this method calculates the audiolatency in ms
    /// based on the amount of buffers, the bufferlenght, and the samplerate of the audio bus.
    /// </summary>
    void OnInfoReceived()
    {
        AudioManager.masterSystem.getDSPBufferSize(out uint bufferLength, out int numBuffers);
        audioLatencyMS = (double)numBuffers * bufferLength / AudioManager.instance.masterSampleRate;
    }

    void Awake()
    {
        manager = transform.GetComponent<NoteManager>();
        lanes = new Lane[lanesCopy.Length];
        bpmSet = false;
        lanesCopy.CopyTo(lanes, 0);
    }

    /// <summary>
    /// Retrieves the BPM of the current song, and calculates the offset for the note spawn position based on the BPM.
    /// </summary>
    void GetSongBPM()
    {
        if (AudioManager.bpm != 0f)
        {
            spawnOffset = ((60 * 16 / SongVariables.bpm) * manager.noteSpeedDistMultiplier + 3f);
            bpmSet = true;
            foreach (var lane in lanes)
            {
                lane.transform.position = new Vector3(lane.transform.position.x, 0, spawnOffset);
            }
        }
    }

    void Update()
    {
        if (!bpmSet)
            GetSongBPM();
    }


    /// <summary>
    /// Copies the values of an notetimes list without having to create unnecessary references, and calls the method to use the list values.
    /// </summary>
    /// <param name="arrayToCopy"></param>
    public static void CopyNoteList(Note[] arrayToCopy)
    {
        notesArray = arrayToCopy;
        AddNotesTimes();
    }

    /// <summary>
    /// Parses the note data and adds the notetimes to dedicated lanes based on the note played.
    /// To factor in audio latency, the audioLatencyMS is added on top of the notetime before being sent to the lane.
    /// </summary>
    static void AddNotesTimes()
    {
        foreach (var note in notesArray)
        {
            double noteTime = note.TimeAs<MetricTimeSpan>(NoteManager.songChart.GetTempoMap()).TotalSeconds +
                              audioLatencyMS;

            noteConcat = string.Concat(note.NoteName, note.Octave);
            switch (noteConcat)
            {
                case "A4":
                    laneIndex = 0;
                    break;

                case "C4":
                    laneIndex = 1;
                    break;

                case "D4":
                    laneIndex = 2;
                    break;

                case "E4":
                    laneIndex = 3;
                    break;

                case "B4":
                    laneIndex = 4;
                    break;
            }

            lanes[laneIndex].timeStamps.Add(noteTime);
        }
    }

    /// <summary>
    /// Spawns a note object in the designated lane when called.
    /// When there are pooled note available, the pooled note is reinstantiated in the lane instead of a new one being spawned.
    /// </summary>
    /// <param name="lane"></param>
    public void SpawnNote(Lane lane)
    {
        if (pooledNotes.Count > 0)
        {
            NoteEnemy note = pooledNotes[0];
            if (note.transform.position != lane.gameObject.transform.position)
                note.transform.position = lane.gameObject.transform.position;
            if (note.gameObject.layer != LayerMask.NameToLayer("p1"))
                note.gameObject.layer = LayerMask.NameToLayer("p1");
            foreach (var trans in note.gameObject.GetComponentsInChildren<Transform>())
            {
                trans.gameObject.layer = LayerMask.NameToLayer("p1");
            }

            note.transform.parent = lane.transform;
            note.canMove = true;
            note.gameObject.SetActive(true);
            note.NotePlaced();
            pooledNotes.Remove(note);
            lane.activeNotes.Add(note);

            if (PlayerManager.multiplayer)
                NoteP2Spawn(lane);
            return;
        }

        GameObject spawnedNote = Instantiate(noteEnemy, lane.gameObject.transform);
        lane.activeNotes.Add(spawnedNote.GetComponent<NoteEnemy>());
        if (PlayerManager.multiplayer)
            NoteP2Spawn(lane);
    }

    /// <summary>
    /// Spawns a new noteobject for player 2, if the game is in multiplayer mode.
    /// The noteobject spawns in sync with the note that spawns for player 1, and spawns in the same lane as well.
    /// </summary>
    /// <param name="lane"></param>
    void NoteP2Spawn(Lane lane)
    {
        GameObject p2Note = Instantiate(noteEnemy, lane.gameObject.transform);
        p2Note.layer = LayerMask.NameToLayer("p2");
        foreach (var trans in p2Note.GetComponentsInChildren<Transform>())
        {
            trans.gameObject.layer = LayerMask.NameToLayer("p2");
        }

        lane.activeNotes2.Add(p2Note.GetComponent<NoteEnemy>());
    }

    /// <summary>
    /// Disables a noteobject and adds it to the pool of objects that can be reinstantiated.
    /// Object will be destroyed instead of being pooled if the list is at the max capacity.
    /// </summary>
    /// <param name="noteObj"></param>
    public void PoolObject(NoteEnemy noteObj)
    {
        Lane parentLane = noteObj.transform.parent.GetComponent<Lane>();
        if (parentLane.activeNotes.Contains(noteObj))
            parentLane.activeNotes.Remove(noteObj);
        else
            parentLane.activeNotes2.Remove(noteObj);

        if (pooledNotes.Count >= notePoolLimit)
        {
            Destroy(noteObj.gameObject);
            return;
        }

        noteObj.gameObject.SetActive(false);
        noteObj.canMove = false;
        pooledNotes.Add(noteObj);
        noteObj.transform.parent = null;
    }
}