using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class SoundArea : MonoBehaviour {

    public AudioSource Source;

    private void Start() {
        Source = GetComponent<AudioSource>();
    }

    /*private void OnTriggerEnter(Collider other) {
        KBMPlayer player = other.GetComponent<KBMPlayer>();
        if (player != null) {
            if (player.ActiveVoiceSource != null) {
                player.ActiveVoiceSource.Source.Stop();
            }
            player.ActiveVoiceSource = this;
            Source.time = 0f;
            Source.Play();
        }
    }*/

}
