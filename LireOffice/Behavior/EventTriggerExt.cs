using System;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace LireOffice.Views
{
    public class EnterKeyDownEventTrigger : EventTrigger
    {
        public EnterKeyDownEventTrigger() : base("KeyUp")
        {
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            if (eventArgs is KeyEventArgs e && e.Key == Key.Enter)
                base.OnEvent(eventArgs);
        }
    }
}