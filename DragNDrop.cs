using UnityEngine;

// Class that gives drag drop and other functionality to the candies
public class DragNDrop : MonoBehaviour
{
    // Various properties which constraint the movement of the candy game object
    private bool drag = false, isClone = false, move = false, rotateC = false, rotateAC = false;
    // Variables that store if object is overlapping with other objects and number of clones created from the object
    private int overlap = 0, cloneCount = 0;
    // Coordinates of the sprite of the game object and the mouse
    private Vector3 mouse, sprite;
    // Angle of rotation of the object, speed of rotation of object and time taken by player to drop the candy inside the box
    private float angleZ = 0, speed = 100, timeToDrop = 0;
    // Game object reference to the box (chocolate container)
    public GameObject box;
    // Reference to the clone object created
    GameObject clone;

    // Function that is called when player clicks down on the mouse
    public void OnMouseDown()
    {
        // If game is paused, don't do anything
        if (PauseMenu.paused)
            return;
        // Allow dragging the object
        drag = true;
        // Transform the coordinates of the mouse from screen to the game world
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Store the object's current position
        sprite = transform.localPosition;
        // If object is not clone
        if (!isClone)
        {
            // Create a clone
            clone = Instantiate(gameObject);
            // Increase the clone count by 1
            cloneCount++;
            // Set the clone's name based on the clone count (for a unique name)
            clone.name = gameObject.name + "_clone" + cloneCount;
            // Set the properties of the clone object
            clone.GetComponent<DragNDrop>().isClone = true;
            clone.GetComponent<DragNDrop>().move = true;
            // Make sorting layer of clone object as 2 so that it is always rendered on the top on the screen
            clone.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // If object is not movable, do nothing
        if (!move)
            return;
        // If game is paused and object was being dragged, destroy the object
        if (PauseMenu.paused)
        {
            Destroy(gameObject);
            return;
        }
        // If player presses down on D, allow rotation clockwise
        if (Input.GetKeyDown(KeyCode.D))
            rotateC = true;
        // If player presses up from D, stop allowing rotation clockwise
        if (Input.GetKeyUp(KeyCode.D))
            rotateC = false;
        // If player presses down on A, allow rotation anti-clockwise
        if (Input.GetKeyDown(KeyCode.A))
            rotateAC = true;
        // If player presses up from A, stop allowing rotation anti-clockwise
        if (Input.GetKeyUp(KeyCode.A))
            rotateAC = false;
        // If clockwise rotation is allowed, rotate the object slightly according to the rotation speed
        if (rotateC)
        {
            angleZ -= Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
        // If anti-clockwise rotation is allowed, rotate the object slightly according to the rotation speed
        else if (rotateAC)
        {
            angleZ += Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
    }

    // Function that is called when player clicks up from the mouse
    public void OnMouseUp()
    {
        // Stop allowing player to drag the object
        drag = false;
        // If clone object is present
        if (clone != null)
        {
            // Freeze movement of the clone
            clone.GetComponent<DragNDrop>().move = false;
            // Check if clone object is inside the box boundary and not overlapping with other objects / box boundary
            if (clone.GetComponent<DragNDrop>().HasOverlap())
            {
                // Object can't be placed here. Play the error audio
                AudioManager.Instance.Play("Error");
                // Destroy the object
                Destroy(clone);
            }
            else
            {
                // Object satisfies all criteria. It can be placed in the current position
                // Play the success audio
                AudioManager.Instance.Play("Success");
                // Add the candy to the tally
                GameManager.Instance.AddCandy(clone);
                // Add XP points based on how long the player took to drop the candy into the box
                GameManager.Instance.AddXP(timeToDrop);
                // Set the sorting order back to 1
                clone.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }

    // Function that is called when player is dragging the mouse 
    public void OnMouseDrag()
    {
        // If game is paused, don't do anything
        if (PauseMenu.paused)
            return;
        // If object can be dragged and it is a clone (original object should not be moved, only copies can move)
        if (drag && clone)
        {
            // Add the delta time to the time it takes for the player to drop the candy into the box
            timeToDrop += Time.deltaTime;
            // Change the position of the clone to follow the mouse
            clone.transform.localPosition = sprite + Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouse;
        }
    }

    // Function that is called when 2 objects enter into a collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If other object is clone, increase the overlap count by 1
        if (collision.gameObject.name.Contains("clone"))
            overlap++;
    }

    // Function that is called when 2 objects exit from a collision
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If other object is clone, decrease the overlap count by 1
        if (collision.gameObject.name.Contains("clone"))
            overlap--;
    }

    // Check if object is placed in a valid location
    private bool HasOverlap()
    {
        // If object is overlapping with other existing objects, return true
        if (overlap > 0)
            return true;
        // Get the collider of the object
        PolygonCollider2D collider = transform.GetComponent<PolygonCollider2D>();
        // For each point of the object, check if the point lies inside the box completely
        foreach (Vector2 point in collider.points)
        {
            Vector3 pt = transform.TransformPoint(point);
            // Point is lying outside/on the boundary the box, so return true
            if (box.GetComponent<BoxCollider2D>().bounds.Contains(pt) == false)
                return true;
        }
        // No overlap at all, return false
        return false;
    }
}
