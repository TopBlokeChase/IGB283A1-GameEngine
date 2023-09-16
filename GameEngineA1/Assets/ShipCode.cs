using UnityEngine;
using System.Collections;
public class ShipCode : MonoBehaviour {
    public Material material;
    private Mesh mesh;

    // Vertices of the ship in its base form
    private Vector3[] baseVertices;
    // Transformation of ship compared to base geometry
    [SerializeField] private float rotation;
    [SerializeField] private Vector3 scale;
    [SerializeField] private Vector3 position;

    public float speed = 1f;
    public float rotationSpeed = 90f; // deg/sec
    private bool hasCargo = false;
    public Vector3 pointA = new Vector3(-10f,0f,0f);
    public Vector3 pointB = new Vector3(10f,0f,0f);

    
    // Use this for initialization
    void Start() {
        // Add a MeshFilter and MeshRenderer to the Empty GameObject
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        // Get the Mesh from the MeshFilter
        mesh = GetComponent<MeshFilter>().mesh;

        // Set the material to the material we have selected
        GetComponent<MeshRenderer>().material = material;

        // Clear all vertex and index data from the mesh
        mesh.Clear();

        // Define each of the ships vertices
        baseVertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f), // 0
            new Vector3(-1f, 5f, 0f), // 1
            new Vector3(-3f, 4.5f, 0f), // 2
            new Vector3(-1f, 2.5f, 0f), // 3
            new Vector3(-3f, 0.5f, 0f), // 4
            new Vector3(-3f, -3f, 0f), // 5
            new Vector3(-1.5f, -3f, 0f), // 6
            new Vector3(-3.5f, -5f, 0f), // 7
            new Vector3(-1f, -5f, 0f), // 8
            new Vector3(1f, 5f, 0f), // 9
            new Vector3(3f, 4.5f, 0f), // 10
            new Vector3(1f, 2.5f, 0f), // 11 
            new Vector3(3f, 0.5f, 0f), // 12
            new Vector3(3f, -3f, 0f), // 13
            new Vector3(1.5f, -3f, 0f), // 14
            new Vector3(3.5f, -5f, 0f), // 15
            new Vector3(1f, -5f, 0f) // 16
        };

        // Set default parameters
        rotation = 0f;
        scale = new Vector3(1f,1f,1f);
        position = new Vector3(0f,0f,0f);

        // Set the mesh vertices
        mesh.vertices = baseVertices;

        // Set the colour of the triangle
        mesh.colors = new Color[]
        {
            new Color(0.8f, 0.8f, 0.8f, 1.0f), // 0
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 1
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 2
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 3
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 4
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 5
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 6
            new Color(0.8f, 0.8f, 0.3f, 1.0f), // 7
            new Color(0.8f, 0.8f, 0.3f, 1.0f), // 8
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 9
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 10
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 11
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 12
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 13
            new Color(0.8f, 0.3f, 0.3f, 1.0f), // 14
            new Color(0.8f, 0.8f, 0.3f, 1.0f), // 15
            new Color(0.8f, 0.8f, 0.3f, 1.0f) // 16
        };
    }

    void Update()
    {
        Move();
        Transform();
        Draw();
    }

    private void Move()
    {
        float pointDir;
        if (position[0] < pointA[0]) {hasCargo = true;}
        if (position[0] > pointB[0]) {hasCargo = false;}
        if (hasCargo)
        {
            pointDir = 270;
            AddPosition(new Vector3(speed, 0f, 0f));
        } else {
            pointDir = 90;
            AddPosition(new Vector3(-speed, 0f, 0f));
        }
        float rotationToTarget = rotation-pointDir;
        if (rotationToTarget != 0)
        {
            if (rotationToTarget < rotationSpeed)
            {
                AddRotation(rotationToTarget);
            }
            else
            {
                AddRotation(rotationSpeed);
            }
        }
        
    }

    // Matrix that scales ship
    private Matrix3x3 ScaleMatrix (Vector3 toScale)
    {
        // Create a new matrix
        Matrix3x3 matrix = new Matrix3x3();
        // Set the rows of the matrix
        matrix.SetRow(0, new Vector3(toScale[0], 0f, 0f));
        matrix.SetRow(1, new Vector3(0f, toScale[1], 0f));
        matrix.SetRow(2, new Vector3(0f, 0f, 1f));
        // Return the matrix
        return matrix;
    }
    // Matrix that rotates the ship
    private Matrix3x3 RotateMatrix (float toRotation)
    {
        // Convert toRotation to radians
        float radRot = toRotation*Mathf.Deg2Rad;
        // Create a new matrix
		Matrix3x3 matrix = new Matrix3x3();
		// Set the rows of the matrix
		matrix.SetRow(0, new Vector3(Mathf.Cos(radRot),
		-Mathf.Sin(radRot), 0.0f));
		matrix.SetRow(1, new Vector3(Mathf.Sin(radRot),
		Mathf.Cos(radRot), 0.0f));
		matrix.SetRow(2, new Vector3(0.0f, 0.0f, 1.0f));
		// Return the matrix
		return matrix;
    }
    // Matrix that positions the mesh
    private Matrix3x3 PositionMatrix (Vector3 toPosition)
    {
        // Create a new matrix
        Matrix3x3 matrix = new Matrix3x3();
        // Set the rows of the matrix
        matrix.SetRow(0, new Vector3(1f, 0f, toPosition[0]));
        matrix.SetRow(1, new Vector3(0f, 1f, toPosition[1]));
        matrix.SetRow(2, new Vector3(0f, 0f, 1f));
        // Return the matrix
        return matrix;
    }

    // Methods to change rotation, scale, and position
    // sets the rotation the base ship is rotated (in degrees)
    public void SetRotation (float newRotation)
    {
        this.rotation = newRotation;
    }
    // Changes the rotation the base ship is rotated by given amount
    //  (in degrees)
    public void AddRotation (float addRotation)
    {
        this.rotation += addRotation;
    }
    // sets the position of the ship
    public void SetPosition (Vector3 newPosition)
    {
        this.position = newPosition;
    }
    // Changes the position of the ship by the given amount
    public void AddPosition (Vector3 addPosition)
    {
        this.position[0] += addPosition[0];
        this.position[1] += addPosition[1];
    }
    // sets the scale of the ship
    public void SetScale (Vector3 newScale)
    {
        this.scale = newScale;
    }
    // Changes the scale of the ship by the given amount
    public void AddScale (Vector3 addScale)
    {
        this.scale[0] += addScale[0];
        this.scale[1] += addScale[1];
    }
    
    // Applies transformations to the ship
    private void Transform()
    {
        // Get the vertices from the matrix
        Vector3[] vertices = new Vector3[baseVertices.Length];
        for (int i = 0; i < baseVertices.Length; i++)
        {
            vertices[i] = baseVertices[i];
        }
        // Clamps rotation between 0 and 360
        if (rotation >=360)
        {
            rotation -= 360;
        }
        if (rotation <0)
        {
            rotation += 360;
        }
        // Get the rotation matrix
        Matrix3x3 S = ScaleMatrix(scale);
        Matrix3x3 R = RotateMatrix(rotation);
        Matrix3x3 P = PositionMatrix(position);

        // Scale each point in the base mesh to its new position
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = S.MultiplyPoint(vertices[i]);
        }
        // Rotate each point in the mesh around the origin
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = R.MultiplyPoint(vertices[i]);
        }
        // Translate each point in the base mesh
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = S.MultiplyPoint(vertices[i]);
        }

        // Set the vertices in the mesh to their new position
        mesh.vertices = vertices;
        // Recalculate the bounding volume
        mesh.RecalculateBounds();
        // Recalculate the bounding volume
        mesh.RecalculateBounds();
    }

    // Draws the ship
    public void Draw()
    {
        // Set vertex indicies
        mesh.triangles = new int[]{1,2,3, 2,3,4, 3,4,0, 4,0,5, 5,0,6, 5,6,7, 6,7,8, 3,11,0, 0,14,6, 9,10,11, 11,10,12, 11,12,0, 0,12,13, 0,13,14, 13,14,15, 14,15,16};
    }
}