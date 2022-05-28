using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : BasePassiveSkill
{
    [Tooltip("Пассивный скилл Берсерк Если уровень ХП пирата ниже n% то пират наносить на n% больше урона")] 
    [SerializeField] private int additionalDamage;

    [Tooltip("n% (0-100)")]
    [SerializeField] private int healthPercent;

    private BaseEntity _casterEntity;

    private string _previousState = "upper";
    private string _healthState = "upper";

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_casterEntity.GetHealthPoints() + "   " +_casterEntity.GetMaxHealthPoints() * (healthPercent / 100) + "   " + healthPercent / 100);
        if(_casterEntity.GetHealthPoints() < _casterEntity.GetMaxHealthPoints() * (healthPercent / 100))
            _healthState = "lower";
        if(_casterEntity.GetHealthPoints() >= _casterEntity.GetMaxHealthPoints() * (healthPercent / 100))
            _healthState = "upper";

        if(_previousState != _healthState)
        {
            Debug.Log("Changed state from " + _previousState + " to " + _healthState);
            _previousState = _healthState;
            ChangeDamage();
        }
    }

    public override void ActivatePassiveSkill(GameObject caster)
    {
        base.ActivatePassiveSkill(caster);
        this._casterEntity = caster.GetComponent<BaseEntity>();
        Debug.Log("Berserk ");
    }

    void ChangeDamage()
    {
        Debug.Log("Change Damage");
        if(_healthState == "lower")
            _casterEntity.IncreaseDamage(additionalDamage);
        if(_healthState == "upper")
            _casterEntity.DecreaseDamage(additionalDamage);
    }
}
