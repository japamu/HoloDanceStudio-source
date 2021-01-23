using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoInstance<T> : MonoBehaviour {
    protected static T instance;
    public static T Instance { get {
			return instance;
		} }

    protected virtual void Awake() {
		//if (instance == null) {

			T _instance = GetComponent<T>();
			instance = _instance;

			PostInitialization();
		//}
    }

	protected virtual void PostInitialization () {}
}