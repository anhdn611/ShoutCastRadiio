using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace LiveStreamRadio.Model
{
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
        /// <summary>
        /// Raise the PropertyChanged event based on some expression (extract the 
        /// name of the property to ensure strongly typedness)
        /// </summary>
        public void RaisePropertyChanged<TValue>(Expression<Func<TValue>> propertySelector)
        {
            try
            {
                var memberExpression = propertySelector.Body as MemberExpression;
                if (memberExpression != null)
                {
                    RaisePropertyChanged(memberExpression.Member.Name);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
