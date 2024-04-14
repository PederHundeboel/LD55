using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class Container : MonoBehaviour
{
    [SerializeField] protected int maxValue, value;
    public const int MIN = 0;

    [SerializeField] protected UnityEvent onEmpty = new UnityEvent();
    [FormerlySerializedAs("onChange")][SerializeField] protected UnityEvent onValueChange = new UnityEvent();
    [SerializeField] protected UnityEvent onMaxChange = new UnityEvent();
    [SerializeField] protected UnityEvent onFull = new UnityEvent();
    [SerializeField] protected UnityEvent onDecrease = new UnityEvent();
    [SerializeField] protected UnityEvent onIncrease = new UnityEvent();

    protected virtual void Awake()
    {
        if (this.value > this.maxValue)
        {
            this.value = maxValue;
        }
    }

    private void OnValidate()
    {
        onMaxChange.Invoke();
        onValueChange.Invoke();
    }

    public int GetValue()
    {
        return value;
    }

    public int GetMax()
    {
        return this.maxValue;
    }

    public virtual void Add(int addValue)
    {
        int oldValue = this.value;
        this.value = Mathf.Clamp(value + addValue, MIN, this.maxValue);

        if (oldValue != this.value)
        {
            onIncrease.Invoke();
            onValueChange.Invoke();
        }

        if (this.value == this.maxValue)
        {
            onFull.Invoke();
        }
    }

    public virtual void Subtract(int subValue)
    {
        int oldValue = this.value;
        this.value = Mathf.Clamp(value - subValue, MIN, this.maxValue);

        if (oldValue != this.value)
        {
            onDecrease.Invoke();
            onValueChange.Invoke();
        }

        if (value <= MIN)
        {
            this.value = MIN;
            onEmpty.Invoke();
        }
    }

    public void SetMax(int newMaxValue)
    {
        if (newMaxValue <= MIN) newMaxValue = 1;
        int oldMaxValue = this.maxValue;
        this.maxValue = newMaxValue;

        if (this.value > newMaxValue)
        {
            this.value = newMaxValue;
            onMaxChange.Invoke();
        }

        if (oldMaxValue != newMaxValue)
        {
            onMaxChange.Invoke();
        }
    }
}
