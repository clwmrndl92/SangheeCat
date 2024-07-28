//#define OneButtonControl

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/*using VContainer;
using VContainer.Unity;*/

public class CarController : MonoBehaviour
{
	[Header("입력")]
	[SerializeField] private InputActionAsset _inputActionAsset;
	[SerializeField] private InputActionReference _horizontalAction;
	[Header("차량")] 
	[SerializeField] public bool isTire = false;
	
	private Rigidbody2D _rigidbody;
	[SerializeField] private WheelJoint2D _rearWheel;
	[SerializeField] private WheelJoint2D _frontWheel;
	[SerializeField] private float force = 1000f;
	[SerializeField] private float _rotationTorque = 100f;

	[SerializeField] internal Rigidbody2D _rearWheelRB;
	[SerializeField] internal Rigidbody2D _frontWheelRB;
	[SerializeField] internal float _speed = 150f;

	//[SerializeField] private float _rotationLimit = 60;
	
	//private GameManager _gameManager;
	
	
#if OneButtonControl
	private float _horizontal;
#endif
	
	/*[Inject]
	public void Construct(GameManager gameManager)
	{
		_gameManager = gameManager;
	}*/

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		//Debug.Log($"Car GameManager : {_gameManager}");

	}
	
	private void OnEnable()
	{
		_inputActionAsset.Enable();
	}
	
	private void OnDisable()
	{
		_inputActionAsset.Disable();
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	private void Update()
	{
		// Debug.Log("speed: " + _rigidbody.velocity.magnitude);
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			var scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		/*if(_gameManager.GameState != GameState.InGame)
		{
			return;
		}*/
		
		/*// rotation limit
		if (transform.eulerAngles.z > _rotationLimit && transform.eulerAngles.z <= 180 )
		{
			transform.rotation = Quaternion.Euler(0, 0, _rotationLimit) ;  
		}
		else if (transform.eulerAngles.z < 360 - _rotationLimit && transform.eulerAngles.z > 180)
		{
			transform.rotation = Quaternion.Euler(0, 0, 360 - _rotationLimit) ;  
		}*/
		
	#if OneButtonControl
		var horizontalInput = _horizontalAction.action.ReadValue<float>();
 		// Update horizontal value
 		if (horizontalInput > 0)
 		{
 			// accelerate
 			_horizontal = Mathf.Lerp(_horizontal, _accelModifier, Time.deltaTime * _inputModifier);
 		}
 		else
 		{
 			_horizontal = 0;
 			// // deaccelerate
 			// _horizontal = Mathf.Lerp(_horizontal, _deaccelModifier, Time.deltaTime * _inputModifier);
 		}
 		var horizontal = _horizontal;
	
		
 		// check is grounded
 		bool isGrounded = Physics2D.Raycast(transform.position, -transform.up , 2f, 1 << LayerMask.NameToLayer("Ground"));
 																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																				Physics2D.Raycast(transform.position, -transform.up , 2f, 1 << LayerMask.NameToLayer("Ground"));
 		if (isGrounded)
 		{
 			_rigidbody.AddForce(transform.right * (force * horizontal));
 			// disable backward force
 			if (_rigidbody.velocity.x < 0)
 			{
 				_rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
 			}
 		}
	
 		if (horizontal > 0)
 		{
 			_rigidbody.AddTorque(-_rotationTorque * horizontal);
 		}
 		if(horizontal < 0)
 		{
 			_rigidbody.AddTorque(-_rotationTorque * horizontal/10);
 		}
	#else
 		var horizontal = _horizontalAction.action.ReadValue<float>();
	    
 		// check is grounded
 		bool isGrounded = Physics2D.Raycast(transform.position, -transform.up , 2f, 1 << LayerMask.NameToLayer("Ground"));

	    if (isTire)
	    {
		    //TODO: Separate Accel and Brake Controlls
		    _frontWheelRB.AddTorque(-_speed * horizontal * 0.2f * Time.fixedDeltaTime);
		    _rearWheelRB.AddTorque(-_speed * horizontal * Time.fixedDeltaTime);
	    }
	    else
	    {
		    if (isGrounded)
		    {
			    _rigidbody.AddForce(transform.right * (force * horizontal));
		    }
	
		    if (horizontal > 0)
		    {
			    _rigidbody.AddTorque(-_rotationTorque * horizontal);
		    }
		    if(horizontal < 0)
		    {
			    _rigidbody.AddTorque(-_rotationTorque * horizontal/10);
		    }
	    }
 		
	#endif
	}
	
}
/*public sealed class RequiresCarEntryPoint : IInitializable
{
    public RequiresCarEntryPoint(CarController carController)
         => Debug.Log(carController);

    public void Initialize() {}
}*/