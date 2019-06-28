using UnityEngine;

public class RandomRadiusPosition : MonoBehaviour
{
	public GameObject player;
	public int radius = 1;
	private Vector3 offset;
		
    void Start()
    {
        // Sets the position to be somewhere inside a circle
        // with radius 5 and the center at zero. Note that
        // assigning a Vector2 to a Vector3 is fine - it will
        // just set the X and Y values.
		
        offset = Random.insideUnitCircle * radius;
	
		transform.position = player.transform.position + offset;
		
    }
}