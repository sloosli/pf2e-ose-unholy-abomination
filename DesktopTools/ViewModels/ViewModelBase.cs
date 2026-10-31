using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using ReactiveUI;

namespace DesktopTools.ViewModels;

public class ViewModelBase : ReactiveObject
{
    protected bool IsInEditMode = false;

    private readonly Dictionary<string, object> _propertyValues = new();

    protected TValue? NotifyGetValue<TValue>(Expression<Func<TValue>> memberExpression)
    {
        if (memberExpression.Body is not MemberExpression body)
            return default;

        var isValueExist = _propertyValues.TryGetValue(body.Member.Name, out var value);
        if (isValueExist && value != null)
        {
            return (TValue)value;
        }

        return default;
    }

    protected bool NotifySetValue<TValue>(Expression<Func<TValue?>> memberExpression, TValue newValue)
    {
        var isValueAlreadyAdded = EqualityComparer<TValue>.Default.Equals(newValue, NotifyGetValue(memberExpression));
        var isValueNull = newValue == null;
        if (isValueAlreadyAdded || isValueNull)
            return false;

        if (memberExpression.Body is not MemberExpression body)
            return false;

        _propertyValues[body.Member.Name] = newValue!;
        ((IReactiveObject)this).RaisePropertyChanged(new PropertyChangedEventArgs(body.Member.Name));
        return true;
    }
}