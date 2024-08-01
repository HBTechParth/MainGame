using UnityEngine;
using UnityEngine.Serialization;

public class GraphManager : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject rocketObject;
    public GameObject lineTargetObject;
    public LineRenderer tailLineRenderer;
    public float speed = 1f;
    public Vector2 controlPoint1 = Vector2.zero;
    public Vector2 controlPoint2 = Vector2.zero;
    public int numPoints = 50;
    public Vector2 lineOffset = Vector2.zero;
    
    // X-axis and Y-axis line variables
    public GameObject xAxisLine;
    public GameObject yAxisLine;
    public float xAxisSpeed = 0.1f;
    public float yAxisSpeed = 0.1f;
    
    private float journeyLength;
    private Vector3 xAxisStartPosition;
    private Vector3 yAxisStartPosition;
    
    private bool isGameRunning = false;
    

    /*private void Start()
    {
        // Set the initial position of the moving object to the start point
        var position = startPoint.transform.position;
        rocketObject.transform.position = position;

        // Calculate the journey length using the Bezier curve points
        journeyLength = CalculateBezierCurveLength();

        // Set the initial tail position
        tailLineRenderer.positionCount = 1;
        tailLineRenderer.SetPosition(0, position - (Vector3)lineOffset);
        
        // Store the starting positions of X-axis and Y-axis lines
        xAxisStartPosition = xAxisLine.transform.position;
        yAxisStartPosition = yAxisLine.transform.position;
        
        AviatorGameManager.Instance.OnGameStart += HandleGameStart;
        AviatorGameManager.Instance.OnGameCrash += HandleGameCrash;
        AviatorGameManager.Instance.OnGameRestart += HandleGameRestart;
        ResetLines();
        isGameRunning = false;
        
        print("GraphManager is called");
    }*/
    
    public void InitializeGraphManager()
    {
        // Set the initial position of the moving object to the start point
        var position = startPoint.transform.position;
        rocketObject.transform.position = position;

        // Calculate the journey length using the Bezier curve points
        journeyLength = CalculateBezierCurveLength();

        // Set the initial tail position
        tailLineRenderer.positionCount = 1;
        tailLineRenderer.SetPosition(0, position - (Vector3)lineOffset);
        
        // Store the starting positions of X-axis and Y-axis lines
        xAxisStartPosition = xAxisLine.transform.position;
        yAxisStartPosition = yAxisLine.transform.position;
        
        AviatorGameManager.Instance.OnGameStart += HandleGameStart;
        AviatorGameManager.Instance.OnGameCrash += HandleGameCrash;
        AviatorGameManager.Instance.OnGameRestart += HandleGameRestart;
        ResetLines();
        isGameRunning = false;
        
        print("GraphManager is called");
    }
    
    private void OnDestroy()
    {
        AviatorGameManager.Instance.OnGameStart -= HandleGameStart;
        AviatorGameManager.Instance.OnGameCrash -= HandleGameCrash;
        AviatorGameManager.Instance.OnGameRestart -= HandleGameRestart;
    }
    
    private void HandleGameStart()
    {
        ResetLines();
        isGameRunning = true;
    }
    
    private void HandleGameCrash()
    {
        isGameRunning = false;
    }
    
    public void HandleGameRestart()
    {
        isGameRunning = false;
        ResetLines();
        ResetRocketPosition();
    }


    private void Update()
    {
        if (isGameRunning)
        {
            RocketMovement();
        }
    }
    
    private void ResetRocketPosition()
    {
        // Reset the rocket position to the starting point
        rocketObject.transform.position = startPoint.transform.position;
    }


    private void RocketMovement()
    {
        // Check if the moving object has reached the end point
        if (Vector2.Distance(rocketObject.transform.position, endPoint.transform.position) > 0.01f)
        {
            // Calculate the distance to move based on the fixed speed and time
            float distanceToMove = speed * Time.deltaTime;
            
            // Calculate the current position along the Bezier curve
            float currentDistance = Vector2.Distance(startPoint.transform.position, rocketObject.transform.position);
            float fractionOfJourney = currentDistance / journeyLength;

            // Calculate the next position along the Bezier curve
            float nextDistance = currentDistance + distanceToMove;
            float nextFractionOfJourney = nextDistance / journeyLength;

            // Move the moving object along the Bezier curve path
            Vector2 position = CalculateBezierPoint(fractionOfJourney);
            Vector2 nextPosition = CalculateBezierPoint(nextFractionOfJourney);

            // Set the Z position to 0
            position = new Vector3(position.x, position.y, 0f);
            nextPosition = new Vector3(nextPosition.x, nextPosition.y, 0f);

            // Move the rocket towards the next position
            rocketObject.transform.position = Vector2.MoveTowards(rocketObject.transform.position, nextPosition, distanceToMove);

            // Calculate the direction towards the next point on the curve
            Vector2 direction = (nextPosition - position).normalized;

            // Rotate the moving object to face the direction of movement
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rocketObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Add the current position to the line renderer, offset behind the rocket
            var positionCount = tailLineRenderer.positionCount;
            positionCount++;
            tailLineRenderer.positionCount = positionCount;
            tailLineRenderer.SetPosition(positionCount - 1, lineTargetObject.transform.position - (Vector3)lineOffset);
        }
        else
        {
            // Move the X-axis and Y-axis lines when the rocket reaches the end point
            MoveXAxisLine();
            MoveYAxisLine();

            // Log a message when the moving object reaches the end point
            //Debug.Log("Moving object has reached the end point!");
        }
    }

    #region X and Y axis Line Movement

    private void MoveXAxisLine()
    {
        // Move the X-axis line in the negative x-direction
        Vector3 newPosition = xAxisLine.transform.position;
        newPosition.x -= xAxisSpeed * Time.deltaTime;
        xAxisLine.transform.position = newPosition;
    }

    private void MoveYAxisLine()
    {
        // Move the Y-axis line in the negative y-direction
        Vector3 newPosition = yAxisLine.transform.position;
        newPosition.y -= yAxisSpeed * Time.deltaTime;
        yAxisLine.transform.position = newPosition;
    }

    public void ResetLines()
    {
        // Reset the X-axis and Y-axis lines to their original positions
        xAxisLine.transform.position = xAxisStartPosition;
        yAxisLine.transform.position = yAxisStartPosition;
        
        // Reset the tail line renderer
        tailLineRenderer.positionCount = 1;
        tailLineRenderer.SetPosition(0, startPoint.transform.position - (Vector3)lineOffset);
    }

    #endregion

    #region BezierCurve Calculation

    private Vector2 CalculateBezierPoint(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        var position1 = startPoint.transform.position;
        Vector2 p0 = new Vector2(position1.x, position1.y);
        Vector2 p1 = p0 + controlPoint1;
        var position2 = endPoint.transform.position;
        Vector2 p2 = new Vector2(position2.x, position2.y) + controlPoint2;
        Vector2 p3 = new Vector2(position2.x, position2.y);

        Vector2 position = uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
        return position;
    }

    private float CalculateBezierCurveLength()
    {
        float length = 0f;
        var position = startPoint.transform.position;
        Vector2 prevPoint = new Vector2(position.x, position.y);

        for (int i = 1; i <= numPoints; i++)
        {
            float t = i / (float)numPoints;
            Vector2 currentPoint = CalculateBezierPoint(t);
            length += Vector2.Distance(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }

        return length;
    }

    #endregion

    #region Gizmose Test lines

    /*private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        Vector2 prevPoint = startPoint.transform.position;

        for (int i = 1; i <= numPoints; i++)
        {
            float t = i / (float)numPoints;
            Vector2 currentPoint = CalculateBezierPoint(t);
            Gizmos.DrawLine(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }#1#
        
        // Draw the tail line
        Gizmos.color = Color.blue;
        Vector2 tailStartPosition = (Vector2)startPoint.transform.position + lineOffset;
        Vector2 tailEndPosition = (Vector2)rocketObject.transform.position + lineOffset;
        Gizmos.DrawLine(tailStartPosition, tailEndPosition);
    }*/

    #endregion
    
}
