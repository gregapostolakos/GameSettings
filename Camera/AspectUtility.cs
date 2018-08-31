using UnityEngine;
using GameSettings;

public class AspectUtility : MonoBehaviour {
	
	public bool aspectRatioFromSettings=true;
	public float wantedAspectRatio = 1.777778f;
	public Camera cam;
	static Camera cameraRef;
	static Camera backgroundCam;

	void Reset(){
		cam = GetComponent<Camera>();
	}
	
	void Start() {
		if (!cam)
			cam = GetComponent<Camera>();
		if (!cam) {
			cam = Camera.main;
		}
		if (!cam) {
			Debug.LogError ("No camera available");
			return;
		}
		if(cam)
			cameraRef = cam;
		if(aspectRatioFromSettings && VideoSettings.singleton){
			wantedAspectRatio = VideoSettings.GetAspectRatio();
			VideoSettings.singleton.onUpdateResolution.AddListener(UpdateCam);
		}
		SetCam();
	}

	public void UpdateCam(){
		if(aspectRatioFromSettings){
			Debug.LogError("event2");
			wantedAspectRatio = VideoSettings.GetAspectRatio();
		}
		cam = GetComponent<Camera>();
		SetCam();
	}

	void OnDestroy(){
		if(aspectRatioFromSettings && VideoSettings.singleton){
			VideoSettings.singleton.onUpdateResolution.RemoveListener(UpdateCam);
		}
	}
	
	[ContextMenu("SetCam")]
	public void SetCam () {
		float currentAspectRatio = (float)Screen.width / Screen.height;
		// If the current aspect ratio is already approximately equal to the desired aspect ratio,
		// use a full-screen Rect (in case it was set to something else previously)
		if ((int)(currentAspectRatio * 100) / 100.0f == (int)(wantedAspectRatio * 100) / 100.0f) {
			cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			if (backgroundCam) {
				Destroy(backgroundCam.gameObject);
			}
			return;
		}
		// Pillarbox
		if (currentAspectRatio > wantedAspectRatio) {
			float inset = 1.0f - wantedAspectRatio/currentAspectRatio;
			cam.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
		}
		// Letterbox
		else {
			float inset = 1.0f - currentAspectRatio/wantedAspectRatio;
			cam.rect = new Rect(0.0f, inset/2, 1.0f, 1.0f-inset);
		}
		if (!backgroundCam) {
			// Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
			backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
			backgroundCam.depth = int.MinValue;
			backgroundCam.clearFlags = CameraClearFlags.SolidColor;
			backgroundCam.backgroundColor = Color.black;
			backgroundCam.cullingMask = 0;
		}
	}
	
	public static int screenHeight {
		get {
			if(cameraRef){return (int)(Screen.height * cameraRef.rect.height);}
			else{return Screen.height;}	  
		}
	}
	
	public static int screenWidth {
		get {
			if(cameraRef){return (int)(Screen.width * cameraRef.rect.width);}
			else{return Screen.width;}	  
		}
	}
	
	public static int xOffset {
		get {
			return (int)(Screen.width * cameraRef.rect.x);
		}
	}
	
	public static int yOffset {
		get {
			return (int)(Screen.height * cameraRef.rect.y);
		}
	}
	
	public static Rect screenRect {
		get {
			return new Rect(cameraRef.rect.x * Screen.width, cameraRef.rect.y * Screen.height, cameraRef.rect.width * Screen.width, cameraRef.rect.height * Screen.height);
		}
	}
	
	public static Vector3 mousePosition {
		get {
			Vector3 mousePos = Input.mousePosition;
			mousePos.y -= (int)(cameraRef.rect.y * Screen.height);
			mousePos.x -= (int)(cameraRef.rect.x * Screen.width);
			return mousePos;
		}
	}
	
	public static Vector2 guiMousePosition {
		get {
			Vector2 mousePos = Event.current.mousePosition;
			mousePos.y = Mathf.Clamp(mousePos.y, cameraRef.rect.y * Screen.height, cameraRef.rect.y * Screen.height + cameraRef.rect.height * Screen.height);
			mousePos.x = Mathf.Clamp(mousePos.x, cameraRef.rect.x * Screen.width, cameraRef.rect.x * Screen.width + cameraRef.rect.width * Screen.width);
			return mousePos;
		}
	}
}