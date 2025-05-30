﻿using System.Collections;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.Feedbacks
{
	/// <summary>
	/// This feedback will let you change the width and color of a target line renderer over time
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the width and color of a target line renderer over time")]
	[FeedbackPath("Renderer/Line Renderer")]
	public class MMF_LineRenderer : MMF_Feedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.RendererColor; } }
		public override bool EvaluateRequiresSetup() => (TargetLineRenderer == null);
		public override string RequiredTargetText => TargetLineRenderer != null ? TargetLineRenderer.name : "";  
		public override string RequiresSetupText => "This feedback requires that a TargetLineRenderer be set to be able to work properly. You can set one below."; 
		#endif
		public override bool HasRandomness => true;
		public override bool HasCustomInspectors => true; 

		/// the possible modes for this feedback
		public enum Modes { OverTime, Instant }

		[MMFInspectorGroup("Line Renderer", true, 24, true)]
		/// the line renderer whose properties you want to modify
		[Tooltip("the line renderer whose properties you want to modify")]
		public LineRenderer TargetLineRenderer;
		/// whether the feedback should affect the sprite renderer instantly or over a period of time
		[Tooltip("whether the feedback should affect the sprite renderer instantly or over a period of time")]
		public Modes Mode = Modes.OverTime;
		/// how long the sprite renderer should change over time
		[Tooltip("how long the sprite renderer should change over time")]
		[MMFEnumCondition("Mode", (int)Modes.OverTime)]
		public float Duration = 2f;
		/// if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")] 
		public bool AllowAdditivePlays = false;
		/// a curve to use to animate the line renderer's density over time
		[Tooltip("a curve to use to animate the line renderer's density over time")]
		[MMFEnumCondition("Mode", (int)Modes.OverTime)]
		public MMTweenType Transition = new MMTweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.3f, 1f), new Keyframe(1, 0)));

		[MMFInspectorGroup("Width", true, 25)]
		/// whether or not to modify the line renderer's width
		[Tooltip("whether or not to modify the line renderer's width")]
		public bool ModifyWidth = true;
		/// a curve defining the new width of the line renderer, describing the world space width of the line at each point along its length
		[Tooltip("a curve defining the new width of the line renderer, describing the world space width of the line at each point along its length")]
		public AnimationCurve NewWidth = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));

		[MMFInspectorGroup("Color", true, 28)]
		/// whether or not to modify the line renderer's color
		[Tooltip("whether or not to modify the line renderer's color")]
		public bool ModifyColor = true;
		/// the colors to apply to the sprite renderer over time
		[Tooltip("the colors to apply to the sprite renderer over time")]
		public Gradient NewColor = new Gradient();

		/// the duration of this feedback is the duration of the sprite renderer, or 0 if instant
		public override float FeedbackDuration { get { return (Mode == Modes.Instant) ? 0f : ApplyTimeMultiplier(Duration); } set { if (Mode != Modes.Instant) { Duration = value; } } }
        
		protected Coroutine _coroutine;
		protected Gradient _initialColor;
		protected AnimationCurve _initialWidth;
		
		protected Gradient _firstColor;
		protected AnimationCurve _firstWidth;
		
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);

			if (Active)
			{
				if (TargetLineRenderer == null)
				{
					Debug.LogWarning("[Line Renderer Feedback] The line renderer feedback on "+Owner.name+" doesn't have a TargetLineRenderer, it won't work. You need to specify one in its inspector.");
					return;
				}
				
				_firstColor = TargetLineRenderer.colorGradient;
				_firstWidth = TargetLineRenderer.widthCurve;
			}
		}

		/// <summary>
		/// On Play we change the values of our line renderer
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized || (TargetLineRenderer == null))
			{
				return;
			}
			
			_initialColor = TargetLineRenderer.colorGradient;
			_initialWidth = TargetLineRenderer.widthCurve;
            
			float intensityMultiplier = ComputeIntensity(feedbacksIntensity, position);
			switch (Mode)
			{
				case Modes.Instant:
					if (ModifyColor)
					{
						TargetLineRenderer.colorGradient = NormalPlayDirection ? NewColor : _firstColor;
					}
					if (ModifyWidth)
					{
						TargetLineRenderer.widthCurve = NormalPlayDirection ? NewWidth : _firstWidth;
					}
					break;
				case Modes.OverTime:
					if (!AllowAdditivePlays && (_coroutine != null))
					{
						return;
					}
					if (_coroutine != null) { Owner.StopCoroutine(_coroutine); }
					_coroutine = Owner.StartCoroutine(LineRendererSequence(intensityMultiplier));
					break;
			}
		}

		/// <summary>
		/// This coroutine will modify the values on the line renderer over time
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerator LineRendererSequence(float intensityMultiplier)
		{
			IsPlaying = true;
			float journey = NormalPlayDirection ? 0f : FeedbackDuration;
			while ((journey >= 0) && (journey <= FeedbackDuration) && (FeedbackDuration > 0))
			{
				float remappedTime = MMFeedbacksHelpers.Remap(journey, 0f, FeedbackDuration, 0f, 1f);
				remappedTime = Transition.Evaluate(remappedTime);
				SetLineRendererValues(remappedTime, intensityMultiplier);

				journey += NormalPlayDirection ? FeedbackDeltaTime : -FeedbackDeltaTime;
				yield return null;
			}
			
			SetLineRendererValues(Transition.Evaluate(FinalNormalizedTime), intensityMultiplier);    
			_coroutine = null;      
			IsPlaying = false;
			yield return null;
		}

		/// <summary>
		/// Sets the various values on the line renderer on a specified time (between 0 and 1)
		/// </summary>
		/// <param name="time"></param>
		protected virtual void SetLineRendererValues(float time, float intensityMultiplier)
		{
			if (ModifyColor)
			{
				if (NormalPlayDirection)
				{
					TargetLineRenderer.colorGradient = MMColors.LerpGradients(_initialColor, NewColor, time);	
				}
				else
				{
					TargetLineRenderer.colorGradient = MMColors.LerpGradients(NewColor, _firstColor, time);
				}
			}

			if (ModifyWidth)
			{
				if (NormalPlayDirection)
				{
					TargetLineRenderer.widthCurve = MMAnimationCurves.LerpAnimationCurves(_initialWidth, NewWidth, time);	
				}
				else
				{
					TargetLineRenderer.widthCurve = MMAnimationCurves.LerpAnimationCurves(NewWidth, _firstWidth, time);
				}
			}
		}
        
		/// <summary>
		/// Stops this feedback
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (!Active || !FeedbackTypeAuthorized || (_coroutine == null))
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			IsPlaying = false;
			Owner.StopCoroutine(_coroutine);
			_coroutine = null;
		}
		
		/// <summary>
		/// On restore, we put our object back at its initial position
		/// </summary>
		protected override void CustomRestoreInitialValues()
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			TargetLineRenderer.widthCurve = _firstWidth;
			TargetLineRenderer.colorGradient = _firstColor;
		}
	}
}