using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Models;
using UnityEngine;

namespace Smartplank.Scripts
{
    public class DataRepository 
    {
        private LocalCacheManager _cacheManager;
        private Dictionary<Type, object> modelsDictionary = new Dictionary<Type, object>();

        public DataRepository(LocalCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public T GetData<T>()
        {
            var type = typeof(T);
            if (modelsDictionary.ContainsKey(type))
                return (T)modelsDictionary[type];

            var model = _cacheManager.Load<T>();
            if (!typeof(T).IsValueType && model == null)
            {
                model = (T)Activator.CreateInstance(typeof(T));
            }
            modelsDictionary.Add(type, model);

            return model;
        }

        public void Save<T>(T data, bool allowOverwrite)
        {
            _cacheManager.Save(data, allowOverwrite);
        }

        public void Clear<T>()
        {
            _cacheManager.Clear<T>();
        }
    }
}
