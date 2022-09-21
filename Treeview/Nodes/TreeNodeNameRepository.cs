using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using ControlEase.Inspec.Extension;


namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeNodeNameRepository : ITreeNodeRepository
    {
        #region ... Variables  ...
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeRepository"/> class.
        /// </summary>VideoManager manager
        /// <param name="manager">The manager.</param>
        public TreeNodeNameRepository ( )
        {
            Nodes = new List<string> ( );
        }

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// Gets the new name.
        /// </summary>
        /// <param name="Parent">The parent.</param>
        /// <returns>System.String.</returns>
        public string GetNewNameIdenty()
        {
            string tempName = SR.GetString ( "Node" );
            do
            {
                string name = TreeNodeNameService.GetNextName ( tempName );
                tempName = name;
                if ( IsValid ( name ) )
                {
                    //Register ( name );
                    return name;
                }
            }
            while (true);
        }

        /// <summary>
        /// Determines whether the specified name is valid.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Parent">The parent.</param>
        /// <returns><c>true</c> if the specified name is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid ( string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            if (!NameService.IsValidName(name))
            {
                return false;
            }
            return !Nodes.Contains ( name );
        }

        /// <summary>
        /// Registers the specified Node.
        /// </summary>
        /// <param name="Node">The Node.</param>
        public bool Register ( string name)
        {
            if ( !Nodes.Contains ( name ) )
            {
                Nodes.Add ( name );
                return true;
            }
            return false;
        }

        /// <summary>
        /// Toes the element.
        /// </summary>
        /// <returns></returns>
        public XElement ToElement()
        {
            XElement element = new XElement("NodeNames");
            foreach (var Node in Nodes)
            {
                element.Add(Node);
            }
            return element;
        }

        /// <summary>
        /// Loads from.
        /// </summary>
        /// <param name="element">The element.</param>
        public void LoadFrom(XElement element)
        {
            Nodes.Clear();
            XElement NodesElement = element.Element ( "NodeNames" );
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        public List<string> Nodes
        {
            get;
            set;
        }
        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns></returns>
        public void New(string name)
        {
            Register ( name );
        }
        /// <summary>
        /// Renames the specified Node.
        /// </summary>
        /// <param name="Node">The Node.</param>
        /// <param name="newName">The new name.</param>
        public void Rename ( string name, string newName )
        {
            Nodes.Remove ( name );
            Register ( newName );
        }
        /// <summary>
        /// Removes the specified Node.
        /// </summary>
        /// <param name="Node">The Node.</param>
        public void Remove ( string  name)
        {
            Nodes.Remove ( name );
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        public void RemoveAll()
        {
            Nodes.Clear();
        }
        #endregion ...Interfaces...
    }
}
