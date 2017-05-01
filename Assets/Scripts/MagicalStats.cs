using UnityEngine;

public class MagicalStats : Stats {

    public enum MagicType { fire, arcane, botany, water, dark, light };

    public MagicType Types;

    [SerializeField]
    private float magicAttack;

    public float _magicAttack
    {
        get
        {
            return magicAttack;
        }
        set
        {
            magicAttack = value;
        }
    }
}
