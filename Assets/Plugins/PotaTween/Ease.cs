using UnityEngine;

namespace Cngine
{

	public class Ease
	{
		public enum Equation
		{
			Linear,
			OutExpo, InExpo, InOutExpo, OutInExpo,
			OutCirc, InCirc, InOutCirc, OutInCirc,
			//OutQuad, InQuad, InOutQuad, OutInQuad,
			OutSine, InSine, InOutSine, OutInSine,
			//OutCubic, InCubic, InOutCubic, OutInCubic,
			//OutQuartic, InQuartic, InOutQuartic, OutInQuartic, 
			//OutQuintic, InQuintic, InOutQuintic, OutInQuintic,
			OutElastic, InElastic, InOutElastic, OutInElastic,
			OutBounce, InBounce, InOutBounce, OutInBounce,
			OutBack, InBack, InOutBack, OutInBack
		}
		
		#region Linear
		public static float Linear(float time, float start, float end, float duration)
		{
			return end * time / duration + start;
		}
		#endregion
		
		#region Expo
		public static float OutExpo(float time, float start, float end, float duration)
		{
			return (time == duration) ? start + end : end * (-Mathf.Pow(2, -10 * time / duration) + 1) + start;
		}
		
		public static float InExpo(float time, float start, float end, float duration)
		{
			return (time == 0) ? start : end * Mathf.Pow(2, 10 * (time / duration - 1)) + start;
		}
		
		public static float InOutExpo(float time, float start, float end, float duration)
		{
			if (time == 0)
				return start;
			
			if (time == duration)
				return start + end;
			
			if ((time /= duration / 2) < 1)
				return end / 2 * Mathf.Pow(2, 10 * (time - 1)) + start;
			
			return end / 2 * (-Mathf.Pow(2, -10 * --time) + 2) + start;
		}
		
		public static float OutInExpo(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return OutExpo(time * 2, start, end / 2, duration);
			
			return InExpo((time * 2) - duration, start + end / 2, end / 2, duration);
		}
		#endregion
		
		#region Circular
		public static float OutCirc(float time, float start, float end, float duration)
		{
			return end * Mathf.Sqrt(1 - (time = time / duration - 1) * time) + start;
		}
		
		public static float InCirc(float time, float start, float end, float duration)
		{
			return -end * (Mathf.Sqrt(1 - (time /= duration) * time) - 1) + start;
		}
		
		public static float InOutCirc(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return -end / 2 * (Mathf.Sqrt(1 - time * time) - 1) + start;
			
			return end / 2 * (Mathf.Sqrt(1 - (time -= 2) * time) + 1) + start;
		}
		
		public static float OutInCirc(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return OutCirc(time * 2, start, end / 2, duration);
			
			return InCirc((time * 2) - duration, start + end / 2, end / 2, duration);
		}
		#endregion
		
		#region Quad
		#endregion
		
		#region Sine
		public static float OutSine(float time, float start, float end, float duration)
		{
			return end * Mathf.Sin(time / duration * (Mathf.PI / 2)) + start;
		}
		
		public static float InSine(float time, float start, float end, float duration)
		{
			return -end * Mathf.Cos(time / duration * (Mathf.PI / 2)) + end + start;
		}
		
		public static float InOutSine(float time, float start, float end, float duration)
		{
			if ((time /= duration / 2) < 1)
				return end / 2 * (Mathf.Sin(Mathf.PI * time / 2)) + start;
			
			return -end / 2 * (Mathf.Cos(Mathf.PI * --time / 2) - 2) + start;
		}
		
		public static float OutInSine(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return OutSine(time * 2, start, end / 2, duration);
			
			return InSine((time * 2) - duration, start + end / 2, end / 2, duration);
		}
		#endregion
		
		#region Cubic
		#endregion
		
		#region Quartic
		#endregion
		
		#region Quintic
		#endregion
		
		#region Elastic
		public static float OutElastic(float time, float start, float end, float duration)
		{
			if ((time /= duration) == 1)
				return start + end;
			
			float p = duration * 0.3f;
			float s = p / 4;
			
			return (end * Mathf.Pow(2, -10 * time) * Mathf.Sin((time * duration - s) * (2 * Mathf.PI) / p) + end + start);
		}
		
		public static float InElastic(float time, float start, float end, float duration)
		{
			if ((time /= duration) == 1)
				return start + end;
			
			float p = duration * 0.3f;
			float s = p / 4;
			
			return -(end * Mathf.Pow(2, 10 * (time -= 1)) * Mathf.Sin((time * duration - s) * (2 * Mathf.PI) / p)) + start;
		}
		
		public static float InOutElastic(float time, float start, float end, float duration)
		{
			if ((time /= duration / 2) == 2)
				return start + end;
			
			float p = duration * (0.3f * 1.5f);
			float s = p / 4;
			
			if (time < 1)
				return -0.5f * (end * Mathf.Pow(2, 10 * (time -= 1)) * Mathf.Sin((time * duration - s) * (2 * Mathf.PI) / p)) + start;
			
			return end * Mathf.Pow(2, -10 * (time -= 1)) * Mathf.Sin((time * duration - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
		}
		
		public static float OutInElastic(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return OutElastic(time * 2, start, end / 2, duration);
			
			return InElastic((time * 2) - duration, start + end / 2, end / 2, duration);
		}
		#endregion
		
		#region Bounce
		public static float OutBounce(float time, float start, float end, float duration)
		{
			if ((time /= duration) < (1 / 2.75f))
				return end * (7.5625f * time * time) + start;
			else if (time < (2 / 2.75f))
				return end * (7.5625f * (time -= (1.5f / 2.75f)) * time + 0.75f) + start;
			else if (time < (2.5f / 2.75f))
				return end * (7.5625f * (time -= (2.25f / 2.75f)) * time + 0.9375f) + start;
			else
				return end * (7.5625f * (time -= (2.625f / 2.75f)) * time + 0.984375f) + start;
		}
		
		public static float InBounce(float time, float start, float end, float duration)
		{
			return end - OutBounce(duration - time, 0, end, duration) + start;
		}
		
		public static float InOutBounce(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return InBounce(time * 2, 0, end, duration) * 0.5f + start;
			
			return OutBounce(time * 2 - duration, 0, end, duration) * 0.5f + end * 0.5f + start;
		}
		
		public static float OutInBounce(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return OutBounce(time * 2, start, end / 2, duration);
			
			return InBounce((time * 2) - duration, start + end / 2, end / 2, duration);
		}
		#endregion
		
		#region Back
		public static float OutBack(float time, float start, float end, float duration)
		{
			return end * ((time = time / duration - 1) * time * ((1.70158f + 1) * time + 1.70158f) + 1) + start;
		}
		
		public static float InBack(float time, float start, float end, float duration)
		{
			return end * (time /= duration) * time * ((1.70158f + 1) * time - 1.70158f) + start;
		}
		
		public static float InOutBack(float time, float start, float end, float duration)
		{
			float s = 1.70158f;
			if ((time /= duration / 2) < 1)
				return end / 2 * (time * time * (((s *= (1.525f)) + 1) * time - s)) + start;
			
			return end / 2 * ((time -= 2) * time * (((s *= (1.525f)) + 1) * time + s) + 2) + start;
		}
		
		public static float OutInBack(float time, float start, float end, float duration)
		{
			if (time < duration / 2)
				return OutBack(time * 2, start, end / 2, duration);
			
			return InBack((time * 2) - duration, start + end / 2, end / 2, duration);
		}
		#endregion
	}
}