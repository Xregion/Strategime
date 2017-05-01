using UnityEngine;

public class PhysicalStats : Stats {
    
    #region Initalize Stats
    [SerializeField]
    private float strength;

    public float _strength
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
        }
    }
    #endregion
}
