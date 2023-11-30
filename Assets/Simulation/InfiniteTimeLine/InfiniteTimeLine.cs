using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(LineRenderer))]
public class InfiniteTimeLine : MonoBehaviour
{
    [SerializeField] private Vector3Reference lineDirection;
    [SerializeField] private int nbOfMarkers;
    [SerializeField] private float markersOffset;
    [SerializeField, Range(0, 1)] private float markersFadeOut;
    private ParticleSystem.Particle[] markers;
    private Vector3 lineDirectionNormalized;
    private int indexParticleSet = 0;
    private float lineHalfSize;
    private Vector3 lineHalfSizeVector3;
    private float lineHalfSizeSqr;
    private float distOffsetSqr = 1;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineDirectionNormalized = lineDirection.Value.normalized;

        lineRenderer = GetComponent<LineRenderer>();


        if (nbOfMarkers % 2 == 0)
        {
            nbOfMarkers++; // round to odd number so first marker is at tx position;
        }
        lineHalfSize = markersOffset*(nbOfMarkers-1)/2;
        lineHalfSizeVector3 = lineDirectionNormalized*lineHalfSize;
        lineHalfSizeSqr = (lineHalfSize*lineDirectionNormalized).sqrMagnitude;

        distOffsetSqr = ((1-markersFadeOut*2) * lineHalfSize *  lineDirectionNormalized).sqrMagnitude;

        nbOfMarkers--; // we do not want to add a marker at the top left position on the line.

        var psMain = GetComponent<ParticleSystem>().main;
        psMain.startSize3D = true;
        psMain.startRotation3D = true;

        InitTimeLine();
    }

    public void InitTimeLine()
    {
        // Init line

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4;
        SetLineRendererPosition();

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(0.0f, 0.0f),
                new GradientAlphaKey(1.0f, markersFadeOut), 
                new GradientAlphaKey(1.0f, 1 - markersFadeOut), 
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        lineRenderer.colorGradient = newGradient; 

        // GameObject lineGo = Instantiate(linePrefab, transform);
        // line = lineGo.transform;
        // line.position = transform.position;
        // Vector3 newScale = line.localScale;
        // newScale.z = lineHalfSize*2;
        // line.localScale = newScale;
        Vector3 rotationTowardsLineDirection = Quaternion.LookRotation(lineDirection.Value).eulerAngles;
        // line.Rotate(rotationTowardsLineDirection);

        // Init markers
        markers = new ParticleSystem.Particle[nbOfMarkers];
        for (int i = 0; i < nbOfMarkers; i++) {
            markers[i].position = transform.position + (lineHalfSize * -lineDirectionNormalized) + (markersOffset * i * lineDirectionNormalized);
            //markers[i].position += lineDirectionNormalized*0.1f;
            markers[i].startSize3D = new Vector3(0.8f,0.1f,0.1f);
            // Marker are oriented such that their forward (blue) axis are along the line direction;
            markers[i].rotation3D = rotationTowardsLineDirection;
            markers[i].startColor = new Color(1,1,1,1);
            //markers[i].startSize = markerSize;
        }
        indexParticleSet = nbOfMarkers-1;
        GetComponent<ParticleSystem>().SetParticles ( markers, markers.Length );

        markerColorUpdate = false;
    }

    void SetLineRendererPosition()
    {
        lineRenderer.SetPosition(0, transform.position + (lineHalfSize * -lineDirectionNormalized));
        lineRenderer.SetPosition(1, transform.position + (1-markersFadeOut*2) * lineHalfSize * -lineDirectionNormalized);
        lineRenderer.SetPosition(2, transform.position + (1-markersFadeOut*2) * lineHalfSize *  lineDirectionNormalized);
        lineRenderer.SetPosition(3, transform.position + (lineHalfSize * lineDirectionNormalized));
    }

    private bool markerColorUpdate = false;

    void FixedUpdate () 
    {
        if (indexParticleSet == -1)
        {
            indexParticleSet = nbOfMarkers-1;
        }

        SetLineRendererPosition();

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

            float distSqr = distance.sqrMagnitude;
            if (distSqr >= distOffsetSqr) 
            {
                float percent = (distSqr - distOffsetSqr) / (lineHalfSizeSqr-distOffsetSqr);
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
