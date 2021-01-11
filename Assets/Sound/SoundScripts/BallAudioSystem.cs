﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class BallAudioSystem : MonoBehaviour
{

    AudioSource audioSource;
    Rigidbody myRigidbody;
    float maxSoundVelocityMagnitudeSqr = 10f;
    float minPitch = 0.5f;
    float maxPitch = 1.0f;
    private bool airborne;
    public AudioClip hitSound;
    Coroutine leaveCheck;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();
        audioSource.volume = 0;
        airborne = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (airborne)
        {
            Debug.Log("Playing Hit");
            airborne = false;
            AudioSource.PlayClipAtPoint(hitSound, collision.GetContact(0).point, percentBasedOnVelocityCalc(0.5f));
            //audioSource.PlayOneShot(hitSound, percentBasedOnVelocityCalc());
        }
        //audioSource.volume = 1f;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!airborne)
        {
            
            leaveCheck = StartCoroutine(interruptableLeaveCheck());
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {



        if (leaveCheck != null)
        {
            StopCoroutine(leaveCheck);
        }
        float percentChange = percentBasedOnVelocityCalc(1f);
        audioSource.volume = percentChange;
        audioSource.pitch = minPitch + (percentChange * (maxPitch - minPitch));
        
    }

    private float percentBasedOnVelocityCalc(float velocityMultiplier)
    {
        float rigidbodyVelocityMagnitude = myRigidbody.velocity.sqrMagnitude * velocityMultiplier;
        //Debug.Log("Velocity is" + rigidbodyVelocityMagnitude.ToString());
        return rigidbodyVelocityMagnitude / maxSoundVelocityMagnitudeSqr;
    }

    IEnumerator interruptableLeaveCheck()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Left the zone");
        audioSource.volume = 0f;
        airborne = true;
    }
}