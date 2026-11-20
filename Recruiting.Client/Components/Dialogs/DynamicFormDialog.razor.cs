using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Recruiting.Client.Components.Dialogs
{
    public partial class DynamicFormDialog
    {
        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; } = default!;

        [Parameter] public string Title { get; set; } = "Добавление записи";
        [Parameter] public string? Subtitle { get; set; }
        [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Add;
        [Parameter] public string IconBgColor { get; set; } = "var(--color-accent-light, #FAF4F0)";
        [Parameter] public string IconColor { get; set; } = "var(--color-accent, #C28163)";
        [Parameter] public string SubmitText { get; set; } = "Создать";

        [Parameter] public List<FormFieldConfig> Fields { get; set; } = new();

        private HashSet<string> _touchedErrors = new();

        private bool HasError(string key) => _touchedErrors.Contains(key);

        private void Cancel() => MudDialog.Cancel();

        private void Submit()
        {
            _touchedErrors.Clear();
            var resultDict = new Dictionary<string, object?>();

            foreach (var f in Fields)
            {
                object? val = f.FieldType switch
                {
                    DynamicFieldType.Number => f.NumberValue,
                    DynamicFieldType.Switch => f.BoolValue,
                    _ => f.StringValue
                };

                resultDict[f.Key] = val;

                if (f.IsRequired)
                {
                    if (val == null || (val is string s && string.IsNullOrWhiteSpace(s)))
                    {
                        _touchedErrors.Add(f.Key);
                    }
                }
            }

            if (_touchedErrors.Count > 0)
            {
                StateHasChanged();
                return;
            }

            MudDialog.Close(DialogResult.Ok(resultDict));
        }

        public enum DynamicFieldType
        {
            Text,
            TextArea,
            Number,
            Select,
            Switch
        }

        public class FormFieldConfig
        {
            public string Key { get; set; } = "";
            public string Label { get; set; } = "";
            public string? Placeholder { get; set; }
            public string? HelperText { get; set; }
            public DynamicFieldType FieldType { get; set; } = DynamicFieldType.Text;
            public bool IsRequired { get; set; } = true;
            public string StringValue { get; set; } = "";
            public int? NumberValue { get; set; }
            public bool BoolValue { get; set; }
            public int Rows { get; set; } = 3;
            public int? Min { get; set; }
            public int? Max { get; set; }
            public List<(string Key, string Label)>? Options { get; set; }
        }
    }
}
