// NodeStore.cs - Tree store implementation for TreeView.
//
// Author: Mike Kestner  <mkestner@novell.com>
//
// Copyright (c) 2003-2005 Novell, Inc.
// Copyright (c) 2009 Christian Hoff
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of version 2 of the Lesser GNU General 
// Public License as published by the Free Software Foundation.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this program; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.


namespace Gtk {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Runtime.InteropServices;

	public class NodeStore : GLib.Object, IEnumerable {

		NodeStoreImplementor implementor;

		public NodeStore (Type node_type)
		{
			implementor = new NodeStoreImplementor (node_type);
		}

		internal TreeModelAdapter Adapter {
			get { return new TreeModelAdapter (implementor); }
		}

		internal TreeIter GetIter (ITreeNode node)
		{
<<<<<<< HEAD:gtk/NodeStore.cs
			return implementor.GetIter (node);
=======
			try {
				return ctypes [col].Val;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}

			return IntPtr.Zero;
>>>>>>> origin/gtk-sharp-2-468-branch:gtk-sharp/gtk/NodeStore.cs
		}

		internal TreePath GetPath (ITreeNode node)
		{
<<<<<<< HEAD:gtk/NodeStore.cs
			return implementor.GetPath (node);
		}

		public ITreeNode GetNode (TreePath path) 
		{ 
			return implementor.GetNode (path); 
		}

		public void AddNode (ITreeNode node) 
		{ 
			implementor.AddNode (node); 
		}

		public void AddNode (ITreeNode node, int position) 
		{ 
			implementor.AddNode (node, position); 
		}

