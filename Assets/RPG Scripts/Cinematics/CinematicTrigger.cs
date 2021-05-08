﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool played = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if(!played && other.gameObject.tag == "Player")
            {
                played = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}