using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class DoPathAnimSystem : MonoBehaviour
{
    [SerializeField] private List<Transform> objs;
    [SerializeField] private List<Transform> nextObjs;
    [SerializeField] private List<Transform> corners;
    [SerializeField] private float oneLapDuration = 5.0f;
    private List<TweenerCore<Vector3, Path, PathOptions>> handlers;

    private void Start()
    {
        handlers = new List<TweenerCore<Vector3, Path, PathOptions>>();
        for (int i = 0; i < objs.Count; i++)
        {
            List<Vector3> queue = new List<Vector3>();
            queue.Add(objs[i].localPosition);
            for (int j = corners.IndexOf(nextObjs[i]); j < corners.Count; j++)
            {
                queue.Add(corners[j].localPosition);
            }
            
            for (int j = 0; j < corners.IndexOf(nextObjs[i]); j++)
            {
                queue.Add(corners[j].localPosition);
            }

            queue.Add(objs[i].localPosition);

            handlers.Add(objs[i].DOLocalPath(queue.ToArray(), oneLapDuration).SetLoops(-1).SetEase(Ease.Linear));
        }
    }

    private void Update()
    {
        if (GameController.Status != GameStatus.Playing)
        {
            foreach (var tweenerCore in handlers)
            {
                tweenerCore.Pause();
            }
            return;
        }
        
        foreach (var tweenerCore in handlers)
        {
            tweenerCore.Play();
        }
        
    }
}
