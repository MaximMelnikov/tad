using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourEX : MonoBehaviour {
	/*void OnEnable()
	{
		UpdateManager.Instance.RegisterUpdate(this);
	}

	void OnDisable()
	{
		UpdateManager.Instance.UnregisterUpdate(this);
	}

	public void ManagedUpdate()
	{
		// do what you normally do in Update here
	}
	
	[HideInInspector, NonSerialized]
	private Animation _animation;

	/// <summary>
	/// Gets the Animation attached to the object.
	/// </summary>
	public new Animation animation { get { return _animation ? _animation : (_animation = GetComponent<Animation>()); } }

	[HideInInspector, NonSerialized]
	private AudioSource _audio;

	/// <summary>
	/// Gets the AudioSource attached to the object.
	/// </summary>
	public new AudioSource audio { get { return _audio ? _audio : (_audio = GetComponent<AudioSource>()); } }

	[HideInInspector, NonSerialized]
	private Camera _camera;

	/// <summary>
	/// Gets the Camera attached to the object.
	/// </summary>
	public new Camera camera { get { return _camera ? _camera : (_camera = GetComponent<Camera>()); } }

	[HideInInspector, NonSerialized]
	private Collider _collider;

	/// <summary>
	/// Gets the Collider attached to the object.
	/// </summary>
	public new Collider collider { get { return _collider ? _collider : (_collider = GetComponent<Collider>()); } }

	[HideInInspector, NonSerialized]
	private Collider2D _collider2D;

	/// <summary>
	/// Gets the Collider2D attached to the object.
	/// </summary>
	public new Collider2D collider2D { get { return _collider2D ? _collider2D : (_collider2D = GetComponent<Collider2D>()); } }

	[HideInInspector, NonSerialized]
	private ConstantForce _constantForce;

	/// <summary>
	/// Gets the ConstantForce attached to the object.
	/// </summary>
	public new ConstantForce constantForce { get { return _constantForce ? _constantForce : (_constantForce = GetComponent<ConstantForce>()); } }

	[HideInInspector, NonSerialized]
	private GUIText _guiText;

	/// <summary>
	/// Gets the GUIText attached to the object.
	/// </summary>
	public new GUIText guiText { get { return _guiText ? _guiText : (_guiText = GetComponent<GUIText>()); } }

	[HideInInspector, NonSerialized]
	private GUITexture _guiTexture;

	/// <summary>
	/// Gets the GUITexture attached to the object.
	/// </summary>
	public new GUITexture guiTexture { get { return _guiTexture ? _guiTexture : (_guiTexture = GetComponent<GUITexture>()); } }

	[HideInInspector, NonSerialized]
	private HingeJoint _hingeJoint;

	/// <summary>
	/// Gets the HingeJoint attached to the object.
	/// </summary>
	public new HingeJoint hingeJoint { get { return _hingeJoint ? _hingeJoint : (_hingeJoint = GetComponent<HingeJoint>()); } }

	[HideInInspector, NonSerialized]
	private Light _light;

	/// <summary>
	/// Gets the Light attached to the object.
	/// </summary>
	public new Light light { get { return _light ? _light : (_light = GetComponent<Light>()); } }

	[HideInInspector, NonSerialized]
	private NetworkView _networkView;

	/// <summary>
	/// Gets the NetworkView attached to the object.
	/// </summary>
	public new NetworkView networkView { get { return _networkView ? _networkView : (_networkView = GetComponent<NetworkView>()); } }

	[HideInInspector, NonSerialized]
	private ParticleSystem _particleSystem;

	/// <summary>
	/// Gets the ParticleSystem attached to the object.
	/// </summary>
	public new ParticleSystem particleSystem { get { return _particleSystem ? _particleSystem : (_particleSystem = GetComponent<ParticleSystem>()); } }

	[HideInInspector, NonSerialized]
	private Renderer _renderer;

	/// <summary>
	/// Gets the Renderer attached to the object.
	/// </summary>
	public new Renderer renderer { get { return _renderer ? _renderer : (_renderer = GetComponent<Renderer>()); } }

	[HideInInspector, NonSerialized]
	private Rigidbody _rigidbody;

	/// <summary>
	/// Gets the Rigidbody attached to the object.
	/// </summary>
	public new Rigidbody rigidbody { get { return _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>()); } }

	[HideInInspector, NonSerialized]
	private Rigidbody2D _rigidbody2D;

	/// <summary>
	/// Gets the Rigidbody2D attached to the object.
	/// </summary>
	public new Rigidbody2D rigidbody2D { get { return _rigidbody2D ? _rigidbody2D : (_rigidbody2D = GetComponent<Rigidbody2D>()); } }*/
	
	public void AddTimer(float time, Action callback)
    {
        AddTimer(time, callback, false, false);
    }

    public void AddTimer(float time, Action callback, bool cyclic)
    {
        AddTimer(time, callback, cyclic, false);
    }

    public void AddTimer(float time, Action callback, bool cyclic, bool ignoreTimeScale)
    {
        if (time != 0)
        {
            if (cyclic)
            {
                StartCoroutine(TimerCyclic(time, callback));
            }
            else if (ignoreTimeScale)
            {
                Debug.Log("ignoreTimeScale");
                StartCoroutine(TimerIgnoreTimescale(time, callback));
            }
            else
            {
                StartCoroutine(Timer(time, callback));
            }
        }
        else
        {
            try
            {
                callback();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }


    IEnumerator TimerCyclic(float time, Action callback)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            try
            {
                callback();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static IEnumerator WaitForRealSeconds(float delay)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + delay)
        {
            yield return null;
        }
    }

    private IEnumerator TimerIgnoreTimescale(float delay, Action callback)
    {
        yield return StartCoroutine(WaitForRealSeconds(delay));
        try
        {
            callback();
        }
        catch (Exception)
        {
            throw;
        }
    }

    IEnumerator Timer(float time, Action callback)
    {
        yield return new WaitForSeconds(time);

        try
        {
            callback();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
