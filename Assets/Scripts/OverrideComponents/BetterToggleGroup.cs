using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BetterToggleGroup : ToggleGroup
{
    public delegate void ChangedEventHandler(Toggle newActive);

    public event ChangedEventHandler OnChange;
    public void Start()
    {
        int a = 1;

        foreach (Transform transformToggle in gameObject.transform)
        {
            var toggle = transformToggle.gameObject.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((isSelected) => {
                if (!isSelected)
                {
                    return;
                }
                var activeToggle = Active();
                DoOnChange(activeToggle);
            });
        }
    }
    public Toggle Active()
    {
        return ActiveToggles().FirstOrDefault();
    }

    protected virtual void DoOnChange(Toggle newactive)
    {
        OnChange?.Invoke(newactive);
    }

    public int GetActiveIndex()
    {
        return System.Array.IndexOf(this.gameObject.transform.GetComponentsInChildren<Toggle>(), this.ActiveToggles().FirstOrDefault());
    }
}