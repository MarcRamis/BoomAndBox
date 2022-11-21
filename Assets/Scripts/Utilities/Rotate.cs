//
//  Rotate.cs
//
//  Created by SummerTeam on 17/06/2022
//
	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SummerTeam.RogueLite
{
	public class Rotate : MonoBehaviour
	{
        public float speedX, speedY, speedZ;
        Vector3 rotation = default;

        void Update()
        {
            rotation.x = speedX * Time.deltaTime;
            rotation.y = speedY * Time.deltaTime;
            rotation.z = speedZ * Time.deltaTime;

            this.transform.Rotate(rotation);
        }
    }
}