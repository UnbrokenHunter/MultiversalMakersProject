using UnityEngine;

public class Parallax : MonoBehaviour {

    //Parallax Scroll Variables
    public Camera cam;//the camera
    public Transform subject;//the subject (usually the player character)


	//Instance variables
	private float zPosition;
	private Vector2 startPos;


	//Properties
	private float TwoAspect => cam.aspect * 2;
	private float TileWidth => (TwoAspect > 3 ? TwoAspect : 3);
	private float ViewWidth => loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit; 
	private Vector2 Travel => (Vector2) subject.transform.position - startPos; //2D distance travelled from our starting position
    private float DistanceFromSubject => transform.position.z - subject.position.z;
    private float ClippingPlane => (cam.transform.position.z + (DistanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    private float ParallaxFactor => Mathf.Abs(DistanceFromSubject) / ClippingPlane;


    //User options
    public bool xAxis = true; //parallax on x?
    public bool yAxis = true; //parallax on y?
    public bool infiniteLoop = false; //are we looping?


    //Loop requirement
    public SpriteRenderer loopSpriteRenderer;


	private void Awake() {
        cam = Camera.main;
        startPos = transform.position;
        zPosition = transform.position.z;

        if (loopSpriteRenderer != null && infiniteLoop) {
            float spriteSizeX = loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
            float spriteSizeY = loopSpriteRenderer.sprite.rect.height / loopSpriteRenderer.sprite.pixelsPerUnit;

            loopSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
            loopSpriteRenderer.size = new Vector2(spriteSizeX * TileWidth, spriteSizeY);
            transform.localScale = Vector3.one;
        }
    }



    private void Update() {
        Vector2 newPos = startPos + Travel * ParallaxFactor;
        transform.position = new Vector3(
            xAxis ? newPos.x : startPos.x, 
            yAxis ? newPos.y : startPos.y, 
            zPosition);

        if (infiniteLoop) {
            Vector2 totalTravel = cam.transform.position - transform.position;
            float boundsOffset = (ViewWidth / 2) * (totalTravel.x > 0 ? 1 : -1);
            float screens = (int)((totalTravel.x + boundsOffset) / ViewWidth);
            transform.position += new Vector3(screens * ViewWidth, 0);
        }
    }

}
