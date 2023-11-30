using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonKeyBinder : MonoBehaviour
{
    public KeyCode key; // Key to be set in the Inspector

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(key) && button.gameObject.activeInHierarchy)
        {
            button.onClick.Invoke();
        }
    }
}
