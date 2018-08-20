using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarp : MonoBehaviour
{

    public struct PointInTime
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    public bool isWarping = true;

    private float recordTime = 10f;
    private float recordsPerSecond = 1;

    PointInTime[] pointsInTime;
    int currentIndex;
    int warpIndex;
    int recordAmount;
    
    Rigidbody rb;
    
    void Start()
    {
        recordsPerSecond = 1 / Time.fixedDeltaTime;
        pointsInTime = new PointInTime[(int) recordTime * Mathf.FloorToInt(recordsPerSecond)];
        ResetRecords();
        currentIndex = pointsInTime.Length - 1;
        recordAmount = pointsInTime.Length - 1;
        rb = GetComponent<Rigidbody>();               
    }

    public void ResetRecords()
    {
        if (pointsInTime!=null)
        {
            for (int i = 0; i < pointsInTime.Length; i++)
            {
                pointsInTime[i].position = transform.position;
                pointsInTime[i].rotation = transform.rotation;
            }
            currentIndex = pointsInTime.Length - 1;
            recordAmount = pointsInTime.Length - 1;
        }
    }

    void FixedUpdate()
    {
      
        if (!isWarping)
            Record();
        
    }

    void Record()
    {
        currentIndex++;
        if (currentIndex == pointsInTime.Length)
        {
            currentIndex = 0;
        }
        PointInTime pt;
        pt.position = transform.position;
        pt.rotation = transform.rotation;
        pointsInTime[currentIndex] = pt;

        if (recordAmount < pointsInTime.Length)
            recordAmount++;



    }

    public void StartWarping()
    {
        isWarping = true;
        warpIndex = currentIndex;
        rb.isKinematic = true;
    }

    public void StopWarping()
    {
        isWarping = false;
        if (currentIndex > warpIndex)
        {
            int size = ((int) recordTime * (int) (recordsPerSecond));
            recordAmount -= (size - currentIndex) + (warpIndex);
        }
        else
            recordAmount -= (warpIndex - currentIndex);                
        rb.isKinematic = false;
    }


    public void SetPointInTime(float point)
    {
        if (isWarping && recordAmount > 0)
        {
            int total = recordAmount - 1;
            
            int offset = ((int) recordTime * (int) (recordsPerSecond)) - recordAmount;
            int normalizedIndex = Mathf.FloorToInt(point * (total - 1));

            int desiredIndex = normalizedIndex - (total - warpIndex);
            if (desiredIndex < 0)
                desiredIndex = total + desiredIndex + 1 + offset;

            try
            {
                PointInTime pointInTime = pointsInTime[desiredIndex];
                transform.position = pointInTime.position;
                transform.rotation = pointInTime.rotation;
               
                currentIndex = desiredIndex;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log(e.ToString() + " " + desiredIndex);
            }
        }

    }
}
