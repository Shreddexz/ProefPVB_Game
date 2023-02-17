using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.MusicTheory;

public class NoteSpawner : MonoBehaviour
{
    public NoteName C3;
    public NoteName D3;
    public NoteName E3;

    public GameObject noteObject;
    List<Note> notes = new();
    public List<double> timeStamps = new();
    int spawnIndex;
    int inputIndex;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
