
using Attack;

public class MaxPowerValues
{
    public float Alpha => _alpha;
    public IAttack Attack => _attack;

    private float _alpha;
    private IAttack _attack;



    public MaxPowerValues(float alpha, IAttack attack)
    {
        _alpha = alpha;
        _attack = attack;
    }

    public MaxPowerValues(float alpha) 
    {
        _alpha = alpha;
        _attack = null;
    }

}
