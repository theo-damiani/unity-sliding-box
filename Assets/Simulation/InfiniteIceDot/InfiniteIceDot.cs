using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteIceDot : MonoBehaviour
{
    [SerializeField] private Vector3Reference lineDirection;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3Reference camDistToTarget;
    [SerializeField, Range(0, 1)] private float markersFadeOut;
    private ParticleSystem.Particle[] markers;
    private Vector3 lineDirectionNormalized;
    private float lineHalfSize;
    private Vector3 lineHalfSizeVector3;
    private float lineHalfSizeSqr;
    private float distOffsetSqr = 1;

    void Start()
    {
        lineDirectionNormalized = lineDirection.Value.normalized;

        //lineHalfSize = markersOffset*(nbOfMarkers-1)/2;
        lineHalfSizeVector3 = lineDirectionNormalized*lineHalfSize;
        lineHalfSizeSqr = (lineHalfSize*lineDirectionNormalized).sqrMagnitude;

        distOffsetSqr = lineHalfSizeSqr-(markersFadeOut * lineHalfSize *  lineDirectionNormalized).sqrMagnitude;

        var psMain = GetComponent<ParticleSystem>().main;
        psMain.startSize3D = true;
        psMain.startRotation3D = true;

        InitMarkerDot();
    }

    public void SetLineHalfSize()
    {
        Vector3 transformInScreen = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 leftCamPointInWorld = mainCamera.ScreenToWorldPoint(new Vector3(0, transformInScreen.y, camDistToTarget.Value.magnitude));
        lineHalfSize = (leftCamPointInWorld - transform.position).magnitude;
        lineHalfSizeVector3 = lineDirectionNormalized*lineHalfSize;
        lineHalfSizeSqr = (lineHalfSize*lineDirectionNormalized).sqrMagnitude;
        distOffsetSqr = lineHalfSizeSqr-(markersFadeOut * lineHalfSize *  lineDirectionNormalized).sqrMagnitude;

    }

    public void InitMarkerDot()
    {
        Vector3 rotationTowardsLineDirection = Quaternion.LookRotation(lineDirection.Value).eulerAngles;

        // Init markers
        markers = new ParticleSystem.Particle[1];
        markers[0].position = transform.position + (lineHalfSize * lineDirectionNormalized);
        markers[0].startSize3D = new Vector3(0.1f,0.1f,0.1f);
        // Marker are oriented such that their forward (blue) axis are along the line direction;
        markers[0].rotation3D = rotationTowardsLineDirection;
        markers[0].startColor = new Color(1,1,1,1);
        GetComponent<ParticleSystem>().SetParticles ( markers, markers.Length );
    }

    private bool isFullOpaque = false;
    private bool systemNeedUpdate = false;

    void Update () 
    {
        Vector3 distance = markers[0].position - transform.position;
        float distSqr = distance.sqrMagnitude;
        if ((distSqr - lineHalfSizeVector3.sqrMagnitude) > 0)
        {
            markers[0].position = transform.position + (lineHalfSize * lineDirectionNormalized);
            systemNeedUpdate = true;
        }

        if (distSqr >= distOffsetSqr) 
        {
            float percent = (distSqr - distOffsetSqr) / (lineHalfSizeSqr-distOffsetSqr);
            markers[0].startColor = new Color(1,1,1, 1-percent);
            isFullOpaque = false;
            systemNeedUpdate = true;
        }
        else
        {
            if (isFullOpaque)
            {
                markers[0].startColor = new Color(1,1,1, 1);
                isFullOpaque = true;
                systemNeedUpdate = true;
            }
        }

        if (systemNeedUpdate)
        {
            GetComponent<ParticleSystem>().SetParticles ( markers, 1 );
            systemNeedUpdate = false;
        }
    }
}
