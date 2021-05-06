using SDMobileXF.Classes;
using System;
using Xamarin.Forms;

namespace SDMobileXF
{
	public class PinchToZoomContainer : ContentView
	{
		double currentScale = 1;
		double startScale = 1;
		double xOffset = 0;
		double yOffset = 0;
		Point startScaleOrigin;

		public PinchToZoomContainer()
		{
			var pinchGesture = new PinchGestureRecognizer();
			pinchGesture.PinchUpdated += OnPinchUpdated;
			GestureRecognizers.Add(pinchGesture);

			var pan = new PanGestureRecognizer();
			pan.PanUpdated += OnPanUpdated;
			GestureRecognizers.Add(pan);
		}

		private static Point CalculateDiff(Point first, Point second)
		{
			return second.Offset(-first.X, -first.Y);
		}

		private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
		{
			if (e.StatusType == GestureStatus.Started)
			{
				this.xOffset = this.Content.TranslationX;
				this.yOffset = this.Content.TranslationY;
			}

			if (e.StatusType != GestureStatus.Completed && e.StatusType != GestureStatus.Canceled)
			{
				this.Content.TranslationX = this.xOffset + e.TotalX;
				this.Content.TranslationY = this.yOffset + e.TotalY;
			}

			if (e.StatusType == GestureStatus.Completed)
			{
				this.xOffset = this.Content.TranslationX;
				this.yOffset = this.Content.TranslationY;
			}
		}

		void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
		{
			if (e.Status == GestureStatus.Started)
			{
				// Store the current scale factor applied to the wrapped user interface element,
				// and zero the components for the center point of the translate transform.
				startScale = Content.Scale;
				Content.AnchorX = 0;
				Content.AnchorY = 0;

				this.startScaleOrigin = e.ScaleOrigin;
			}
			if (e.Status == GestureStatus.Running)
			{
				var originDiff = PinchToZoomContainer.CalculateDiff(e.ScaleOrigin, this.startScaleOrigin);

				// Calculate the scale factor to be applied.
				currentScale += (e.Scale - 1) * startScale;
				currentScale = Math.Max(0.5, currentScale);

				// The ScaleOrigin is in relative coordinates to the wrapped user interface element,
				// so get the X pixel coordinate.
				double renderedX = Content.X + xOffset;
				double deltaX = renderedX / Width;
				double deltaWidth = Width / (Content.Width * startScale);
				double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

				// The ScaleOrigin is in relative coordinates to the wrapped user interface element,
				// so get the Y pixel coordinate.
				double renderedY = Content.Y + yOffset;
				double deltaY = renderedY / Height;
				double deltaHeight = Height / (Content.Height * startScale);
				double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

				// Calculate the transformed element pixel coordinates.
				//double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
				//double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);
				double targetX = this.xOffset - ((originX) * this.Content.Width) * (this.currentScale - this.startScale) - originDiff.X * this.Content.Width;
				double targetY = this.yOffset - ((originY) * this.Content.Height) * (this.currentScale - this.startScale) - originDiff.Y * this.Content.Height;

				// Apply translation based on the change in origin.
				Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
				Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

				// Apply scale factor
				Content.Scale = currentScale;
			}
			if (e.Status == GestureStatus.Completed)
			{
				// Store the translation delta's of the wrapped user interface element.
				xOffset = Content.TranslationX;
				yOffset = Content.TranslationY;
			}
		}
	}
}
