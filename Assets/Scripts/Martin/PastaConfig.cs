using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PastaConfig_", menuName = "Configs/Pasta", order = 0)]
public class PastaConfig : ScriptableObject
{
    public string pastaName;
    public Sprite iconSprite;
    public int startingAmount;


    [Header("Collectible")]
    public int collectibleGivenAmount;

    public Sprite collectibleSprite;
    public Vector2 collectibleGraphicsScale = Vector2.one;
    public float collectibleColliderRadius = 1f;

    [Header("Gameplay")]
    public float cookingSpeed;

    [Header("Shots")]
    public PastaShotConfig crudeShot;
    public PastaShotConfig cookedShot;



}
