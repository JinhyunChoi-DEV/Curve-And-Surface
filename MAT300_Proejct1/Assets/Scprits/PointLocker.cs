using UnityEngine;

public class PointLocker : MonoBehaviour
{
    GameObject currentObject;

    public bool IsLocked => currentObject != null;

    public bool IsMe(GameObject target)
    {
        if (currentObject == null)
            return false;

        return currentObject == target;
    }

    public void Set(GameObject target)
    {
        if (currentObject == null)
            currentObject = target;
    }

    public void Release()
    {
        currentObject = null;
    }
}
