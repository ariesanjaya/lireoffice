using Prism.Mvvm;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Reactive;
using System.Reactive.Linq;

namespace LireOffice.Models
{
    public abstract class Rule<T>
    {
        protected Rule(string propertyName, object error)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _error = error ?? throw new ArgumentNullException(nameof(error));
        }

        private string _propertyName;

        public string PropertyName
        {
            get => _propertyName;
        }

        private object _error;

        public object Error
        {
            get => _error; 
        }

        public abstract bool Apply(T obj);
    }

    public sealed class DelegateRule<T> : Rule<T>
    {
        private Func<T, bool> rule;

        public DelegateRule(string propertyName, object error, Func<T, bool> rule)
            : base(propertyName, error)
        {
            this.rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        public override bool Apply(T obj)
        {
            return rule(obj);
        }
    }

    public sealed class RuleCollection<T> : Collection<Rule<T>>
    {
        public void Add(string propertyName, object error, Func<T, bool> rule)
        {
            Add(new DelegateRule<T>(propertyName, error, rule));
        }

        public IEnumerable<object> Apply(T obj, string propertyName)
        {
            List<object> errors = new List<object>();
            foreach (Rule<T> rule in this)
            {
                if (string.IsNullOrEmpty(propertyName) || rule.PropertyName.Equals(propertyName))
                {
                    if (!rule.Apply(obj))
                    {
                        errors.Add(rule.Error);
                    }
                }
            }

            return errors;
        }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public abstract class NotifyDataErrorInfo<T> : ReactiveObject, INotifyDataErrorInfo
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
            where T : NotifyDataErrorInfo<T>
        {

            private const string HasErrorPropertyName = "HasErrors";
            private RuleCollection<T> rules = new RuleCollection<T>();
            private Dictionary<string, List<object>> errors;

            private event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

            event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
            {
                add { ErrorsChanged += value; }
                remove { ErrorsChanged -= value; }
            }
            
            public bool HasErrors
            {
                get
                {
                    InitializeErrors();
                    return errors.Count > 0;
                }
            }

            protected RuleCollection<T> Rules
            {
                get => rules;
            }

            public IEnumerable GetErrors()
            {
                return GetErrors(null);
            }
                       
            public IEnumerable GetErrors(string propertyName)
            {
                Debug.Assert(string.IsNullOrEmpty(propertyName) || 
                    (GetType().GetRuntimeProperty(propertyName) != null), 
                    "Check that the property name exits for this instance");

                InitializeErrors();

                IEnumerable result;
                if (string.IsNullOrEmpty(propertyName))
                {
                    List<object> allErrors = new List<object>();
                    foreach (KeyValuePair<string, List<object>> keyValuePair in errors)
                    {
                        allErrors.AddRange(keyValuePair.Value);
                    }

                    result = allErrors;
                }
                else
                {
                    if (errors.ContainsKey(propertyName))
                    {
                        result = errors[propertyName];
                    }
                    else
                    {
                        result = new List<object>();
                    }
                }

                return result;
            }
            
            protected void OnPropertyChange([CallerMemberName] string propertyName = null)
            {
                if (string.IsNullOrEmpty(propertyName))
                    ApplyRules();
                else
                    ApplyRules(propertyName);
            }

            protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
            {
                Debug.Assert(
                    string.IsNullOrEmpty(propertyName) ||
                    (GetType().GetRuntimeProperty(propertyName) != null),
                    "Check that the property name exists for this instance.");

                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }

            private void ApplyRules()
            {
                InitializeErrors();
                foreach (string propertyName in rules.Select(x => x.PropertyName))
                {
                    ApplyRules(propertyName);
                }
            }

            private void ApplyRules(string propertyName)
            {
                InitializeErrors();

                List<object> propertyErrors = rules.Apply((T)this, propertyName).ToList();

                if (propertyErrors.Count > 0)
                {
                    if (errors.ContainsKey(propertyName))
                    {
                        errors[propertyName].Clear();
                    }
                    else
                    {
                        errors[propertyName] = new List<object>();
                    }

                    errors[propertyName].AddRange(propertyErrors);
                    OnErrorsChanged(propertyName);
                }
                else if (errors.ContainsKey(propertyName))
                {
                    errors.Remove(propertyName);
                    OnErrorsChanged(propertyName);
                }
            }

            private void InitializeErrors()
            {
                if (errors == null)
                {
                    errors = new Dictionary<string, List<object>>();

                    ApplyRules();
                }
            }
        }
    }
    
}
