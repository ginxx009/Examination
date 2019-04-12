using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    private BoxCollider2D backgroundCollider;
    private float backgroundHorizontalLength;

    // Start is called before the first frame update
    void Start()
    {
        backgroundCollider = GetComponent<BoxCollider2D>();
        backgroundHorizontalLength = backgroundCollider.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= -backgroundHorizontalLength)
        {
            RepositionBackground();
        }
    }

    private void RepositionBackground()
    {
        Vector2 backgroundOffset = new Vector2(backgroundHorizontalLength * 2f, 0);
        transform.position = (Vector2)transform.position + backgroundOffset;
    }
}
