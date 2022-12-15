using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;

namespace ReflectionStudio.Controls
{
	public class WaitSpin : Control
	{
		static WaitSpin()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitSpin),
				new FrameworkPropertyMetadata(typeof(WaitSpin)));
		}

		#region --------------------DEPENDENCY PROPERTIES--------------------

		public static readonly DependencyProperty ThicknessProperty =
			   DependencyProperty.Register("Thickness", typeof(int), typeof(WaitSpin));

		public int Thickness
		{
			get { return (int)GetValue(ThicknessProperty); }
			set { SetValue(ThicknessProperty, value); }
		}
		#endregion

		#region Data
		private DispatcherTimer _animationTimer;
		private RotateTransform SpinnerRotate;
        #endregion

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			SpinnerRotate = this.Template.FindName("PART_SpinnerRotate", this) as RotateTransform;

			Canvas cs= this.Template.FindName("PART_Frame", this) as Canvas;
			cs.IsVisibleChanged += new DependencyPropertyChangedEventHandler(HandleVisibleChanged);
			cs.SizeChanged += new SizeChangedEventHandler(HandleSizeChanged);

			//Initialize();
			if (_animationTimer == null)
			{
				_animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
				_animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 120);
			}
		}

		void HandleSizeChanged(object sender, SizeChangedEventArgs e)
		{
		}

        #region Private Methods
        private void Start()
        {
            _animationTimer.Tick += HandleAnimationTick;
            _animationTimer.Start();
        }

        private void Stop()
        {
            _animationTimer.Stop();
            _animationTimer.Tick -= HandleAnimationTick;
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 10) % 360;
        }

		private void CreateCircle()
		{
			const double offset = Math.PI;
			const double step = Math.PI * 2 / 10.0;

			for (int i = 0; i < 9; i++)
			{
				Ellipse e = this.Template.FindName("PART_Ellipse" + i, this) as Ellipse;
				SetPosition(e, offset, i, step);
			}
		}

		//private void Initialize()
		//{
		//    if (_animationTimer == null)
		//    {
		//        _animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
		//        _animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);
		//    }
		//    const double offset = Math.PI;
		//    const double step = Math.PI * 2 / 10.0;

		//    for (int i = 0; i < 9; i++)
		//        SetPosition(listEllipse[i], offset, i, step);

		//    //SetPosition(C1, offset, 1.0, step);
		//    //SetPosition(C2, offset, 2.0, step);
		//    //SetPosition(C3, offset, 3.0, step);
		//    //SetPosition(C4, offset, 4.0, step);
		//    //SetPosition(C5, offset, 5.0, step);
		//    //SetPosition(C6, offset, 6.0, step);
		//    //SetPosition(C7, offset, 7.0, step);
		//    //SetPosition(C8, offset, 8.0, step);
		//}

		private void SetPosition(Ellipse ellipse, double offset, double posOffSet, double step)
		{
			ellipse.SetValue(Canvas.LeftProperty, 50.0 + Math.Sin(offset + posOffSet * step) * 50.0);
			ellipse.SetValue(Canvas.TopProperty, 50 + Math.Cos(offset + posOffSet * step) * 50.0);
		}

        protected void HandleVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible)
                Start();
            else
                Stop();
        }
        #endregion
	}
}
