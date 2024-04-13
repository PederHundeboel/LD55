using UnityEngine;
using UnityEngine.Events;

public abstract class Container : MonoBehaviour
{
    [SerializeField] protected int maxValue, value;
    public const int MIN = 0;

    [SerializeField] private UnityEvent onEmpty = new UnityEvent();
    [SerializeField] private UnityEvent onChange = new UnityEvent();
    [SerializeField] private UnityEvent onFull = new UnityEvent();
    [SerializeField] private UnityEvent onDecrease = new UnityEvent();
    [SerializeField] private UnityEvent onIncrease = new UnityEvent();

    protected virtual void Awake()
    {
        if (this.value > this.maxValue)
        {
            this.value = maxValue;
        }
    }

    private void OnValidate()
    {
        onChange.Invoke();
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
            onChange.Invoke();
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
            onChange.Invoke();
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
            onChange.Invoke();
        }

        if (oldMaxValue != newMaxValue)
        {
            onChange.Invoke();
        }
    }
}
