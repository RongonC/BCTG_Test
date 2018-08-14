using System;
using Xamarin.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;



[assembly: 
	InternalsVisibleTo ("StemmonsMobile.iOS"),
 InternalsVisibleTo ("StemmonsMobile.Droid")]
namespace StemmonsMobile.Views
{
	public enum SwipeGestureRecognizerDirection
	{
		Right = 1,
		Left = 2,
		Up = 4,
		Down = 8
	}

	public class SwipeGestureRecognizer : BaseGestureRecognizer
	{

		public int NumberOfTouchesRequired { get; set; }

		public SwipeGestureRecognizerDirection Direction { get; set; }

		public SwipeGestureRecognizer ()
		{
			NumberOfTouchesRequired = 1;
		}

		public override string ToString ()
		{
			return string.Format ("[SwipeGestureRecognizer: NumberOfTouchesRequired={0}, Direction={1} State={2}]", NumberOfTouchesRequired, Direction, State);
		}
		

	}
}

