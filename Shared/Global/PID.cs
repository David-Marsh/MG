using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace MG.Shared.Global
{
  public class PID
  {
    private Vector2 Error;
    private Vector2 LastError;
    private Vector2 ErrorSum;
    private Vector2 ErrorDelta;
    private Vector2 Output;
    private float OutputLengthSquared;
    public float Kp;
    public float Ki;
    public float Kd;

    public PID(float kp, float ki, float kd)
    {
      Kp = kp;
      Ki = ki;
      Kd = kd;
    }
    #region Used for tuning only
    public Vector2 P => Error * Kp;
    public Vector2 I => ErrorSum * Ki;
    public Vector2 D => ErrorDelta * Kd;
    public float KpSaturation => 1 / Kp;        // Ammount of error that will saturate the P term
    public float KiSaturation => 1 / Kp;        // Ammount of ErrorSum that will saturate the I term
    public float KdSaturation => 1 / Kp;        // Ammount of ErrorDelta that will saturate the D term
    public static bool Visable { get; set; }
    public static PID Tuning { get; set; }
    #endregion
    public Vector2 Target(Vector2 approch, Vector2 avoid)
    {
      Error = approch;
      Error += avoid;
      return Target();
    }
    public Vector2 Target(Vector2 approch)
    {
      Error = approch;
      return Target();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 Target()
    {
      ErrorDelta = Error - LastError;
      ErrorSum += Error;
      Output = (Error * Kp) + (ErrorSum * Ki) + (ErrorDelta * Kd);
      LastError = Error;
      ClampOutputAndErrorSum();
      return Output;
    }
    private void ClampOutputAndErrorSum()
    {
      if (ClampOutput()) { ErrorSum = Vector2.Zero; }
      else { ErrorSum *= 0.9f; }
    }
    private bool ClampOutput()
    {
      OutputLengthSquared = Output.LengthSquared();
      if (OutputLengthSquared > 1.0f)
      {
        Output = Vector2.Normalize(Output);
        return true;
      }
      if (OutputLengthSquared < 0.0001)
        Output = Vector2.Zero;
      return false;
    }
  }
}