		public void RemoveNode (ITreeNode node) 
		{ 
			implementor.RemoveNode (node); 
=======
			try {
				if (path == IntPtr.Zero)
					throw new ArgumentNullException ("path");

				TreePath treepath = new TreePath (path);
				node_idx = -1;

				ITreeNode node = GetNodeAtPath (treepath);
				if (node == null)
					return false;

				node_idx = node.ID;
				node_hash [node.ID] = node;
				return true;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			node_idx = -1;
			return false;
		}

		IntPtr get_path_cb (int node_idx)
		{
			try {
				ITreeNode node = node_hash [node_idx] as ITreeNode;
				if (node == null) throw new Exception ("Invalid Node ID");

				return GetPath (node).Handle;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			return IntPtr.Zero;
>>>>>>> origin/gtk-sharp-2-468-branch:gtk-sharp/gtk/NodeStore.cs
		}

		public void Clear () 
		{ 
			implementor.Clear (); 
		}

<<<<<<< HEAD:gtk/NodeStore.cs
		public IEnumerator GetEnumerator () 
		{ 
			return implementor.GetEnumerator (); 
		}

		internal class NodeStoreImplementor : GLib.Object, ITreeModelImplementor, IEnumerable {
			TreeModelAdapter model_adapter;
 			GLib.GType[] ctypes; 
			MemberInfo [] getters;
			int n_cols;
			bool list_only = false;
			ArrayList nodes = new ArrayList ();

			public readonly int Stamp;

			public NodeStoreImplementor (Type node_type)
			{
				// Create a random stamp for the iters
				Random RandomStampGen = new Random ();
				this.Stamp = RandomStampGen.Next (int.MinValue, int.MaxValue);

				ScanType (node_type);

				model_adapter = new Gtk.TreeModelAdapter (this);
			}

			void ScanType (Type type)
			{
				TreeNodeAttribute tna = (TreeNodeAttribute) Attribute.GetCustomAttribute (type, typeof (TreeNodeAttribute), false);
				if (tna != null)
					list_only = tna.ListOnly;
			
				var minfos = new List<MemberInfo> ();
			
				foreach (PropertyInfo pi in type.GetProperties ())
					foreach (TreeNodeValueAttribute attr in pi.GetCustomAttributes (typeof (TreeNodeValueAttribute), false))
						minfos.Add (pi);
			
				foreach (FieldInfo fi in type.GetFields ())
					foreach (TreeNodeValueAttribute attr in fi.GetCustomAttributes (typeof (TreeNodeValueAttribute), false))
						minfos.Add (fi);

				n_cols = minfos.Count;
 				ctypes = new GLib.GType [n_cols];
 				getters = new MemberInfo [n_cols];

				foreach (MemberInfo mi in minfos) {
					foreach (TreeNodeValueAttribute attr in mi.GetCustomAttributes (typeof (TreeNodeValueAttribute), false)) {
						int col = attr.Column;

						if (getters [col] != null)
							throw new Exception (String.Format ("You have two TreeNodeValueAttributes with the Column={0}", col));
					
						getters [col] = mi;
						Type t = mi is PropertyInfo ? ((PropertyInfo) mi).PropertyType : ((FieldInfo) mi).FieldType;
						ctypes [col] = (GLib.GType) t;
					}
				}
			}

			public TreeModelFlags Flags {
				get {
					TreeModelFlags result = TreeModelFlags.ItersPersist;
					if (list_only)
						result |= TreeModelFlags.ListOnly;
					return result;
				}
			}

			public int NColumns {
				get {
					return n_cols;
				}
			}

			public GLib.GType GetColumnType (int col)
			{
				return ctypes [col];
			}

#region Gtk.TreePath handling
			internal TreePath GetPath (ITreeNode node)
			{
				TreePath path = new TreePath ();
				int idx;

				while (node.Parent != null) {
					idx = node.Parent.IndexOf (node);
					if (idx < 0) throw new Exception ("Badly formed tree");
					path.PrependIndex (idx);
					node = node.Parent;
				}
				idx = Nodes.IndexOf (node);
				if (idx < 0) throw new Exception ("Node not found in Nodes list");
				path.PrependIndex (idx);
=======
		void get_value_cb (int node_idx, int col, ref GLib.Value val)
		{
			try {
				ITreeNode node = node_hash [node_idx] as ITreeNode;
				if (node == null)
					return;
				g_value_init (ref val, ctypes [col].Val);
				object col_val;
				if (getters [col] is PropertyInfo)
					col_val = ((PropertyInfo) getters [col]).GetValue (node, null);
				else
					col_val = ((FieldInfo) getters [col]).GetValue (node);
				val.Val = col_val;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
		}

		bool next_cb (ref int node_idx)
		{
			try {
				ITreeNode node = node_hash [node_idx] as ITreeNode;
				if (node == null)
					return false;

				int idx;
				if (node.Parent == null)
					idx = Nodes.IndexOf (node);
				else
					idx = node.Parent.IndexOf (node);
			
				if (idx < 0) throw new Exception ("Node not found in Nodes list");

				if (node.Parent == null) {
					if (++idx >= Nodes.Count)
						return false;
					node = Nodes [idx] as ITreeNode;
				} else {
					if (++idx >= node.Parent.ChildCount)
						return false;
					node = node.Parent [idx];
				}
				node_hash [node.ID] = node;
				node_idx = node.ID;
				return true;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			return false;
		}

		bool children_cb (out int child_idx, int parent)
		{
			try {
				child_idx = -1;
				ITreeNode node;

				if (parent == -1) {
					if (Nodes.Count <= 0)
						return false;
					node = Nodes [0] as ITreeNode;
					child_idx = node.ID;
					node_hash [node.ID] = node;
					return true;
				}
				
				node = node_hash [parent] as ITreeNode;
				if (node == null || node.ChildCount <= 0)
					return false;

				ITreeNode child = node [0];
				node_hash [child.ID] = child;
				child_idx = child.ID;
				return true;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			child_idx = -1;
			return false;
		}

		bool has_child_cb (int node_idx)
		{
			try {
				ITreeNode node = node_hash [node_idx] as ITreeNode;
				if (node == null || node.ChildCount <= 0)
					return false;

				return true;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			return false;
		}

		int n_children_cb (int node_idx)
		{
			try {
				if (node_idx == -1)
					return Nodes.Count;
				
				ITreeNode node = node_hash [node_idx] as ITreeNode;
				if (node == null || node.ChildCount <= 0)
					return 0;

				return node.ChildCount;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			return 0;
		}

		bool nth_child_cb (out int child_idx, int parent, int n)
		{
			child_idx = -1;
			try {
				ITreeNode node;

				if (parent == -1) {
					if (Nodes.Count <= n)
						return false;
					node = Nodes [n] as ITreeNode;
					child_idx = node.ID;
					node_hash [node.ID] = node;
					return true;
				}
				
				node = node_hash [parent] as ITreeNode;
				if (node == null || node.ChildCount <= n)
					return false;

				ITreeNode child = node [n];
				node_hash [child.ID] = child;
				child_idx = child.ID;
				return true;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			return false;
		}

		bool parent_cb (out int parent_idx, int child)
		{
			parent_idx = -1;
			try {
				ITreeNode node = node_hash [child] as ITreeNode;
				if (node == null || node.Parent == null)
					return false;

				node_hash [node.Parent.ID] = node.Parent;
				parent_idx = node.Parent.ID;
				return true;
			} catch (Exception e) {
				GLib.ExceptionManager.RaiseUnhandledException (e, false);
			}
			return false;
		}
>>>>>>> origin/gtk-sharp-2-468-branch:gtk-sharp/gtk/NodeStore.cs

				path.Owned = false;
				return path;
			}

			public ITreeNode GetNode (TreePath path)
			{
				if (path == null)
					throw new ArgumentNullException ();

				int[] indices = path.Indices;

				if (indices[0] >= Nodes.Count)
					return null;

				ITreeNode node = Nodes [indices [0]] as ITreeNode;
				int i;
				for (i = 1; i < path.Depth; i++) {
					if (indices [i] >= node.ChildCount)
						return null;

					node = node [indices [i]];
				}

				return node;
			}
#endregion

#region Gtk.TreeIter handling
			IList<GCHandle> gc_handles = new List<GCHandle> ();

			protected override void Dispose (bool disposing)
			{
				// Free all the GCHandles pointing to the iters since they won't be garbage collected
				foreach (GCHandle handle in gc_handles)
					handle.Free ();

				base.Dispose (disposing);
			}

			internal void GetIter (ITreeNode node, ref TreeIter iter)
			{
				if (node == null)
					throw new ArgumentNullException ("node");

				iter.Stamp = this.Stamp;
				GCHandle gch = GCHandle.Alloc (node);
				iter.UserData = (IntPtr) gch;
				gc_handles.Add (gch);
			}

			public TreeIter GetIter (ITreeNode node)
			{
				Gtk.TreeIter result = Gtk.TreeIter.Zero;
				GetIter (node, ref result);

				return result;
			}

			public ITreeNode GetNode (TreeIter iter)
			{
				if (iter.Stamp != this.Stamp)
					throw new InvalidOperationException (String.Format ("iter belongs to a different model; it's stamp is not equal to the stamp of this model({0})", this.Stamp.ToString ()));

				System.Runtime.InteropServices.GCHandle gch = (System.Runtime.InteropServices.GCHandle) iter.UserData;
				return gch.Target as ITreeNode;
			}

			void ITreeModelImplementor.RefNode (Gtk.TreeIter iter) { }
			void ITreeModelImplementor.UnrefNode (Gtk.TreeIter iter) { }
#endregion

			public bool GetIter (out TreeIter iter, TreePath path)
			{
				if (path == null)
					throw new ArgumentNullException ("path");
			
				ITreeNode node = GetNode (path);
				if (node == null) {
					iter = TreeIter.Zero;
					return false;
				} else {
					iter = GetIter (node);
					return true;
				}
			}

			public Gtk.TreePath GetPath (TreeIter iter)
			{
				return GetPath (GetNode (iter));
			}

			public void GetValue (Gtk.TreeIter iter, int col, ref GLib.Value val)
			{
				ITreeNode node = GetNode (iter);
				val.Init (ctypes [col]);

				object col_val;
				if (getters [col] is PropertyInfo)
					col_val = ((PropertyInfo) getters [col]).GetValue (node, null);
				else
					col_val = ((FieldInfo) getters [col]).GetValue (node);
				val.Val = col_val;
			}

			public bool IterNext (ref TreeIter iter)
			{
				ITreeNode node = GetNode (iter);

				int idx;
				if (node.Parent == null)
					idx = Nodes.IndexOf (node);
				else
					idx = node.Parent.IndexOf (node);

				if (idx < 0) throw new Exception ("Node not found in Nodes list");

				if (node.Parent == null) {
					if (++idx >= Nodes.Count)
						return false;
					node = Nodes [idx] as ITreeNode;
				} else {
					if (++idx >= node.Parent.ChildCount)
						return false;
					node = node.Parent [idx];
				}

				GetIter (node, ref iter);
				return true;
			}

			public bool IterPrevious (ref TreeIter iter)
			{
				ITreeNode node = GetNode (iter);

				int idx;
				if (node.Parent == null)
					idx = Nodes.IndexOf (node);
				else
					idx = node.Parent.IndexOf (node);

				if (idx < 0) throw new Exception ("Node not found in Nodes list");
				else if (idx == 0) return false;
				node = node.Parent == null ? Nodes [idx - 1] as ITreeNode : node.Parent [idx - 1];
				GetIter (node, ref iter);
				return true;
			}

			public bool IterChildren (out Gtk.TreeIter first_child, Gtk.TreeIter parent)
			{
				first_child = Gtk.TreeIter.Zero;

				if (parent.Equals (TreeIter.Zero)) {
					if (Nodes.Count <= 0)
						return false;
					first_child = GetIter (Nodes [0] as ITreeNode);
				} else {
					ITreeNode node = GetNode (parent);
					if (node.ChildCount <= 0)
						return false;

					first_child = GetIter (node [0]);
				}
				return true;
			}

			public bool IterHasChild (Gtk.TreeIter iter)
			{
				return IterNChildren (iter) > 0;
			}

			public int IterNChildren (Gtk.TreeIter iter)
			{
				if (iter.Equals (TreeIter.Zero))
					return Nodes.Count;
				else
					return GetNode (iter).ChildCount;
			}

			public bool IterNthChild (out Gtk.TreeIter child, Gtk.TreeIter parent, int n)
			{
				child = TreeIter.Zero;

				if (parent.Equals (TreeIter.Zero)) {
					if (Nodes.Count <= n)
						return false;
					child = GetIter (Nodes [n] as ITreeNode);
				} else {
					ITreeNode parent_node = GetNode (parent);
					if (parent_node.ChildCount <= n)
						return false;
					child = GetIter (parent_node [n]);
				}
				return true;
			}

			public bool IterParent (out Gtk.TreeIter parent, Gtk.TreeIter child)
			{
				parent = TreeIter.Zero;
				ITreeNode child_node = GetNode (child);

				if (child_node.Parent == null)
					return false;
				else {
					parent = GetIter (child_node.Parent);
					return true;
				}
			}

			private IList Nodes {
				get {
					return nodes as IList;
				}
			}

			private void changed_cb (object o, EventArgs args)
			{
				ITreeNode node = o as ITreeNode;
				model_adapter.EmitRowChanged (GetPath (node), GetIter (node));
			}

			private void EmitRowInserted (ITreeNode node)
			{
				model_adapter.EmitRowInserted (GetPath (node), GetIter (node));
				for (int i = 0; i < node.ChildCount; i++)
					EmitRowInserted (node [i]);
			}

			private void child_added_cb (object sender, ITreeNode child)
			{
				AddNodeInternal (child);
				EmitRowInserted (child);
			}

			private void child_deleted_cb (object sender, ITreeNode child, int idx)
			{
				ITreeNode node = sender as ITreeNode;
			
				TreePath path = GetPath (node);
				TreePath child_path = path.Copy ();
				child_path.AppendIndex (idx);

				model_adapter.EmitRowDeleted (child_path);

				if (node.ChildCount <= 0)
					model_adapter.EmitRowHasChildToggled (GetPath (node), GetIter (node));
			}

			private void AddNodeInternal (ITreeNode node)
			{
				node.Changed += new EventHandler (changed_cb);
				node.ChildAdded += new TreeNodeAddedHandler (child_added_cb);
				node.ChildRemoved += new TreeNodeRemovedHandler (child_deleted_cb);

				for (int i = 0; i < node.ChildCount; i++)
					AddNodeInternal (node [i]);
			}

			public void AddNode (ITreeNode node)
			{
				nodes.Add (node);
				AddNodeInternal (node);
				EmitRowInserted (node);
			}

			public void AddNode (ITreeNode node, int position)
			{
				nodes.Insert (position, node);
				AddNodeInternal (node);
				EmitRowInserted (node);
			}

			public void RemoveNode (ITreeNode node)
			{
				int idx = nodes.IndexOf (node);
				if (idx < 0)
					return;
				nodes.Remove (node);

				TreePath path = new TreePath ();
				path.AppendIndex (idx);

				model_adapter.EmitRowDeleted (path);
			}

			public void Clear ()
			{
				while (nodes.Count > 0)
					RemoveNode ((ITreeNode)nodes [0]);
			}

			public IEnumerator GetEnumerator ()
			{
				return nodes.GetEnumerator ();
			}
		}
	}	
}
