using UnityEngine;
using System.Collections;
[System.Serializable]
public class RotateFlag
{
    private Transform _tr;
    private bool rotating = false;

    private float _rotaTime;
    private float _rotaSpeed;
    [SerializeField]
    private Vector3 _tarDir;
    [SerializeField]
    private Quaternion _tarQuat;

    private float _time;

    public Vector3 curDir
    {
        get
        {
            return _tr.localEulerAngles;
        }
        private set
        {

            _tr.localEulerAngles = value;
        }
    }
    public RotateFlag(Transform tr)
    {
        _tr = tr;
    }
    public void SetRotate(float rotaTime, float rotaSpeed, Vector3 tarDir)
    {
        rotating = true;
        _time = 0;
        _rotaTime = rotaTime;
        _rotaSpeed = rotaSpeed;
        _tarDir = tarDir;
        _tarQuat = Quaternion.LookRotation(tarDir);
    }
    public void Update(float deltaTime)
    {
        if (rotating)
        {
            _rotate(deltaTime);
        }

    }
    private void _rotate(float deltaTime)
    {
        if(Mathf.Abs(_rotaSpeed)<1e-5)
        {
            rotating = false;
            return;
        }
        if(_tr.localRotation != _tarQuat)
        {
            _tr.localRotation = Quaternion.Lerp(_tr.localRotation, _tarQuat, _rotaSpeed * deltaTime);
        }
//        _time += deltaTime;
//        if(_time<_rotaTime )
//        {
//            _tr.localRotation = Quaternion.Lerp(_tr.localRotation, _tarQuat, _rotaSpeed*deltaTime*2f);
//
//        }
//        if(_rotaTime>deltaTime)
//        {
//            _tr.localRotation = Quaternion.Lerp(_tr.localRotation, _tarQuat, deltaTime/_rotaTime);
////            curDir = Vector3.Lerp(curDir, _tarDir, deltaTime / _rotaTime);
//            _rotaTime -= deltaTime;
//        }
//        else
//        {
//            _tr.localRotation = _tarQuat;
////            curDir = _tarDir;
//            _rotaTime = 0;
//            rotating = false;
//        }
//
    }

}
