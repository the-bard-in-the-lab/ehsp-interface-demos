using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraphGenerator : MonoBehaviour
{
    public double xMin = 0f;
    public double xMax = 100f;
    public double yMin = 0f;
    public double yMax = 50f;    
    public Gradient myGradient;
    private RectTransform myRectTransform;
    [SerializeField] private GameObject myRenderer;
    private List<GameObject> renderSquad;
    int numRenderers = 100;
    public float lineWidth = 2f;
    public Slider widthController;
    public List<Vector2> myCoords = new List<Vector2>{
            new Vector2(0, 0),
            new Vector2(100, 100),
            new Vector2(200, 150),
            new Vector2(500, 300),
        };

    // Start is called before the first frame update
    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        renderSquad = new List<GameObject>();
        widthController.value = lineWidth;
        myRenderer.GetComponent<LineRendererUI>().lineWidth = lineWidth;
        for (int i = 0; i < numRenderers; i ++) {
            renderSquad.Add(Instantiate(myRenderer, gameObject.transform));
            renderSquad[i].SetActive(true);
        }
    }
    public void UpdateWidth() {
        lineWidth = widthController.value;
        foreach (var lr in renderSquad) {
            lr.GetComponent<LineRendererUI>().SetLineWidth(lineWidth);
        }
    }

    public void DrawGraph(List<Vector2> points, List<float> segColors) {
        myCoords = points;
        
        double xScalar = myRectTransform.rect.width / (xMax - xMin);
        double yScalar = myRectTransform.rect.height / (yMax - yMin);

        double xShift = xMin;
        double yShift = yMin;

        List<Vector2> pointsConverted = new List<Vector2>();

        for (int i = 0; i < points.Count; i ++) {
            pointsConverted.Add(new Vector2((float) ((points[i].x - xShift) * xScalar),
                                            (float) ((points[i].y - yShift) * yScalar)));
        }
        
        // Multi-renderer version:
        if (pointsConverted.Count > 1) {
            int start = 1;
            int end = pointsConverted.Count - 1;
            if (pointsConverted.Count > numRenderers) {
                start = pointsConverted.Count - numRenderers;
                end = numRenderers;
            }
            
            for (int i = 0; i < end; i ++) {
                // Uncomment for Debug information
                /*
                Debug.Log($"renderSquad.Count: {renderSquad.Count}");
                Debug.Log($"pointsConverted.Count: {pointsConverted.Count}");
                Debug.Log($"start + i - 1: {start + i - 1}");
                Debug.Log($"end: {end}");
                */
                LineRendererUI thisRenderer = renderSquad[i].GetComponent<LineRendererUI>();
                thisRenderer.SetVertices(new List<Vector2> {pointsConverted[start + i - 1], pointsConverted[start + i]});
                thisRenderer.color = myGradient.Evaluate(segColors[start + i + 1]);
            }
        }
    }
}
