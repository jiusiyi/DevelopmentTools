using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Inspec.Animates;
using System.Xml.Linq;
using ControlEase.Inspec.ViewCore;
using ControlEase.AI.View;
using ControlEase.Nexus;
using ControlEase.Inspec.AniInfos;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AniInputBase<T> : PropertyLinkEvent<T>, IKnownType
    {
        #region ... Variables  ...
        /// <summary>
        /// Key.
        /// </summary>
        protected IIdentity mKey = null;
        /// <summary>
        /// Operation key.
        /// </summary>
        protected new IIdentity mOperationKey = null;
        /// <summary>
        /// Animate type.
        /// </summary>
        protected Type mAnimateType = null;
        /// <summary>
        /// Animate connection.
        /// </summary>
        protected IAnimateConnection mAnimateConnection = null;
        /// <summary>
        /// ParentName.
        /// </summary>
        private string mParentName = string.Empty;
        /// <summary>
        /// The m animate name
        /// </summary>
        private string mAnimateName = string.Empty;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// Gets or sets key.
        /// </summary>
        public IIdentity Key
        {
            get { return mKey; }
            set
            {
                mKey = value;
            }
        }
        ///// <summary>
        ///// Key chagned.
        ///// </summary>
        //protected virtual void OnKeyChagned ( )
        //{
        //}
        ///// <summary>
        ///// Gets or sets operation key.
        ///// </summary>
        //public IIdentity OperationKey
        //{
        //    get { return mOperationKey; }
        //    set { mOperationKey = value; }
        //}

        /// <summary>
        /// Gets or sets animate type.
        /// </summary>
        public Type AnimateType
        {
            get { return mAnimateType; }
            set
            {
                //&& value.IsInheritFrom ( typeof ( Animate ) ) 
                if ( value != null )
                {
                    mAnimateType = value;
                    mAnimateConnection = mAnimateType.GenerateConnection ( );
                }
                else if ( value == null )
                {
                    mAnimateType = null;
                    mAnimateConnection = null;
                }
                else
                {
                    throw new InvalidOperationException ( "Type is not invalid." );
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public IAnimateConnection AnimateConnection
        //{
        //    get { return mAnimateConnection; }
        //    set { mAnimateConnection = value; }
        //}
        /// <summary>
        /// 
        /// </summary>
        public string ParentName
        {
            get { return mParentName; }
            set { mParentName = value; }
        }
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual void SaveToXElement ( XElement xe )
        {
            this.SaveToXElement<T>(xe);
        }

        public virtual void LoadFromXElement ( XElement xe )
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public virtual PropertyLinkAnimate<T> LoadPropertyAnimateFromXElement()
        {
            return null ;
        }
        /// <summary>
        /// Gets default creator.
        /// </summary>
        public virtual Func<object> DefaultCreator
        {
            get { return ( ) => null; }
        }
        /// <summary>
        /// Gets parameter creator.
        /// </summary>
        public virtual Func<object, object> ParameterCreator
        {
            get { return null; }
        }

        #endregion ...Interfaces...
    }
}
