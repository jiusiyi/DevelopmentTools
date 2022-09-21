using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Interface INodeRepository
    /// </summary>
    public interface ITreeNodeRepository
    {
        /// <summary>
        /// Gets the Nodes.
        /// </summary>
        List<string> Nodes { get; }
        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns></returns>
        string GetNewNameIdenty ( );
        /// <summary>
        /// Renames the specified Node.
        /// </summary>
        /// <param name="Node">The Node.</param>
        /// <param name="newname">The newname.</param>
        void Rename ( string name, string newname );
        /// <summary>
        /// Removes the specified Node.
        /// </summary>
        /// <param name="Node">The Node.</param>
        void Remove ( string  name );
    }
}
