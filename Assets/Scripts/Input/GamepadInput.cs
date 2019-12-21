using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gamepad Input for Keyboard and Xbox Controller for Windows Platform.
/// </summary>
public class GamepadInput : MonoBehaviour
{
    /// <summary>
    /// If false, all input is suppressed.
    /// </summary>
    public bool EnablePlayerControls;

    /// <summary>
    /// The controller number from 0-4. Mapped via <see cref="ControllerType"/>.
    /// </summary>
    public int ControllerNumber;

    /// <summary>
    /// The threshold for shooting buttons.
    /// </summary>
    public float ShootingButtonThreshold;

    public enum ControllerType
    {
        Keyboard = 0,
        Joystick1 = 1,
        Joystick2 = 2,
        Joystick3 = 3,
        Joystick4 = 4
    }

    private const string LeftHorizontal = "LeftHorizontal";
    private const string LeftVertical = "LeftVertical";
    private const string RightHorizontal = "RightHorizontal";
    private const string RightVertical = "RightVertical";
    private const string Shoot = "Shoot";

    // Use this for initialization
    // ReSharper disable once Unity.RedundantEventFunction
    void Start()
    {
    }

    // Update is called once per frame
    // ReSharper disable once Unity.RedundantEventFunction
    void Update()
    {
    }

    private string _leftHorizontalAxisName = null;

    private string LeftHorizontalAxisName
    {
        get
        {
            if (_leftHorizontalAxisName == null)
            {
                ControllerType t = (ControllerType) ControllerNumber;
                _leftHorizontalAxisName = LeftHorizontal + t.ToString();
            }

            return _leftHorizontalAxisName;
        }
    }

    /// <summary>
    /// Returns the value from the left stick X axis.
    /// </summary>
    /// <returns></returns>
    public float GetLeftHorizontalValue()
    {
        var val = Input.GetAxis(LeftHorizontalAxisName);
        return EnablePlayerControls ? val : 0;
    }

    private string _leftVerticalAxisName = null;

    private string LeftVerticalAxisName
    {
        get
        {
            if (_leftVerticalAxisName == null)
            {
                ControllerType t = (ControllerType) ControllerNumber;
                _leftVerticalAxisName = LeftVertical + t.ToString();
            }

            return _leftVerticalAxisName;
        }
    }

    /// <summary>
    /// Returns the value from the left stick Y axis.
    /// </summary>
    /// <returns></returns>
    public float GetLeftVerticalValue()
    {
        var val = Input.GetAxis(LeftVerticalAxisName);
        return EnablePlayerControls ? val : 0;
    }

    /// <summary>
    /// Returns the value from the right stick X axis.
    /// </summary>
    /// <returns></returns>
    public float GetRightHorizontalValue()
    {
        ControllerType t = (ControllerType) ControllerNumber;
        var horVal = Input.GetAxis(RightHorizontal + t.ToString());

        return EnablePlayerControls ? horVal : 0;
    }

    /// <summary>
    /// Returns the value from the right stick Y axis.
    /// </summary>
    /// <returns></returns>
    public float GetRightVerticalValue()
    {
        ControllerType t = (ControllerType) ControllerNumber;
        var horVal = Input.GetAxis(RightVertical + t.ToString());

        return EnablePlayerControls ? horVal : 0;
    }

    private KeyCode JumpKeyCode
    {
        get
        {
            KeyCode code = KeyCode.Space;
            switch ((ControllerType) ControllerNumber)
            {
                case ControllerType.Joystick1:
                    code = KeyCode.Joystick1Button0;
                    break;
                case ControllerType.Joystick2:
                    code = KeyCode.Joystick2Button0;
                    break;
                case ControllerType.Joystick3:
                    code = KeyCode.Joystick3Button0;
                    break;
                case ControllerType.Joystick4:
                    code = KeyCode.Joystick4Button0;
                    break;
                default:
                    break;
            }

            return code;
        }
    }

    /// <summary>
    /// Returns true if the jump button was pressed.
    /// </summary>
    /// <returns></returns>
    public bool IsJumpPressed()
    {
        return EnablePlayerControls && Input.GetKeyDown(JumpKeyCode);
    }

    /// <summary>
    /// Returns true if the jump button was released.
    /// </summary>
    /// <returns></returns>
    public bool IsJumpReleased()
    {
        return EnablePlayerControls && Input.GetKeyUp(JumpKeyCode);
    }

