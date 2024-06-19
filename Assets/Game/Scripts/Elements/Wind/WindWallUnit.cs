using UnityEngine;



public class WindWall : MonoBehaviour
{
    public Vector2 WindForce;
    public Vector2 WindDirection;

    public Vector2 GetWindForce() => WindForce;
    public Vector2 GetWindDirection() => WindDirection;
}
