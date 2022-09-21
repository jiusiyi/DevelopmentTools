using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 专门用于属性编辑器的TypeConvertor.将资源图片的ID值转换成文字图片
    /// </summary>
    public class ResourceImageTypeConvertor:TypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceImageTypeConvertor"/> class.
        /// </summary>
        public ResourceImageTypeConvertor()
        { 
        }
        //public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        //{
        //    return null;
        //}

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
        /// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return SR.GetString("Image");
        }
    }
}
