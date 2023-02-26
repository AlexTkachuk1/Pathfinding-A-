using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 1f;
    private Vector3[] _path;
    private int _targetIndex;

    private void Start()
    {
        PathRequestManager.ReuestPath(transform.position, _target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] path, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = path;
            StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
        }
    }

    public void OnDrawGizmos()
    {
        if (_path != null)
        {
            for (int i = _targetIndex; i < _path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(_path[i], Vector3.one);

                if (i == _targetIndex)
                {
                    Gizmos.DrawLine(transform.position, _path[i]);
                }
                else
                {
                    Gizmos.DrawLine(_path[i - 1], _path[i]);
                }
            }
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypopint = _path[0];
        while (true)
        {
            if (transform.position == currentWaypopint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    yield break;
                }
                currentWaypopint = _path[_targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypopint, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
