using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Type", menuName = "Enemy Type")]
public class EnemyType : ScriptableObject
{
    [Header("Attributes")]
    public int health = 100;
    public string enemyType = "Default";
    public int pointsGiven = 10;
    public float moveSpeed = 3f;
    public int strength = 10;
}
