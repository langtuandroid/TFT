
using Attack;

public class MaxPowerValues
{
    public float FillAmount => _fillAmount;
    public IAttack Attack => _attack;

    private float _fillAmount;
    private IAttack _attack;

    public MaxPowerValues(float alpha, IAttack attack)
    {
        _fillAmount = alpha;
        _attack = attack;
    }

}
