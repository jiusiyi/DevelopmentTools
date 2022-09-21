using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Drawing.Design;
using ControlEase.Inspec.ViewPresentation;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Inspec.Resources;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace ControlEase.Inspec.TreeView
{
    public class FontFamilyEditor : UITypeEditor
    {
        new  bool IsDropDownResizable
        { 
            get {return false;}
        }
        private IWindowsFormsEditorService _editorService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle ( System.ComponentModel.ITypeDescriptorContext context )
        {
            //制定弹出Editor的样式，为弹出窗口或dropdownlist，或者为
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue ( System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            List<string> fontnames = new List<string>();;
            _editorService = ( System.Windows.Forms.Design.IWindowsFormsEditorService ) provider.GetService ( typeof ( System.Windows.Forms.Design.IWindowsFormsEditorService ) );
            if ( _editorService != null )
            {
                ListBox mlb = new ListBox ( );
                mlb.SelectionMode = SelectionMode.One;
                mlb.SelectedValueChanged += OnListBoxSelectedValueChanged;
                foreach ( FontFamily fontfamily in Fonts.SystemFontFamilies )
                {
                    LanguageSpecificStringDictionary lsd = fontfamily.FamilyNames;
                    if ( lsd.ContainsKey ( XmlLanguage.GetLanguage ( "zh-cn" ) ) )
                    {
                        string fontname = null;
                        if ( lsd.TryGetValue ( XmlLanguage.GetLanguage ( "zh-cn" ), out fontname ) )
                        {
                            int index = mlb.Items.Add ( fontname );
                            if ( fontname.Equals ( value ) )
                            {
                                mlb.SelectedIndex = index;
                            }
                        }
                    }
                    else
                    {
                        string fontname = null;
                        if ( lsd.TryGetValue ( XmlLanguage.GetLanguage ( "en-us" ), out fontname ) )
                        {
                            int index = mlb.Items.Add ( fontname );
                            if ( fontname.Equals ( value ) )
                            {
                                mlb.SelectedIndex = index;
                            }
                        }
                    }
                }
                // show this model stuff
                _editorService.DropDownControl ( mlb );
                if ( mlb.SelectedItem == null ) // no selection, return the passed-in value as is
                    return value;

                return mlb.SelectedItem.ToString ( );
            }
            return value;
        }



        private void OnListBoxSelectedValueChanged ( object sender, EventArgs e )
        {
            // close the drop down as soon as something is clicked
            _editorService.CloseDropDown ( );
        }




        
    }
}
