using UnityEngine;

public class SectionView : MonoBehaviour
{
    public Mesh originalMesh;
    public Mesh sectionViewMesh;
    public Material cutsurfaceMat;
    private Material fullMat;

    private MeshFilter meshFilter;
    private MeshRenderer mr;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        fullMat = mr.material;
    }

    private void OnEnable()
    {
        SectionViewUI.SectionViewToggled += OnSectionViewToggeled;
    }

    private void OnSectionViewToggeled(bool isInSectionView)
    {
        if (isInSectionView)
        {
            meshFilter.mesh = sectionViewMesh;
            mr.materials = new Material[] { fullMat, cutsurfaceMat };
        }
        else
        {
            meshFilter.mesh = originalMesh;
            mr.materials = new Material[] { fullMat};
        }
    }
}
