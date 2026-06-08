using UnityEngine;

public class BossUltimateEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _paramCharge = "bCharge";

    private int _hashCharge;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        _hashCharge = Animator.StringToHash(_paramCharge);
    }

    public void ChargeStart()
    {
        _animator.SetBool(_hashCharge, true);
    }

    public void ChargeEnd()
    {
        _animator.SetBool(_hashCharge, false);
    }
}
