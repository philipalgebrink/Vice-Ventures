using UnityEngine;

public abstract class UIManagerBase : MonoBehaviour
{

    public virtual void OpenUI()
    {
        // gameObject.SetActive(true);

    }

    public abstract void CloseUI();
}