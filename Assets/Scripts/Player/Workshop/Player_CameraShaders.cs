using UnityEngine;

public class Player_CameraShaders : MonoBehaviour
{
    public static int _positionID = Shader.PropertyToID("_PlayerPosition");
    public static int _sizeID = Shader.PropertyToID("_Size");
    public static int _smoothnessID = Shader.PropertyToID("_Smoothness");
    public static int _opacityIDID = Shader.PropertyToID("_Opacity");


    public Material _obstacleMaterial1, _obstacleMaterial2;
    public LayerMask _layerMask;
    public Camera _mainCamera;

    // Update is called once per frame
    void Update()
    {
        var _cameraDirection = _mainCamera.transform.position - transform.position;
        var _cameraRay = new Ray(transform.position, _cameraDirection.normalized); 

        if (Physics.Raycast(_cameraRay, 10000, _layerMask)) // player is behind a wall
        {
            _obstacleMaterial1.SetFloat(_sizeID, 1.5f); // white
            _obstacleMaterial2.SetFloat(_sizeID, 1.7f); // blue
        }
        else
        {
            _obstacleMaterial1.SetFloat(_sizeID, 0); // white
            _obstacleMaterial2.SetFloat(_sizeID, 0); // blue
        }
        var _cameraView = _mainCamera.WorldToViewportPoint(transform.position);
        _obstacleMaterial1.SetVector(_positionID, _cameraView);
        _obstacleMaterial2.SetVector(_positionID, _cameraView);
    }
}
