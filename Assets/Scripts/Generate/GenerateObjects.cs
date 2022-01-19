using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateObjects : MonoBehaviour
{
    public bool MeanHeight = true;

    //public bool isGenerateGrass = true;
    //public List<GameObject> GrassObj;

    [SerializeField]
    private MeshFilter GenerateSurface;
    private List<Vector3> EmployedPosition = new List<Vector3>();

    public GameObject BlockedZones;

    //public List<GameObject> GenObjects;

    [Space(20)]
    [Tooltip("Куда упаковывать")]
    public GameObject ParentGenerateTrees;
    [Tooltip("Генерация деревьев")]
    public bool isGenerateTrees = true;
    [Tooltip("Шанс создания дерева")]
    public float ChanceGenerateTree = 7f;
    [Tooltip("Генерируемые деревья")]
    public List<GameObject> GenerateTrees;

    [Space(20)]
    [Tooltip("Куда упаковывать")]
    public GameObject ParentGenerateRocks;
    [Tooltip("Генерация каменй")]
    public bool isGenerateRock = true;
    [Tooltip("Шанс создания камня")]
    public float ChanceGenerateRock = 4f;
    [Tooltip("Генерируемые камни")]
    public List<GameObject> GenerateRock;

    [Space(20)]
    [Tooltip("Куда упаковывать")]
    public GameObject ParentGenerateOther;
    [Tooltip("Генерация прочего")]
    public bool isGenerateOther = true;
    [Tooltip("Шанс создания прочего")]
    public float ChanceGenerateOther = 2f;
    [Tooltip("Генерируемое прочее")]
    public List<GameObject> GenerateOther;

    void Start()
    {
        GenerateSurface = GetComponent<MeshFilter>();

        if (isGenerateTrees)
            GenerateTreesLayout();
        if (isGenerateRock)
            GenerateRocksLayout();
        if (isGenerateOther)
            GenerateOtherLayout();

        DisableBlockGenerateZones();
    }

    private void GenerateTreesLayout() {
        IEnumerable<Vector3> vertexs = from vert in GenerateSurface.mesh.vertices.Distinct() select vert;

        foreach (Vector3 vertex in vertexs)
        {
            if (!CheckGeneratePoint(vertex * this.transform.localScale.x))
                continue;
            if (ChanceGenerateTree <= Random.Range(0f,100f))
                continue;
            EmployedPosition.Add(vertex);
            GameObject generateTree = GenerateTrees[Random.Range(0,GenerateTrees.Count)];
            Instantiate(generateTree, (vertex * this.transform.localScale.x) + GenerateSurface.transform.position, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0))).transform.parent = ParentGenerateTrees.transform;
        }
    }

    public void GenerateRocksLayout() {
        IEnumerable<Vector3> vertexs = from vert in GenerateSurface.mesh.vertices.Distinct() select vert;

        foreach (Vector3 vertex in vertexs)
        {
            if (!CheckGeneratePoint(vertex * this.transform.localScale.x))
                continue;
            if (ChanceGenerateRock <= Random.Range(0f, 100f))
                continue;
            EmployedPosition.Add(vertex);
            GameObject generateRock = GenerateRock[Random.Range(0, GenerateOther.Count)];
            Instantiate(generateRock, (vertex * this.transform.localScale.x) + GenerateSurface.transform.position, Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)))).transform.parent = ParentGenerateRocks.transform;
        }
    }

    public void GenerateOtherLayout() {
        IEnumerable<Vector3> all_vertexs = from vert in GenerateSurface.mesh.vertices.Distinct() select vert;

        EmployedPosition.AddRange(all_vertexs);

        IEnumerable<Vector3> vertexs = from vert in EmployedPosition.Distinct() select vert;

        List<Vector3> newPositions = new List<Vector3>();

        foreach (Vector3 vertex in vertexs)
        {
            if (!CheckGeneratePoint(vertex * this.transform.localScale.x))
                continue;
            if (ChanceGenerateOther <= Random.Range(0f, 100f))
                continue;
            newPositions.Add(vertex);
            GameObject generateOther = GenerateOther[Random.Range(0, GenerateOther.Count)];
            Instantiate(generateOther, (vertex * this.transform.localScale.x) + GenerateSurface.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360),0))).transform.parent = ParentGenerateOther.transform;
        }

        EmployedPosition.AddRange(newPositions);
    }

    private bool CheckGeneratePoint(Vector3 point) {
        bool checkAllZones = true;
        foreach (Transform zone in BlockedZones.transform) {
            var zoneColaider = zone.GetComponent<Collider>();
            if (!zoneColaider)
                continue;
            if (zoneColaider.bounds.Contains(point))
                checkAllZones = false;
        }
        return checkAllZones;
    }

    private void DisableBlockGenerateZones() {
        foreach (Transform zone in BlockedZones.transform)
        {
            zone.gameObject.SetActive(false);
        }
    }
}
