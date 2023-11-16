using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTimeLineGO : MonoBehaviour
{
    [SerializeField] private Transform tx;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private Vector3Reference lineDirection;
    [SerializeField] private int nbOfMarkers;
    [SerializeField] private float markersOffset;
    private Transform line;
    private GameObject[] markers;
    private Vector3 lineDirectionNormalized;
    private int indexParticleSet = 0;
    private float lineHalfSize;
    private float lineHalfSizeSqr;

    void Start()
    {
        lineDirectionNormalized = lineDirection.Value.normalized;


        if (nbOfMarkers % 2 == 0)
        {
            nbOfMarkers++; // round to odd number so first marker is at tx position;
        }
        lineHalfSize = markersOffset*(nbOfMarkers-1)/2;
        lineHalfSizeSqr = (lineHalfSize*lineDirectionNormalized).sqrMagnitude;

        InitTimeLine();
    }

    void InitTimeLine()
    {
        GameObject topLine = new("Top point on line");
        topLine.transform.parent = transform;
        topLine.transform.position = transform.position + tx.position + ((lineHalfSize+1) * -lineDirectionNormalized);

        // Init line
        GameObject lineGo = Instantiate(linePrefab, transform);
        line = lineGo.transform;
        line.position = transform.position + tx.position;
        Vector3 newScale = line.localScale;
        newScale.z = lineHalfSize*2;
        line.localScale = newScale;
        line.LookAt(topLine.transform);

        // Init markers
        markers = new GameObject[nbOfMarkers];
        for (int i = 0; i < nbOfMarkers; i++) {
            markers[i] = Instantiate(markerPrefab, transform);
            markers[i].name = "Marker " + i;
            markers[i].transform.position = transform.position + tx.position + (lineHalfSize * -lineDirectionNormalized) + (markersOffset * i * lineDirectionNormalized);
            // Marker are oriented such that their forward (blue) axis are along the line direction;
            markers[i].transform.LookAt(topLine.transform);
            //markers[i].startColor = new Color(1,1,1, 1);
            //markers[i].startSize = markerSize;
        }
        indexParticleSet = nbOfMarkers-1;
    }

    void LateUpdate () 
    {
        line.position = tx.position + transform.localPosition;
        if (indexParticleSet == -1)
        {
            indexParticleSet = nbOfMarkers-1;
        }

        if((markers[indexParticleSet].transform.position - (tx.position + transform.localPosition)).sqrMagnitude > lineHalfSizeSqr) 
        {
            markers[indexParticleSet].transform.position = transform.localPosition + tx.position + (lineHalfSize * -lineDirectionNormalized);
            markers[indexParticleSet].GetComponent<Renderer>().material.color = Color.red;
            // Vector3 velocity = GetCurrentVelocity();
            // points[i].position = RandomTangent(velocity) * Random.Range(0f, starMaxSpawnSize) + tx.position + (velocity.normalized * starSpawnDistance);
        
            indexParticleSet--;
        }

    }

    // private Transform tx;
    // private ParticleSystem.Particle[] points;
    // [SerializeField] private Vector3Reference systemDirection;
    // [SerializeField] private int systemSize = 10;
    // [SerializeField] private float desiredMarkerPeriod = 0.1f;
    // private Vector3 systemDirectionNormalized;
    // private int indexParticleSet = 0;
    // private float markerFrequency;
    // private float time = 0;

    // // ======== to remove ?
    // public float markerSize = 1;

    // private Rigidbody parentRigidbody;
 
 
    // void Start () {
    //     tx = transform;
    //     systemDirectionNormalized = systemDirection.Value.normalized;
    // }

    // private void InitSystem()
    // {
    //     // Init particle System
    //     points = new ParticleSystem.Particle[systemSize];
    //     for (int i = 0; i < systemSize; i++) {
    //         points[i].position = tx.position + (desiredMarkerPeriod * i * systemDirectionNormalized);
    //         points[i].startColor = new Color(1,1,1, 1);
    //         points[i].startSize = markerSize;
    //     }
    //     indexParticleSet = systemSize-1;

    //     parentRigidbody = GetComponentInParent<Rigidbody>();
    //     GetComponent<ParticleSystem>().SetParticles ( points, points.Length );
    // }
 
    // // Update is called once per frame
    // void FixedUpdate () 
    // {
    //     if (points == null)
    //     {
    //         InitSystem();
    //     }
    //     if (parentRigidbody.velocity == Vector3.zero)
    //     {
    //         return;
    //     }
        
    //     float freq = desiredMarkerPeriod / parentRigidbody.velocity.magnitude;
    //     if (time < freq)
    //     {
    //         time += Time.fixedDeltaTime;
    //         return;
    //     }

    //     if (indexParticleSet == -1)
    //     {
    //         indexParticleSet = systemSize-1;
    //     }
    //     // Every markers has been created,
    //     // Then modify position of markers
    //     points[indexParticleSet].position = tx.position;
    //     // points[indexParticleSet].startColor = new Color(1,1,1, 1);
    //     points[indexParticleSet].startSize = markerSize;

    //     indexParticleSet--;
 
    //     time = 0;
    //     GetComponent<ParticleSystem>().SetParticles ( points, points.Length );
    // }
}
