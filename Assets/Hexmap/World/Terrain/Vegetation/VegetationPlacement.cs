using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationPlacement : MonoBehaviour
{

    public int xRotation;
    public int yRotation;
    public int zRotation;

    public int maxSlope;

    public bool belowZero;
    
    public void PlaceObject(GameObject parent){

        //Place Object on Surface of parent object
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);

        if(hitData.collider == null){
            Destroy(this.gameObject);
            return;
        }
        if(hitData.collider.gameObject != parent) {
            Destroy(this.gameObject);
            return;
        }

        transform.position = hitData.point;

        //adjust rotation
        transform.rotation = Quaternion.Euler(Random.Range(-xRotation, xRotation), Random.Range(-yRotation, yRotation), Random.Range(-zRotation, zRotation));

        //Check slope
        if(Vector3.Angle(Vector3.up, hitData.normal) > maxSlope){
            Destroy(this.gameObject);
            return;
        }

        //Check zero
        if(hitData.point.y < 0 && !belowZero){
            Destroy(this.gameObject);
            return;
        }

        //Ceck maxHeight = 200
        if(hitData.point.y > 200){
            Destroy(this.gameObject);
            return;
        }

    }

}