    private KeyCode PauseKeyCode
    {
        get
        {
            KeyCode code = KeyCode.Escape;
            switch ((ControllerType) ControllerNumber)
            {
                case ControllerType.Joystick1:
                    code = KeyCode.Joystick1Button7;
                    break;
                case ControllerType.Joystick2:
                    code = KeyCode.Joystick2Button7;
                    break;
                case ControllerType.Joystick3:
                    code = KeyCode.Joystick3Button7;
                    break;
                case ControllerType.Joystick4:
                    code = KeyCode.Joystick4Button7;
                    break;
                default:
                    break;
            }

            return code;
        }
    }

    /// <summary>
    /// Pauses the game for all controllers.
    /// </summary>
    /// <returns></returns>
    public bool IsPausePressed()
    {
        return EnablePlayerControls && Input.GetKeyUp(PauseKeyCode);
    }

    private string _fireAxisName = null;

    private string FireAxisName
    {
        get
        {
            if (_fireAxisName == null)
            {
                ControllerType t = (ControllerType) ControllerNumber;
                _fireAxisName = Shoot + t.ToString();
            }

            return _fireAxisName;
        }
    }
    
    private bool regularFirePressed = false;

    /// <summary>
    /// Returns true if the fire button was pressed.
    /// </summary>
    /// <returns></returns>
    public bool IsRegularFirePressed()
    {
        if (!EnablePlayerControls)
            return false;

        float pressedDepth = Input.GetAxis(FireAxisName);

        if (pressedDepth > ShootingButtonThreshold)
        {
            regularFirePressed = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the fire button was released after it was pressed.
    /// </summary>
    /// <returns></returns>
    public bool IsRegularFireReleased()
    {
        if (!EnablePlayerControls)
            return false;

        float pressedDepth = Input.GetAxis(FireAxisName);

        if (regularFirePressed && pressedDepth < ShootingButtonThreshold)
        {
            regularFirePressed = false;
            return true;
        }

        return false;
    }
    
    private bool specialFirePressed = false;

    /// <summary>
    /// Returns true if the special fire button was pressed.
    /// </summary>
    /// <returns></returns>
    public bool IsSpecialFirePressed()
    {
        if (!EnablePlayerControls)
            return false;
        
        float pressedDepth = Input.GetAxis(FireAxisName);

        if (pressedDepth < -ShootingButtonThreshold)
        {
            specialFirePressed = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the special fire button was released after it was pressed.
    /// </summary>
    /// <returns></returns>
    public bool IsSpecialFireReleased()
    {
        if (!EnablePlayerControls)
            return false;

        float pressedDepth = Input.GetAxis(FireAxisName);

        if (specialFirePressed && pressedDepth > -ShootingButtonThreshold)
        {
            specialFirePressed = false;
            return true;
        }

        return false;
    }

    private KeyCode YKeyCodeController
    {
        get
        {
            KeyCode code = KeyCode.Joystick1Button3;
            switch ((ControllerType) ControllerNumber)
            {
                case ControllerType.Joystick1:
                    code = KeyCode.Joystick1Button3;
                    break;
                case ControllerType.Joystick2:
                    code = KeyCode.Joystick2Button3;
                    break;
                case ControllerType.Joystick3:
                    code = KeyCode.Joystick3Button3;
                    break;
                case ControllerType.Joystick4:
                    code = KeyCode.Joystick4Button3;
                    break;
            }

            return code;
        }
    }

    public bool IsChangeFeaturePressed()
    {
        return EnablePlayerControls &&
               (Input.GetKeyDown(YKeyCodeController) || Input.GetKeyDown(KeyCode.LeftControl));
    }

    public bool IsAltChangeFeaturePressed()
    {
        return EnablePlayerControls &&
               (Input.GetKeyDown(YKeyCodeController) || Input.GetKeyDown(KeyCode.RightControl));
    }

    private KeyCode BKeyCodeController
    {
        get
        {
            KeyCode code = KeyCode.Joystick1Button1;
            switch ((ControllerType) ControllerNumber)
            {
                case ControllerType.Joystick1:
                    code = KeyCode.Joystick1Button1;
                    break;
                case ControllerType.Joystick2:
                    code = KeyCode.Joystick2Button1;
                    break;
                case ControllerType.Joystick3:
                    code = KeyCode.Joystick3Button1;
                    break;
                case ControllerType.Joystick4:
                    code = KeyCode.Joystick4Button1;
                    break;
            }

            return code;
        }
    }

    public bool IsShockwaveFeaturePressed()
    {
        return EnablePlayerControls &&
               (Input.GetKeyDown(BKeyCodeController) || Input.GetKeyDown(KeyCode.LeftShift));
    }

    public bool IsAltShockwaveFeaturePressed()
    {
        return EnablePlayerControls &&
               (Input.GetKeyDown(BKeyCodeController) || Input.GetKeyDown(KeyCode.RightShift));
    }
}