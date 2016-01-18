// Pango.Units.cs - Unit to pixel mapping class.
//
// Author: Mike Kestner <mkestner@novell.com>
//
// Copyright (c) 2005 Novell, Inc.
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

namespace Pango {

	using System;
	using System.Runtime.InteropServices;

	public class Units {

		private Units () {}
		
		[DllImport("pangosharpglue-2")]
		static extern int pangosharp_pixels (int units);

		[DllImport("pangosharpglue-2")]
		static extern int pangosharp_scale ();

		public static int FromPixels (int pixels)
		{
			return pixels * pangosharp_scale ();
		}

		public static int ToPixels (int units)
		{
			return pangosharp_pixels (units);
		}

	}
}
