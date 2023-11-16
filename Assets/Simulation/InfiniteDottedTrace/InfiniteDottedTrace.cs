using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteDottedTrace : MonoBehaviour
{
    private Transform tx;
    private ParticleSystem.Particle[] points;
 
    [SerializeField] private int systemSize = 10;
    [SerializeField] private float markerStepInSeconds = 0.1f;
    [SerializeField] private float markerSize = 1;
    private int indexParticleSet = 0;
    private float time = 0;
 
 
    void Start () {
        tx = transform;

        // Init particle System
        points = new ParticleSystem.Particle[systemSize];
        for (int i = 0; i < systemSize; i++) {
            points[i].position = tx.position;
            points[i].startColor = new Color(1,1,1, 1);
            points[i].startSize = 0;
        }
        indexParticleSet = 0;
    }
 
    // Update is called once per frame
    void FixedUpdate () {

        if (time < markerStepInSeconds)
        {
            time += Time.fixedDeltaTime;
            return;
        }

        if (indexParticleSet == systemSize)
        {
            indexParticleSet = 0;
        }
        
        points[indexParticleSet].position = tx.position;
        // points[indexParticleSet].startColor = new Color(1,1,1, 1);
        points[indexParticleSet].startSize = markerSize;

        indexParticleSet++;
 
        time = 0;
        GetComponent<ParticleSystem>().SetParticles ( points, points.Length ); 
    }
}
