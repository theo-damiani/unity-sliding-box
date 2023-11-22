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
    private Vector3 lineHalfSizeVector3;
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
        lineHalfSizeVector3 = lineDirectionNormalized*lineHalfSize;
        lineHalfSizeSqr = (lineHalfSize*lineDirectionNormalized).sqrMagnitude;

        nbOfMarkers--; // we do not want to add a marker at the top left position on the line.

        var psMain = GetComponent<ParticleSystem>().main;
        psMain.startSize3D = true;
        psMain.startRotation3D = true;

        InitTimeLine();
    }

    public void InitTimeLine()
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
            //markers[i].position += lineDirectionNormalized*0.1f;
            markers[i].startSize3D = new Vector3(1f,0.1f,0.1f);
            // Marker are oriented such that their forward (blue) axis are along the line direction;
            markers[i].rotation3D = rotationTowardsLineDirection;
            markers[i].startColor = new Color(1,1,1,1);
            //markers[i].startSize = markerSize;
        }
        indexParticleSet = nbOfMarkers-1;
        GetComponent<ParticleSystem>().SetParticles ( markers, markers.Length );

        markerColorUpdate = false;
    }

    private bool markerColorUpdate = false;

    void FixedUpdate () 
    {
        if (indexParticleSet == -1)
        {
            indexParticleSet = nbOfMarkers-1;
        }

        Vector3 distance = markers[indexParticleSet].position - transform.position;
        //if(-(distance.sqrMagnitude - lineHalfSizeSqr) < 0.1f )
        if ((distance - lineHalfSizeVector3).x <= 0)
        {
            markers[indexParticleSet].position = transform.position + (lineHalfSize * -lineDirectionNormalized);
            markers[indexParticleSet].position += distance - lineHalfSizeVector3; // correct the position, because of the precision of FixedUpdate

            indexParticleSet--;
            GetComponent<ParticleSystem>().SetParticles ( markers, markers.Length );
        }
        
        for (int i = 0; i < nbOfMarkers; i++) {
 
            distance = markers[i].position - transform.position;

            if (distance.sqrMagnitude <= lineHalfSizeSqr) 
            {
                float percent = (distance.sqrMagnitude) / (lineHalfSizeSqr);
                markers[i].startColor = new Color(1,1,1, 1-percent);

                markerColorUpdate = true;
            }
        }

        if (markerColorUpdate)
        {
            GetComponent<ParticleSystem>().SetParticles ( markers, markers.Length );
            markerColorUpdate = false;
        }
    }
}
