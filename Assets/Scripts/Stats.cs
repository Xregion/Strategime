using UnityEngine;

public class Stats : MonoBehaviour {

    #region Initalize Stats
    public enum AttackType { physical, magical };

    public AttackType types;

    [SerializeField]
    private float health;

    public float _health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    [SerializeField]
    private float armor;

    public float _armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }

    [SerializeField]
    private float magicDefense;

    public float _magicDefense
    {
        get
        {
            return magicDefense;
        }
        set
        {
            magicDefense = value;
        }
    }

    [SerializeField]
    private float speed;

    public float _speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    [SerializeField]
    private float range;

    public float _range
    {
        get
        {
            return range;
        }
        set
        {
            range = value;
        }
    }
#endregion
}
