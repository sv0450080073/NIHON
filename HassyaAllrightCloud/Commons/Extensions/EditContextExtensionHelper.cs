using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class EditContextExtensionHelper
    {
        /// <summary>
        /// Clears any modification flag that may be tracked for the specified field.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="editContext"></param>
        /// <param name="accessor"></param>
        public static void MarkAsUnmodified<TField>(this EditContext editContext, Expression<Func<TField>> accessor)
        {
            editContext.MarkAsUnmodified(FieldIdentifier.Create(accessor));
        }

        /// <summary>
        /// Clears any modification flag that may be tracked for the specified list of fields.
        /// </summary>
        /// <param name="editContext"></param>
        /// <param name="accessorList"></param>
        public static void MarkAsUnmodified(this EditContext editContext,  params Expression<Func<object>>[] accessorList)
        {
            foreach (var expression in accessorList)
            {
                editContext.MarkAsUnmodified(FieldIdentifier.Create(expression));
            }
        }

        /// <summary>
        /// Signals that the value for the specified field has changed.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="editContext"></param>
        /// <param name="accessor"></param>
        public static void NotifyFieldChanged<TField>(this EditContext editContext, Expression<Func<TField>> accessor)
        {
            editContext.NotifyFieldChanged(FieldIdentifier.Create(accessor));
        }

        /// <summary>
        /// Signals that the value for the specified list of fields has changed.
        /// </summary>
        /// <param name="editContext"></param>
        /// <param name="accessorList"></param>
        public static void NotifyFieldChanged(this EditContext editContext, params Expression<Func<object>>[] accessorList)
        {
            foreach (var expression in accessorList)
            {
                editContext.NotifyFieldChanged(FieldIdentifier.Create(expression));
            }
        }
        
        /// <summary>
        /// Get Custom CSS of field
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="editContext">current edit context from</param>
        /// <param name="accessor">linq expression to get field identifier</param>
        /// <returns>
        /// custom-invalid for invalid case
        /// custom-modified for modified case
        /// custom-valid for valid case
        /// </returns>
        public static string GetCustomCss<TField>(this EditContext editContext, Expression<Func<TField>> accessor)
        {
            string fieldCss = editContext.FieldCssClass(accessor);
            var status = fieldCss.Split(' ');
            if (status.Contains("invalid"))
            {
                return "custom-invalid";
            }
            if (status.Contains("modified"))
            {
                return "custom-modified";
            }
            return "custom-valid";
        }
    }
}
