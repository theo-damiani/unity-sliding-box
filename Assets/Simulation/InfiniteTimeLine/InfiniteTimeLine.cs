using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTimeLine : MonoBehaviour
{
    [SerializeField] private Transform tx;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Vector3Reference lineDirection;
    [SerializeField] private int nbOfMarkers;
    [SerializeField] private float markersOffset;
    private Transform line;
    private ParticleSystem.Particle[] markers;
    private Vector3 lineDirectionNormalized;
    private int indexParticleSet = 0;
    private float lineHalfSize;
    private float lineHalfSizeSqr;
    private Rigidbody parentRigidbody;

    void Start()
    {
        tx = transform;
        lineDirectionNormalized = lineDirection.Value.normalized;

        parentRigidbody = GetComponentInParent<Rigidbody>();


        if (nbOfMarkers % 2 == 0)
        {
            nbOfMarkers++; // round to odd number so first marker is at tx position;
        }
        lineHalfSize = markersOffset*(nbOfMarkers-1)/2;
        lineHalfSizeSqr = (lineHalfSize*lineDirectionNormalized).sqrMagnitude;

        var psMain = GetComponent<ParticleSystem>().main;
        psMain.startSize3D = true;
        psMain.startRotation3D = true;

        InitTimeLine();
    }

    void InitTimeLine()
    {
        // Init line
        GameObject lineGo = Instantiate(linePrefab, transform);
        line = lineGo.transform;
        line.position = transform.position;
        Vector3 newScale = line.localScale;
        newScale.z = lineHalfSize*2;
        line.localScale = newScale;
        Vector3 rotationTowardsLineDirection = Quaternion.LookRotation(lineDirection.Value).eulerAngles;
        line.Rotate(rotationTowardsLineDirection);

        // Init markers
        markers = new ParticleSystem.Particle[nbOfMarkers];
        for (int i = 0; i < nbOfMarkers; i++) {
            markers[i].position = transform.position + (lineHalfSize * -lineDirectionNormalized) + (markersOffset * i * lineDirectionNormalized);
            markers[i].startSize3D = new Vector3(1f,0.1f,0.1f);
            // Marker are oriented such that their forward (blue) axis are along the line direction;
            markers[i].rotation3D = rotationTowardsLineDirection;
            //markers[i].startColor = new Color(1,1,1, 1);
            //markers[i].startSize = markerSize;
        }
        indexParticleSet = nbOfMarkers-1;
        GetComponent<ParticleSystem>().SetParticles ( markers, markers.Length );
    }

    void FixedUpdate () 
    {
        if (indexParticleSet == -1)
        {
            indexParticleSet = nbOfMarkers-1;
        }
        //Vector3 lineNorm = parentRigidbody.velocity.normalized;
        if((markers[indexParticleSet].position - transform.position).sqrMagnitude > lineHalfSizeSqr) 
        {
            markers[indexParticleSet].position = transform.position + (lineHalfSize * -lineDirectionNormalized);

            indexParticleSet--;
            GetComponent<ParticleSystem>().SetParticles ( markers, markers.Length );
        }
        

    }
}
