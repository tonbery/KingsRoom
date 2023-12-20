#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;


public class NPCBaseController : MonoBehaviour
{
    [SerializeField] protected Canvas UICanvas;
    protected NPCCanvas _NPCCanvas;
    //public UnityEvent<EBuildingType, EDirection> OnRequestConstruction = new UnityEvent<EBuildingType, EDirection>();
    private PlayerController _playerController;
    private bool _isOnPlayerVision;
    private bool _canShowUI;
    private bool _onPositionToShowUI = true;

    private float _angleToPlayer;
   

    protected virtual void Start()
    {
        //HideUI();
        _playerController = GameMode.Instance.PlayerController;
        _NPCCanvas = UICanvas.GetComponent<NPCCanvas>();
        UICanvas.worldCamera = _playerController.MainCamera;
        _NPCCanvas.OnAccepted.AddListener(OnAcceptedButtonClicked);
    }

    protected virtual void OnAcceptedButtonClicked()
    {
        //OnRequestConstruction.Invoke(EBuildingType.NUM, EDirection.NUM);
    }

    protected virtual void Update()
    {
        if (UICanvas)
        {
            var camForward = _playerController.MainCamera.transform.forward;
            camForward.y = 0;
            UICanvas.transform.forward = camForward;
        }

        _angleToPlayer = _playerController.MainCamera.transform.forward.Angle( transform.position - _playerController.transform.position);
    }


    void UpdateUIState()
    {
        if (_canShowUI && _isOnPlayerVision && _onPositionToShowUI)
        {
            ShowUI();
        }
        else HideUI();
    }
    
    void ShowUI()
    {
        UICanvas.gameObject.SetActive(true);
    }

    void HideUI()
    {
        UICanvas.gameObject.SetActive(false);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, _angleToPlayer.ToString());
    }
    #endif

    public void SetOnPlayerVision(bool state)
    {
        _isOnPlayerVision = state;
        UpdateUIState();
    }
    
    public void SetOnGameStateVisible(bool state)
    {
        _canShowUI = state;
        UpdateUIState();
    }
    
    
}
