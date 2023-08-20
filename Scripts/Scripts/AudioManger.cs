using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour{
    //
    private float _timer;
    //
    private int _count;

    void Start(){
        _timer = 0;
        _count = 0;

    }

    // Update is called once per frame
    void FixedUpdate(){
        _timer += Time.deltaTime;
        if (_timer >= 1.5f && _count == 0) {
            GetComponent<AudioSource>().Play();
            _count++;

        }

    }

}
