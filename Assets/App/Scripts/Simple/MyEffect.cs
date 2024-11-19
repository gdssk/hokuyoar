using UnityEngine;

public class MyEffect : MonoBehaviour
{
    // component
    [SerializeField] private ParticleSystem _particle;

    // property
    public int Id { get; set; }

    /// <summary>
    /// start
    /// </summary>
    void Start() { _particle.Stop(); }

    /// <summary>
    /// is playing
    /// </summary>
    public bool IsPlaying => _particle.isPlaying;

    /// <summary>
    /// Play
    /// </summary>
    public void Play(Vector3 position)
    {
        transform.localPosition = position;
        _particle.Play();
    }

    /// <summary>
    /// Move
    /// </summary>
    public void Move(Vector3 position)
    {
        transform.localPosition = position;
    }

    /// <summary>
    /// Stop
    /// </summary>
    public void Stop()
    {
        _particle.Stop();
    }
}
