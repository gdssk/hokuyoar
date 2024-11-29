using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using uOSC;

public class MySimpleManager : MonoBehaviour
{
    // var
    private List<MyEffect> _effectCache = new ();

    // component
    [SerializeField] private uOscServer _oscServer;
    [SerializeField] private ARTrackedImageManager _imageManager;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _effect;

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        // モニターを消さない
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // cache
        for (int i = 0; i < 10; i++)
        {
            var obj = Instantiate(_effect, _content);
            var effect = obj.GetComponent<MyEffect>();
            effect.Id = i;
            effect.Stop();
            _effectCache.Add(effect);
        }

        // ar
        _imageManager.trackedImagesChanged += OnTrackedImagesChanged;

        // osc
        var result = new List<int>();
        _oscServer.onDataReceived.AddListener((data) =>
        {
            result.Clear();

            var list = data.values;
            for (int i = 0; i < list.Length; i += 4)
            {
                var x = float.Parse(list[i + 0].ToString()) * -1;
                var y = float.Parse(list[i + 1].ToString()) * -1;
                var s = float.Parse(list[i + 2].ToString());
                var t = float.Parse(list[i + 3].ToString());
                if (s < 0.001f && t < 0.001f) continue;
                var id = PlayEffect(new Rect(x, y, s, t));
                result.Add(id);
            }
            foreach (var e in _effectCache)
            {
                if (result.Contains(e.Id)) { continue; }
                e.Stop();
            }
        });
    }

    private int PlayEffect(Rect rect)
    {
        var effect = GetNearEffect(rect);
        if (effect != null)
        {
            effect.Move(new Vector3(rect.x, 0, rect.y));
            return effect.Id;
        }
        foreach (var e in _effectCache)
        {
            if (e.IsPlaying) { continue; }
            e.Play(new Vector3(rect.x, 0, rect.y));
            return e.Id;
        }
        return -1;
    }

    private MyEffect GetNearEffect(Rect rect)
    {
        var min = 0.2;
        MyEffect near = null;
        foreach (var effect in _effectCache)
        {
            if (!effect.IsPlaying) { continue; }
            var p = effect.transform.localPosition;
            var d = Vector2.Distance(new Vector2(p.x, p.z), new Vector2(rect.x, rect.y));
            if (d < min)
            {
                min = d;
                near = effect;
            }
        }
        return near;
    }

    /// <summary>
    /// OnTrackedImagesChanged
    /// </summary>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (args.updated.Count <= 0) { return; }
        foreach (var img in args.updated) switch (img.trackingState)
        {
            case TrackingState.Tracking:
                var t = img.transform;
                var p = t.position;
                var r = t.rotation;
                _content.SetPositionAndRotation(p, r);
                _imageManager.enabled = false;
                break;
        }
    }
}
