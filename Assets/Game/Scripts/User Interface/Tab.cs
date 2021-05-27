using UnityEngine;

public class Tab : MonoBehaviour
{
    [SerializeField]
    private GameObject _tabGroup;

    [SerializeField]
    private GameObject _graphics;

    [SerializeField]
    private Color[] _activeColours;

    [SerializeField]
    private Color[] _inactiveColours;

    [SerializeField]
    private GameObject[] _showOnSelected;

    [SerializeField]
    private GameObject[] _hideOnSelected;
}
