using UnityEngine;

// interface for all pooled objects
public interface IPooledObject {
	void OnObjectSpawn(string key);
}
