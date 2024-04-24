using UnityEngine;


[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float forwardAcceleration = 15;
    public float backwardAcceleration = 5;
    public float breakPower = 2;
    public float handbreakPower = 1;
    public float maxSpeed = 50;
    public float groundFriction = .02f;
    


}
