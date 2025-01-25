using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform                mTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = mTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mTarget.position;
    }
}
