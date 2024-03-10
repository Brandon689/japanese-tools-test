using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SentenceFarm
{
    public abstract class VmBase : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler? propertyChanged;

        // Implement the event subscription manually so ViewModels can subscribe and unsubscribe
        // from events raised by their models.
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (NotifyPropertyChangedHelper.AddHandler(ref propertyChanged, value))
                {
                    OnPropertyChangedHasSubscribers();
                }
            }
            remove
            {
                if (NotifyPropertyChangedHelper.RemoveHandler(ref propertyChanged, value))
                {
                    OnPropertyChangedHasNoSubscribers();
                }
            }
        }

        /// <summary>
        /// Called when the PropertyChangedEventHandler subscription that goes from "no subscribers"
        /// to "at least one subscriber". Derived classes should perform any event subscriptions here.
        /// </summary>
        protected virtual void OnPropertyChangedHasSubscribers()
        {
        }

        /// <summary>
        /// Called when the PropertyChangedEventHandler subscription that goes from "at least one subscriber"
        /// to "no subscribers". Derived classes should perform any event unsubscriptions here.
        /// </summary>
        protected virtual void OnPropertyChangedHasNoSubscribers()
        {
        }

        protected void RaisePropertyChanged(string name) =>
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T field, T value, bool valid, [CallerMemberName] string? name = null)
        {
            if (!valid)
            {
                throw new ArgumentException($"Invalid value: {value}");
            }
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            RaisePropertyChanged(name!);
            return true;
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null) =>
            SetProperty(ref field, value, true, name);
    }

    /// <summary>
    /// Base class for view models which wrap a single model.
    /// </summary>
    /// <typeparam name="TModel">The model type this is based on.</typeparam>
    public abstract class ViewModelBase<TModel> : VmBase
    {
        protected TModel Model { get; }

        private protected ViewModelBase(TModel model) =>
            Model = model;

        protected override void OnPropertyChangedHasSubscribers()
        {
            if (Model is INotifyPropertyChanged inpc)
            {
                inpc.PropertyChanged += OnPropertyModelChanged;
            }
        }

        protected override void OnPropertyChangedHasNoSubscribers()
        {
            if (Model is INotifyPropertyChanged inpc)
            {
                inpc.PropertyChanged -= OnPropertyModelChanged;
            }
        }

        protected virtual void OnPropertyModelChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }

    public static class NotifyPropertyChangedHelper
    {
        /// <summary>
        /// Adds a handler, returning whether or not this has caused the underlying handler
        /// to go from "unsubscribed" to "subscribed".
        /// </summary>
        /// <param name="value">The handler to add.</param>
        /// <returns>true if there were previously no handlers, but now there's at least one; false otherwise.</returns>
        public static bool AddHandler(ref PropertyChangedEventHandler? field, PropertyChangedEventHandler? value)
        {
            if (value is null)
            {
                return false;
            }
            // TODO: Make this thread-safe.
            bool ret = field is null;
            field += value;
            return ret;
        }

        /// <summary>
        /// Adds a handler, returning whether or not this has caused the underlying handler
        /// to go from "subscribed" to "unsubscribed".
        /// </summary>
        /// <param name="value">The handler to add.</param>
        /// <returns>true if there were previously no handlers, but now there's at least one; false otherwise.</returns>
        public static bool RemoveHandler(ref PropertyChangedEventHandler? field, PropertyChangedEventHandler? value)
        {
            if (value is null || field is null)
            {
                return false;
            }
            field -= value;
            return field is null;
        }
    }
}

