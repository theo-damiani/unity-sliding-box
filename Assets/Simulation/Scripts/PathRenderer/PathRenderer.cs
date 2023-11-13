using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [SerializeField] private BoolReference showPath;
    [SerializeField] private Material materialTrail;
    [SerializeField] private GameObject startTrailIndicator;
    [SerializeField] private BoolVariable isForceActive;
    [SerializeField] private GameObject startForceIndicator;
    [SerializeField] private GameObject endForceIndicator;
    private TrailRenderer trailRenderer;
    private GameObject startPoint;
    private bool isStartPointSet = false;
    private List<GameObject> startForcePoints;
    private List<GameObject> endForcePoints;
    private bool isStartForceIndicatorSet = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!TryGetComponent<TrailRenderer>(out trailRenderer))
        {
            trailRenderer = gameObject.AddComponent<TrailRenderer>();
        }

        trailRenderer.Clear();

        trailRenderer.material = materialTrail;

        // Trail Renderer On:
        trailRenderer.autodestruct = false;
        trailRenderer.enabled = showPath.Value;
        trailRenderer.time = float.PositiveInfinity;
        //trailRenderer.time = 5f;
        trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        AnimationCurve curve = new AnimationCurve();
        float curveWidth = 0.1f;
        curve.AddKey(0f, curveWidth);
        curve.AddKey(1f, curveWidth);
        trailRenderer.widthCurve = curve;

        trailRenderer.sortingOrder = 1;

        isStartForceIndicatorSet = false;
        startForcePoints = new List<GameObject>();
        endForcePoints = new List<GameObject>();
    }

    void Update()
    {
       if (!isStartPointSet && trailRenderer)
        {
            if (trailRenderer.positionCount == 0)
            {
                return;
            }

            Vector3 startPosition = trailRenderer.GetPosition(trailRenderer.positionCount-1);
            startPoint = Instantiate(startTrailIndicator);
            startPoint.transform.localPosition = startPosition;
            isStartPointSet = true;

            return;
        } 

        if ((!isStartForceIndicatorSet) && isForceActive.Value)
        {
            GameObject startThurstPoint = Instantiate(startForceIndicator);
            startThurstPoint.transform.localPosition = transform.localPosition;
            startForcePoints.Add(startThurstPoint);

            isStartForceIndicatorSet = true;
        }

        if (isStartForceIndicatorSet && (!isForceActive.Value))
        {
            GameObject endThurstPoint = Instantiate(endForceIndicator);
            endThurstPoint.transform.localPosition = transform.localPosition;
            endForcePoints.Add(endThurstPoint);
            isStartForceIndicatorSet = false;
        }
    }

    public void OnUpdateShowPath()
    {
        SetPathRenderer(showPath.Value);
    }

    public void SetPathRenderer(bool enable)
    {
        ClearPath();
        if (trailRenderer)
        {
            trailRenderer.enabled = enable;
        }
    }

    public void ClearPath()
    {
        if (trailRenderer)
        {
            trailRenderer.Clear();
        }
        if (startPoint)
        {
            Destroy(startPoint);
            isStartPointSet = false;
        }

        if (startForcePoints!=null)
        {
            DestroyGameObjectList(startForcePoints);
        }
        if (endForcePoints!=null)
        {
            DestroyGameObjectList(endForcePoints);
        }
    }

    private void DestroyGameObjectList(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i]);
        }
        gameObjects.Clear();
    }
}