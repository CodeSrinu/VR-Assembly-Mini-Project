using UnityEngine;

public class SectionView : MonoBehaviour
{
    public Mesh originalMesh;
    public Mesh sectionViewMesh;
    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnEnable()
    {
        SectionViewUI.SectionViewToggled += OnSectionViewToggeled;
    }

    private void OnSectionViewToggeled(bool isInSectionView)
    {
        if (isInSectionView)
            meshFilter.mesh = sectionViewMesh;
        else
            meshFilter.mesh = originalMesh;
    }
}
