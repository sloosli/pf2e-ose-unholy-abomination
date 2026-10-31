using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace DesktopTools.UserControls;

public partial class EnumCheckboxGroup : UserControl
{
    public static readonly StyledProperty<Type?> EnumTypeProperty =
        AvaloniaProperty.Register<EnumCheckboxGroup, Type?>(nameof(EnumType));

    public static readonly StyledProperty<IEnumerable?> SelectedValuesProperty =
        AvaloniaProperty.Register<EnumCheckboxGroup, IEnumerable?>(nameof(SelectedValues),
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<AvaloniaList<string>> EnumNamesProperty =
        AvaloniaProperty.Register<EnumCheckboxGroup, AvaloniaList<string>>(nameof(EnumNames),
            defaultValue: []);

    public Type? EnumType
    {
        get => GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }

    public IEnumerable? SelectedValues
    {
        get => GetValue(SelectedValuesProperty);
        set => SetValue(SelectedValuesProperty, value);
    }

    public AvaloniaList<string> EnumNames
    {
        get => GetValue(EnumNamesProperty);
        set => SetValue(EnumNamesProperty, value);
    }

    public EnumCheckboxGroup()
    {
        InitializeComponent();
        this.GetObservable(EnumTypeProperty).Subscribe(type =>
        {
            var newNames = new AvaloniaList<string>();
            if (type is { IsEnum: true })
                newNames.AddRange(Enum.GetNames(type));

            EnumNames = newNames;
        });

        this.GetObservable(SelectedValuesProperty).Subscribe(OnSelectedValuesChangedInViewModel);
    }

    private void OnSelectedValuesChangedInViewModel(IEnumerable? values)
    {
        if (values == null)
            return;

        var stringList = values.Cast<object>().Select(x => x.ToString()).ToList();

        var checkBoxes = this.GetVisualDescendants().OfType<CheckBox>().ToList();

        foreach (var cb in checkBoxes)
        {
            if (cb.Content is string content)
            {
                cb.IsChecked = stringList.Contains(content);
            }
        }
    }

    private void OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (EnumType is not { IsEnum: true })
            return;

        var checkBoxes = this.GetVisualDescendants().OfType<CheckBox>().ToList();
        var selectedEnums = new List<object>();

        foreach (var cb in checkBoxes)
        {
            if (cb.IsChecked != true || cb.Content is not string targetStr)
                continue;

            if (Enum.TryParse(EnumType, targetStr, out var enumResult))
                selectedEnums.Add(enumResult);
        }

        var typedArray = Array.CreateInstance(EnumType, selectedEnums.Count);
        for (var i = 0; i < selectedEnums.Count; i++)
        {
            typedArray.SetValue(selectedEnums[i], i);
        }

        SelectedValues = typedArray;
    }
}