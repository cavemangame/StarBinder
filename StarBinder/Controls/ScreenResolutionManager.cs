using System;
using CocosSharp;

namespace StarBinder
{
	public class ScreenResolutionManager
	{
		// Singleton standart implementation
		private static ScreenResolutionManager instance;
		public static ScreenResolutionManager Instance
		{
			get 
			{
				if (instance == null)
					throw new NullReferenceException ("Cannot find resolution manager!");
				return instance; 
			}
		}

		// Properties and create
		public float Width { get; private set;}
		public float Height { get; private set;}

		protected ScreenResolutionManager (float width, float height) 
		{
			Width = width;
			Height = height;
		}

		public static ScreenResolutionManager CreateResolutionManager(float width, float height) 
		{
			if (instance == null)
				instance = new ScreenResolutionManager (width, height);
			return instance;
		}

		// Methods to resolute

		/// <summary>
		/// Gets the rect in absolute coordinates by relative.
		/// </summary>
		/// <returns>The absolute rect.</returns>
		/// <param name="rect">The relative rect.</param>
		public CCRect GetRect(CCRect rect)
		{
			if (instance == null)
				throw new NullReferenceException ("Cannot find resolution manager!");
			// for test
			float x = rect.MinX * Width;
			float y = rect.MinY * Height;
			float width = rect.Size.Width * Width;
			float height = rect.Size.Height * Height;

			return new CCRect (x, y, width, height);
			// return new CCRect(rect.MinX * Width, rect.MinY * Height, rect.Size.Width * Width, rect.Size.Height * Height);
		}
	}
}

