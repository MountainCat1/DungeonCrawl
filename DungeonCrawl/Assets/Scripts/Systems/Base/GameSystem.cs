using System;
using UnityEngine;

namespace DefaultNamespace.Systems.Base
{
    public abstract class GameSystem : MonoBehaviour
    {
        protected virtual void Start()
        {
            Debug.Log("Start in GameSystem");
        }

        protected virtual void Awake()
        {
        }
    }
}