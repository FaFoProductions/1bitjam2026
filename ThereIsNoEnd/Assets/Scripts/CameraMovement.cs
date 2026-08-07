using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform _playerT;
    [SerializeField] int yOffset;
    [SerializeField] int someSpeedFactor;
    float heightFromCenter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerT = GameObject.FindWithTag("Player").transform;
        float heightFromCenter = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(Mathf.Lerp(transform.position.x, _playerT.position.x, Time.deltaTime * someSpeedFactor), Mathf.Lerp(transform.position.y, _playerT.position.y + yOffset, Time.deltaTime * someSpeedFactor), -10);
        transform.position = newPos;
    }
}
