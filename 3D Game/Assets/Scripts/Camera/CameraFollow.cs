using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                //Objetivo
    public float smoothing = 5f;            //velocidad del lerping
    Vector3 offset;                         //offset para saber la posicion de la camara con respecto a la posicion del target.

    void Start()
    {
        //toma la posicion de la camara menos la del objetivo para conseguir el offset.
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;  //siempre ve actualizando la posición de el objetivo con la camara
        
        //asigna su posición a una interpolación entre el mismo, su objetivo, y el smoothing * delta.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        

    }
}
