using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : BasePassiveSkill
{
    [Tooltip("Пассивный скилл Берсерк Если уровень ХП пирата ниже n% то пират наносить на n% больше урона")] 
    [SerializeField] private int additionalDamage;

    [Tooltip("n% (0-100)")]
    [SerializeField] private float healthPercent;

    private BaseEntity _casterEntity;

    private string _previousState = "upper";
    private string _healthState = "upper";

    private void Start() {
        this._casterEntity = gameObject.GetComponent<BaseEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if((float)_casterEntity.GetHealthPoints() < (float)_casterEntity.GetMaxHealthPoints() * (float)((float)healthPercent / 100))
            _healthState = "lower";
        if((float)_casterEntity.GetHealthPoints() >= (float)_casterEntity.GetMaxHealthPoints() * (float)((float)healthPercent / 100))
            _healthState = "upper";

        if(_previousState != _healthState)
        {
            Debug.Log("Changed state from " + _previousState + " to " + _healthState);
            _previousState = _healthState;
            ChangeDamage();
        }
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
