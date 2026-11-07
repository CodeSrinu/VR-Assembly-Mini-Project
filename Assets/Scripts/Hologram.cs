using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Hologram : MonoBehaviour
{
    public Material hologramMat;
    public Material correctFeedbackMat;
    public Material incorrectFeedbackMat;
    private GameObject currentHoloGramObj;
    private MeshRenderer hologramMRender;

    private void OnEnable()
    {
        AssemblyManager.AssemblyStepGiven += OnNewStepGiven;
        AssemblyManager.AssemblyComplete += OnAssemblyEnded;
    }

    private void OnAssemblyEnded()
    {
        //Destroy(currentHoloGramObj);
        currentHoloGramObj.SetActive(false);
    }

    private void OnNewStepGiven(AssemblyStep step)
    {
        ShowCurrentStepHologram(step.requiredPart, step.socket.attachTransform);
    }

    private void ShowCurrentStepHologram(GameObject partMesh, Transform socketTransform)
    {
        //if(currentHoloGramObj != null) Destroy(currentHoloGramObj);
        if(currentHoloGramObj != null) currentHoloGramObj.SetActive(false);

        currentHoloGramObj = Instantiate(partMesh, socketTransform.position, socketTransform.rotation);
        ResetObj(currentHoloGramObj);
        hologramMRender = currentHoloGramObj.GetComponent<MeshRenderer>();
        ResetToHologramMat();
    }

    private void ResetObj(GameObject obj)
    {
        Component[] allComponents = obj.GetComponents<Component>();
        foreach(var comp in allComponents)
        {
            if(comp is XRGrabInteractable ||  comp is GrabbableOutline || comp is MeshCollider)
            {
                Destroy(comp);
            }
        }

        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Rigidbody>().useGravity = false;
        if(obj.transform.childCount > 0)  Destroy(obj.transform.GetChild(0).gameObject);
    }

    public void RemoveCurrentHologram()
    {
        currentHoloGramObj?.SetActive(false);

    }
    public void AddCurrentHologram()
    {
        currentHoloGramObj?.SetActive(true);
    }

    public void ShowIncorrectFeedBack()
    {
        hologramMRender.materials = new Material[] { incorrectFeedbackMat };
    }
    public void ShowCorrectFeedBack()
    {
        hologramMRender.materials = new Material[] { correctFeedbackMat };
    }
    public void ResetToHologramMat()
    {
        hologramMRender.materials = new Material[] { hologramMat };
    }
    
}
